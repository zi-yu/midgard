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
using System.Xml;
using Loki.DataRepresentation.Loaders;
using Loki.Exceptions;

namespace Loki.DataRepresentation {
	public class EntityParameter : IEntityAttribute {

		#region Private Fields

		private string id = string.Empty;
		private string name = string.Empty;
		private Entity type = null;
		private bool isReturn = false;
		private string modifier = string.Empty;
		private bool resolved = true;
		private Multiplicity mult = Multiplicity.OneToOne;

		#endregion

		#region Properties

		public string Id {
			get { return id; }
			set { id = value; }
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}
		
		
		public bool IsReturn {
			get { return isReturn; }
			set { isReturn = value; }
		}

		public Entity Type {
			get { return type; }
			set { type = value; }
		}

		public bool HasModifier {
			get { return !string.IsNullOrEmpty(modifier); }
		}

		public string Modifier {
			get { return modifier; }
			set { modifier = value; }
		}

		public bool Resolved {
			get { return resolved; }
			set { resolved = value; }
		}

		public Multiplicity Mult {
			get { return mult; }
			set { mult = value; }
		}

		#endregion

	}
}
