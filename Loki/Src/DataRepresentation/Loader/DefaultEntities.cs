using System;
using System.Collections.Generic;
using System.Text;
using Loki.Interfaces;

namespace Loki.DataRepresentation.Loaders {
	public class DefaultEntities {

		#region Fields

		private static Entity stringType = IntrinsicTypes.Create( "System.String" );
		private static Entity intType = IntrinsicTypes.Create( "System.Int32" );
		private static Entity boolType = IntrinsicTypes.Create( "System.Boolean" );
		private static Entity dateTimeType = IntrinsicTypes.Create( "System.DateTime" );

		#endregion

		#region CreateDefaultEntities

		private static void CreatePrincipalEntity( IProject project ) {
			EntityClass principal = project.GetEntity( "Principal" );
			if( principal == null ) {
				principal = new EntityClass( "Principal", "public" );

				EntityField id = new EntityField( "id" );
				id.IsPrimaryKey = true;
				id.Type = intType;
                id.IsPreview = true;
				
				principal.AddField( id );
				project.Model.Add( principal );
			}

			principal.Lazy = true;

			if( !principal.HasField( "name" ) ) {
				EntityField name = new EntityField( "name" );
				name.MaxSize = 200;
				name.IsRequired = true;
                name.IsPreview = true;
                name.Represents = true;
				name.Type = stringType;
				principal.AddField( name );
			}

			if( !principal.HasField( "password" ) ) {
				EntityField password = new EntityField( "password" );
				password.MaxSize = 50;
				password.IsRequired = true;
				password.Type = stringType;
				password.Secret = true;
				principal.AddField( password );

			}

			if( !principal.HasField( "email" ) ) {
				EntityField mail = new EntityField( "email" );
				mail.MaxSize = 200;
                mail.IsPreview = true;
				mail.IsRequired = true;
				mail.Type = stringType;
				//mail.Regex.Add(
				mail.Unique = true;
				principal.AddField( mail );
			}

			if( !principal.HasField( "ip" ) ) {
				EntityField ip = new EntityField( "ip" );
				ip.MaxSize = 15;
				ip.Type = stringType;
                ip.IsPreview = true;
				//ip.Regex.Add(
				principal.AddField( ip );
			}

			if( !principal.HasField( "registDate" ) ) {
				EntityField registDate = new EntityField( "registDate" );
				registDate.IsRequired = true;
				registDate.Type = dateTimeType;
				//principal.Regex.Add(
				principal.AddField( registDate );
			}

			if( !principal.HasField( "lastLogin" ) ) {
				EntityField lastLogin = new EntityField( "lastLogin" );
				lastLogin.IsRequired = true;
				lastLogin.Type = dateTimeType;
				//principal.Regex.Add(
				principal.AddField( lastLogin );
			}

			if( !principal.HasField( "approved" ) ) {
				EntityField approved = new EntityField( "approved" );
				approved.IsRequired = true;
				approved.Type = boolType;
				approved.Default = false;
				principal.AddField( approved );
			}

			if( !principal.HasField( "isOnline" ) ) {
				EntityField isOnline = new EntityField( "isOnline" );
				isOnline.Type = boolType;
				isOnline.Default = false;
				principal.AddField( isOnline );
			}

			if( !principal.HasField( "locked" ) ) {
				EntityField locked = new EntityField( "locked" );
				locked.Type = boolType;
				locked.Default = false;
				principal.AddField( locked );
			}

			if( !principal.HasField( "locale" ) ) {
				EntityField locale = new EntityField( "locale" );
				locale.Type = stringType;
				locale.IsRequired = true;
				locale.MaxSize = 6;
				principal.AddField( locale );
			}

			if( !principal.HasField( "roles" ) ) {
				EntityField role = new EntityField( "roles" );
				role.Type = CreateRoleEntity( project, principal );
				role.Mult = Multiplicity.ManyToMany;
                role.Lazy = false;

				principal.AddField( role );
			}

			if( !principal.HasField("confirmationCode") ) {
				EntityField confirmationCode = new EntityField("confirmationCode");
				confirmationCode.Type = stringType;
				confirmationCode.IsRequired = true;

				principal.AddField(confirmationCode);
			}

			EntityInterface iPrincipal = new EntityInterface();
			iPrincipal.Name = "System.Security.Principal.IPrincipal";

			principal.Interfaces.Add( iPrincipal );

		}

		private static EntityClass CreateRoleEntity( IProject project, EntityClass principal ) {
			EntityClass role = new EntityClass( "Roles", "public" );

			EntityField id = new EntityField( "id" );
			id.IsPrimaryKey = true;
            id.IsPreview = true;
			id.Type = intType;

			EntityField name = new EntityField( "name" );
			name.MaxSize = 20;
			name.IsRequired = true;
            name.Represents = true;
            name.IsPreview = true;
			name.Type = stringType;

			EntityField user = new EntityField( "users" );
			user.Type = principal;
			user.Mult = Multiplicity.ManyToMany;
			user.InfoOnly = true;

			role.AddField( id );
			role.AddField( name );
			role.AddField( user );

			project.Model.Add( role );

			return role;
		}

		private static void CreateExceptionInfoEntity( IProject project ) {
			EntityClass exception = new EntityClass( "ExceptionInfo", "public" );

			EntityField id = new EntityField( "id" );
			id.IsPrimaryKey = true;
            id.IsPreview = true;
			id.Type = intType;

			EntityField name = new EntityField( "name" );
			name.MaxSize = 15000;
			name.IsRequired = true;
			name.Type = stringType;

			EntityField message = new EntityField( "message" );
            message.Represents = true;
            message.IsPreview = true;
			message.IsRequired = true;
			message.MaxSize = 15000;
			message.Type = stringType;

			EntityField date = new EntityField( "date" );
			date.Type = dateTimeType;

			EntityField exceptions = new EntityField( "exceptions" );
			exceptions.Mult = Multiplicity.OneToMany;
			exceptions.Type = exception;

			EntityField principal = null;
			principal = new EntityField( "principal" );
			principal.Mult = Multiplicity.ManyToOne;

			EntityClass principalEntity = project.GetEntity( "Principal" );
			principalEntity.Fields.Add( exceptions );
			principal.Type = principalEntity;

			EntityField url = new EntityField( "url" );
			url.Regex.Add( "$http://" );
			url.MaxSize = 15000;
			url.Type = stringType;
			EntityField stackTrace = new EntityField( "stackTrace" );
			stackTrace.MaxSize = 15000;
			stackTrace.IsRequired = true;
			stackTrace.Type = stringType;

			exception.AddField( id );
			exception.AddField( name );
			exception.AddField( message );
			exception.AddField( date );
			exception.AddField( principal );
			exception.AddField( url );
			exception.AddField( stackTrace );

			project.Model.Add( exception );
		}

		#endregion CreateDefaultEntities

		#region Public

		public static void CreateDefaultEntities( IProject project ) {
			CreatePrincipalEntity( project );
			CreateExceptionInfoEntity( project );
		}

		#endregion

	}
}
