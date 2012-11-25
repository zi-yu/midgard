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
using Loki.Interfaces;
using Loki.Exceptions;

namespace Loki.Generic {

	public class BuildAggregator : IBuildAggregator {

		#region Private FIelds

		private IProject project;
		private Dictionary<string, BuildInformation> informations = new Dictionary<string, BuildInformation>();
		private Dictionary<string, List<string> > lateReferences = new Dictionary<string, List<string> >();
		
		#endregion

		#region Properties

		public Dictionary<string, BuildInformation> Informations {
			get { return informations; }
		}
	
		#endregion

		#region Private

		private void ResolveLateReferences( string name ) {
			if( lateReferences.ContainsKey( name ) ) {
				List<string>.Enumerator iter = lateReferences[name].GetEnumerator();
				while( iter.MoveNext() ) {
					BuildInformation b = informations[iter.Current];
					b.References.Add( new ReferenceInformation( name, informations[name].Guid ) );

					lateReferences[name].Remove( iter.Current );
					iter = lateReferences[name].GetEnumerator();
				}
				lateReferences.Remove( name );
			}
		}

		#endregion

		#region IBuildAggregator Members

		public void Init( IProject project ) {
			this.project = project;
		}

		public void RegisterComponent( string name ) {
			if( !informations.ContainsKey( name ) ) {
				informations.Add( name, new BuildInformation( name ) );
				ResolveLateReferences( name );

				RegisterGacAssembly( name, "System" );
			}
		}

		public void RegisterFile( string component, string name ) {
			RegisterFile( component, name, false, null,VSFileType.Compile );
		}

		public void RegisterFile( string component, string name, bool isSubType, string dependsUpon ) {
			RegisterFile( component, name, isSubType, dependsUpon, VSFileType.Compile );
		}

		public void RegisterFile( string component, string name, VSFileType type ) {
			RegisterFile( component, name, false, null, type );
		}

		public void RegisterFile( string component, BuildFile file ) {
			if( informations.ContainsKey( component ) ) {
				file.Component = component;
				informations[component].Files.Add( file );
			} else {
				throw new UnknownComponentException( component );
			}
		}

		public void RegisterFile( string component, string name, bool isSubType, string dependsUpon, VSFileType type ) {
			if( informations.ContainsKey( component ) ) {
				informations[component].Files.Add( new BuildFile( component, name, isSubType, dependsUpon, type ) );
			} else {
				throw new UnknownComponentException( component );
			}
		}

		public void RegisterAssembly( string component, string name ) {
			if( informations.ContainsKey( component ) ) {
				if( !informations[component].Assemblies.Contains( name ) ) {
					informations[component].Assemblies.Add( name );
				}
			} else {
				throw new UnknownComponentException( component );
			}
		}

		public void RegisterComponentReference( string component, string reference ) {
			if( informations.ContainsKey( component ) ) {
				if( informations.ContainsKey( reference ) ) {
					informations[component].References.Add( new ReferenceInformation( reference, informations[reference].Guid ) );
				} else {
					if( !lateReferences.ContainsKey( reference ) ) {
						lateReferences.Add( reference, new List<string>() );
					}
					lateReferences[reference].Add( component );
				}
			} else {
				throw new UnknownComponentException( component );
			}
		}

		public void RegisterGacAssembly( string component, string name ) {
			if( informations.ContainsKey( component ) ) {
				if( !informations[component].GacAssemblies.Contains( name ) ) {
					informations[component].GacAssemblies.Add( name );
				}
			} else {
				throw new UnknownComponentException( component );
			}
		}

		public void RegisterProjectType( string component, ProjectTypes type ) {
			if( informations.ContainsKey( component ) ) {
				informations[component].ProjectType = type;	
			} else {
				throw new UnknownComponentException( component );
			}
		}

		public void RegisterResource( string component, string name ) {
			if( informations.ContainsKey( component ) ) {
				informations[component].Resources.Add( name );
			} else {
				throw new UnknownComponentException( component );
			}
		}

		#endregion
	}

}
