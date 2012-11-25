#region Licence Statment
// Copyright (c) Zi-Yu.com - All Rights Reserved
// http://midgard.zi-yu.com/
//
// The use and distribution terms for this software are covered by the
// LGPL (http://opensource.org/licenses/lgpl-license.php).
// By using this software in any fashion, you are agreeing to be bound by
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Loki.Generic;
using Loki.Exceptions;
using DesignPatterns;
using Loki.Interfaces;
using Loki.DataRepresentation;
using Loki.DataRepresentation.Loaders;
using System.IO;

namespace Loader.Xmi {
	public class XmiLoader {

		#region Private Fields

		private static XmiLoader loader = new XmiLoader();

		private Dictionary<string, ReadTags> tags = new Dictionary<string,ReadTags>();
		private Dictionary<string, InnerTags> innerTags = new Dictionary<string, InnerTags>();
		private delegate void ReadTags( XmlNode element );
		private delegate void InnerTags( IEntity entity, XmlNode element );

		private IProject project;
		private string filename;
		
		#endregion
		
		#region Static Fields

		private List<UnresolvedInfo> unresolved = new List<UnresolvedInfo>();
		private Dictionary<string, Entity> dataTypes = new Dictionary<string, Entity>();
		private Dictionary<string, EntityField> associations = new Dictionary<string, EntityField>();

		#endregion
		
		#region Initializer

		public void Init( IProject project ) {
			this.project = project;
			this.filename = Path.Combine(project.ProjectPath, string.Format("{0}.xmi", project.Name));

			unresolved.Clear();
			dataTypes.Clear();
			
			//Primary Tags
			tags["uml:model"] = new ReadTags( ParseModel );
			tags["uml:class"] = new ReadTags( ParseClass );
			tags["uml:interface"] = new ReadTags( ParseInterface );
			
			//Tiveram de ser adicionas senão o Motor iria ler a estas tags
			tags["xmi:extension"] = new ReadTags( Ignore );
			tags["xmi:documentation"] = new ReadTags( Ignore );
			
			//Inner Tags
			innerTags["uml:generalization"] = new InnerTags( ParseGeneralization );
			innerTags["uml:operation"] = new InnerTags( ParseOperation );
			innerTags["uml:parameter"] = new InnerTags( ParseParameter );
			innerTags["uml:property"] = new InnerTags( ParseProperty );
			innerTags["uml:primitivetype"] = new InnerTags( ParsePrimitiveType );
			innerTags["uml:datatype"] = new InnerTags( ParsePrimitiveType );
			innerTags["uml:interfacerealization"] = new InnerTags( ParseInterfaceRealization );
			innerTags["uml:property"] = new InnerTags( ParseProperty );
			innerTags["uppervalue"] = new InnerTags( UpperValue );
			innerTags["lowervalue"] = new InnerTags( LowerValue );
		}
		
		#endregion

		#region Properties

		public Dictionary<string, Entity> DataTypes {
			get { return dataTypes; }
		}

		public IProject Project {
			get { return project; }
			set { project = value; }
		}

		public static XmiLoader Instance {
			get { return loader; }
		}

		#endregion

		#region Private

		private string GetNodeName( XmlNode node ) {
			XmlAttribute attr = node.Attributes["xmi:type"];
			if( attr != null ) {
				return attr.Value.ToLower();
			}
			return node.Name.ToLower();
		}

		private string GetInnerNodeName( XmlNode node ) {
			XmlAttribute attr = node.Attributes["xmi:type"];
			if( attr != null && innerTags.ContainsKey( attr.Value.ToLower() ) ) {
				return attr.Value.ToLower();
			}
			return node.Name.ToLower();
		}

		private void ResolveUnresolved() {
			foreach( UnresolvedInfo info in unresolved ) {
				info.Resolver.ResolveConflits( info.Node );
			}
		}

		private void ResolveReferenceMultiplicity( IEntityAttribute field ) {
			if( field is EntityField ) {
				ResolveReferenceMultiplicity( (EntityField)field );
			}
		}

		private void ResolveReferenceMultiplicity( EntityField field ) {
			if( field.ReferenceType != null ) {
				switch( field.Mult ) {
					case Multiplicity.OneToMany:
						field.ReferenceType.Mult = Multiplicity.ManyToOne;
						break;
					case Multiplicity.ManyToOne:
						field.ReferenceType.Mult = Multiplicity.OneToMany;
						break;
					default:
						field.ReferenceType.Mult = field.Mult;
						break;
				}
			}
		}

