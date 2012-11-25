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

namespace Odin.Plugin {

    public class Controls : WebPluginBase {

        #region Properties

        public string Namespace {
            get {
                return string.Format("{0}.{1}.Controls", Project.Name, ComponentType.WebUserInterface.ToString() );
            }
        }

		#endregion

        #region ICodeGenerator Members

        public override void Init(IProject project, IDependencyManager dependencies, IBuildAggregator aggregator)
        {
            WebUtilities.Dependencies.Instance.RegistDependency("Controls");
			base.Init(project, dependencies, aggregator);
        }

        public override void Generate()
        {
            Aggregator.RegisterAssembly(ComponentType.WebUserInterface.ToString(), "Loki.dll");
            //Aggregator.RegisterGacAssembly(ComponentType.WebUserInterface.ToString(), "System.Design");
            
            GenerateBaseEntityItem();
			GenerateBaseEntityEdit();
            GenerateBaseFieldControl();
            GenerateBaseEntityFieldEditor();
            GenerateBaseList();
			GenerateBasePagination();
            GenerateBasePagedList();
			GenerateStringEditor();
			GenerateDoubleEditor();
            GenerateIntEditor();
            GenerateBoolEditor();
			GenerateDateTimeEditor();
            GenerateUpdateButton();
            GenerateBaseEntityCount();
            GenerateSourceType();

            foreach (Entity entity in Project.Model) {
                if ( entity.Persistable ) {
                    GenerateEntityItem(entity);
                    GenerateEntitySearch(entity);
					GenerateEntityEdit( entity );
                    GenerateEntityDelete(entity);
                    GenerateEntityDeleteAll(entity);
                    GenerateEntityJson(entity);
                    GenerateEntityXml(entity);
                    GenerateEntityCount(entity);
                    GenerateEntityList(entity);
					GenerateEntityPagination( entity );
                    GenerateEntityPagedList(entity);
                    foreach (EntityField field in ((EntityClass)entity).Fields) {
                        GenerateEntityFieldControl(entity, field);
						GenerateEntityFieldEdit( entity, field );
                        if (field.Mult == Multiplicity.ManyToMany) {
                            EntityClass other = (EntityClass) field.Type;
                            string propertyName = null;
                            foreach (EntityField f in other.Fields) {
                                if (f.Mult == Multiplicity.ManyToMany && f.Type.Name == entity.Name) {
                                    propertyName = f.PropertyName;
                                }
                            }
                            GenerateEntityDeleteManyToMany(entity, field, propertyName);
                        }
                    }
                }
            }

			GenerateAdminBanner();
        }
		
        public override void AfterGenerate()
        {
        }

        public override string Name {
            get { return "Web.Controls"; }
        }

        #endregion

        #region Generate

        private void GenerateBaseEntityItem()
        {
            string output = GetControlsOutputDir("BaseEntityItem.cs");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);

            Templates.Generate(GetResource("BaseEntityItem.cs.vtl"), output, variables);
            Log.Info("Generated `{0}'", output);
        }

		private void GenerateBaseEntityEdit() {
			string output = GetControlsOutputDir( "BaseEntityEdit.cs" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add( "namespace", Namespace );

			Templates.Generate( GetResource( "BaseEntityEdit.cs.vtl" ), output, variables );
			Log.Info( "Generated `{0}'", output );
		}

        private void GenerateBaseEntityCount()
        {
            string output = GetControlsOutputDir("BaseEntityCount.cs");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);

            Templates.Generate(GetResource("BaseEntityCount.cs.vtl"), output, variables);
            Log.Info("Generated `{0}'", output);
        }

