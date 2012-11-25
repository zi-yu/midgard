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
using System.IO;
using System.Reflection;
using DesignPatterns;
using Loki.Generic;
using Loki.Interfaces;
using Loki.Exceptions;

namespace Odin.Core {

	public class PluginManager<T> where T : IPlugin  {

		#region Constructor

		private FactoryContainer factoryContainer = null;

		#endregion

		#region Privates

		private Assembly[] LoadAssemblies(string path) {
			Log.Info("Loading assemblies from '{0}'", Path.GetFullPath(path));

            List<Assembly> list = new List<Assembly>();

            try {
				string[] files = Directory.GetFiles(path, "*.dll");
				for( int i = 0; i < files.Length; ++i) {
                    try {
                        Assembly asm = Assembly.LoadFrom(files[i]);
                        list.Add(asm);
                        Log.Info("\tAssembly `{0}' Loaded", Path.GetFileName(files[i]));
                    } catch(Exception ex) {
                        Log.Info("\tLoading Assembly `{0}' failed!", Path.GetFileName(files[i]));
                        Log.Error(ex);
                    }
				}

    			Log.Info("\t** {0} Assemblies Loaded!", list.Count);

			} catch( IOException e ) {
				Log.Fatal(e);
			}

			return list.ToArray();
		}

    	#endregion

        #region Properties

        public T[] Registered {
            get {
                List<T> plugins = new List<T>();

                foreach(IFactory f in factoryContainer.Values) {
                    plugins.Add((T)f.create(null));
                }

                return plugins.ToArray();
            }
        }

		public FactoryContainer Factories {
			get { return factoryContainer; }
		}

        #endregion

        #region Public

        public T Get( string name ) {
            if( !factoryContainer.ContainsKey(name) ) {
                throw new LokiException("Plugin '{0}' does not exist in 'PluginManager<{1}>'", name, typeof(T).Name);
            }
            return (T)((IFactory)factoryContainer[name]).create(null);
        }

        #endregion

        #region Constructor
        
        public PluginManager(Type factoryType, string pluginPath) {
            Log.Debug("PluginManager<{0}> Initialized", factoryType.Name);
            try {  
                factoryContainer = new FactoryContainer(factoryType, LoadAssemblies(pluginPath));
                Log.Info("\t** Loaded `{0}' {1} plugins", factoryContainer.Count, factoryType.Name);
            } catch(ReflectionTypeLoadException ex) {
                ShowDomainInfo();
                foreach(Exception e in ex.LoaderExceptions) {
                    Log.Fatal(e);
                }
                throw;
            }
		}

        public static void ShowDomainInfo() {
            Log.Warn("=== AppDomain Information ===");
            Log.Warn("FriendlyName: {0}",AppDomain.CurrentDomain.FriendlyName);
            Log.Warn("BaseDirectory: {0}", AppDomain.CurrentDomain.BaseDirectory);
            Log.Warn("DynamicDirectory: {0}", AppDomain.CurrentDomain.DynamicDirectory);
            Log.Warn("RelativeSearchPath: {0}", AppDomain.CurrentDomain.RelativeSearchPath);
            Log.Warn("=== AppDomain Information - Setup Information ===");
            Log.Warn("ApplicationBase: {0}", AppDomain.CurrentDomain.SetupInformation.ApplicationBase);
            Log.Warn("ConfigurationFile: {0}", AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            Log.Warn("PrivateBinPath: {0}", AppDomain.CurrentDomain.SetupInformation.PrivateBinPath);
            Log.Warn("PrivateBinPathProbe: {0}", AppDomain.CurrentDomain.SetupInformation.PrivateBinPathProbe);
        }

		#endregion

    };

}