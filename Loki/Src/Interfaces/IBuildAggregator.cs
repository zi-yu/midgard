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

using Loki.Generic;
using System.Collections.Generic;
namespace Loki.Interfaces {

	public interface IBuildAggregator {
		
		void Init( IProject project );

		/*
		 RegisterComponent( "DAL");acesso a dados
		 RegisterComponent( "DLL");logica
		 */
		void RegisterComponent( string name );

		void RegisterFile( string component, string name );

		void RegisterFile( string component, string name, VSFileType type );

		void RegisterFile( string component, string name, bool isSubType, string dependsUpon );

		void RegisterFile( string component, string name, bool isSubType, string dependsUpon, VSFileType type );

		void RegisterFile( string component, BuildFile file );

		void RegisterAssembly( string component, string name );

		void RegisterGacAssembly( string component, string name );

		void RegisterComponentReference( string component, string reference );
		
		void RegisterProjectType( string component, ProjectTypes type );

		void RegisterResource( string component, string name );

		Dictionary<string, BuildInformation> Informations { get; }

		
	}
}