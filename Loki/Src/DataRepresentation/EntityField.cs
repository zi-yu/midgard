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

	public enum Multiplicity { 
		OneToOne,
		ManyToOne,
		OneToMany,
		ManyToMany
	}

	public class EntityField : IEntityAttribute {

		#region Private Fields

		private static string uniqueId = "0";

		private string id = string.Empty;
		private string name = string.Empty;
		private string fieldModifier = string.Empty;
		private Entity type = null;
		private EntityClass parent = null;
		private EntityField referenceType = null;
		private Multiplicity mult = Multiplicity.OneToOne;
		private List<string> regex = null;
		private bool isRequired = false;
		private bool primaryKey = false;
        private bool unique = false;
		private bool infoOnly = false;
		private object defaultValue = null;
		private int size = 100;
        private string visibility = "private";
        private bool secret = false;
		private bool isPreview = false;
		private bool represents = false;
        private bool lazy = true;

		private string targetId = string.Empty;
		private bool resolved = true;

		#endregion

		#region Properties

        public string Visibility
        {
            get { return visibility; }
            set { visibility = value; }
        }

		public string Id {
			get {
				if( string.Empty == id ) {
					id = uniqueId;
					int i = int.Parse( uniqueId );
					uniqueId = ( ++i ).ToString();
				}
				return id; }
			set { id = value; }
		}

		public string Name {
			get { return name; }
			set {
				name = value[0].ToString().ToLower();
				if( value.Length > 1 ) {
					name += value.Substring( 1, value.Length - 1 );
				}
			}
		}

		public string FieldModifier {
			get { return fieldModifier; }
			set {
				if( fieldModifier != string.Empty ) {
					fieldModifier += " ";
				}
				fieldModifier += value;
			}
		}

		public string PropertyName {
			get {
				name = name.Trim( new char[] { '_' } );
				string s = name[0].ToString().ToUpper();
				if( name.Length > 1 ) {
					s += name.Substring( 1, name.Length - 1 );
				}
				return s;
			}
		}

		public Entity Type {
			get { return type; }
			set { type = value; }
		}

		public EntityClass Parent {
			get { return parent; }
			set { parent = value; }
		}

        public bool Lazy {
            get { return lazy; }
            set { lazy = value; }
        }

		public EntityField ReferenceType {
			get { return referenceType; }
			set { referenceType = value; }
		}

		public string TargetId {
			get { return targetId; }
			set { targetId = value; }
		}

		public Multiplicity Mult {
			get { return mult; }
			set { mult = value; }
		}

		public List<string> Regex {
			get {
                if (regex == null) {
                    regex = new List<string>();
                }
                return regex; 
            }
			set { regex = value; }
		}

        public bool HasRegex {
            get {
                if (Regex == null) {
                    return false;
                }
                return Regex.Count > 0;
            }
        }
		
		public string ParentName {
			get {
				if( Parent != null ) {
					return Parent.Name;
				}
				return "null";
			}
		}

		public bool IsRequired {
			get { return isRequired; }
			set { isRequired = value; }
		}

        public bool Unique {
            get { return unique; }
            set { unique = value; }
        }

		public bool HasDefault {
			get { return defaultValue != null; }
		}

		public object Default {
			get { return defaultValue; }
			set { defaultValue = value; }
		}

		public bool IsPrimaryKey {
			get { return primaryKey; }
			set { primaryKey = value; }
		}

		public int MaxSize {
			get { return size; }
			set { size = value; }
		}

		public bool InfoOnly {
			get { return infoOnly; }
			set { infoOnly = value; }
		}

		public bool Resolved {
			get { return resolved; }
			set { resolved = value; }
		}

        public bool Secret {
            get { return secret; }
            set { secret = value; }
        }

		public bool IsPreview {
			get { return isPreview; }
			set { isPreview = value; }
		}

		public bool Represents {
			get { return represents; }
			set { represents = value; }
		}

		#endregion

		#region Constructor

		public EntityField( string name ) {
			Name = name;
		}

		public EntityField() {}

		#endregion
	}
}
