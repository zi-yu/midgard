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

namespace Odin.Plugin {

	public class PersistanceClasses : PluginBase, ICodeGenerator {

		#region ICodeGenerator Members

		public void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) {
			Project = project;
			Aggregator = aggregator;
            Dependencies = dependencies;
			Aggregator.RegisterComponent( ComponentType.DataAccessLayer.ToString() );
			Aggregator.RegisterComponentReference( ComponentType.DataAccessLayer.ToString(), ComponentType.Core.ToString() );
			Aggregator.RegisterAssembly( ComponentType.DataAccessLayer.ToString(), "NHibernate.dll" );
			Aggregator.RegisterAssembly( ComponentType.DataAccessLayer.ToString(), "Loki.dll" );
			Aggregator.RegisterGacAssembly( ComponentType.DataAccessLayer.ToString(), "System.Data" );
			Aggregator.RegisterGacAssembly( ComponentType.DataAccessLayer.ToString(), "System.Xml" );
			Aggregator.RegisterGacAssembly( ComponentType.DataAccessLayer.ToString(), "System.Web" );
		}

		public void BeforeGenerate() {
		}

		public void Generate() {
			try {
				string baseOutPut = Path.Combine(Project.OutputPath, ComponentType.DataAccessLayer.ToString());
				Directory.CreateDirectory(baseOutPut);
				foreach( Entity e in Project.Model ) {
					if (e.Persistable){
						string outPut = Path.Combine(baseOutPut, e.Name + "Persistance.cs");
						TemplateModel(outPut, e);

						Aggregator.RegisterFile( ComponentType.DataAccessLayer.ToString(), outPut );
						Log.Info("Generated `{0}`", outPut);
					}
				}
				string output = Path.Combine(baseOutPut, "NHibernateUtilities.cs");
				UtilitiesTemplateModel(output);

				Aggregator.RegisterFile( ComponentType.DataAccessLayer.ToString(), output );
				Log.Info("Generated `{0}`", output);

				output = Path.Combine(baseOutPut, "Persistance.cs");
				PersistanceTemplateModel(output);

				Aggregator.RegisterFile(ComponentType.DataAccessLayer.ToString(), output);
				Log.Info("Generated `{0}`", output);

			} catch( LokiException ex ) {
				Log.Error( ex );
			} catch( Exception ex ) {
				Log.Error( ex );
			}
		}

		public void AfterGenerate() {
		}

		public override string Name {
			get { return "NHibernate.PersistanceClasses"; }
		}

		public override Dictionary<string, string> DefaultParameters {
			get {
				Dictionary<string, string> p = new Dictionary<string, string>();
				p.Add( "OutputDir?", string.Empty );
				return p;
			}
		}

		#endregion

		#region NVelocity Implementation

		private void TemplateModel( string output, Entity e ) 
		{
			Dictionary<string, object> param = new Dictionary<string, object>();
			param.Add( "entity", e );
			param.Add( "namespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString() );
			param.Add("coreNamespace", Project.Name + "." + ComponentType.Core.ToString());
			param.Add( "allFields", GetFields( (EntityClass)e ) );

			string template = GetResource( "PersistanceClasses.vtl" );
			Templates.Generate( template, output, param );
		}

		private void UtilitiesTemplateModel(string output)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();
			param.Add("namespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString());
			param.Add("coreNamespace", Project.Name + "." + ComponentType.Core.ToString());
			param.Add("configFile", Project.Name+".cfg.xml");

			string template = GetResource("NHibernateUtility.vtl");
			Templates.Generate(template, output, param);
		}

		private void PersistanceTemplateModel(string output)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();
			param.Add("namespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString());
			param.Add("coreNamespace", Project.Name + "." + ComponentType.Core.ToString());

			string template = GetResource("Persistance.vtl");
			Templates.Generate(template, output, param);
		}

		private List<EntityField> GetFields( EntityClass e ) {
			List<EntityField> fields = new List<EntityField>();

			do {
				foreach( EntityField field in e.Fields ) {
					if( !e.RootEntity && field.Name == "id" ) {
						continue;
					}
					fields.Add( field );
				}
				
				e = (EntityClass) e.Parent;
			} while( e != null );

			return fields;
		}

		#endregion
	};

}
