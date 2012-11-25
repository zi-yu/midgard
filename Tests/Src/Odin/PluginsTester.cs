using System;
using System.Collections;
using DesignPatterns;
using System.Collections.Generic;
using System.Text;
using Loki.Generic;
using Loki.Interfaces;
using Odin.Core;
using NUnit.Framework;

namespace Midgard.Tests {

	[TestFixture()]
	public class PluginsTester	{

		#region General Plugin Tests

		[Test]
		public void PluginNames()
		{
			Generator gen = new Generator(null);

			IDictionaryEnumerator it = gen.PluginManager.Factories.GetEnumerator();
			while (it.MoveNext()) {
				string factoryName = it.Key.ToString();
				string pluginName = ((IPlugin)((IFactory)it.Value).create(null)).Name;
				Assert.AreEqual(factoryName, pluginName, "Plugin Key/Name out of sync");
			}
		}

		#endregion


	};
}
