using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Loki.DataRepresentation;

namespace Loader.Xmi {
	internal class EntityClassResolver : EntityResolver {

		#region Constructor

		public EntityClassResolver( EntityClass entity ) : base( entity )
			{}

		#endregion

		#region Public

		public override void ResolveConflits( XmlNode node ) {
			XmlAttribute general = node.Attributes["general"];
			if( null != general ) {
				if( XmiLoader.Instance.DataTypes.ContainsKey( general.Value ) ) {
					( (EntityClass)entity ).Parent = XmiLoader.Instance.DataTypes[general.Value];
				}
			}
			base.ResolveConflits( node );
		}

		#endregion
	}
}
