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
	public abstract class Entity : IEntity {

		#region Private Fields

		private static int staticId = 0;

		private string id = string.Empty;
		private string name = string.Empty;
		private string visibility = string.Empty;
		private bool persistable = true;
		private List<EntityInterface> interfaces = new List<EntityInterface>();
		private List<EntityMethod> methods = new List<EntityMethod>();
		private bool resolved = true;
        private bool lazy = true;

		#endregion

		#region Properties

		public string Id {
			get {
				if( string.Empty == id ) {
					id = ( staticId++ ).ToString();
				}
				return id; }
			set { id = value; }
		}

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public string Visibility {
			get { return visibility; }
			set { visibility = value; }
		}

		public List<EntityInterface> Interfaces {
			get { return interfaces; }
			set { interfaces = value; }
		}

		public List<EntityMethod> Methods {
			get { return methods; }
			set { methods = value; }
		}

		public virtual string AccessInterface {
			get { return string.Format("{0}",Name); }
		}

		public bool Persistable {
			get { return persistable; }
			set { persistable = value; }
		}

		public virtual bool HasParent {
			get { return false; }
		}

		public virtual bool IsIntrinsic	{
			get { return false; }
		}

		public bool Resolved {
			get { return resolved; }
			set { resolved = value; }
		}

		public virtual bool RootEntity {
			get { return true; }
		}

        public bool Lazy {
            get { return lazy; }
            set { lazy = value; }
        }

		#endregion

		#region Constructor
		
		public Entity() {}

		#endregion
	}
}
