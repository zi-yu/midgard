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
using System.Collections.Generic;
using System.Text;

namespace Loki.DataRepresentation.IntrinsicEntities {

	public class String : EntityClass
	{
		#region Ctor

		public String()
		{
			this.Id = "Intrinsic.System.String";
			this.Name = "string";
			this.Parent = null;
			this.Visibility = "private";
			this.Persistable = false;
		}

		#endregion

		#region Properties

		public override string AccessInterface {
			get { return "string"; }
		}

		public override bool IsIntrinsic {
			get { return true; }
		}

		#endregion

	};
}
