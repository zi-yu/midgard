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

namespace Loki.Exceptions {
	
	public class UnknownIntrinsicTypeException : LokiException {
        public UnknownIntrinsicTypeException(string typeName)
            : base(string.Format("´{0}' is a unknown primitive type!", typeName))
            { }
	}
}
