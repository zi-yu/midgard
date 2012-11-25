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
using System.Data;

namespace Loki.DataRepresentation {
    
    public class LokiDataColumn : DataColumn {

		#region Private Fields

		private bool isRequired;
		private List<string> regex = new List<string>();
		
		#endregion
		
		#region Properties
		
		public bool IsRequired {
			get { return isRequired; }
			set { isRequired = value; }
		}

		public List<string> Regex {
			get { return regex; }
			set { regex = value; }
		} 

		#endregion

    }
}
