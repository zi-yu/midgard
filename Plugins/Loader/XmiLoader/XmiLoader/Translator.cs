using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Loki.DataRepresentation.IntrinsicEntities;
using System.Configuration;

namespace Loader.Xmi {

	internal class Translator {

		private static Dictionary<string, string> dictionary = new Dictionary<string, string>();

		static Translator() {
				//dictionary.Add( "int","System.Int32" );
				//dictionary.Add( "int16","System.Int16" );
				//dictionary.Add( "int32","System.Int32" );
				dictionary.Add( "uml standard profile::uml standard profile::datatypes::int", "System.Int32" );
				dictionary.Add( "uml standard profile::uml standard profile::datatypes::integer", "System.Int32" );
				dictionary.Add( "uml standard profile::uml standard profile::datatypes::boolean", "System.Boolean" );
				//dictionary.Add( "bool","System.Boolean" );
				//dictionary.Add( "boolean","System.Boolean" );
				//dictionary.Add( "system.boolean","System.Boolean" );
				//dictionary.Add( "string","System.String" );
				//dictionary.Add( "system.string","System.String" );
				dictionary.Add( "uml standard profile::uml standard profile::datatypes::string", "System.String" );
				//dictionary.Add( "java.lang.string","System.String" );
				//dictionary.Add( "double", "System.Double" );
				dictionary.Add( "uml standard profile::uml standard profile::datatypes::double", "System.Double" );
				dictionary.Add( "uml standard profile::uml standard profile::datatypes::float", "float" );
				dictionary.Add( "uml standard profile::uml standard profile::datatypes::date", "System.DateTime" );
		}
		

		public static string Translate( string type ) {
			if( dictionary.ContainsKey( type ) ) {
				return dictionary[type.ToLower()];
			} else { 
			
			}
			return type;
		}

		public override string ToString() {
			return dictionary.Count.ToString();
		}

	}
}
