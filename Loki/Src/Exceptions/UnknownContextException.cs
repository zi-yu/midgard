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
	
	public class UnknownContextException : LokiException {
		public UnknownContextException( string exception ) : base( exception )
            { }

		public UnknownContextException( string exception, params object[] p )
            : base(string.Format(exception,p)) { }
	}
}
