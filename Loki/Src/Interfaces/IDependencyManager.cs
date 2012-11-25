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

    /// <summary>
    /// This interface gives generic utilities to the plugins execution
    /// </summary>
    public interface IDependencyManager {

        #region Artifact Methods

        /// <summary>
        /// Registers an artifact
        /// </summary>
        /// <param name="name">Artifact name</param>
        /// <param name="artifact">Object</param>
        void RegisterArtifact(string name, object artifact);

        /// <summary>
        /// Get's a named artifact
        /// </summary>
        /// <param name="name">Artifact's name</param>
        /// <returns>Artifact</returns>
        object GetArtifact(string name);

        /// <summary>
        /// Indicates if a named artifact exists
        /// </summary>
        /// <param name="name">Artifact's name</param>
        /// <returns>Artifact</returns>
        bool HasArtifact(string name);

        #endregion Artifact Methods

        #region Plugin Methods

        /// <summary>
        /// Indicates if a given plugin is registered for execution
        /// </summary>
        /// <param name="name">Plugin Name</param>
        /// <returns>True if the plugin is registered</returns>
        bool IsPluginRegistered(string name);

        #endregion

        #region General Dependencies

        /// <summary>
        /// Registers a named dependency
        /// </summary>
        /// <param name="reference">Dependency name</param>
        void RegistDependency(string reference);

        /// <summary>
        /// Indicates if a named dependency exists
        /// </summary>
        /// <param name="dep">Dependency Name</param>
        /// <returns>True if the dependecy is registered</returns>
        bool HasDependency(string dep);
        
        #endregion General Dependencies

        #region Localization

        /// <summary>
        /// Adds a key to be handled by the localization utilities
        /// </summary>
        /// <param name="category">The Token category</param>
        /// <param name="key">The key</param>
        /// <param name="defaultText">The sample content</param>
        void Localize( string category, string key, string defaultText );

        /// <summary>
        /// Retrieves all the registered localization tokens
        /// </summary>
        Dictionary<string, List<LocalizationToken>> LocalizationTokens { get;}

        #endregion Localization

    };

}

