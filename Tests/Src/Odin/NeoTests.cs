using System;
using System.Collections;
using DesignPatterns;
using System.Collections.Generic;
using System.Text;
using Loki.Generic;
using Loki.Interfaces;
using Odin.Core;
using NUnit.Framework;
using System.IO;

namespace Midgard.Tests {

//	[TestFixture()]
	public class NeoTester {

		#region Fields

		ICodeGenerator neo = new Generator(null).PluginManager.Get("Neo.ModelSchema");

		#endregion

		#region Start Up

		public NeoTester()
		{
			neo.Init( Globals.SmsTestProject, null, new BuildAggregator() );
		}

		#endregion

		#region Generic Tests

		[Test]
		public void TestIntrinsicSupport()
		{
			// Ir ao sítio onde se encontram as factories de IntrinsicEntity
			// e ver se estão todas mapeadas para o neo
			Assert.Fail("TODO");
		}

		#endregion

		#region Schema Tests

		private string smsModelOuput = @"<?xml version='1.0' standalone='no'?>
<database name='Sms' package='Sms' defaultIdMethod='native'>
	<table name='Category' javaName='Category'>
		<column name='Id' primaryKey='true' type='INTEGER' />
		<column name='Description' required='true' type='VARCHAR' size='500' />
	</table>
	<table name='Message' javaName='Message'>
		<column name='Id' primaryKey='true' type='INTEGER' />
		<column name='Category' required='true' type='INTEGER' />
		<column name='Text' required='true' type='VARCHAR' size='500' />
		<iforeign-key foreignTable='Category' name='MessageCategory' onDelete='cascade'>
			<ireference local='Category' foreign='Id' />
		</iforeign-key>
	</table>
</database>
".Replace("'", "\"");

		[Test]
		public void TestSmsSchemaGeneration()
		{
			neo.BeforeGenerate();
			using (StreamReader reader = new StreamReader(Path.Combine(Globals.SmsTestProject.OutputPath, "NeoModelSchema.xml")))	{
				string xml = reader.ReadToEnd();
				//Assert.AreEqual(xml, smsModelOuput);
			}
		}

		#endregion


	};
}
