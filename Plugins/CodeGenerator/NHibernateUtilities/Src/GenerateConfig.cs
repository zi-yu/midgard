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

	public class GenerateConfig : PluginBase, ICodeGenerator {

		#region Static Members

		public static Dictionary<string, List<Property>> BDMapping = new Dictionary<string, List<Property>>();
		public static Dictionary<string, string> ConnectionStrings = new Dictionary<string, string>();

		static GenerateConfig()
		{
			BDMapping.Add(Database.MsSql2000.ToString(), new List<Property>() );
			BDMapping[Database.MsSql2000.ToString()].Add(new Property("connection.driver_class", "NHibernate.Driver.SqlClientDriver" ));
			BDMapping[Database.MsSql2000.ToString()].Add(new Property("dialect", "NHibernate.Dialect.MsSql2000Dialect" ));

			BDMapping.Add(Database.DB2.ToString(), new List<Property>());
			BDMapping[Database.DB2.ToString()].Add(new Property("connection.driver_class", "NHibernate.Driver.DB2Driver"));
			BDMapping[Database.DB2.ToString()].Add(new Property("dialect", "NHibernate.Dialect.DB2Dialect"));

			BDMapping.Add(Database.Firebird.ToString(), new List<Property>());
			BDMapping[Database.Firebird.ToString()].Add(new Property("connection.driver_class", "NHibernate.Driver.FirebirdDriver"));
			BDMapping[Database.Firebird.ToString()].Add(new Property("dialect", "NHibernate.Dialect.FirebirdDialect"));

			BDMapping.Add(Database.MySQL.ToString(), new List<Property>());
			BDMapping[Database.MySQL.ToString()].Add(new Property("connection.driver_class", "NHibernate.Driver.MySqlDataDriver"));
			BDMapping[Database.MySQL.ToString()].Add(new Property("dialect", "NHibernate.Dialect.MySQLDialect"));

			BDMapping.Add(Database.Oracle.ToString(), new List<Property>());
			BDMapping[Database.Oracle.ToString()].Add(new Property("connection.driver_class", "NHibernate.Driver.OracleDriver"));
			BDMapping[Database.Oracle.ToString()].Add(new Property("dialect", "NHibernate.Dialect.OracleDialect"));

			BDMapping.Add(Database.PostgreSQL.ToString(), new List<Property>());
			BDMapping[Database.PostgreSQL.ToString()].Add(new Property("connection.driver_class", "NHibernate.Driver.NpgsqlDriver"));
			BDMapping[Database.PostgreSQL.ToString()].Add(new Property("dialect", "NHibernate.Dialect.PostgreSQLDialect"));

			BDMapping.Add(Database.Sybase.ToString(), new List<Property>());
			BDMapping[Database.Sybase.ToString()].Add(new Property("connection.driver_class", "NHibernate.Driver.SybaseDriver"));
			BDMapping[Database.Sybase.ToString()].Add(new Property("dialect", "NHibernate.Dialect.SybaseDialect"));

			ConnectionStrings[Database.MsSql2000.ToString()] =  
"Data Source=SERVER;Initial Catalog=DATABASE;User Id=USER;Password=PASSWORD;" ;
			ConnectionStrings[Database.DB2.ToString()] = "Provider=DB2OLEDB;Network Transport Library=TCPIP;Network Address=XXX.XXX.XXX.XXX;Initial Catalog=MyCtlg;Package Collection=MyPkgCol;Default Schema=Schema;User ID=MyUser;Password=MyPW";
			ConnectionStrings[Database.Firebird.ToString()] = "User=SYSDBA;Password=masterkey;Database=SampleDatabase.fdb;DataSource=localhost;Port=3050;Dialect=3;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;MinPoolSize=0;MaxPoolSize=50;Packet Size=8192;ServerType=0";
			ConnectionStrings[Database.MySQL.ToString()] = "Server=Server;Database=Test;Uid=UserName;Pwd=asdasd;";
			ConnectionStrings[Database.Oracle.ToString()] = "Data Source=MyOracleDB;Integrated Security=yes;";
			ConnectionStrings[Database.PostgreSQL.ToString()] = "User ID=root; Password=pwd; Host=localhost; Port=5432; Database=testdb;Pooling=true; Min Pool Size=0; Max Pool Size=100; Connection Lifetime=0";
			ConnectionStrings[Database.Sybase.ToString()] = "Data Source='myASEserver';Port=5000;Database='myDBname';UID='username';PWD='password';";
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
				string dir = Path.Combine(ComponentType.WebUserInterface.ToString(),"bin");
				string output = Path.Combine(Project.OutputPath, dir);
				Directory.CreateDirectory(output);

				output = new FileInfo(Path.Combine(output, Project.Name+".cfg.xml")).FullName;
				TemplateModel(output);

				Log.Info("Generated `{0}'", output);

			} catch (LokiException ex){
				Log.Error(ex);
			} catch (Exception ex) {
				Log.Error("Error Generating `{0}.cfg.xml'", Project.Name);
				Log.Error(ex);
			}
		}

		public void AfterGenerate()
		{
		}

		public override string Name	{
			get { return "NHibernate.GenerateConfig"; }
		}

		public override Dictionary<string, string> DefaultParameters {
			get	{
				Dictionary<string, string> p = new Dictionary<string, string>();
				p.Add("OutputDir?", string.Empty);
				p.Add("ConnectionString", string.Empty);
				p.Add("Isolation", "ReadUncommitted");
				p.Add("ShowSQL", "true");
				p.Add("UseOuterJoin", "true");
				p.Add("QuerySubstitutions", "true 1, false 0, yes 'Y', no 'N'");

				return p;
			}
		}

		#endregion

		#region NVelocity Implementation

		private void TemplateModel(string output)
		{
			Dictionary<string, object> param = new Dictionary<string, object>();
			param.Add("pluginName", Name);
			param.Add("projectName", Project.Name);
			param.Add("target", BD);
			param.Add("mappingFile", "Model.hbm.xml");
			param.Add("properties", GetProperties());
			param.Add("connectionStringSample", ConnectionStrings[BD]);

			string template = GetResource("Configuration.cfg.xml.vtl");
			Templates.Generate(template, output, param);
		}

		private List<Property> GetProperties()
		{
			List<Property> list = new List<Property>();

			list.Add(new Property("connection.provider", "NHibernate.Connection.DriverConnectionProvider"));
			AddDatabaseProperties(list);

			list.Add( new Property("connection.connection_string", GetParam("ConnectionString", "No Connection String Found!") ));
			list.Add(new Property("connection.isolation", GetParam("Isolation", "ReadUncommitted")));
			list.Add(new Property("show_sql", GetParam("ShowSQL", "true")));
			list.Add(new Property("use_outer_join", GetParam("UseOuterJoin", "true")));
			list.Add(new Property("query.substitutions", GetParam("QuerySubstitutions", "true 1, false 0, yes 'Y', no 'N'")));

			return list;
		}

		#endregion

		#region BD Mapping Utilities

		private void AddDatabaseProperties(List<Property> list)
		{
			foreach (Property prop in BDMapping[BD]) {
				list.Add(prop);
			}
		}

		private string BD {
			get { return GetParam("Database", Database.MsSql2000.ToString()); }
		}

		#endregion

	};

	public class Property {
		public string key = "Empty";
		public string val = "Empty";
		public Property(string k, string v) { Key = k; Value = v; }
		public string Key {
			get { return key; }
			set { key = value; }
		}
		public string Value	{
			get { return val; }
			set { val = value; }
		}
	};

}
