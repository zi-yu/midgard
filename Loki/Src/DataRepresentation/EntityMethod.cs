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

namespace Loki.DataRepresentation {

	public class EntityMethod : IEntity {

		#region Private Fields

		private string id = string.Empty;
		private string name = string.Empty;

		private string methodModifier = string.Empty;
		private bool isStatic = false;
		private EntityParameter returnEntity;
		private List<EntityParameter> parameters = new List<EntityParameter>();
		private bool resolved = true;

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

		public EntityParameter ReturnEntity {
			get { return returnEntity; }
			set { returnEntity = value; }
		}
		
		public string MethodModifier {
			get { return methodModifier; }
			set { methodModifier = value; }
		}

		public bool IsStatic {
			get { return isStatic; }
			set { isStatic = value; }
		}

		public bool HasModifier {
			get { return !string.IsNullOrEmpty(methodModifier); }
		}

		public List<EntityParameter> Parameters {
			get { return parameters; }
			set { parameters = value; }
		}

		public bool Resolved {
			get { return resolved; }
			set { resolved = value; }
		}
 
		#endregion

		#region Public

		public void ResolveConflits( XmlNode node ) {

		}

		#endregion

	};
}