		private void GenerateEntityEdit( Entity entity ) {
			string output = GetControlsOutputDir( entity.Name, entity.Name + "Editor.cs" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add( "namespace", Namespace );
			variables.Add( "entity", entity );
			variables.Add( "DALNamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString() );
			variables.Add( "CoreNamespace", Project.Name + "." + ComponentType.Core.ToString() );

			Templates.Generate( GetResource( "EntityEdit.cs.vtl" ), output, variables );
			Log.Info( "Generated `{0}'", output );
		}

        private void GenerateEntityDelete(Entity entity)
        {
            string output = GetRelativeOutputDir(entity.Name + "Delete.cs", "Controls/" + entity.Name + "/Delete/");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);
            variables.Add("entity", entity);
            variables.Add("DALNamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString());
            variables.Add("CoreNamespace", Project.Name + "." + ComponentType.Core.ToString());

            Templates.Generate(GetResource("EntityDelete.cs.vtl"), output, variables);
            Log.Info("Generated `{0}'", output);
        }

        private void GenerateEntityDeleteAll(Entity entity)
        {
            string output = GetRelativeOutputDir(entity.Name + "DeleteAll.cs", "Controls/" + entity.Name + "/Delete/");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);
            variables.Add("entity", entity);
            variables.Add("DALNamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString());
            variables.Add("CoreNamespace", Project.Name + "." + ComponentType.Core.ToString());

            Templates.Generate(GetResource("EntityDeleteAll.cs.vtl"), output, variables);
            Log.Info("Generated `{0}'", output);
        }

        private void GenerateEntityItem( Entity entity )
        {
            string output = GetControlsOutputDir(entity.Name, entity.Name + "Item.cs");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);
            variables.Add("entity", entity);
            variables.Add("DALNamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString());
            variables.Add("CoreNamespace", Project.Name + "." + ComponentType.Core.ToString());

            Templates.Generate(GetResource("EntityItem.cs.vtl"), output, variables);
            Log.Info("Generated `{0}'", output);
        }

        private void GenerateEntitySearch(Entity entity)
        {
            string output = GetControlsOutputDir(entity.Name, entity.Name + "Search.cs");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);
            variables.Add("entity", entity);
            variables.Add("DALNamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString());
            variables.Add("CoreNamespace", Project.Name + "." + ComponentType.Core.ToString());

            Templates.Generate(GetResource("EntitySearch.cs.vtl"), output, variables);
            Log.Info("Generated `{0}'", output);
        }

        private void GenerateEntityJson(Entity entity)
        {
            string output = GetControlsOutputDir(entity.Name, entity.Name + "Json.cs");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);
            variables.Add("entity", entity);
            variables.Add("DALNamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString());
            variables.Add("CoreNamespace", Project.Name + "." + ComponentType.Core.ToString());

            Templates.Generate(GetResource("EntityJson.cs.vtl"), output, variables);
            Log.Info("Generated `{0}'", output);
        }

        private void GenerateEntityXml(Entity entity)
        {
            string output = GetControlsOutputDir(entity.Name, entity.Name + "Xml.cs");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);
            variables.Add("entity", entity);
            variables.Add("DALNamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString());
            variables.Add("CoreNamespace", Project.Name + "." + ComponentType.Core.ToString());

            Templates.Generate(GetResource("EntityXml.cs.vtl"), output, variables);
            Log.Info("Generated `{0}'", output);
        }

        private void GenerateEntityCount(Entity entity)
        {
            string output = GetControlsOutputDir(entity.Name, entity.Name + "Count.cs");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);
            variables.Add("entity", entity);
            variables.Add("DALNamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString());
            variables.Add("CoreNamespace", Project.Name + "." + ComponentType.Core.ToString());

            Templates.Generate(GetResource("EntityCount.cs.vtl"), output, variables);
            Log.Info("Generated `{0}'", output);
        }

		private void GenerateEntityPagination( Entity entity ) {
			string output = GetControlsOutputDir( entity.Name, entity.Name + "Pagination.cs" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add( "namespace", Namespace );
			variables.Add( "entity", entity );
			variables.Add( "DALNamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString() );
			variables.Add( "CoreNamespace", Project.Name + "." + ComponentType.Core.ToString() );

			Templates.Generate( GetResource( "EntityPagination.cs.vtl" ), output, variables );
			Log.Info( "Generated '{0}'", output );
		}

		void GenerateEntityList(Entity entity)
        {
            string output = GetControlsOutputDir(entity.Name, entity.Name + "List.cs");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);
            variables.Add("entity", entity);
            variables.Add("DALNamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString());
            variables.Add("CoreNamespace", Project.Name + "." + ComponentType.Core.ToString());

            Templates.Generate(GetResource("EntityList.cs.vtl"), output, variables);
            Log.Info("Generated '{0}'", output);
        }

        private void GenerateEntityPagedList(Entity entity)
        {
            string output = GetControlsOutputDir(entity.Name, entity.Name + "PagedList.cs");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);
            variables.Add("entity", entity);
            variables.Add("DALNamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString());
            variables.Add("CoreNamespace", Project.Name + "." + ComponentType.Core.ToString());

            Templates.Generate(GetResource("EntityPagedList.cs.vtl"), output, variables);
            Log.Info("Generated '{0}'", output);
        }

        private void GenerateEntityFieldControl(Entity entity, EntityField field)
        {
            string output = GetRelativeOutputDir(entity.Name + field.PropertyName + ".cs", "Controls/" + entity.Name + "/Caption/");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);
            variables.Add("entity", entity);
            variables.Add("field", field);
            variables.Add("DALNamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString());
            variables.Add("CoreNamespace", Project.Name + "." + ComponentType.Core.ToString());

            Templates.Generate(GetResource("FieldControl.cs.vtl"), output, variables);
            Log.Info("Generated `{0}'", output);
        }

        private void GenerateEntityFieldControlDesigner(Entity entity, EntityField field)
        {
            string output = GetRelativeOutputDir(entity.Name + field.PropertyName + "Designer.cs", "Controls/" + entity.Name + "/Designer/");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);
            variables.Add("entity", entity);
            variables.Add("field", field);
            variables.Add("DALNamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString());
            variables.Add("CoreNamespace", Project.Name + "." + ComponentType.Core.ToString());

            Templates.Generate(GetResource("Designer/CaptionDesigner.cs.vtl"), output, variables);
            Log.Info("Generated `{0}'", output);
        }

        private void GenerateEntityFieldEditorDesigner(Entity entity, EntityField field)
        {
            string output = GetRelativeOutputDir(entity.Name + field.PropertyName + "EditorDesigner.cs", "Controls/" + entity.Name + "/Designer/");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);
            variables.Add("entity", entity);
            variables.Add("field", field);
            variables.Add("DALNamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString());
            variables.Add("CoreNamespace", Project.Name + "." + ComponentType.Core.ToString());

            Templates.Generate(GetResource("Designer/EditorDesigner.cs.vtl"), output, variables);
            Log.Info("Generated `{0}'", output);
        }

		private void GenerateEntityFieldEdit( Entity entity, EntityField field ) {
            string output = GetRelativeOutputDir(entity.Name + field.PropertyName + "Editor.cs", "Controls/" + entity.Name + "/Edit/");
			//string output = GetControlsOutputDir( entity.Name, entity.Name + field.PropertyName + "Edit.cs" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add( "namespace", Namespace );
			variables.Add( "entity", entity );
			variables.Add( "field", field );
			variables.Add( "DALNamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString() );
			variables.Add( "CoreNamespace", Project.Name + "." + ComponentType.Core.ToString() );

			Templates.Generate( GetResource( "EditControl.cs.vtl" ), output, variables );
			Log.Info( "Generated `{0}'", output );
		}

        private void GenerateEntityDeleteManyToMany(Entity entity, EntityField field, string property)
        {
            string output = GetRelativeOutputDir(entity.Name + "DeleteFrom" + field.Type.Name + ".cs", "Controls/" + entity.Name + "/Delete/");
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add( "namespace", Namespace );
			variables.Add( "entity", entity );
			variables.Add( "field", field );
			variables.Add( "DALNamespace", Project.Name + "." + ComponentType.DataAccessLayer.ToString() );
			variables.Add( "CoreNamespace", Project.Name + "." + ComponentType.Core.ToString() );
            variables.Add("className", entity.Name + "DeleteFrom" + field.Type.Name);
            variables.Add("property", property);

			Templates.Generate( GetResource( "EntityDeleteManyToMany.cs.vtl" ), output, variables );
			Log.Info( "Generated `{0}'", output );
		}

        private void GenerateBaseEntityFieldEditor()
        {
            string output = GetControlsOutputDir("BaseEntityFieldEditor.cs");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);

            Templates.Generate(GetResource("BaseEntityFieldEditor.cs.vtl"), output, variables);
            Log.Info("Generated `{0}'", output);
        }

        private void GenerateBaseFieldControl()
        {
            string output = GetControlsOutputDir("BaseFieldControl.cs");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);

            Templates.Generate(GetResource("BaseFieldControl.cs.vtl"), output, variables);
            Log.Info("Generated `{0}'", output);
        }

        private void GenerateBaseList()
        {
            string output = GetControlsOutputDir("BaseList.cs");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);

            Templates.Generate(GetResource("BaseList.cs.vtl"), output, variables);
            Log.Info("Generated `{0}'", output);
        }

		private void GenerateBasePagination() {
			string output = GetControlsOutputDir( "BasePagination.cs" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add( "namespace", Namespace );

			Templates.Generate( GetResource( "BasePagination.cs.vtl" ), output, variables );
			Log.Info( "Generated `{0}'", output );
		}

        private void GenerateBasePagedList()
        {
            string output = GetControlsOutputDir("BasePagedList.cs");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);

            Templates.Generate(GetResource("BasePagedList.cs.vtl"), output, variables);
            Log.Info("Generated `{0}'", output);
        }

		private void GenerateStringEditor() {
			string output = GetRelativeOutputDir( "StringEditor.cs", "Controls/EditControls" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add( "namespace", Namespace );

			Templates.Generate( GetResource( "StringEditor.cs.vtl" ), output, variables );
			Log.Info( "Generated `{0}'", output );
		}

		private void GenerateDoubleEditor() {
			string output = GetRelativeOutputDir("DoubleEditor.cs", "Controls/EditControls");
			Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add("namespace", Namespace);

			Templates.Generate(GetResource("DoubleEditor.cs.vtl"), output, variables);
			Log.Info("Generated `{0}'", output);
		}

        private void GenerateIntEditor()
        {
            string output = GetRelativeOutputDir("IntEditor.cs", "Controls/EditControls");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);

            Templates.Generate(GetResource("IntEditor.cs.vtl"), output, variables);
            Log.Info("Generated `{0}'", output);
        }

        private void GenerateBoolEditor()
        {
            string output = GetRelativeOutputDir("BoolEditor.cs", "Controls/EditControls");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);

            Templates.Generate(GetResource("BoolEditor.cs.vtl"), output, variables);
            Log.Info("Generated `{0}'", output);
        }

		private void GenerateDateTimeEditor() {
			string output = GetRelativeOutputDir( "DateTimeEditor.cs", "Controls/EditControls" );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add( "namespace", Namespace );

			Templates.Generate( GetResource( "DateTimeEditor.cs.vtl" ), output, variables );
			Log.Info( "Generated `{0}'", output );
		}

        private void GenerateUpdateButton()
        {
            string output = GetRelativeOutputDir("UpdateButton.cs", "Controls/EditControls");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);

            Templates.Generate(GetResource("UpdateButton.cs.vtl"), output, variables);
            Log.Info("Generated `{0}'", output);
        }

        private void GenerateSourceType()
        {
            string output = GetControlsOutputDir("SourceType.cs");
            Aggregator.RegisterFile(ComponentType.WebUserInterface.ToString(), output);

            Dictionary<string, object> variables = new Dictionary<string, object>();
            variables.Add("namespace", Namespace);

            Templates.Generate(GetResource("SourceType.cs.vtl"), output, variables);
            Log.Info("Generated '{0}'", output);
        }

		#endregion

		#region Generic

		private void GenerateAdminBanner() {
			string output = GetRelativeOutputDir( "AdminBanner.cs", Path.Combine( "Controls", "Generic" ) );
			Aggregator.RegisterFile( ComponentType.WebUserInterface.ToString(), output );

			Dictionary<string, object> variables = new Dictionary<string, object>();
			variables.Add( "namespace", Namespace );
            variables.Add("coreNamespace", Project.Name + "." + ComponentType.Core);
            variables.Add("dalNamespace", Project.Name + "." + ComponentType.DataAccessLayer);

			Templates.Generate( GetResource( "AdminBanner.cs.vtl" ), output, variables );
			Log.Info( "Generated '{0}'", output );
		}

		#endregion

	};

}
