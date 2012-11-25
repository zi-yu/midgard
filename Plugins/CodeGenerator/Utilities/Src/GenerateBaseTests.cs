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
using Loki.Generic.Factories;
using System.IO;
using Loki.Exceptions;
using Loki.DataRepresentation;

namespace Odin.Plugin
{
    public class GenerateBaseTests : PluginBase, ICodeGenerator
    {
        #region ICodeGenerator Members

		public void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator )
        {
            Project = project;
			Aggregator = aggregator;
            Dependencies = dependencies;
			Aggregator.RegisterComponent( ComponentType.Tests.ToString() );
			Aggregator.RegisterComponentReference( ComponentType.Tests.ToString(),ComponentType.Core.ToString() );
			Aggregator.RegisterComponentReference( ComponentType.Tests.ToString(), ComponentType.DataAccessLayer.ToString() );
			Aggregator.RegisterAssembly( ComponentType.Tests.ToString(), "Loki.dll");
            Aggregator.RegisterAssembly( ComponentType.Tests.ToString(), "nunit.framework.dll" );
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
                baseDir = Path.Combine(baseDir,ComponentType.Tests.ToString());
                Directory.CreateDirectory(baseDir);

                foreach (Entity entity in Project.Model)
                {
                    if (entity is EntityClass)
                    {
                        string currDirectory = Path.Combine(baseDir, entity.Name);
                        Directory.CreateDirectory(currDirectory);
                        string output = new FileInfo(Path.Combine(currDirectory, entity.Name + "Tests.cs")).FullName;
                        TemplateClassTests(output, (EntityClass)entity);
                        Log.Info("Generated `{0}'", output);
						Aggregator.RegisterFile( ComponentType.Tests.ToString(), output );
                    }
                }
            }
            catch (LokiException ex)
            {
                Log.Error(ex);
            }
            catch (Exception ex)
            {
                Log.Error("Error Generating 'BaseClassTests.cs'");
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
            get { return "Odin.GenerateBaseTests"; }
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

        private void TemplateClassTests(string output, EntityClass entity)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            RelationDependencyFactory depFactory = new RelationDependencyFactory();
            RelationDependency dep = depFactory.create(entity, Project.Model);
            param.Add("entity", entity);
			param.Add("namespace", Project.Name + "." + ComponentType.Tests.ToString());
            param.Add("entityClass", (EntityClass)entity);
			param.Add( "coreName", Project.Name.ToString() + "." + ComponentType.Core.ToString() );
			param.Add( "dataName", Project.Name.ToString()  + "." + ComponentType.DataAccessLayer.ToString() );
            param.Add("dep", dep);
            param.Add("depsWhithDeclaration", DependenciesString(dep));
            param.Add("depsWhithoutDeclaration", DependenciesWhithoutDeclarationString(dep));

            string template = GetResource("BaseClassTestsTemplate.vtl");
            Templates.Generate(template, output, param);
        }

        #endregion

        #region private methods

        private string DependenciesString(RelationDependency deps)
        {
 	        StringBuilder toReturn = new StringBuilder();

            foreach (RelationDependency depRec in deps.Dependencies)
            {
                if (!depRec.IsReusable)
                {
                    toReturn.Append(depRec.Name);
                    toReturn.Append("Persistance _");
                    toReturn.Append(depRec.Name);
                    toReturn.Append("Persistance = ");
                    toReturn.Append(depRec.Name);
                    toReturn.Append("Persistance.GetSession(); \n\t\t\t");

                    toReturn.Append(depRec.Name);
                    toReturn.Append(" _" + depRec.Name);
                    toReturn.Append(" = _" + depRec.Name);
                    toReturn.Append("Persistance.Create(); \n\t\t\t");
                }

                toReturn.Append(DependenciesString(depRec));

                toReturn.Append("_" + deps.Name);
                toReturn.Append("." + depRec.Name);
                toReturn.Append(" = _" + depRec.Name + "; \n\t\t\t");
            }
            toReturn.Append("_" + deps.Name);
            toReturn.Append("Persistance.Update( _");
            toReturn.Append(deps.Name + " ); \n\n\t\t\t");

            return toReturn.ToString();
        }

        private string DependenciesWhithoutDeclarationString(RelationDependency deps)
        {
            StringBuilder toReturn = new StringBuilder();

            foreach (RelationDependency depRec in deps.Dependencies)
            {
                if (!depRec.IsReusable)
                {
                    toReturn.Append("_");
                    toReturn.Append(depRec.Name);
                    toReturn.Append("Persistance = ");
                    toReturn.Append(depRec.Name);
                    toReturn.Append("Persistance.GetSession(); \n\t\t\t");

                    toReturn.Append("_" + depRec.Name);
                    toReturn.Append(" = _" + depRec.Name);
                    toReturn.Append("Persistance.Create(); \n\t\t\t");
                }

                toReturn.Append(DependenciesWhithoutDeclarationString(depRec));

                toReturn.Append("_" + deps.Name);
                toReturn.Append("." + depRec.Name);
                toReturn.Append(" = _" + depRec.Name + "; \n\t\t\t");
            }
            toReturn.Append("_" + deps.Name);
            toReturn.Append("Persistance.Update ( _");
            toReturn.Append(deps.Name + " ); \n\n\t\t\t");

            return toReturn.ToString();
        }

        #endregion private methods
    }
}