		private void AddIds() {
			foreach( IEntity entity in project.Model ) {
				if( entity is EntityClass ) {
					EntityClass e = entity as EntityClass;
					if (!e.RootEntity) {
						// Derived classes inherit the Id from the parent
						continue;
					}
					EntityField field = new EntityField();
					field.Name = "id";
					field.IsPrimaryKey = true;
					field.FieldModifier = "private";
					field.Type = new Loki.DataRepresentation.IntrinsicEntities.Int();
					e.Fields.Insert( 0, field );
				}
			}
		}

		private bool IsAttributeValid( XmlAttribute attr ) {
			return null != attr && !string.IsNullOrEmpty( attr.Value );
		}
		
		#region Parser Utility

		private void ParseNode( XmlNode element ) {
			string name = GetNodeName( element );
			if( !string.IsNullOrEmpty( name ) && tags.ContainsKey( name ) ) {
				tags[name]( element );
			} else {
				Log.Warn( "Don't know how to handle element '{0}'... ignoring it.", element.Name );
				foreach( XmlNode child in element.ChildNodes ) {
					if( child.NodeType == XmlNodeType.Element ) {
						ParseNode( child );
					}
				}
			}
		}

		private void ParseInnerNode( IEntity entity, XmlNode element ) {
			string name = GetInnerNodeName( element );
			if( !string.IsNullOrEmpty( name ) && innerTags.ContainsKey( name ) ) {
				innerTags[name]( entity, element );
			} else {
				Log.Warn( "Don't know how to handle element '{0}'... ignoring it.", element.Name );
				foreach( XmlNode child in element.ChildNodes ) {
					if( child.NodeType == XmlNodeType.Element ) {
						ParseInnerNode( entity, child );
					}
				}
			}
		}

		private void IterateChilds( XmlNode parent, Entity entity ) {
			foreach( XmlNode child in parent.ChildNodes ) {
				if( child.NodeType == XmlNodeType.Element ) {
					ParseInnerNode( entity, child );
				}
			}

			project.Model.Add( entity );
			dataTypes.Add( entity.Id, entity );
		}

		private void IterateInnerChilds( XmlNode element, IEntity entity ) {
			foreach( XmlNode child in element.ChildNodes ) {
				if( child.NodeType == XmlNodeType.Element ) {
					ParseInnerNode( entity, child );
				}
			}
		}

		private void ResolveIntrinsicType( XmlNode element, IEntityAttribute field ) {
			if( element != null && element.HasChildNodes && element.FirstChild.HasChildNodes ) {
				XmlAttribute type = element.FirstChild.FirstChild.Attributes["referentPath"];
				if( type != null ) {
					if( !dataTypes.ContainsKey( type.Value ) ) {
						string key = Translator.Translate( type.Value.ToLower() );
						if( null == key ) {
							throw new UnknownIntrinsicTypeException( type.Value );
						}

						dataTypes.Add( type.Value, Loki.DataRepresentation.Loaders.IntrinsicTypes.Create( key ) );
					}
					field.Type = dataTypes[type.Value];
				}
			}
		}

		#endregion

		#region Create Entities

		private EntityClass CreateEntityClass( XmlNode element ) {
			EntityClass entity = new EntityClass();
			entity.Project = Project;

			if( element.Attributes["xmi:id"] != null ) {
				entity.Id = element.Attributes["xmi:id"].Value;
			}

			if( element.Attributes["name"] != null ) {
				entity.Name = element.Attributes["name"].Value;
			}

			if( element.Attributes["visibility"] != null ) {
				entity.Visibility = element.Attributes["visibility"].Value;
			}

			if( element.Attributes["isAbstract"] != null ) {
				entity.IsAbstract = XmiLoader.ParseBool( element.Attributes["isAbstract"] );
			}

			return entity;
		}

		private EntityInterface CreateEntityInterface( XmlNode element ) {
			EntityInterface entity = new EntityInterface();

			if( element.Attributes["xmi:id"] != null ) {
				entity.Id = element.Attributes["xmi:id"].Value;
			}

			if( element.Attributes["name"] != null ) {
				entity.Name = element.Attributes["name"].Value;
			}

			if( element.Attributes["visibility"] != null ) {
				entity.Visibility = element.Attributes["visibility"].Value;
			}

			return entity;
		}

		private EntityMethod CreateEntityMethod( XmlNode element ) {
			EntityMethod method = new EntityMethod();

			if( element.Attributes["xmi:id"] != null ) {
				method.Id = element.Attributes["xmi:id"].Value;
			}

			if( element.Attributes["name"] != null ) {
				method.Name = element.Attributes["name"].Value;
			}

			if( element.Attributes["visibility"] != null ) {
				method.MethodModifier += element.Attributes["visibility"].Value;
			}

			if( element.Attributes["isAbstract"] != null && XmiLoader.ParseBool( element.Attributes["isAbstract"] ) ) {
				method.MethodModifier += " abstract";
			}

			if( element.Attributes["isStatic"] != null && element.Attributes["isStatic"].Value.CompareTo( "static" ) == 0 ) {
				method.MethodModifier += " static";
			}

			return method;
		}

