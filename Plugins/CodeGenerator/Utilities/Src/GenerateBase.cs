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
using Loki.Generic;
using System.IO;
using Loki.Exceptions;
using Loki.DataRepresentation;

namespace Odin.Plugin
{
    public class GenerateBase : PluginBase, ICodeGenerator {
        
        #region ICodeGenerator Members

		public void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator )
        {
            Project = project;
			Aggregator = aggregator;
            Dependencies = dependencies;
			Aggregator.RegisterComponent( ComponentType.Core.ToString() );
			Aggregator.RegisterAssembly( ComponentType.Core.ToString(), "Loki.dll" );
            Aggregator.RegisterGacAssembly(ComponentType.Core.ToString(), "System.Xml");
        }

        public void BeforeGenerate()
        {

        }

        public void Generate()
        {
            try
            {
                string dir = ".";
                if (pluginParameters != null && pluginParameters.ContainsKey("OutputDir"))
                {
                    dir = pluginParameters["OutputDir"];
                }
                string baseDir = Path.Combine(Project.OutputPath, dir);
                baseDir = Path.Combine(baseDir,ComponentType.Core.ToString());
                Directory.CreateDirectory(baseDir);

                foreach (Entity entity in Project.Model)
                {
                    string currDirectory = AppendDirectory(baseDir, "Entities");
                    currDirectory = AppendDirectory(currDirectory, entity.Name);
                    string output = new FileInfo(Path.Combine(currDirectory, entity.Name + ".cs")).FullName;

                    if (entity is EntityClass)
                    {
                        TemplateClassFields(output, (EntityClass)entity);
                        Aggregator.RegisterFile(ComponentType.Core.ToString(), output);
                        Log.Info("Generated `{0}'", output);

                        output = new FileInfo(Path.Combine(currDirectory, entity.Name + "ToChange.cs")).FullName;

						if (!File.Exists(output))
                        {
                            TemplateClassMethods(output, (EntityClass)entity);
                            Log.Info("Generated `{0}'", output);
                        }

						Aggregator.RegisterFile( ComponentType.Core.ToString(), output );
                    }
                    else
                    {
                        currDirectory = AppendDirectory(baseDir, "Interfaces");
                        output = new FileInfo(Path.Combine(currDirectory, entity.Name + ".cs")).FullName;

                        TemplateInterface(output, (EntityInterface)entity);
                        Aggregator.RegisterFile(ComponentType.Core.ToString(), output);
                        Log.Info("Generated `{0}'", output);
                    } 
                }
            }
            catch (LokiException ex)
            {
                Log.Error(ex);
            }
            catch (Exception ex)
            {
                Log.Error("Error Generating 'BaseClass.cs'");
                Log.Error(ex);
            }
        }

        public void AfterGenerate()
        {
           
        }

        #endregion

        #region IPlugin Members

        public override string Name
        {
            get { return "Odin.GenerateBase"; }
        }

        public override void SetParameters(Dictionary<string, string> parameters)
        {
            pluginParameters = parameters;
        }

        public override Dictionary<string, string> DefaultParameters
        {
            get
            {
                Dictionary<string, string> p = new Dictionary<string, string>();
                p.Add("OutputDir?", string.Empty);
                return p;
            }
        }

        #endregion

        #region NVelocity Implementation

        private void TemplateClassFields(string output, EntityClass entity)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("entity", entity);
			param.Add("namespace", Project.Name + "." + ComponentType.Core.ToString());
            param.Add("entityClass", (EntityClass)entity);

            string template = GetResource("BaseClassFieldsTemplate.vtl");
            Templates.Generate(template, output, param);
        }

        private void TemplateClassMethods(string output, EntityClass entity)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("entity", entity);
            param.Add("entityClass", (EntityClass)entity);
			param.Add("namespace", Project.Name + "." + ComponentType.Core.ToString());

            string template = GetResource("BaseClassMethodsTemplate.vtl");
            Templates.Generate(template, output, param);
        }

        private void TemplateInterface(string output, EntityInterface entity)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("entity", entity);
			param.Add("namespace", Project.Name + "." + ComponentType.Core.ToString());

            string template = GetResource("BaseInterfaceTemplate.vtl");
            Templates.Generate(template, output, param);
        }

        #endregion
    }
}
