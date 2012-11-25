using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Loki.Generic;
using Loki.Interfaces;

namespace Loki.DataRepresentation.Loaders {
	public class ConfigLoader {

		#region Fields

		private static ConfigLoader configLoader = new ConfigLoader();
		private delegate void ParseNode( XmlNode node, Object obj );
		private Dictionary<string, ParseNode> parsers = new Dictionary<string, ParseNode>();

		private IProject project;
		private XmlDocument doc;

		#endregion

		#region Static

		public static ConfigLoader Instance {
			get { return configLoader; }
		}

		#endregion

		#region Private

		private void Load() {
			foreach( XmlNode node in doc.DocumentElement.ChildNodes ) {
				if( node is XmlComment ) {
					continue;
				}
				Log.Info( "Start parsing element " + node.Name + "..." );
				if( parsers.ContainsKey( node.Name ) ) {
					parsers[node.Name]( node, null );
					Log.Info( "Done" );
				} else {
					Log.Info( "Don't know how to parse {0}", node.Name );
				}
				
			}
		}

		private void ParsePlugin( XmlNode node, ProjectPluginParameters pluginParameters ) {
			foreach( XmlNode child in node.ChildNodes ) {
				if( child is XmlComment ) {
					continue;
				}
				pluginParameters.Add( new KeyValuePair<string, Dictionary<string, string>>( child.Attributes["name"].Value, GetPlugins( child ) ) );
			}
		}

		private Dictionary<string, string> GetPlugins( XmlNode node ) {
			Dictionary<string, string> param = new Dictionary<string, string>();
			if( node.HasChildNodes ) {
				foreach( XmlNode child in node.ChildNodes ) {
					param.Add( child.Attributes["name"].Value, child.Attributes["value"].Value );
				}
			}
			return param;
		}

		private bool ToBool( string value ) {
			if( string.IsNullOrEmpty( value ) ) { 
				return false;
			}
			return value.ToLower() == "true"; 
		}

		private string GetAttribute( XmlNode node, string value ) {
			if( node.Attributes[value] != null ) {
				return node.Attributes[value].Value;
			}
			return string.Empty;
		}
		
		private void ParseCodeGenerator( XmlNode node, Object obj ) {
			ParsePlugin( node, project.PluginParameters );
		}

		private void ParseBuildGenerator( XmlNode node, Object obj ) {
			ParsePlugin( node, project.BuildPluginParameters );
		}

		private void ParseLoadGenerator( XmlNode node, Object obj ) {
			ParsePlugin( node, project.LoadPluginParameters );
		}

		private void ParseEntities( XmlNode node, Object obj ) {
			foreach( XmlNode child in node.ChildNodes ) {
				if( child is XmlComment ) {
					continue;
				}
				parsers[child.Name]( child, null );
			}
		}

		private void ParseEntity( XmlNode node, Object obj ) {
			try {
				EntityClass entity = project.GetEntity( GetAttribute( node, "name" ) );
				if( entity != null ) {
					Log.Info( "Parsing Entity {0}...", entity.Name );
					foreach( XmlNode child in node.ChildNodes ) {
						if( child is XmlComment ) {
							continue;
						}
						parsers[child.Name]( child, entity );
					}
					Log.Info( "Entity {0} parsed!", entity.Name );
				} else {
					Log.Info( "Ignoring entity '{0}' . This entity is not a class.", GetAttribute( node, "name" ) );
				}
			} catch( Exception e ) {
				Log.Error( "Error parsing entity '{0}'! - {1}", GetAttribute( node, "name") , e.Message );
			}
		}

		private void ParseField( XmlNode node, Object obj ) {
			try {
				EntityClass entity = (EntityClass)obj;

				EntityField field = entity.GetField( GetAttribute( node, "name" ) );
				if( field != null ) {
					Log.Info( "Parsing Field {0}...", field.Name );

					field.IsPreview = ToBool( GetAttribute( node, "isPreview" ) );
					field.Represents = ToBool( GetAttribute( node, "represents" ) );

					foreach( XmlNode child in node.ChildNodes ) {
						if( child is XmlComment ) {
							continue;
						}
						parsers[child.Name]( child, field );
					}

					Log.Info( "Field {0} parsed!", field.Name );
				} else {
					Log.Info( "Ignoring field {0}. This field doesn't exist.", node.Name );
				}
			} catch( Exception e ) {
				Log.Error( "Error parsing field {0}! - {1}", GetAttribute( node, "name" ), e.Message );
			}
		}

		private void ParseRegex( XmlNode node, Object obj ) {
			try {
				EntityField field = (EntityField)obj;
				Log.Info( "Parsing Regex..." );

				string regex = GetAttribute( node, "value" );
				if( !string.IsNullOrEmpty( regex ) ) {
					field.Regex.Add( regex );
				}
				
				Log.Info( "Regex parsed!" );
			} catch( Exception e ) {
				Log.Error( "Error parsing Regex! - {0}", e.Message );
			}
		}

		private void ParseMaxSize( XmlNode node, Object obj ) {
			try {
				EntityField field = (EntityField)obj;
				Log.Info( "Parsing MaxSize..." );

				string maxsize = GetAttribute( node, "value" );
				if( !string.IsNullOrEmpty( maxsize ) ) {
					field.MaxSize = int.Parse( maxsize );					
				}

				Log.Info( "MaxSize parsed!" );
			} catch( Exception e ) {
				Log.Error( "Error parsing MaxSize! - {0}", e.Message );
			}
		}

		#endregion

		#region Public

		public void LoadPlugins( Project project, XmlDocument doc ) {
			this.project = project;
			this.doc = doc;
			
			parsers.Clear();
			parsers.Add( "codeGenerator", ParseCodeGenerator );
			parsers.Add( "buildGenerator", ParseBuildGenerator );
			parsers.Add( "loader", ParseLoadGenerator );

			Load();
		}

		public void LoadEntities( Project project, XmlDocument doc ) {
			this.project = project;
			this.doc = doc;

			parsers.Clear();
			parsers.Add( "entities", ParseEntities );
			parsers.Add( "entity", ParseEntity );
			parsers.Add( "field", ParseField );
			parsers.Add( "regex", ParseRegex );
			parsers.Add( "maxsize", ParseMaxSize );

			Load();
		}

		#endregion
	}
}