		private EntityParameter CreateEntityParameter( XmlNode element ) {
			EntityParameter parameter = new EntityParameter();

			if( element.Attributes["xmi:id"] != null ) {
				parameter.Id = element.Attributes["xmi:id"].Value;
			}

			if( element.Attributes["name"] != null ) {
				parameter.Name = element.Attributes["name"].Value;
			}

			if( element.Attributes["direction"] != null ) {
				string kind = element.Attributes["direction"].Value.ToLower();

				if( kind.CompareTo( "return" ) == 0 ) {
					parameter.IsReturn = true;
				} else {
					if( kind.CompareTo( "inout" ) != 0 ) {
						parameter.Modifier = kind;
					}
				}
			}

			return parameter;
		}

		private EntityField CreateEntityField( XmlNode element ) {
			EntityField field = new EntityField();

			if( element.Attributes["xmi:id"] != null ) {
				field.Id = element.Attributes["xmi:id"].Value;
			}

			if( element.Attributes["name"] != null ) {
				field.Name = element.Attributes["name"].Value;
			}

			if( element.Attributes["visibility"] != null ) {
				field.Visibility = element.Attributes["visibility"].Value;
			}

			if( element.Attributes["ownerScope"] != null && element.Attributes["ownerScope"].Value.CompareTo( "instance" ) != 0 ) {
				field.FieldModifier += "static";
			}

			return field;
		}

		

		#endregion

		#region Primary Tags

		private void ParseModel( XmlNode element ) {
			foreach( XmlNode child in element.ChildNodes ) {
				if( child.NodeType == XmlNodeType.Element ) {
					ParseNode( child );
				}
			}
		}

		private void ParseClass( XmlNode element ) {
			if( IsAttributeValid( element.Attributes["name"] ) ) {
				EntityClass entity = CreateEntityClass( element );

				IterateChilds( element, entity );
			}
		}

		private void ParseInterface( XmlNode element ) {
			if( IsAttributeValid( element.Attributes["name"] ) ) {
				EntityInterface entity = CreateEntityInterface( element );
				IterateChilds( element, entity );
			}
		}

		private void Ignore( XmlNode element ) { }

		#endregion

		#region Inner Tags

		private void ParseGeneralization( IEntity entity, XmlNode element ) {
			if( entity is Entity ) {
				EntityClass e = entity as EntityClass;
				XmlAttribute attr = element.Attributes["general"];
				if( null != attr ) {
					if( dataTypes.ContainsKey( attr.Value ) ) {
						e.Parent = dataTypes[attr.Value];
					} else {
						unresolved.Add( new UnresolvedInfo( element, new EntityClassResolver( e ) ) );
					}
				}
			} else {
				throw new UnknownContextException( "Generalization is being parse in a unknown context.Generalization show be child of UML:Class" );
			}
		}

		private void ParseOperation( IEntity entity, XmlNode element ) {
			if( entity is Entity ) {
				EntityMethod method = CreateEntityMethod( element );
				( (Entity)entity ).Methods.Add( method );
				IterateInnerChilds( element, method );
			} else {
				throw new UnknownContextException( "Operation is being parse in a unknown context.Operation show be child of UML:Class or UML:Interface" );
			}
		}

		private void ParseParameter( IEntity entity, XmlNode element ) {
			if( entity is EntityMethod ) {
				EntityParameter parameter = CreateEntityParameter( element );
				ResolveType( element, parameter, true );
				IterateInnerChilds( element, parameter );
				
				if( parameter.IsReturn ) {
					( (EntityMethod)entity ).ReturnEntity = parameter;
				} else {
					( (EntityMethod)entity ).Parameters.Add( parameter );
				}
			} else {
				throw new UnknownContextException( "Parameter is being parse in a unknown context.Parameter show be child of UML:Operation" );
			}
		}

		private void ParseProperty( IEntity entity, XmlNode element ) {
			if( entity is EntityInterface ) {
				return;
			}

			if( entity is EntityClass ) {
				if( IsAttributeValid( element.Attributes["name"] ) ) {
					EntityField field = CreateEntityField( element );
					EntityClass entityClass = (EntityClass)entity;

					entityClass.AddField( field );
					field.Parent = entityClass;

					ResolveType( element, field, true );

					if( null != element.Attributes["association"] ) {
						associations.Add( field.Id, field );
					}
					IterateInnerChilds( element, field );
				}
			} else {
				throw new UnknownContextException( "Property is being parse in a unknown context. Property show be child of UML:Class" );
			}
		}
		
