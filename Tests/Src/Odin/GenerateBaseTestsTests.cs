using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Loki.Interfaces;
using Odin.Core;
using Loki.Generic;
using DesignPatterns;
using Loki.DataRepresentation.Loaders;
using Odin.Plugin;
using Loki.DataRepresentation;
using System.IO;

namespace Midgard.Tests
{
    [TestFixture()]
    public class GenerateBaseTestsTests
    {
        #region Fields

        ICodeGenerator baseGenerator = new Generator(null).PluginManager.Get("Odin.GenerateBaseTests");

        #endregion Fields

        #region Start Up

        public GenerateBaseTestsTests()
		{
			
        }

        #endregion Start Up

        #region Generic Tests

        [Test]
        public void TestClassesTestGenerator()
        {
            try
            {
                baseGenerator.Init(Globals.SmsTestProject, null, new BuildAggregator());
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            baseGenerator.Generate();
        }

        [Test]
        public void GlobalTestGenerator()
        {
            try
            {
                baseGenerator.Init(Globals.Global, null, new BuildAggregator());
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            baseGenerator.Generate();
        }

        #endregion Generic Tests
    }
}
