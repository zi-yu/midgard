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
using System.Reflection;

namespace Midgard.Tests {

	[TestFixture()]
	public class MSBuildTests {

		#region Fields

		private IBuildGenerator msbuild = new Generator( null ).BuildManager.Get( "Odin.MSBuild" );

		#endregion

		#region Tests

		[SetUp]
		public void Init() {
			BuildAggregator aggregator = new BuildAggregator();
			aggregator.Init( Globals.SmsTestProject );
			aggregator.RegisterComponent( "DAL" );
			aggregator.RegisterGacAssembly( "DAL", "System" );
			aggregator.RegisterGacAssembly( "DAL", "System.Web" );
			aggregator.RegisterGacAssembly( "DAL", "System.IO" );

			aggregator.RegisterAssembly( "DAL", Assembly.GetCallingAssembly().FullName );
			aggregator.RegisterAssembly( "DAL", Assembly.GetAssembly( typeof(Loki.Generic.Globals) ).FullName );
			
			aggregator.RegisterFile( "DAL", "baa" );
			aggregator.RegisterFile( "DAL", "bee" );
			aggregator.RegisterFile( "DAL", "bii" );
			aggregator.RegisterFile( "DAL", "boo" );
			aggregator.RegisterFile( "DAL", "buu" );
			
			msbuild.Init( Globals.SmsTestProject, null, aggregator.Informations  );
		}
		
		[Test]
		public void GenerateTest() {
			try {
				msbuild.Generate();
			} catch( Exception ex ) {
				Log.Error( ex );
			}
		}

		#endregion
	};
}
