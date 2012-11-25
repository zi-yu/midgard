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
using Loki.Exceptions;

namespace WebUtilities {
	public class ResourceRegister {

		#region Fields

		private static Dictionary<string, Dictionary<string, string>> resources = new Dictionary<string, Dictionary<string, string>>();

		#endregion

		#region Constructor

		static ResourceRegister() {
			resources["Resource"] = new Dictionary<string, string>();
		}
		
		#endregion

		#region Properties

		public static Dictionary<string, Dictionary<string, string>> Resources {
			get { return resources; }
		}

		#endregion

		#region Public Static

		/// <summary>
		/// Regista um resource
		/// </summary>
		/// <param name="name">nome do ficheiro onde o recurso irá ficar</param>
		/// <param name="key">key para indexar o recurso</param>
		/// <param name="value">valor do recurso</param>
		public static void RegisterResource( string name, string key, string value ) {
			if( !resources.ContainsKey( name ) ) {
				resources[name] = new Dictionary<string, string>();
			}
			if( !resources[name].ContainsKey( key ) ) {
				resources[name].Add( key, value );
			} else {
				throw new ResourceExists( name );
			}
		}

		/// <summary>
		/// Regista um resource no contentor por omissão
		/// </summary>
		/// <param name="key">key para indexar o recurso</param>
		/// <param name="value">valor do recurso</param>
		public static void RegisterResource( string key, string value ) {			
			if( !resources["Resource"].ContainsKey(key) ) {
				resources["Resource"].Add( key, value );
			}else{
				throw new ResourceExists( "Resource" );
			}
		}

		#endregion

	}
}
