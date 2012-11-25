using System;
using System.Collections;
using DesignPatterns;
using System.Collections.Generic;
using System.Text;
using Loki.Generic;
using Loki.Interfaces;
using Odin.Core;
using Loki.DataRepresentation.Loaders;
using NUnit.Framework;
using System.IO;
using Odin.Plugin;
using Loki.DataRepresentation;

namespace Midgard.Tests {

	[TestFixture()]
	public class NHibernateConfigTester {

		#region Fields

		ICodeGenerator config = new Generator(null).PluginManager.Get("NHibernate.GenerateConfig");

		#endregion

		#region Start Up

		public NHibernateConfigTester()
		{
			try {
				config.Init( Globals.SmsTestProject, null, new BuildAggregator() );
			} catch (Exception ex) {
				Log.Error(ex);
			}
		}

		#endregion

		#region Config Tests

		[Test]
		public void TestDatabaseProviders()
		{
			foreach (string val in Enum.GetNames(typeof(Database))) {
				Assert.IsTrue( GenerateConfig.BDMapping.ContainsKey(val), 
						"Provider `" + val + "` not supported"
					);
			}
		}

		[Test]
		public void TestSmsConfigGeneration()
		{
			config.BeforeGenerate();
			/*using (StreamReader reader = new StreamReader(Path.Combine(Globals.SmsTestProject.OutputPath, "Model.hbm.xml"))) {
				string xml = reader.ReadToEnd();
				Assert.AreEqual(xml.IndexOf("$"), -1, "Unvalid command?");
				Assert.AreEqual(xml, smsModelOuput);
			}*/
		}

		#endregion

	};
}
