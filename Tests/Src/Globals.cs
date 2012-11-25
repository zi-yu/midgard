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
using Loki.DataRepresentation;
using Loki.DataRepresentation.IntrinsicEntities;
using Loki.Generic;
using Loki.DataRepresentation.Loaders;

namespace Midgard.Tests {

	public static class Globals {

		#region Fields

		private static Project smsTestModel = new Project("Sms", "../../Tools/Examples/Sms/Project/", "Midgard","");
		private static Project interfaces = new Project( "Interfaces", "../../Tools/Examples/Interfaces/Project/", "Midgard", "" );
		private static Project classes = new Project( "Classes", "../../Tools/Examples/Classes/Project/", "Midgard", "" );
		private static Project global = new Project( "../../TestProjects/Sms/Project/Sms.xml" );

        #endregion

		#region Properties

        public static Project Global
        {
            get { return Globals.global; }
        }

		public static Project SmsTestProject	{
			get { return smsTestModel; }
		}

        public static Project InterfaceProject
        {
            get { return interfaces; }
        }

        public static Project ClassesProject
        {
            get { return classes; }
        }

		#endregion

		#region Ctor

		static Globals()
		{
			BuildSmsTestModel();
            BuildInterfaceTestModel();
            BuildClassTestModel();
		}

        private static void BuildInterfaceTestModel()
        {
            EntityInterface iPerson = GetInterface();

            Model list = new Model();
            list.Add(iPerson);
            interfaces.Model = list;
        }

        private static void BuildClassTestModel()
        {
            EntityInterface iPerson = GetInterface();
            EntityClass parent = new EntityClass();
            parent.Name = "Pai";
            EntityClass category = new EntityClass();
            category.Name = "Category";
            category.Visibility = "public";
            category.Interfaces.Add(iPerson);
            category.Parent = parent;

            EntityField field = new EntityField();
            field.Name = "Id";
            field.Visibility = "protected";
            field.IsPrimaryKey = true;
            field.Type = new Int(); 
            category.Fields.Add(field);

            field = new EntityField();
            field.Name = "Description";
            field.IsRequired = true;
            field.MaxSize = 500;
            field.Type = new Loki.DataRepresentation.IntrinsicEntities.String();
            category.Fields.Add(field);
            category.IsAbstract = true;
            category.Persistable = true;
            category.Visibility = "protected";

            EntityMethod init = new EntityMethod();
            init.Name = "Init";

            category.Methods.Add(init);
          
            Model list = new Model();
            list.Add(category);
            classes.Model = list;

        }

        private static EntityInterface GetInterface()
        {
            /** IPerson Definition **/
            EntityInterface iPerson = new EntityInterface();
            iPerson.Name = "IPerson";
            iPerson.Visibility = "public";

            EntityMethod getDescription = new EntityMethod();
            getDescription.Name = "GetPerson";
            EntityParameter paramRet = new EntityParameter();
            paramRet.IsReturn = true;
			paramRet.Type = IntrinsicTypes.Create( "System.String" );
            EntityParameter param = new EntityParameter();
            param.IsReturn = false;
			param.Type = IntrinsicTypes.Create( "System.Int32" );
            param.Name = "id";

            getDescription.ReturnEntity = paramRet;
            List<EntityParameter> parames = new List<EntityParameter>();
            parames.Add(param);
            getDescription.Parameters = parames;

            EntityMethod init = new EntityMethod();
            init.Name = "Init";

            EntityMethod set = new EntityMethod();
            set.Name = "SetData";
            EntityParameter param1 = new EntityParameter();
            param1.IsReturn = true;
			param1.Type = IntrinsicTypes.Create( "System.String" );
            param1.Name = "nome";
            EntityParameter param2 = new EntityParameter();
            param2.IsReturn = false;
			param2.Type = IntrinsicTypes.Create( "System.Boolean" );
            param2.Name = "isMale";
            List<EntityParameter> parames2 = new List<EntityParameter>();
            parames2.Add(param1);
            parames2.Add(param2);
            set.Parameters = parames2;

            iPerson.Methods.Add(getDescription);
            iPerson.Methods.Add(init);
            iPerson.Methods.Add(set);
            return iPerson;
        }

