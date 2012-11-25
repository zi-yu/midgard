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

    public class Atlas : WebPluginBase {

        #region ICodeGenerator Members

        public override void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) {
			base.Init( project, dependencies, aggregator );
            WebUtilities.Dependencies.Instance.RegistDependency("Atlas");
            Aggregator.RegisterAssembly(ComponentType.WebUserInterface.ToString(), "Microsoft.Web.Atlas.dll");
            Aggregator.RegisterGacAssembly(ComponentType.WebUserInterface.ToString(), "System.Web.Services");
		}

		public override string Name {
			get { return "Web.Atlas"; }
		}

		#endregion

	};

}

