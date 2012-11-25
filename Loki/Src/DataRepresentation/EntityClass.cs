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
using Loki.Generic;
using Loki.Interfaces;

namespace Loki.DataRepresentation {

	public class EntityClass : Entity {

		#region Private Fields

		private IProject project = null;
		private Entity parent = null;
		private List<EntityField> fields = new List<EntityField>();
		private bool isAbstract = false;

		#endregion

		#region Properties
		
		public bool IsAbstract {
			get { return isAbstract; }
			set { isAbstract = value; }
		}

		public override bool HasParent {
			get { return Parent != null; }
		}

		public override bool RootEntity {
			get { return !HasParent; }
		}
		
		public Entity Parent {
			get { return parent; }
			set { parent = value; }
		}

		public List<EntityField> Fields {
			get { return fields; }
			set { fields = value; }
		}

		public IProject Project {
			get { return project; }
			set { project = value; }
		}

        public List<EntityField> AllFields {
            get {
				CheckParent(this);
                if (!HasParent) {
                    return Fields;
                }
                List<EntityField> all = new List<EntityField>(Fields);

                EntityClass parent = (EntityClass) Parent;
                while (parent != null) {
                    all.AddRange(parent.Fields);
					CheckParent(parent);
                    parent = (EntityClass) parent.Parent;
                }

                return all;
            }
        }

        public List<EntityField> DirectPreviewFields {
            get {
                List<EntityField> preview = new List<EntityField>();

                foreach (EntityField field in AllFields) {
                    if (field.IsPreview) {
                        preview.Add(field);
                    }
                }

                if (preview.Count > 0) {
                    return preview;
                }

                foreach (EntityField field in AllFields) {
                    if (field.Mult == Multiplicity.OneToOne) {
                        preview.Add(field);
                    }
                }

                return preview;
            }
        }

        public List<EntityField> PreviewFields {
            get {
                List<EntityField> preview = new List<EntityField>();
                
                foreach (EntityField field in AllFields ) {
                    //Console.WriteLine("[{0}] {1} : {2}", Name, field.PropertyName, field.IsPreview);
                    if ( field.IsPreview ) {
                        preview.Add(field);
                    }
                }

                if (preview.Count > 0) {
                    return preview;
                }

                return AllFields;
            }
        }

        public bool HasPreviewFields {
            get {
                foreach (EntityField field in AllFields) {
                    if (field.IsPreview) {
                        return true;
                    }
                }
                return false;
            }
        }

        public EntityField MainProperty {
            get {
                foreach (EntityField field in AllFields) {
                    if ( field.Represents ) {
                        return field;
                    }
                }

                return null;
            }
        }

        public bool HasMainField {
            get {
                foreach (EntityField field in AllFields) {
                    if (field.Represents) {
                        return true;
                    }
                }
                return false;
            }
        }

		public bool IsBaseRoot {
			get {
				if( !HasParent && null != Project ) {
					foreach( Entity entity in Project.Model ) {
						if( entity is EntityClass ) {
							EntityClass e = ( (EntityClass)entity );
							if( e.HasParent && e.Parent.Id == Id ) {
								return true;
							}
						}
					}
				}
				return false;
			}
		}

        public List<Entity> AllDerivedEntities {
            get {
                List<Entity> list = new List<Entity>();

                if ( null == Project ) {
                    return list;
                }

                GetDerived(list, this);

                return list;
            }
        }

        private void GetDerived(List<Entity> list, Entity source)
        {
            foreach (Entity entity in Project.Model) {
                if (entity is EntityClass) {
                    EntityClass e = (EntityClass)entity;
                    if (e.HasParent && e.Parent.Id == source.Id) {
                        list.Add(e);
                        GetDerived(list, e);
                    }
                }
            }
        }
		
		private void CheckParent( EntityClass entity )
		{
			foreach( EntityField field in entity.Fields ) {
				field.Parent = entity;
			}
		}

		#endregion

		#region Public Methods

		public void AddField( EntityField field ) {
			fields.Add(field);
		}

		public EntityField GetField( string fieldName ) {
			if( fieldName != null ) {
				foreach( EntityField field in Fields ) {
					if( field.Name == fieldName ) {
						return field;
					}
				}
			}
			return null;
		}

		public bool HasField( string fieldName ) {
			foreach( EntityField field in Fields ) {
				if( field.Name == fieldName ) {
					return true;
				}
			}
			return false;
		}

		#endregion

		#region Constructor

		public EntityClass( string name, string visibility ) {
			Name = name;
			Visibility = visibility;
			IsAbstract = isAbstract;
		}

		public EntityClass() { }

		#endregion
	}

};
