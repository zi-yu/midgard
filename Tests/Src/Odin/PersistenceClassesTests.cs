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
	public class PersistenceClassesTester {

		#region Fields

		ICodeGenerator model = new Generator(null).PluginManager.Get("NHibernate.PersistenceClasses");

		#endregion

		#region Start Up

		public PersistenceClassesTester()
		{
			try {
				model.Init( Globals.SmsTestProject, null, new BuildAggregator() );
			} catch (Exception ex) {
				Log.Error(ex);
			}
		}

		#endregion

		#region Schema Tests

		[Test]
		public void TestSmsSchemaGeneration()
		{
			model.BeforeGenerate();
			/*using (StreamReader reader = new StreamReader(Path.Combine(Globals.SmsTestProject.OutputPath, "Model.hbm.xml"))) {
				string xml = reader.ReadToEnd();
				Assert.AreEqual(xml.IndexOf("$"), -1, "Unvalid command?");
				Assert.AreEqual(xml, smsModelOuput);
			  */
			
		}

		#endregion


	};
}
