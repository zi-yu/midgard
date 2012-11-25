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
using Loki.DataRepresentation;

namespace Loki.Interfaces {

	public interface IProject {

		/// <summary>
		/// Project Name
		/// </summary>
		string Name{get;}

        /// <summary>
        /// Gets the project parameters.
        /// </summary>
        /// <value>The parameters.</value>
		Parameters Parameters{get;}

        /// <summary>
        /// Adds a project parameter.
        /// </summary>
        /// <param name="name">The parameter name.</param>
        /// <param name="value">The value.</param>
        void AddParameter(string name, string value);

        /// <summary>
        /// Gets the project plugin parameters.
        /// </summary>
        /// <value>The plugin parameters.</value>
        ProjectPluginParameters PluginParameters { get;}

		/// <summary>
		/// Gets the project build plugin parameters.
		/// </summary>
		/// <value>The plugin parameters.</value>
		ProjectPluginParameters BuildPluginParameters {get ; }

		/// <summary>
		/// Gets the project load plugin parameters.
		/// </summary>
		/// <value>The plugin parameters.</value>
		ProjectPluginParameters LoadPluginParameters { get; }
		
        /// <summary>
        /// Register and Adds a plugin parameter to the project.
        /// </summary>
        /// <param name="name">The plugin name.</param>
        /// <param name="param">The plugin params.</param>
        void AddPluginParameter( string name, Dictionary<string, string> param );

		/// <summary>
		/// Register and Adds a build plugin parameter to the project.
		/// </summary>
		/// <param name="name">The plugin name.</param>
		/// <param name="param">The plugin params.</param>
		void AddBuildPluginParameter( string name, Dictionary<string, string> param );

		/// <summary>
		/// Register and Adds a load plugin parameter to the project.
		/// </summary>
		/// <param name="name">The plugin name.</param>
		/// <param name="param">The plugin params.</param>
		void AddLoadPluginParameter( string name, Dictionary<string, string> param );

        /// <summary>
        /// Gets the output path.
        /// </summary>
        /// <value>The output path.</value>
        string OutputPath { get; }

		/// <summary>
		/// Gets the project path.
		/// </summary>
		/// <value>The project path.</value>
		string ProjectPath { get;}

		/// <summary>
		/// Gets the model.
		/// </summary>
		/// <value>The model.</value>
		Model Model { get;set; }

		/// <summary>
		/// Gets the generic project information.
		/// </summary>
		/// <value>The model.</value>
		Dictionary<string,object> Generic { get;set; }

		/// <summary>
		/// Gets an Entity.
		/// </summary>
		/// <value>The model.</value>
		EntityClass GetEntity( string p );

		/// <summary>
		/// loads the config file
		/// </summary>		
		void LoadConfig();

		/// <summary>
		/// loads the configuration header file
		/// </summary>	
		void LoadEntityInfo();
	}

}

