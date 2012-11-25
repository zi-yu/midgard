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
using Loki.Interfaces;
using System.IO;
using System.Collections.Generic;
using Loki.Exceptions;
using Loki.Generic;
using Loki.DataRepresentation;
using Loki.DataRepresentation.Loaders;
using System.Xml;

namespace Odin.Plugin {

	public class XmlDomLoader : PluginBase, ILoader {

		#region IPlugin

		public override Dictionary<string, string> DefaultParameters {
			get { 
                Dictionary<string, string> param = new Dictionary<string, string>();

                param.Add("XmlFile", "");
                param.Add("ArtifactName", "");

                return param;
            }
		}

        public override string Description {
            get {
                return "Loads a XmlDocument into a given Artifact Name";
            }
        }

		#endregion

		#region ILoadGenerator Members

		public void Init( IProject project, IDependencyManager dependencies ) {
            Project = project;
            Dependencies = Dependencies;
		}

		public void Load() {
            XmlDocument doc = new XmlDocument();
            doc.Load(GetParam("XmlFile"));
            Dependencies.RegisterArtifact(GetParam("ArtifactName", "XmlDocument"), doc);
		}

		#endregion

		
	};

}
