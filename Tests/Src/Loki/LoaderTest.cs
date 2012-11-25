using System;
using System.Collections.Generic;
using System.Text;
using Odin.Core;
using NUnit.Framework;
using Loki.DataRepresentation;
using Loki.DataRepresentation.Loaders;
using System.IO;
using Loki.Generic;

namespace Midgard.Tests {

	[TestFixture()]
	public class LoaderTest {

		private Project project = new Project("Sms", "../../Tools/Examples/Sms/Project/", "Midgard","");

		public LoaderTest() {
			//Loader.Xmi.XmiLoader.Instance.Init( project );
			//Loader.Xmi.XmiLoader.Instance.Load();
		}

		[Test]
		public void EntityCount() {
			Assert.AreEqual( 6, project.Model.Count, "6 Entities were expected!" );
		}

		[Test]
		public void UserTest() {
			EntityClass entity = project.Model[0] as EntityClass;

			//Class stuff
			Assert.AreEqual( "User", entity.Name, "Class Name should be 'User'!" );
			Assert.AreEqual( "public", entity.Visibility, "Class Visibility should be 'public'!" );
			Assert.AreEqual( false, entity.IsAbstract, "Class should not be abstract!" );

			//Attribute Stuff
			Assert.AreEqual( 2, entity.Fields.Count, "Person should Have 2 fields" );

			EntityField id = entity.Fields[0];
			Assert.AreEqual( "id", id.Name, "Field id has an incorrect Name" );
			Assert.AreEqual( "private", id.Visibility, "Field id has an incorrect modifier" );
			Assert.AreEqual( "int", id.Type.Name, "Field id has an incorrect type" );

			EntityField messages = entity.Fields[1];
			Assert.AreEqual( "_10_5_1dc00e8_1135161054677_14851_167", messages.Id, "Field messages has an incorrect Id" );
			Assert.AreEqual( "messages", messages.Name, "Field messages has an incorrect Name" );
			Assert.AreEqual( "private", messages.Visibility, "Field messages has an incorrect modifier" );
			Assert.AreEqual( Multiplicity.OneToMany, messages.Mult, "Field messages has an incorrect Multiplicity" );
			Assert.AreEqual( "Sms", messages.Type.Name, "Field messages has an incorrect type" );
			Assert.IsNotNull( messages.ReferenceType, "Field messages should have a ReferenceType" );

			//Method Stuff
			Assert.AreEqual( 0, entity.Methods.Count, "0 Method were expected" );			
		}

