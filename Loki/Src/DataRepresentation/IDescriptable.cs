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

namespace Loki.DataRepresentation {

	public interface IDescriptable {

		string ToString();
		string ToHtml();
		string ToXml();
		string ToLog();
        string ToJson();

		int Id { get;}
		string TypeName { get;}

	};
}