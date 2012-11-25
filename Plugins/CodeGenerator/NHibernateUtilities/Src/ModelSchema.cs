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

	public class ModelSchema : PluginBase, ICodeGenerator {

		#region Static

		private static Dictionary<string, string> intrinsicTypeMapping = new Dictionary<string, string>();

		public static Dictionary<string, string> IntrinsicTypeMapping {
			get { return intrinsicTypeMapping; }
		}

		public static List<string> InverseList = new List<string>();

		static ModelSchema()
		{
			intrinsicTypeMapping.Add("int", "Int32");
			intrinsicTypeMapping.Add("string", "String");
			intrinsicTypeMapping.Add("bool", "Boolean");
            intrinsicTypeMapping.Add("DateTime", "DateTime");
            intrinsicTypeMapping.Add("double", "Double");
		}

		#endregion

		#region ICodeGenerator Members

		public void Init(IProject project, IDependencyManager dependencies, IBuildAggregator aggregator)
		{
			Project = project;
			Aggregator = aggregator;
            Dependencies = dependencies;
		}

		public void Generate()
		{
		}

		public void BeforeGenerate()
		{
			try {
				string dir = GetParam("OutputPath", Path.Combine(ComponentType.WebUserInterface.ToString(),"bin"));
				string output = Path.Combine(Project.OutputPath, dir);
				Directory.CreateDirectory(output);

				output = new FileInfo(Path.Combine(output, "Model.hbm.xml")).FullName;

				//GenerateModel(output);
				TemplateModel(output);

				Log.Info("Generated `{0}'", output);

			} catch (LokiException ex){
				Log.Error(ex);
			} catch (Exception ex) {
				Log.Error("Error Generating `Model.hbm.xml'");
				Log.Error(ex);
			}
		}

		public void AfterGenerate()
		{
		}

		public override string Name	{
			get { return "NHibernate.ModelSchema"; }
		}

		public override Dictionary<string, string> DefaultParameters {
			get	{
				Dictionary<string, string> p = new Dictionary<string, string>();
				p.Add("OutputDir?", string.Empty);
				p.Add("Schema?", string.Empty);
				return p;
			}
		}

		#endregion

		#region NVelocity Callable Methods

		public string GetRootName(object obj)
		{
			EntityClass entity = obj as EntityClass;
			if (entity != null) {
				while (entity.HasParent) {
					entity = entity.Parent as EntityClass;
				}
				return entity.Name;
			}
			return "[ERROR:NotEntityClass]";
		}

		public string Get(object type)
		{
			if (type == null) {
				type = "NULL";
			}
			try {
				if( IntrinsicTypeMapping.ContainsKey(type.ToString())) {
					return IntrinsicTypeMapping[type.ToString()];
				}
				return "Serializable";
			} catch (Exception ex) {
				Log.Error(type + " :: " + ex);
				return "[ERROR:" + type + "]";
			}
		}

		public string Generate(object type)
		{
			if (type == null) {
				return "-1";
			}
			return type.GetHashCode().ToString();
		}

        public string GetLength( object type )
        {
            EntityField field = type as EntityField;
            if (field == null) {
                return string.Empty;
            }

            if (field.Type.AccessInterface == "string") {
                return string.Format("length=\"{0}\"", field.MaxSize);
            }

            return string.Empty;
        }

		public string CheckInverse( object type, object entity ) 
		{
			if( InverseList.Contains( string.Format("{0}{1}",type,entity) ) ) {
				return "true";
			}
			if( InverseList.Contains( string.Format("{1}{0}",type,entity) ) ) {
				return "true";
			}

			InverseList.Add( string.Format( "{1}{0}", type, entity ) );
			return "false";
		}

		public string InverseKey( object type, object entity ) {
			string k1 = string.Format( "{0}{1}", type, entity );
			string k2 = string.Format( "{1}{0}", type, entity );

			if( InverseList.Contains( k1 ) ) {
				return k1;
			}
			
			return k2;
		}

		#endregion

		#region NVelocity Implementation

		private void TemplateModel(string output)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();
			param.Add("entities", Project.Model);
			param.Add("pluginName", Name);
            param.Add("projectName", Project.Name);
			param.Add("namespace", Project.Name+"."+ComponentType.Core.ToString());
			param.Add("hTypes", this);
			param.Add("discriminator", this);
			param.Add("root", this);
            param.Add("length", this);
            param.Add("schema", GetParam("Schema", "dbo"));

			string template = GetResource("Model.hbm.xml.vtl");
			Log.Debug(template);

			Templates.Generate(template, output, param);
		}

		#endregion

	};

}