		[Test]
		public void SmsTest() {
			EntityClass entity = project.Model[1] as EntityClass;

			//Class stuff
			Assert.AreEqual( "Sms", entity.Name, "Class Name should be 'Sms'!" );
			Assert.AreEqual( "public", entity.Visibility, "Class Visibility should be 'public'!" );
			Assert.AreEqual( false, entity.IsAbstract, "Class should not be abstract!" );

			//Implemented Interfaces
			Assert.AreEqual( 1, entity.Interfaces.Count, "Sms should have 1 Interface" );

			EntityInterface iDescription = entity.Interfaces[0];

			Assert.AreEqual( "IDescription", iDescription.Name, "Sms interface name is incorrect" );
			Assert.AreEqual( "_10_5_1dc00e8_1135161125319_156036_314", iDescription.Id, "Sms interface id is incorrect" );

			//Attribute Stuff
			Assert.AreEqual( 4, entity.Fields.Count, "Sms should Have 4 fields" );

			EntityField id = entity.Fields[0];
			Assert.AreEqual( "id", id.Name, "Field id has an incorrect Name" );
			Assert.AreEqual( "private", id.Visibility, "Field id has an incorrect modifier" );
			Assert.AreEqual( "int", id.Type.Name, "Field id has an incorrect type" );
			Assert.IsNull( id.ReferenceType, "Field id should not have a ReferenceType" );
			
			EntityField description = entity.Fields[1];
			Assert.AreEqual( "_10_5_1dc00e8_1135160692997_580873_136", description.Id, "Field size has an incorrect Id" );
			Assert.AreEqual( "description", description.Name, "Field description has an incorrect Name" );
			Assert.AreEqual( "private", description.Visibility, "Field description has an incorrect modifier" );
			Assert.AreEqual( "string", description.Type.Name, "Field description has an incorrect type" );
			Assert.IsNull( description.ReferenceType, "Field description should not have a ReferenceType" );

			EntityField messages = entity.Fields[2];
			Assert.AreEqual( "category", messages.Name, "Field messages has an incorrect Name" );
			Assert.AreEqual( "private", messages.Visibility, "Field messages has an incorrect modifier" );
			Assert.AreEqual( "Category", messages.Type.Name, "Field messages has an incorrect type" );
			Assert.AreEqual( true, messages.InfoOnly, "Field messages should be InfoOnly" );
			Assert.IsNotNull( messages.ReferenceType, "Field messages should have a ReferenceType" );
			Assert.AreEqual( Multiplicity.ManyToOne, messages.Mult, "Field messages has an incorrect Multiplicity" );

			messages = entity.Fields[3];
			Assert.AreEqual( "user", messages.Name, "Field messages has an incorrect Name" );
			Assert.AreEqual( "private", messages.Visibility, "Field messages has an incorrect modifier" );
			Assert.AreEqual( "User", messages.Type.Name, "Field messages has an incorrect type" );
			Assert.AreEqual( true, messages.InfoOnly, "Field messages should be InfoOnly" );
			Assert.IsNotNull( messages.ReferenceType, "Field messages should have a ReferenceType" );
			Assert.AreEqual( Multiplicity.ManyToOne, messages.Mult, "Field messages has an incorrect Multiplicity" );

			//Method Stuff
			Assert.AreEqual( 1, entity.Methods.Count, "1 Method were expected" );

			EntityMethod method = entity.Methods[0];
			Assert.IsNotNull( method.ReturnEntity, "Return type was expected" );
			Assert.AreEqual( "ToHtml", method.Name, "Incorrect method name" );
			Assert.AreEqual( "public", method.MethodModifier, "Incorrect method modifier" );
						
			//Parameter Stuff
			Assert.AreEqual( 0, entity.Methods[0].Parameters.Count, "0 Parameters were expected" );
		}

		[Test]
		public void ImageSmsTest() {
			EntityClass entity = project.Model[2] as EntityClass;

			//Class stuff
			Assert.AreEqual( "ImageSms", entity.Name, "Class Name should be 'ImageSms'!" );
			Assert.AreEqual( "public", entity.Visibility, "Class Visibility should be 'public'!" );
			Assert.AreEqual( false, entity.IsAbstract, "Class should not be abstract!" );

			Assert.AreEqual( true, entity.HasParent, "ImageSms should have a parent" );
			Assert.AreEqual( "Sms", entity.Parent.Name, "ImageSms has um incorrect parent" );

			//Attribute Stuff
			Assert.AreEqual( 1, entity.Fields.Count, "ImageSms should have 1 Fields" );

			EntityField imageUrl = entity.Fields[0];
			Assert.AreEqual( "_10_5_1dc00e8_1135162298666_787321_407", imageUrl.Id, "Field size has an incorrect Id" );
			Assert.AreEqual( "imageUrl", imageUrl.Name, "Field imageUrl has an incorrect Name" );
			Assert.AreEqual( "private", imageUrl.Visibility, "Field imageUrl has an incorrect modifier" );
			Assert.AreEqual( "string", imageUrl.Type.Name, "Field imageUrl has an incorrect type" );

			//Method Stuff
			Assert.AreEqual( 0, entity.Methods.Count, "1 Method were expected" );
		}

		[Test]
		public void TextSmsTest() {
			EntityClass entity = project.Model[3] as EntityClass;

			//Class stuff
			Assert.AreEqual( "TextSms", entity.Name, "Class Name should be 'TextSms'!" );
			Assert.AreEqual( "public", entity.Visibility, "Class Visibility should be 'public'!" );
			Assert.AreEqual( false, entity.IsAbstract, "Class should not be abstract!" );

			Assert.AreEqual( true, entity.HasParent, "TextSms should have a parent" );
			Assert.AreEqual( "Sms", entity.Parent.Name, "TextSms has um incorrect parent" );

			//Attribute Stuff
			Assert.AreEqual( 1, entity.Fields.Count, "Person should have 1 fields" );

			EntityField text = entity.Fields[0];
			Assert.AreEqual( "_10_5_1dc00e8_1135162395655_206118_410", text.Id, "Field text has an incorrect Id" );
			Assert.AreEqual( "text", text.Name, "Field text has an incorrect Name" );
			Assert.AreEqual( "private", text.Visibility, "Field size has an incorrect modifier" );
			Assert.AreEqual( "string", text.Type.Name, "Field text has an incorrect type" );

			//Method Stuff
			Assert.AreEqual( 0, entity.Methods.Count, "1 Method were expected" );
		}

