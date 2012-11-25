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

	public class Admin : WebPluginBase {

		#region Private

		private List<EntityClass> GetEntities() {
			List<EntityClass> entities = new List<EntityClass>();
			foreach( Entity e in Project.Model ) {
				if( e is EntityClass ) {
					entities.Add( (EntityClass)e );
				}
			}
			return entities;
		} 
		
		#endregion

		#region ICodeGenerator Members

		public override void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) {
			base.Init( project, dependencies, aggregator );

            WebUtilities.Dependencies.Instance.RegistDependency("Admin");
		}

		public override void Generate() {
			Aggregator.RegisterAssembly( ComponentType.WebUserInterface.ToString(), "NHibernate.dll" );

			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables["assembly"] = Assembly;
			variables["namespace"] = AdminNamespace;
            variables["controlsNamespace"] = AdminControlsNamespace;
            variables["dependencies"] = WebUtilities.Dependencies.Instance;
            variables["webControls"] = Project.Name + "." + ComponentType.WebUserInterface + ".Controls";
            variables.Add("coreNamespace", Project.Name + "." + ComponentType.Core);
            variables.Add("dalNamespace", Project.Name + "." + ComponentType.DataAccessLayer);

			RegisterAdminStaticFiles();
			GenerateAdminDefaultCode( variables );
			GenerateAdminDefaultControl( variables );
			GenerateAdminMaster( variables );
			GenerateAdminMasterCode( variables );

			variables["namespace"] = AdminControlsNamespace;
			variables["entities"] = GetEntities() ;

			GenerateLeftMenu( variables );
			GenerateTopMenu( variables );
            GenerateEditLink(variables);

			GenerateStateManager( variables );
			GenerateSmtp( variables );
			GenerateSmtpAspx( variables );
			GenerateHttpHeaders( variables );
			GenerateHttpHeadersAspx( variables );
			GenerateQueryAnalyser( variables );
			GenerateQueryAnalyserAspx( variables );
			GenerateSendMail( variables );
			GenerateSendMailAspx( variables );

			GenerateAction();
			GenerateStateManagerAspx();

			GenerateInformation( variables );
            GenerateAdminUpdateButton( variables );
			
			GenerateOfflineModule();
		}

		public override string Name {
			get { return "Web.Admin"; }
		}

		#endregion

		#region Admin Pages

		private void RegisterAdminStaticFiles() {
			string output = GetRelativeOutputDir( "style.css", "Admin/Style" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, VSFileType.Content );
		}

		private void GenerateAdminDefaultControl( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "default.aspx", "Admin" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, VSFileType.Content );

			Templates.Generate( GetResource( "AdminDefault.aspx.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateAdminDefaultCode( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "Default.aspx.cs", "Admin" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, true, "default.aspx" );

			Templates.Generate( GetResource( "AdminDefault.aspx.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateAdminMaster( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "adminMaster.master", "Admin" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, VSFileType.Content );

			Templates.Generate( GetResource( "Admin.master.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateAdminMasterCode( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "AdminMaster.master.cs", "Admin" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, true, "adminMaster.master" );

			Templates.Generate( GetResource( "Admin.master.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		#endregion

		#region Admin Menus

		private void GenerateLeftMenu( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "LeftMenu.cs", "Admin/Controls" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Templates.Generate( GetResource( "LeftMenu.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateTopMenu( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "TopMenu.cs", "Admin/Controls" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Templates.Generate( GetResource( "TopMenu.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

        private void GenerateEditLink(Dictionary<string, object> variables){
			string output = GetRelativeOutputDir( "EditLink.cs", "Admin/Controls" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Templates.Generate( GetResource( "EditLink.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		#endregion

		#region Actions

		private void GenerateAction() {
			string[] actions = new string[] { "Create", "Manage", "Search" };
			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add( "namespace", AdminControlsNamespace );
			variables.Add( "assembly", Assembly );
			variables.Add( "controls", Assembly + ".Controls" );
			variables.Add( "projectName", Project.Name );
            variables["controlsNamespace"] = AdminControlsNamespace;

			foreach( Entity e in GetEntities() ) {
				foreach( string action in actions ) {
					variables["entity"] = e.Name;
					variables["action"] = action;
					variables["file"] = string.Format( "{0}.{1}{2}", AdminControlsNamespace, e.Name, action );
					variables["obj"] = e as EntityClass;

					GenerateActionAspxFile( variables, e.Name, action );
				}
				GenerateDefaultAspxFile( variables, e.Name );
				GenerateActionEditAspxFile( variables, e.Name );
			}
		}

		private void GenerateActionCsFile( Dictionary<string, object> variables, string name, string action ) {
			string output = GetRelativeOutputDir( string.Format( "{0}{1}.cs", name, action ), "Admin/Controls" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Templates.Generate( GetResource( "Action.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateActionAspxFile( Dictionary<string, object> variables, string name, string action ) {
			string output = GetRelativeOutputDir( string.Format( "{0}{1}.aspx", name.ToLower(), action ), "Admin" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, VSFileType.Content );

			Templates.Generate( GetResource( string.Format( "Action{0}.vtl", action ) ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateActionEditAspxFile( Dictionary<string, object> variables, string name ) {
			string output = GetRelativeOutputDir( string.Format( "{0}Edit.aspx", name.ToLower() ), "Admin" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, VSFileType.Content );

			Templates.Generate( GetResource( string.Format( "ActionEdit.vtl" ) ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		#endregion

		#region Admin Menu Options

		private void GenerateSmtp( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "SmtpSettings.cs", "Admin" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, true, "smtpsettingsHome.aspx" );

			Templates.Generate( GetResource( "SmtpSettings.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateSmtpAspx( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "smtpsettingsHome.aspx", "Admin" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, VSFileType.Content );

			Templates.Generate( GetResource( "SmtpSettings.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}



		private void GenerateDefaultAspxFile( Dictionary<string, object> variables, string name) {
			string output = GetRelativeOutputDir( string.Format( "{0}Home.aspx", name.ToLower() ), "Admin" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, VSFileType.Content );

			Templates.Generate( GetResource( "ActionDefault.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateStateManagerAspx() {
			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add( "namespace", AdminControlsNamespace );
			variables.Add( "assembly", Assembly );
			variables.Add( "controls", AdminControlsNamespace );
			
			string[] stateManagers = new string[]{ "Application", "Session", "Cache", "Items" };
			foreach( string manager in stateManagers ) {
				string output = GetRelativeOutputDir( string.Format( "{0}Home.aspx", manager.ToLower() ), "Admin" );
				Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, VSFileType.Content );
				variables["stateManager"] = manager;
				
				Templates.Generate( GetResource( "StateManager.vtl" ), output, variables );

				Log.Info( "Generated '{0}'", output );
			}
		}

		private void GenerateStateManager( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "StateManager.cs", "Admin/Controls" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Templates.Generate( GetResource( "StateManager.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateHttpHeadersAspx( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "httpheadersHome.aspx", "Admin" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, VSFileType.Content );

			Templates.Generate( GetResource( "HttpHeaders.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateHttpHeaders( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "HttpHeader.cs", "Admin/Controls" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Templates.Generate( GetResource( "HttpHeaders.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateQueryAnalyserAspx( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "queryanalyserHome.aspx", "Admin" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, VSFileType.Content );

			Templates.Generate( GetResource( "QueryAnalyser.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateQueryAnalyser( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "QueryAnalyser.cs", "Admin" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, true, "queryanalyserHome.aspx" );

			Templates.Generate( GetResource( "QueryAnalyser.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateSendMailAspx( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "sendmailHome.aspx", "Admin" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, VSFileType.Content );

			Templates.Generate( GetResource( "SendMail.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateSendMail( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "SendMail.cs", "Admin" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, true, "sendmailHome.aspx" );

			Templates.Generate( GetResource( "SendMail.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		private void GenerateOfflineModule() {
			Dictionary<string, object> variables = new Dictionary<string,object>();
			variables["namespace"] = ModulesNamespace;

			string output = GetRelativeOutputDir( "OfflineModule.cs", "Modules" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Templates.Generate( GetResource( "OfflineModule.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

		#endregion

		#region Admin Utilities

		private void GenerateInformation( Dictionary<string, object> variables ) {
			string output = GetRelativeOutputDir( "Information.cs", "Admin/Controls" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Templates.Generate( GetResource( "Information.cs.vtl" ), output, variables );

			Log.Info( "Generated '{0}'", output );
		}

        private void GenerateAdminUpdateButton(Dictionary<string, object> variables)
        {
            string output = GetRelativeOutputDir("AdminUpdateButton.cs", "Admin/Controls");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Templates.Generate(GetResource("AdminUpdateButton.cs.vtl"), output, variables);

            Log.Info("Generated '{0}'", output);
        }

		#endregion

	};

}
