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

using Loki.Interfaces;
using System.Collections.Generic;
using Loki.Exceptions;
using System;
using System.IO;

namespace Loki.Generic {

	public abstract class PluginBase : IPlugin, IComparable  {

        #region Fields

        private IProject project;
		private IBuildAggregator aggregator;
        private IDependencyManager dependencies;
        protected Dictionary<string, string> pluginParameters;

        #endregion

        #region Constructor

        public PluginBase() {}

        #endregion

        #region Properties

        public IProject Project {
            get { return project; }
            set { project = value; }
        }

		public IBuildAggregator Aggregator {
			get { return aggregator; }
			set { aggregator = value; }
		}

        public IDependencyManager Dependencies {
            get { return dependencies; }
            set { dependencies = value; }
        }

        public Dictionary<string, string> PluginParameters {
            get { return pluginParameters; }
            set { pluginParameters = value; }
        }

        #endregion

		#region IPlugin Members

        public virtual string Company {
			get { return "zi-yu.com"; }
		}

        public virtual string Name {
			get { return "Odin." + this.GetType().Name; }
		}

        public virtual string Description {
            get { return string.Empty; }
        }

        public virtual Dictionary<string, string> DefaultParameters {
            get { return new Dictionary<string, string>(); }
        }

        /// <summary>
        /// Plugin Parameters
        /// </summary>
		public virtual void SetParameters(Dictionary<string, string> parameters){
			pluginParameters = parameters;
		}

        /// <summary>
        /// Gets the plugin dependencies
        /// </summary>
        /// <returns>The dependency list</returns>
        public virtual List<IPlugin> GetDependencies()
        {
            return new List<IPlugin>();
        }

		#endregion

        #region Utilities

		protected string GetResource(string file)
		{
			return Globals.GetResource(Project, this, file);
		}

        protected string GetParam( string key ) {

            if(pluginParameters == null || !pluginParameters.ContainsKey(key)) {
                throw new LokiException("Requested param '{0}' not available for plugin '{1}'", key, Name);
            }

            return pluginParameters[key];
        }

		protected string GetParam(string key, string defaultValue) {

			if (pluginParameters == null || !pluginParameters.ContainsKey(key))	{
				return defaultValue;
			}

			return pluginParameters[key];
		}

        /// <summary>
        /// Gets the general parameters.
        /// </summary>
        /// <returns>The parameter collection</returns>
        protected Dictionary<string, object> GetGeneralParameters() {
            Dictionary<string, object> param = new Dictionary<string, object>();
            Dictionary<string, string> defaultParam = DefaultParameters;

            foreach (KeyValuePair<string, string> var in pluginParameters) {
                if (!defaultParam.ContainsKey(var.Key)) {
                    param.Add(var.Key, var.Value);
                }
            }

            return param;
        }

        /// <summary>
        /// Combines paths and insures that the directories are created
        /// </summary>
        /// <param name="path1">root path</param>
        /// <param name="path2">path to append</param>
        /// <returns>The output path</returns>
        public string AppendDirectory(string path1, string path2)
        {
            string output = Path.Combine(path1, path2);
            if (!Directory.Exists(output)) {
                Directory.CreateDirectory(output);
            }
            return output;
        }

        #endregion

        #region IComparable Members

        public int CompareTo(object obj)
        {
            PluginBase other = obj as PluginBase;
            if (other == null) {
                return -1;
            }

            return other.Name.CompareTo(Name);
        }

        #endregion

    };

}