		[Test]
		public void CategoryTest() {
			EntityClass entity = project.Model[4] as EntityClass;
			
			//Class stuff
			Assert.AreEqual( "Category", entity.Name, "Class Name should be 'Category'!" );
			Assert.AreEqual( "public", entity.Visibility, "Class Visibility should be 'public'!" );
			Assert.AreEqual( false, entity.IsAbstract, "Class should not be abstract!" );

			//Attribute Stuff
			Assert.AreEqual( 3, entity.Fields.Count, "Category should Have 3 fields" );

			EntityField id = entity.Fields[0];
			Assert.AreEqual( "id", id.Name, "Field id has an incorrect Name" );
			Assert.AreEqual( "private", id.Visibility, "Field id has an incorrect modifier" );
			Assert.AreEqual( "int", id.Type.Name, "Field id has an incorrect type" );

			EntityField description = entity.Fields[1];
			Assert.AreEqual( "_10_5_1dc00e8_1135161420293_516114_378", description.Id, "Field description has an incorrect Id" );
			Assert.AreEqual( "description", description.Name, "Field description has an incorrect Name" );
			Assert.AreEqual( "private", description.Visibility, "Field description has an incorrect modifier" );
			Assert.AreEqual( "string", description.Type.Name, "Field description has an incorrect type" );

			EntityField messages = entity.Fields[2];
			Assert.AreEqual( "_10_5_46601c1_1135362050395_684076_141", messages.Id, "Field messages has an incorrect Id" );
			Assert.AreEqual( "messages", messages.Name, "Field messages has an incorrect Name" );
			Assert.AreEqual( "private", messages.Visibility, "Field messages has an incorrect modifier" );
			Assert.AreEqual( Multiplicity.OneToMany, messages.Mult, "Field messages has an incorrect Multiplicity" );
			Assert.AreEqual( "Sms", messages.Type.Name, "Field messages has an incorrect type" );
			Assert.IsNotNull( messages.ReferenceType, "Field messages should have a ReferenceType" );

			//Method Stuff
			Assert.AreEqual( 4, entity.Methods.Count, "1 Method were expected" );
			
			//Sms[] GetMessages(int start,int count)
			EntityMethod method = entity.Methods[0];
			Assert.IsNotNull( method.ReturnEntity, "Return type was expected" );
			Assert.AreEqual( "GetMessages", method.Name, "Incorrect method name" );
			Assert.AreEqual( "public", method.MethodModifier, "Incorrect method modifier" );

			Assert.AreEqual( 2, method.Parameters.Count, "2 Parameters were expected" );

			EntityParameter param = method.ReturnEntity;
			Assert.IsNotNull( param.Type, "Return should not be null" );
			Assert.AreEqual( string.Empty, param.Name, "Incorrect parameter name" );
			Assert.AreEqual( "Sms", param.Type.Name, "Incorrect parameter type" );
			Assert.AreEqual( true, param.IsReturn, "Incorrect return information" );
			Assert.AreEqual( Multiplicity.OneToMany, param.Mult, "Incorrect multiplicity" );

			param = method.Parameters[0];
			Assert.IsNotNull( param.Type, "Start should not be null" );
			Assert.AreEqual( "start", param.Name, "Incorrect parameter name" );
			Assert.AreEqual( "int", param.Type.Name, "Incorrect parameter type" );
			Assert.AreEqual( false, param.IsReturn, "Incorrect return information" );
			Assert.AreEqual( Multiplicity.OneToOne, param.Mult, "Incorrect multiplicity" );

			param = method.Parameters[1];
			Assert.IsNotNull( param.Type, "Count should not be null" );
			Assert.AreEqual( "count", param.Name, "Incorrect parameter name" );
			Assert.AreEqual( "int", param.Type.Name, "Incorrect parameter type" );
			Assert.AreEqual( false, param.IsReturn, "Incorrect return information" );
			Assert.AreEqual( Multiplicity.OneToOne, param.Mult, "Incorrect multiplicity" );

			//Sms[] GetMessages(User owner)
			method = entity.Methods[1];
			Assert.IsNotNull( method.ReturnEntity, "Return type was expected" );
			Assert.AreEqual( "GetMessages", method.Name, "Incorrect method name" );
			Assert.AreEqual( "public", method.MethodModifier, "Incorrect method modifier" );

			Assert.AreEqual( 1, method.Parameters.Count, "1 Parameter were expected" );

			param = method.ReturnEntity;
			Assert.IsNotNull( param.Type, "Return should not be null" );
			Assert.AreEqual( string.Empty, param.Name, "Incorrect parameter name" );
			Assert.AreEqual( "Sms", param.Type.Name, "Incorrect parameter type" );
			Assert.AreEqual( true, param.IsReturn, "Incorrect return information" );
			Assert.AreEqual( Multiplicity.OneToMany, param.Mult, "Incorrect multiplicity" );

			Assert.AreEqual( 1, method.Parameters.Count, "1 Parameters were expected" );
			param = method.Parameters[0];
			Assert.IsNotNull( param.Type, "Owner should not be null" );
			Assert.AreEqual( "owner", param.Name, "Incorrect parameter name" );
			Assert.AreEqual( "User", param.Type.Name, "Incorrect parameter type" );
			Assert.AreEqual( false, param.IsReturn, "Incorrect return information" );
			Assert.AreEqual( Multiplicity.OneToOne, param.Mult, "Incorrect multiplicity" );

			//Sms[] GetMessages(User owner)
			method = entity.Methods[2];
			Assert.IsNotNull( method.ReturnEntity, "Return type was expected" );
			Assert.AreEqual( "GetMessages", method.Name, "Incorrect method name" );
			Assert.AreEqual( "public", method.MethodModifier, "Incorrect method modifier" );

			Assert.AreEqual( 1, method.Parameters.Count, "1 Parameter were expected" );

			param = method.ReturnEntity;
			Assert.IsNotNull( param.Type, "Return should not be null" );
			Assert.AreEqual( string.Empty, param.Name, "Incorrect parameter name" );
			Assert.AreEqual( "Sms", param.Type.Name, "Incorrect parameter type" );
			Assert.AreEqual( true, param.IsReturn, "Incorrect return information" );
			Assert.AreEqual( Multiplicity.OneToMany, param.Mult, "Incorrect multiplicity" );

			Assert.AreEqual( 1, method.Parameters.Count, "1 Parameters were expected" );
			param = method.Parameters[0];
			Assert.IsNotNull( param.Type, "Owner should not be null" );
			Assert.AreEqual( "owner", param.Name, "Incorrect parameter name" );
			Assert.AreEqual( "User", param.Type.Name, "Incorrect parameter type" );
			Assert.AreEqual( false, param.IsReturn, "Incorrect return information" );
			Assert.AreEqual( Multiplicity.OneToMany, param.Mult, "Incorrect multiplicity" );

			//Categories GetCategories(User owner)
			method = entity.Methods[3];
			Assert.IsNotNull( method.ReturnEntity, "Return type was expected" );
			Assert.AreEqual( "GetCategories", method.Name, "Incorrect method name" );
			Assert.AreEqual( "public", method.MethodModifier, "Incorrect method modifier" );

			param = method.ReturnEntity;
			Assert.IsNotNull( param.Type, "Return should not be null" );
			Assert.AreEqual( string.Empty, param.Name, "Incorrect parameter name" );
			Assert.AreEqual( "Category", param.Type.Name, "Incorrect parameter type" );
			Assert.AreEqual( true, param.IsReturn, "Incorrect return information" );
			Assert.AreEqual( Multiplicity.OneToMany, param.Mult, "Incorrect multiplicity" );

			Assert.AreEqual( 0, method.Parameters.Count, "0 Parameters were expected" );
		}


		[Test]
		public void IDescriptionTest() {
			EntityInterface ientity = project.Model[5] as EntityInterface;
			
			//Class stuff
			Assert.AreEqual( "IDescription", ientity.Name, "Interface Name should be 'IDescription'!" );
			Assert.AreEqual( "public", ientity.Visibility, "Class Visibility should be 'public'!" );
		}

		[Test]
		public void CompareLoadAndGlobal() {
			Project p = Globals.SmsTestProject;
			Entity e0 = p.Model[0];
			Entity e1 = p.Model[1];
			Entity e2 = p.Model[2];
			Entity e3 = p.Model[3];
			Entity e4 = p.Model[4];
		}
	}
}