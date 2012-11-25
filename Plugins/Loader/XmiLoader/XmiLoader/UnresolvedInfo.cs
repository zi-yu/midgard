using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;

namespace Loader.Xmi {
	internal class UnresolvedInfo {

		#region Private Fields

		private XmlNode node;
		private IXmiResolver resolver;

		#endregion

		#region Properties

		public XmlNode Node {
			get { return node; }
		}

		public IXmiResolver Resolver {
			get { return resolver; }
		} 
		
		#endregion

		#region Constructor

		public UnresolvedInfo( XmlNode node, IXmiResolver resolver ) {
			this.node = node;
			this.resolver = resolver;
		}
		
		#endregion

	}
}
