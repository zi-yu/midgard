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
using Loki.Generic;
using Loki.Exceptions;
using DesignPatterns;
using Loki.Interfaces;
using Loki.DataRepresentation.IntrinsicEntities;

namespace Loki.DataRepresentation.Loaders {
	public class IntrinsicTypes {

		#region Static Fields

		private static FactoryContainer intrinsicTypes = new FactoryContainer( typeof( IntrinsicFactory ) );
		
		#endregion

		#region Public

		public static Entity Create( string type ) {
            try {
                return (Entity)((IntrinsicFactory)intrinsicTypes[type]).create(null);
            } catch (Exception ex) {
                throw new UnknownIntrinsicTypeException(type );
            }
		}

		public static System.Collections.ICollection Values {
			get{ return intrinsicTypes.Values; }
		}

		#endregion

		
	};
}