		private static void BuildSmsTestModel()
		{
			EntityField field = null;

			/** User **/
			EntityClass user = new EntityClass();
			user.Name = "Principal";
			user.Visibility = "public";

			field = new EntityField();
			field.Name = "Id";
			field.IsPrimaryKey = true;
			field.Type = new Int();
			user.Fields.Add(field);

			/** IDescription Definition **/
			EntityInterface iDescription = new EntityInterface();
			iDescription.Name = "IDescription";
			iDescription.Visibility = "public";
			EntityMethod getDescription = new EntityMethod();
			getDescription.Name = "GetDescription";
			EntityParameter param = new EntityParameter();
			param.IsReturn = true;
			param.Type = IntrinsicTypes.Create( "System.String" );
			getDescription.ReturnEntity = param;
			iDescription.Methods.Add(getDescription);

			/** Category Definition **/

			EntityClass category = new EntityClass();
			category.Name = "Category";
			category.Visibility = "public";
			//category.Interfaces.Add(iDescription);

			field = new EntityField();
			field.Name = "Id";
			field.IsPrimaryKey = true;
			field.Type = new Int();
			category.Fields.Add(field);

			field = new EntityField();
			field.Name = "Description";
			field.IsRequired = true;
			field.MaxSize = 500;
			field.Type = new Loki.DataRepresentation.IntrinsicEntities.String();
			category.Fields.Add(field);

			/** Sms Definition **/

			EntityClass message = new EntityClass();
			message.Name = "SmsBase";
			message.IsAbstract = true;
			message.Visibility = "public";
			//message.Interfaces.Add(iDescription);
			
			field = new EntityField();
			field.Name = "Id";
			field.IsPrimaryKey = true;
			field.Type = new Int();
			message.Fields.Add(field);

			field = new EntityField();
			field.Name = "Description";
			field.IsRequired = true;
			field.MaxSize = 500;
			field.Default = "No Description";
			field.Type = new Loki.DataRepresentation.IntrinsicEntities.String();
			message.Fields.Add(field);

			field = new EntityField();
			field.Name = "Category";
			field.Type = category;
			field.Mult = Multiplicity.ManyToOne;
			field.IsRequired = true;
			message.Fields.Add(field);

			field = new EntityField();
			field.Name = "Principal";
			field.Type = user;
			field.Mult = Multiplicity.ManyToOne;
			field.IsRequired = true;
			message.Fields.Add(field);

			field = new EntityField();
			field.Name = "Messages";
			field.Type = message;
			field.Mult = Multiplicity.OneToMany;
			field.InfoOnly = true;
			user.Fields.Add(field);

			field = new EntityField();
			field.Name = "Messages";
			field.Type = message;
			field.InfoOnly = true;
			field.Mult = Multiplicity.OneToMany;
			category.Fields.Add(field);

			/** ImageSms **/
			EntityClass imageSms = new EntityClass();
			imageSms.Name = "ImageSms";
			imageSms.Visibility = "public";
			imageSms.Parent = message;

			field = new EntityField();
			field.Name = "ImageUrl";
			field.Type = new Loki.DataRepresentation.IntrinsicEntities.String();
			field.IsRequired = true;
			field.Default = "#";
			imageSms.Fields.Add(field);

			/** TextSms **/
			EntityClass textSms = new EntityClass();
			textSms.Name = "TextSms";
			textSms.Visibility = "public";
			textSms.Parent = message;

			field = new EntityField();
			field.Name = "Text";
			field.Type = new Loki.DataRepresentation.IntrinsicEntities.String();
			field.IsRequired = true;
			field.Default = "Empty";
			textSms.Fields.Add(field);

			/** Setting up project **/
			Model list = new Model();
			list.Add(category);
			list.Add(iDescription);
			list.Add(user);
			list.Add(message);
			list.Add(imageSms);
			list.Add(textSms);
			smsTestModel.Model = list;
		}

		#endregion

	};
}
