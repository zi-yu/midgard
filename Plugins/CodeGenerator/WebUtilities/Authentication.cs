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
using WebUtilities;

namespace Odin.Plugin {

	public class Authentication : WebPluginBase {

		#region Properties

		
		#endregion

		#region ICodeGenerator Members

		public override void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) {
			base.Init( project, dependencies, aggregator );

			Aggregator.RegisterGacAssembly( ComponentType.WebUserInterface.ToString(), "System.Xml" );

            WebUtilities.Dependencies.Instance.RegistDependency("Authentication");
		}

		public override void Generate() {
			GenerateLogin();
			GenerateAuthenticateModule();
			GenerateLoginPageCode();
			GenerateLoginPageControl();
			GenerateRoleManager();
			GenerateRoleVisible();
			GenerateRoleXml();
			GenerateSendMail();
		}

		public override string Name {
			get { return "Web.Authentication"; }
		}

		#endregion

		#region Generate

		private void GenerateAuthenticateModule() {
			string output = GetRelativeOutputDir( "AuthenticateModule.cs", "Modules" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add("namespace", ModulesNamespace );
            variables.Add("coreNamespace", Project.Name + "." + ComponentType.Core);
            variables.Add("dalNamespace", Project.Name + "." + ComponentType.DataAccessLayer);

			Templates.Generate( GetResource( "AuthenticateModule.cs.vtl" ), output, variables );
			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateLogin() {
			string output = GetRelativeOutputDir( "Login.cs", Path.Combine( "Controls", "Generic" ) );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add("namespace", ControlsNamespace );
			variables.Add("projectName", Project.Name );
            variables.Add("coreNamespace", Project.Name + "." + ComponentType.Core);
            variables.Add("dalNamespace", Project.Name + "." + ComponentType.DataAccessLayer);

			Templates.Generate( GetResource( "Login.cs.vtl" ), output, variables );
			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateLoginPageCode() {
			string output = GetRootOutputDir( "login.aspx.cs" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, true, "login.aspx" );
			
			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add("namespace", Namespace );
			variables.Add("projectName", Project.Name );
            variables.Add("coreNamespace", Project.Name + "." + ComponentType.Core);
            variables.Add("dalNamespace", Project.Name + "." + ComponentType.DataAccessLayer);

			Templates.Generate( GetResource( "Login.aspx.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateLoginPageControl() {
			string output = GetRootOutputDir( "login.aspx" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, VSFileType.Content );

			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add("namespace", Namespace );
            variables.Add("projectName", Project.Name);
			variables.Add("controlsNamespace", ControlsNamespace );
            variables.Add("coreNamespace", Project.Name + "." + ComponentType.Core);
            variables.Add("dalNamespace", Project.Name + "." + ComponentType.DataAccessLayer);

			Templates.Generate( GetResource( "Login.aspx.vtl" ), output, variables );
			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateRoleManager() {
			string output = GetRelativeOutputDir( "RoleManager.cs", "Engine" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add("namespace", Namespace );

			Templates.Generate( GetResource( "RoleManager.cs.vtl" ), output, variables );
			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateRoleVisible() {
			string output = GetControlsOutputDir( "Generic", "RoleVisible.cs" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add( "namespace", ControlsNamespace );

			Templates.Generate( GetResource( "RoleVisible.cs.vtl" ), output, variables );
			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateRoleXml() {
			string output = GetRelativeOutputDir( "Roles.xml", "bin" );

			Templates.Generate( GetResource( "Roles.xml.vtl" ), output, new Dictionary<string, object>() );
			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateSendMail() {
			string output = GetRelativeOutputDir( "SendMail.cs","Engine");
			Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);
			
			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add("namespace", Namespace);

			Templates.Generate(GetResource("SendMail.cs.vtl"), output, variables);
			Log.Info("Generated '{0}'", output);
		}

		#endregion
		
	};

}
