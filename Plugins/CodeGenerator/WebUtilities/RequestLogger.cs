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
using Loki.DataRepresentation.Loaders;

namespace Odin.Plugin {

    public class RequestLogger : WebPluginBase {

		#region ICodeGenerator Members

		public override void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) 
        {
			base.Init( project, dependencies, aggregator );
            Model model = project.Model;

            EntityClass logger = CreateLogger();
            EntityClass principal = (EntityClass) model.GetByName("Principal");

            SetupConnection(logger, principal);

            model.Add(logger);

            WebUtilities.Dependencies.Instance.RegistDependency("RequestLogger");
		}

		public override void Generate() {
            GenerateRequestLoggerModule();
            GenerateLatestRequests();
            GenerateLatestReferrals();
            GenerateLatestRequestsAspxHome();
            GenerateLatestReferralsAspxHome();
		}

		public override string Name {
            get { return "Web.RequestLogger"; }
		}

		#endregion

        #region Generate Methods

        private void GenerateRequestLoggerModule()
        {
            string output = this.GetRelativeOutputDir("RequestLoggerModule.cs", "Modules");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output, VSFileType.Compile);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("dependencies", WebUtilities.Dependencies.Instance);
            variables.Add("webNamespace", Project.Name + "." + ComponentType.WebUserInterface.ToString());
            variables.Add("dalNamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString());
            variables.Add("coreNamespace", Project.Name + "." + ComponentType.Core.ToString());

            Templates.Generate(GetResource("RequestLoggerModule.cs.vtl"), output, variables);
        }

        private void GenerateLatestRequests()
        {
            string output = this.GetRelativeOutputDir("LatestRequests.cs", "Admin/Controls/RequestLogger/");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output, VSFileType.Compile);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("dependencies", WebUtilities.Dependencies.Instance);
            variables.Add("webNamespace", Project.Name + "." + ComponentType.WebUserInterface.ToString());
            variables.Add("dalNamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString());
            variables.Add("coreNamespace", Project.Name + "." + ComponentType.Core.ToString());

            Templates.Generate(GetResource("LatestRequests.cs.vtl"), output, variables);
        }

        private void GenerateLatestReferrals()
        {
            string output = this.GetRelativeOutputDir("LatestReferrals.cs", "Admin/Controls/RequestLogger/");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output, VSFileType.Compile);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("dependencies", WebUtilities.Dependencies.Instance);
            variables.Add("webNamespace", Project.Name + "." + ComponentType.WebUserInterface.ToString());
            variables.Add("dalNamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString());
            variables.Add("coreNamespace", Project.Name + "." + ComponentType.Core.ToString());

            Templates.Generate(GetResource("LatestReferrals.cs.vtl"), output, variables);
        }

        private void GenerateLatestRequestsAspxHome()
        {
            string output = GetRelativeOutputDir("latestrequestsHome.aspx", "Admin");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output, VSFileType.Content);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);
            variables.Add("modulesNamespace", ModulesNamespace);
            variables["assembly"] = Assembly;
            variables["namespace"] = AdminNamespace;
            variables["projectName"] = Project.Name;
            variables["controlsNamespace"] = AdminControlsNamespace;
            variables["dependencies"] = WebUtilities.Dependencies.Instance;
            variables["webControls"] = Project.Name + "." + ComponentType.WebUserInterface + ".Controls";

            Templates.Generate(GetResource("latestrequestsHome.aspx.vtl"), output, variables);
        }

        private void GenerateLatestReferralsAspxHome()
        {
            string output = GetRelativeOutputDir("latestreferralsHome.aspx", "Admin");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output, VSFileType.Content);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);
            variables.Add("modulesNamespace", ModulesNamespace);
            variables["assembly"] = Assembly;
            variables["namespace"] = AdminNamespace;
            variables["controlsNamespace"] = AdminControlsNamespace;
            variables["projectName"] = Project.Name;
            variables["dependencies"] = WebUtilities.Dependencies.Instance;
            variables["webControls"] = Project.Name + "." + ComponentType.WebUserInterface + ".Controls";

            Templates.Generate(GetResource("latestreferralsHome.aspx.vtl"), output, variables);
        }

        #endregion Generate Methods

        #region Utilities

        private EntityClass CreateLogger()
        {
			Entity intEntity = IntrinsicTypes.Create( "System.Int32" );
			Entity stringEntity = IntrinsicTypes.Create( "System.String" );
			Entity dateTimeEntity = IntrinsicTypes.Create( "System.DateTime" );

            EntityClass logger = new EntityClass();
            logger.Name = "RequestLogger";
            logger.Visibility = "public";

            EntityField field = new EntityField();
            field.Name = "id";
            field.IsPrimaryKey = true;
            field.IsPreview = true;
            field.Type = intEntity;
            logger.Fields.Add(field);

            field = new EntityField();
            field.Name = "url";
            field.IsPreview = true;
            field.Represents = true;
            field.Type = stringEntity;
            field.MaxSize = 1000;
            logger.Fields.Add(field);

            field = new EntityField();
            field.Name = "date";
            field.IsPreview = true;
            field.Type = dateTimeEntity;
            logger.Fields.Add(field);

            field = new EntityField();
            field.Name = "requestTime";
            field.IsPreview = true;
            field.Type = intEntity;
            logger.Fields.Add(field);

            field = new EntityField();
            field.Name = "referrer";
            field.Type = stringEntity;
            field.IsRequired = false;
            field.MaxSize = 1000;
            logger.Fields.Add(field);

            field = new EntityField();
            field.Name = "ip";
            field.Type = stringEntity;
            field.MaxSize = 15;
            logger.Fields.Add(field);

            field = new EntityField();
            field.Name = "userAgent";
            field.Type = stringEntity;
            field.MaxSize = 1500;
            logger.Fields.Add(field);

            return logger;
        }

        private void SetupConnection(EntityClass logger, EntityClass principal)
        {
            EntityField principalRef = new EntityField();
            principalRef.Name = "principal";
            principalRef.IsRequired = false;
            principalRef.Mult = Multiplicity.ManyToOne;
            principalRef.Type = principal;
            logger.Fields.Add(principalRef);

            EntityField loggerRef = new EntityField();
            loggerRef.Name = "Requests";
            loggerRef.Type = logger;
            loggerRef.Mult = Multiplicity.OneToMany;
            principal.Fields.Add(loggerRef);
        }

        #endregion Utilities

    };

}
