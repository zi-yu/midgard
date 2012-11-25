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
using Loki.Generic;
using Loki.Interfaces;
using System.IO;
using System.Collections.Generic;

namespace Odin.Plugin {

    public class Exceptions : PluginBase, ICodeGenerator {

        #region ICodeGenerator Members

        public void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) {
            Project = project;
			Aggregator = aggregator;
            Dependencies = dependencies;
            SetExceptions();
        }

        public void Generate() 
        {
        }

        public void BeforeGenerate() 
        {
        }

        public void AfterGenerate() 
        {
            GenerateExceptions();
        }

        #endregion

        #region Generation

        private void GenerateExceptions()
        {
            foreach (BuildInformation info in Aggregator.Informations.Values) {
                ExceptionInformation exception;
                if (Infos.ContainsKey(info.Name)) {
                    exception = Infos[info.Name];
                    Generate(info.Name, exception);

                    if (info.Name == ComponentType.Core.ToString()) {
                        exception = Infos["BaseException"];
                        Generate(info.Name, exception);
                    }
                }
                
            }
        }

        private void Generate(string component, ExceptionInformation exception)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();

            param.Add("name", exception.Label);
            if (exception.Base) {
                param.Add("parent", "Exception");
            } else {
                param.Add("parent", Project.Name + "." + ComponentType.Core.ToString() + "." + Project.Name + "Exception");
            }
            param.Add("namespace", Project.Name + "." + component);

            string output = AppendDirectory(Project.OutputPath, component);
            output = AppendDirectory(output, "Exceptions");
            output = Path.Combine(output, exception.Label + ".cs");

            Aggregator.RegisterFile(component, output);

            string template = GetResource("ExceptionClass.cs");
            Templates.Generate(template, output, param);
        }

        #endregion

        #region ExceptionInformation class

        private class ExceptionInformation {
            public string Label;
            public bool Base;
            public ExceptionInformation(string l, bool b)
            {
                Label = l;
                Base = b;
            }
        };

        #endregion ExceptionInformation class

        #region Static information

        private Dictionary<string, ExceptionInformation> Infos = new Dictionary<string, ExceptionInformation>();

        public void SetExceptions()
        {
            Infos.Add("BaseException", new ExceptionInformation(Project.Name+"Exception", true));
            Infos.Add(ComponentType.Core.ToString(), new ExceptionInformation("EntityException", false));

            Infos.Add(ComponentType.DataAccessLayer.ToString(), new ExceptionInformation("DALException", false));
            Infos.Add(ComponentType.WebUserInterface.ToString(), new ExceptionInformation("UIException", false));
        }

        #endregion Static information
    }

}