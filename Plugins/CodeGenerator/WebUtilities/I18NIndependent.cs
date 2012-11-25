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
using Loki.Exceptions;
using Loki.DataRepresentation;
using WebUtilities;

namespace Odin.Plugin {

	public class I18NIndependent : WebPluginBase {

		#region ICodeGenerator Members

		public override void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) {
			base.Init( project, dependencies, aggregator );
            WebUtilities.Dependencies.Instance.RegistDependency("I18N.Independent");
            dependencies.RegistDependency("I18N.Independent");

            AddModelToLocalization();
		}

		public override void Generate() {
			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables["namespace"] = Namespace;
			variables["controlsNamespace"] = ControlsNamespace;
			variables["modulesNamespace"] = ModulesNamespace;
            variables["entities"] = Project.Model;

            List<string> list = new List<string>();
            if ( PluginParameters != null && PluginParameters.Count > 0 ) {
                foreach ( string lang in PluginParameters.Values ) {
                    list.Add( lang );
                }
            } else {
                list.Add( "en" );
            }
            variables["languages"] = list;
            variables["defaultLanguage"] = GetParam("default", "en");
            variables["languageCSV"] = GetListCSV(list);
            variables["localizationNamespace"] = Project.Name + "." + ComponentType.WebUserInterface.ToString();

			GenerateResourceLabel( variables );
			GenerateLanguageModule( variables );
			GenerateLanguageManager( variables );
            GenerateResourcesFile( variables );
            GenerateLocalizationFiles(variables);
            GenerateLanguageResources(variables);
            CopyTemplates();
		}

		public override string Name {
			get { return "Web.I18N.Independent"; }
		}

        public override string Description{
            get {
                return "I18N Implementation independent from the .NET Resources Framework";
            }
        }

        public override Dictionary<string, string> DefaultParameters {
            get {
                Dictionary<string, string> args = new Dictionary<string, string>();
                args.Add("lang*", "");
                args.Add("default", "en");
                return args;
            }
        }

		#endregion

		#region Generate

		private void GenerateResourceLabel( Dictionary<string, object> variables ) {
			string output = GetControlsOutputDir( "Generic", "Label.cs" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Templates.Generate( GetResource( "Label.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateLanguageModule( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "IndependentLanguageModule.cs", "Modules" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Templates.Generate( GetResource( "LanguageModule.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateLanguageManager( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "LanguageManager.cs", "Localization" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Templates.Generate( GetResource( "LanguageManager.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

        private void GenerateResourcesFile( Dictionary<string, object> variables )
        {
            string code = GetRelativeOutputDir("Resources.cs", "Localization");

            Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), code, VSFileType.Compile );

            Log.Info( "Generated '{0}'", code );
        }

        private void GenerateLocalizationFiles(Dictionary<string, object> variables)
        {
            foreach (string category in Dependencies.LocalizationTokens.Keys) {
                string output = GetRelativeOutputDir( category + ".xml", "Localization/Resources");
                variables["tokens"] = Dependencies.LocalizationTokens[category];

                Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output, VSFileType.None);

                Templates.Generate(GetResource("Resources.xml.vtl"), output, variables);

                Log.Info("Generated '{0}'", output);
            }
        }

        private void GenerateLanguageResources(Dictionary<string, object> variables)
        {
            string output = GetRelativeOutputDir("LanguageResources.cs", "Localization");

            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output, VSFileType.Compile);

            Templates.Generate(GetResource("LanguageResources.cs.vtl"), output, variables);

            Log.Info("Generated '{0}'", output);
        }

        private void CopyTemplates()
        {
            string output = GetRelativeOutputDir("LocalizationTemplate.cs.vtl", "Localization");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output, VSFileType.None);
            if (File.Exists(output)) {
                return;
            }
            File.Copy(GetResource("LocalizationTemplate.cs.vtl"), output);
        }

		#endregion

        #region Utilities

        private string GetListCSV(List<string> list)
        {
            StringWriter writer = new StringWriter();
            bool first = true;
            foreach (string lang in list) {
                if (first) {
                    first = false;
                } else {
                    writer.Write(", ");
                }
                writer.Write("\"{0}\"", lang);
            }
            return writer.ToString();
        }

        private void AddModelToLocalization()
        {
            foreach (Entity anything in Project.Model) {
                EntityClass entity = anything as EntityClass;
                if (entity != null) {
                    Dependencies.Localize("Model", entity.Name, entity.Name);
                    foreach (EntityField field in entity.Fields) {
                        Dependencies.Localize("Model", entity.Name + field.PropertyName, field.PropertyName);
                    }
                }
            }
        }

        #endregion Utilities

    };

}
