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
    public class GenerateBaseTests
    {
        #region Fields

        ICodeGenerator baseGenerator = new Generator(null).PluginManager.Get("Odin.GenerateBase");

        #endregion Fields

        #region Start Up

        public GenerateBaseTests()
		{
			
        }

        #endregion Start Up

        #region Generic Tests

        [Test]
        public void TestInterfacesGenerator()
        {
            try
            {
                baseGenerator.Init(Globals.InterfaceProject, null, new BuildAggregator());
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            baseGenerator.BeforeGenerate();
            string ret = @"
	// WARNING: Generated File! Do not modify by hand!
	// *************************************************************

using System;

public interface IPerson {

	string GetPerson ( int id );
	void Init ( );
	void SetData ( out string nome , bool isMale );
}";
            using (StreamReader reader = new StreamReader(Path.Combine(Path.Combine(Globals.InterfaceProject.OutputPath, "Core/IPerson"), "IPerson.cs")))
            {
                string inter = reader.ReadToEnd();
                Assert.AreEqual(inter, ret);
            }
        }

        [Test]
        public void TestClassesFieldGenerator()
        {
            try
            {
				baseGenerator.Init( Globals.ClassesProject, null, new BuildAggregator() );
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            baseGenerator.BeforeGenerate();
			string ret = @"
	// WARNING: Generated File! Do not modify by hand!
	// *************************************************************

using System;
using System.Collections.Generic;

namespace Sms.Core {
	public  partial class Category 	{

		#region Fields

  		private int id;
  		private string description;
  		private System.Collections.ICollection messages;

		#endregion Fields

		#region Properties

  		public int Id
		{
			set{ id = value;}
			get{ return id;}
		}
  		public string Description
		{
			set{ description = value;}
			get{ return description;}
		}
  		public System.Collections.ICollection Messages
		{
			set{ messages = value;}
			get{ return messages;}
		}
		#endregion Properties
	};
}";

            using (StreamReader reader = new StreamReader(Path.Combine(Path.Combine(Globals.ClassesProject.OutputPath, "Core/Category"), "Category.cs")))
            {
                string inter = reader.ReadToEnd();
                Assert.AreEqual(inter, ret);
            }
        }

        [Test]
        public void TestClassesMethodGenerator()
        {
            try
            {
				baseGenerator.Init( Globals.ClassesProject, null, new BuildAggregator() );
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            baseGenerator.BeforeGenerate();
            string ret = @"
	// WARNING: This file is only generated in the first time you generate the classes.
	//			If you want to generate again you must delete this file.
	// *************************************************************

using System;
using System.Collections.Generic;

protected abstract partial class Category : Pai, IPerson, ICategory
{

	void Init ( )
	{
		//TO DO
	}

}";

            using (StreamReader reader = new StreamReader(Path.Combine(Path.Combine(Globals.ClassesProject.OutputPath, "BaseClasses/Category"), "CategoryToChange.cs")))
            {
                string inter = reader.ReadToEnd();
                Assert.AreEqual(inter, ret);
            }
        }

        [Test]
        public void TestSMSGenerator()
        {
            try
            {
				baseGenerator.Init( Globals.SmsTestProject, null, new BuildAggregator() );
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
            baseGenerator.BeforeGenerate();

            string[] dir = Directory.GetDirectories(Path.Combine(Globals.SmsTestProject.OutputPath, "BaseClasses"));
            Assert.AreEqual(dir.Length, 7);
        }

        #endregion Generic Tests
    }
}