		private void ParsePrimitiveType( IEntity entity, XmlNode element ) {
            try {
                if (entity is IEntityAttribute) {
                    IEntityAttribute field = entity as IEntityAttribute;
                    ResolveIntrinsicType(element, field);
                    IterateInnerChilds(element, field);
                } else {
                    throw new UnknownContextException("uml:PrimitiveType is being parse in a unknown context. uml:PrimitiveType show be child of UML:Property or UML:Parameter");
                }
            } catch (UnknownIntrinsicTypeException ex) {
                if (entity is EntityField) {
                    EntityField field = (EntityField)entity;
                    throw new Exception("Error loading field '"+field.PropertyName+"'", ex);
                }
                throw;
            }
		}

		private void ParseInterfaceRealization( IEntity entity, XmlNode element ) {
			if( entity is Entity ) {
				string id = element.Attributes["supplier"].Value;
				if( dataTypes.ContainsKey( id ) ) {
					( (Entity)entity ).Interfaces.Add( (EntityInterface)dataTypes[id] );
				} else {
					unresolved.Add( new UnresolvedInfo( element, new EntityClassResolver( (EntityClass)entity ) ) );
					entity.Resolved = false;
				}
			} else {
				throw new UnknownContextException( "InterfaceRealization is being parse in a unknown context. InterfaceRealization show be child of UML:Class or UML:Interface" );
			}
		}

		private void UpperValue( IEntity entity, XmlNode element ) {
			if( entity is IEntityAttribute ) {
				IEntityAttribute field = entity as IEntityAttribute;
				XmlAttribute upperValue = element.Attributes["value"];
				if( null != upperValue ) {
					if( upperValue.Value == "*" ) {
						field.Mult = Multiplicity.OneToMany;
					} else {
						field.Mult = Multiplicity.OneToOne;
					}

					ResolveReference( field );
				}
			} else {
				throw new UnknownContextException( "UpperValue is being parse in a unknown context. UpperValue show be child of UML:Property" );
			}
		}

		private void LowerValue( IEntity entity, XmlNode element ) {
			if( entity is IEntityAttribute ) {
				IEntityAttribute field = entity as IEntityAttribute;
				XmlAttribute lowerValue = element.Attributes["value"];
				if( null != lowerValue ) {
					if( lowerValue.Value == "*" ) {
						field.Mult = Multiplicity.ManyToMany;
						ResolveReferenceMultiplicity( field );
					}
				}
			} else {
				throw new UnknownContextException( "LowerValue is being parse in a unknown context. UpperValue show be child of UML:Property" );
			}
		}

		#endregion

		#endregion

		#region Public

		public void Load() {
			try {
				if( tags.Count == 0 && innerTags.Count == 0 ) {
					Log.Error( "Loader init is required!" );
					return;
				}

				Log.Info("Start loading file '{0}'.", filename);

				XmlDocument doc = new XmlDocument();
				doc.Load(filename);
				foreach(XmlNode element in doc.DocumentElement) {
					ParseNode(element);
				}

				ResolveUnresolved();
				AddIds();

				Log.Info("File '{0}' loaded successufuly!", filename);
			} catch(Exception ex) {
				Log.Fatal(ex.ToString());
				throw;
			}
		}
				
		public void ResolveType(XmlNode element, IEntityAttribute field, bool addifnotfound) {
			XmlAttribute type = element.Attributes["type"];
			if( null != type ) {
				if( dataTypes.ContainsKey( type.Value ) ) {
					field.Type = dataTypes[type.Value];
					ResolveReference( field );
				} else {
					if( addifnotfound ) {
						field.Resolved = false;
						unresolved.Add( new UnresolvedInfo( element, new EntityAttributeResolver( field ) ) );
					}
				}
			}
		}

		public void ResolveReference( IEntityAttribute field ) {
			if( field is EntityField && field.Type is EntityClass ) {
				EntityField e = (EntityField)field;
				if( e.ReferenceType == null && e.Type != null && e.Mult != Multiplicity.OneToOne ) { 
					EntityField reference = new EntityField();
					reference.Name = e.Parent.Name;
					reference.Type = e.Parent;
					reference.InfoOnly = true;
					reference.ReferenceType = e;
					
					EntityClass entity = ((EntityClass)field.Type);
					if( entity.HasField( reference.Name ) ) {
						e.ReferenceType = entity.GetField( reference.Name );
					}else{
						e.ReferenceType = reference;
						entity.AddField( reference );	
					}
					
					ResolveReferenceMultiplicity( e );
				}
			}
		}

		#endregion

		#region Static Utilities

		public static bool ParseBool( XmlAttribute attr ) {
			if(null != attr) {
				string value = attr.Value;
				if(!string.IsNullOrEmpty(value) && value.ToLower() == bool.TrueString) {
					return true;
				}
			}
			return false;
		}

		#endregion

		#region Constructor

		private XmiLoader() { }

		#endregion

	};
}