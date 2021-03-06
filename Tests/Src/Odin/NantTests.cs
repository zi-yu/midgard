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
	public class NantTests {

		#region Fields

		private IBuildGenerator nant = new Generator( null ).BuildManager.Get( "Odin.Nant" );

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

			aggregator.RegisterAssembly( "DAL", Assembly.GetAssembly( typeof(Loki.Generic.Globals) ).FullName );
			
			aggregator.RegisterFile( "DAL", "baa" );
			aggregator.RegisterFile( "DAL", "bee" );
			aggregator.RegisterFile( "DAL", "bii" );
			aggregator.RegisterFile( "DAL", "boo" );
			aggregator.RegisterFile( "DAL", "buu" );
			
			nant.Init( Globals.SmsTestProject, null, aggregator.Informations  );
		}
		
		#endregion

		#region TestResult

		private string result = @"<!-- Generated by Midgard plugin 'Odin.Nant' -->
<?xml version='1.0'?>
<project name='DAL' default='build-all' basedir='.'>
	<description>DAL Nant Builder</description>
	<property name='debug' value='true' unless='\$\{property::exists('debug')\}'/>
	
	<target name='clean' description='remove all generated files'>
		<delete>
			<fileset>
				<include name='*~' />
				<include name='bin/*.dll' />
				<include name='bin/*.exe' />
				<include name='bin/*.pdb' />
				<include name='bin/*.mdb' />
				<include name='bin/*.xml' />
				<include name='bin/*~' />
				<include name='bin/.*' />
			</fileset>
		</delete>
  	</target>
  	
  	<!-- compila o projecto -->
	<target name='build-all' description='compiles Odin.exe' depends='build-dll'>

		<csc target='dll' output='bin/DAL.dll'
			debug='${debug}' doc='bin/DAL.dll'
			optimize='true'
			define='NANT'>

			<nowarn>
	            <!-- do not report warnings for missing XML comments -->
				<warning number='1591' />
				<warning number='1570' />
				<warning number='1572' />
				<warning number='1573' />
				<warning number='0649' />
				<warning number='0169' />
				<warning number='1699' />
				<warning number='0414' if='\$\{platform::is-unix()\}'/>
			</nowarn>

			<sources>
				<include name='DAL/**/*.cs' />
			</sources>

			<references>
				<include name='nunit.core, Version=2.2.3.0, Culture=neutral, PublicKeyToken=96d09a1eb7f44a77' />
				<include name='Loki, Version=1.0.0.0, Culture=neutral, PublicKeyToken=09352869ad1dca42' />
				<include name='System.dll' />
				<include name='System.Web.dll' />
				<include name='System.IO.dll' />
			</references>
		</csc>
		<echo message='DAL compiled successfully!' />
		<echo message='' />
		<echo message='Debug Build     : \$\{debug\}' />
		<echo message='' />
		<echo message='OS              : \$\{platform::get-name()\}' />
		<echo message='Target Platform : \$\{framework::get-target-framework()\}' />
  	</target>

</project>
".Replace("'", "\"");

		[Test]
		public void GenerateTest() {
			try {
				nant.Generate();
				//Comparer();
			} catch( Exception ex ) {
				Log.Error( ex );
			}
		}

		private void Comparer() {
			throw new Exception( "The method or operation is not implemented." );
		}
		
		#endregion
	};
}
