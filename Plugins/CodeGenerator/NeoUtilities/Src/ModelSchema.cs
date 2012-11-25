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

		public Dictionary<string, string> IntrinsicTypeMapping {
			get { return intrinsicTypeMapping; }
		}

		static ModelSchema()
		{
			intrinsicTypeMapping.Add("int", "INTEGER");
			intrinsicTypeMapping.Add("string", "VARCHAR");
            
		}

		#endregion

		#region ICodeGenerator Members

		private List<EntityField> foreign = new List<EntityField>();

		public void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator )
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
				string dir = ".";
				if (pluginParameters != null && pluginParameters.ContainsKey("OutputDir")) {
					dir = pluginParameters["OutputDir"];
				}
				string output = Path.Combine(Project.OutputPath, dir);
				output = Path.Combine(output, "NeoModelSchema.xml");

				GenerateNeoSchema(output);

				Log.Info("Generated `{0}'", output);

			} catch (LokiException ex){
				Log.Error(ex);
			} catch (Exception ex) {
				Log.Error("Error Generating `NeoModelSchema.xml'");
				Log.Error(ex);
			}
		}

		public void AfterGenerate()
		{
		}

		public override string Name	{
			get { return "Neo.ModelSchema"; }
		}

		public override Dictionary<string, string> DefaultParameters {
			get	{
				Dictionary<string, string> p = new Dictionary<string, string>();
				p.Add("OutputDir?", string.Empty);
				return p;
			}
		}

		#endregion

		#region Utilities

		private void GenerateNeoSchema(string output)
		{
			using (StreamWriter writer = new StreamWriter(output)) {
				writer.WriteLine("<?xml version=\"1.0\" standalone=\"no\"?>");
				writer.WriteLine("<database name=\"{0}\" package=\"{0}\" defaultIdMethod=\"native\">", Project.Name);
				foreach( Entity entity in Project.Model ) {
					if (entity is EntityClass) {
						WriteEntity(writer, (EntityClass)entity);
					}
				}
				writer.WriteLine("</database>");
			}

		}

		private void WriteEntity(StreamWriter writer, EntityClass entity)
		{
			ClearInformation();
			writer.WriteLine("\t<table name=\"{0}\" javaName=\"{0}\">", entity.Name);
			foreach (EntityField field in entity.Fields) {
				if (!field.InfoOnly) {
					WriteEntityField(writer, field);
				}
			}
			WriteInformation(writer, foreign, entity);
			writer.WriteLine("\t</table>");
		}

		private void WriteInformation(StreamWriter writer, List<EntityField> foreigns, EntityClass home)
		{
			foreach (EntityField field in foreigns) {
				writer.Write("\t\t<iforeign-key foreignTable=\"{0}\" ", field.Type.Name);
				writer.Write("name=\"{0}{1}\" ", home.Name, field.Type.Name);
				writer.WriteLine("onDelete=\"cascade\">");
				writer.Write("\t\t\t<ireference local=\"{0}\" ",field.Name);
				writer.WriteLine("foreign=\"Id\" />");
				writer.WriteLine("\t\t</iforeign-key>");
			}
		}

		private void ClearInformation()
		{
			foreign.Clear();
		}

		private void WriteEntityField(StreamWriter writer, EntityField field)
		{
			writer.Write("\t\t<column name=\"{0}\" ", field.Name);
			if (field.IsPrimaryKey) {
				writer.Write("primaryKey=\"true\" ");
			}
			if (field.IsRequired) {
				writer.Write("required=\"true\" ");
			}
			if (field.HasDefault) {
				writer.Write("default=\"{0}\" ", field.Default);
			}
			writer.Write("type=\"{0}\" ", GetEntityType(field));
			
			writer.WriteLine("/>");
		}

		private string GetEntityType(EntityField field)
		{
			if (field.Type.IsIntrinsic) {
				object obj = IntrinsicTypeMapping[field.Type.AccessInterface];
				if (obj == null) {
					throw new LokiException("Can't map intrinsic `{0}' to a Neo type", field.Type.Name);
				}
				return obj.ToString();
			}
			// é chave estrangeira
			foreign.Add(field);
			return IntrinsicTypeMapping["int"];
		}

		#endregion

	};

}