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
using Loki.Interfaces;
using Loki.Generic.Factories;
using Loki.Generic;
using Loki.DataRepresentation.Loaders;

namespace Odin.Core {

    public class Generator: IDependencyManager {

        #region Fields 

        private PluginManager<ICodeGenerator> pluginManager = new PluginManager<ICodeGenerator>(typeof(CodeGeneratorFactory), Globals.CodeGeneratorDirectory);
		private PluginManager<IBuildGenerator> buildManager = new PluginManager<IBuildGenerator>( typeof( BuildGeneratorFactory ), Globals.BuildGeneratorDirectory );
		private PluginManager<ILoader> loadManager = new PluginManager<ILoader>( typeof( LoadGeneratorFactory ), Globals.LoadGeneratorDirectory );

        private IProject project = null;
        private List<ICodeGenerator> currentPlugins = new List<ICodeGenerator>();
		private List<IBuildGenerator> currentBuildPlugins = new List<IBuildGenerator>();
		private List<ILoader> currentLoadPlugins = new List<ILoader>();

        private delegate void Action( ICodeGenerator codeGenerator );

		private IBuildAggregator aggregator = new BuildAggregator();

        #endregion

		#region Properties

		public PluginManager<ICodeGenerator> PluginManager{	
			get { return pluginManager; }
		}

		public PluginManager<IBuildGenerator> BuildManager {
			get { return buildManager; }
		}

		public PluginManager<ILoader> LoadManager {
			get { return loadManager; }
		}

        public IBuildAggregator Agregator {
            get { return aggregator; }
        }

		#endregion

		#region Private

		private void GeneratePlugin<T>( ProjectPluginParameters parameters, PluginManager<T> manager, List<T> container ) where T : IPlugin {
			foreach( KeyValuePair<string, Dictionary<string, string>> key in parameters ) {
				T generator = manager.Get( key.Key );
				generator.SetParameters( key.Value );
				container.Add( generator );
			}
		}

		private void GeneratePlugins() {
			GeneratePlugin<ILoader>( project.LoadPluginParameters, loadManager, currentLoadPlugins );
			GeneratePlugin<ICodeGenerator>( project.PluginParameters, pluginManager, currentPlugins );
			GeneratePlugin<IBuildGenerator>( project.BuildPluginParameters, buildManager, currentBuildPlugins );
        }

        private void OperateParameters(Action f){
            foreach(ICodeGenerator codeGenerator in currentPlugins) {
                f(codeGenerator);
            }
        }

		private void CallLoaders() {
			foreach( ILoader loader in currentLoadPlugins ) {
				loader.Init( project, this );
				loader.Load();
			}
			DefaultEntities.CreateDefaultEntities( project );
			project.LoadEntityInfo();
		}

		private void CallBuilders() {
			foreach( IBuildGenerator buildGenerator in currentBuildPlugins) {
				buildGenerator.Init( project, this, aggregator.Informations );
				buildGenerator.Generate();
			}
		}
		
        #endregion

        #region Generator Events

        protected void OnInit() {
            OperateParameters(InitAction);
        }

        protected void OnBeforeGenerate() {
            OperateParameters(BeforeGenerateAction);
        }

        protected void Generate() {
            OperateParameters(GenerateAction);
        }

        protected void OnAfterGenerate() {
            OperateParameters(AfterGenerateAction);
        }

		#endregion

        #region Action Delegates

        private void InitAction( ICodeGenerator codeGenerator ) {
			codeGenerator.Init( project, this, aggregator );
        }

        private void BeforeGenerateAction( ICodeGenerator codeGenerator ) {
            codeGenerator.BeforeGenerate();
        }

        private void GenerateAction( ICodeGenerator codeGenerator ) {
            codeGenerator.Generate();
        }

        private void AfterGenerateAction( ICodeGenerator codeGenerator ) {
            codeGenerator.AfterGenerate();
        }

        #endregion

        #region Public

        public void Process() {
			Log.Info( "--- Generating Plugins ---" );
			GeneratePlugins();
			Log.Info( "--- Call Loaders ---" );
			CallLoaders();
            Log.Info("--- Processing project '{0}' ---", project.Name);
            OnInit();
            Log.Info("--- Before Generate ---");
            OnBeforeGenerate();
            Log.Info("--- Generate ---");
            Generate();
            Log.Info("--- After Generate ---");
            OnAfterGenerate();
			Log.Info( "--- Call Builders ---" );
			CallBuilders();
            Log.Info("--- Project '{0}' generation complete!", project.Name);
			
        }

        public IPlugin[] GetPlugins() {
            return pluginManager.Registered;
        }

		public IPlugin[] GetBuildPlugins() {
			return buildManager.Registered;
		}

        public IPlugin[] GetLoaderPlugins() {
			return loadManager.Registered;
		}

        #endregion

        #region Constructor

        public Generator( IProject project ) {
            this.project = project;
        }

        #endregion

        #region IDependencyManager Members

        private List<string> dependencies = new List<string>();
        private Dictionary<string, List<LocalizationToken>> localizationTokens = new Dictionary<string, List<LocalizationToken>>();

        public void RegisterArtifact(string name, object artifact)
        {
            project.Generic[name] = artifact;
        }

        public object GetArtifact(string name)
        {
            return project.Generic[name];
        }

        public bool HasArtifact(string name)
        {
            return project.Generic.ContainsKey(name);
        }

        public bool IsPluginRegistered(string name)
        {
            foreach (ILoader plugin in currentLoadPlugins) {
                if (plugin.Name == name) {
                    return true;
                }
            }

            foreach (IBuildGenerator plugin in currentBuildPlugins) {
                if (plugin.Name == name) {
                    return true;
                }
            }

            foreach (ICodeGenerator plugin in currentPlugins) {
                if (plugin.Name == name) {
                    return true;
                }
            }

            return false;
        }

        public void RegistDependency(string reference)
        {
            dependencies.Add(reference);
        }

        public bool HasDependency(string dep)
        {
            return dependencies.Find(delegate(string e) { return e == dep; }) != null;
        }

        /// <summary>
        /// Adds a key to be handled by the localization utilities
        /// </summary>
        /// <param name="category">The Token category</param>
        /// <param name="key">The key</param>
        /// <param name="defaultText">The sample content</param>
        public void Localize(string category, string key, string defaultText)
        {
            LocalizationToken token = new LocalizationToken(category, key, defaultText);

            if (!localizationTokens.ContainsKey(category)) {
                localizationTokens.Add(category, new List<LocalizationToken>());
            }

            localizationTokens[category].Add(token);
        }

        /// <summary>
        /// Retrieves all the registered localization tokens
        /// </summary>
        public Dictionary<string, List<LocalizationToken>> LocalizationTokens {
            get { return localizationTokens; }
        }

        #endregion
    
    };
}

