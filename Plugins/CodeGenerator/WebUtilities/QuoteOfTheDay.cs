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

    public class QuoteOfTheDay : WebPluginBase {

		#region ICodeGenerator Members

		public override void Init( IProject project, IDependencyManager dependencies, IBuildAggregator aggregator ) 
        {
			base.Init( project, dependencies, aggregator );
            Model model = project.Model;

            EntityClass quotes = new EntityClass();
            quotes.Name = "QuoteOfTheDay";
            quotes.Visibility = "public";

            EntityField id = new EntityField();
            id.Name = "id";
            id.IsPreview = true;
            id.IsPrimaryKey = true;
            id.Type = IntrinsicTypes.Create("System.Int32");
            quotes.Fields.Add(id);

            EntityField quote = new EntityField();
            quote.Name = "quote";
            quote.Represents = true;
            quote.IsPreview = true;
			quote.Type = IntrinsicTypes.Create( "System.String" );
            quote.IsRequired = true;
            quote.MaxSize = 1000;
            quotes.Fields.Add(quote);

            model.Add(quotes);
		}

		public override void Generate() {
			string output = this.GetControlsOutputDir( "Generic/Quote.cs" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output, VSFileType.Compile );

			Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("dependencies", WebUtilities.Dependencies.Instance);
			variables.Add( "namespace", Project.Name + "." + ComponentType.WebUserInterface.ToString() );

			Templates.Generate( GetResource( "QuoteOfTheDay.cs.vtl" ), output, variables );
		}

		public override string Name {
            get { return "Web.Controls.QuoteOfTheDay"; }
		}

		#endregion

	};

}
