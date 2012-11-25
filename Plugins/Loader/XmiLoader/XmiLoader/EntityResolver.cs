using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Loki.DataRepresentation;

namespace Loader.Xmi {
	internal class EntityResolver : IXmiResolver {

		#region Fields

		protected IEntity entity = null;

		#endregion

		#region Constructor
		
		public EntityResolver( IEntity entity ) {
			this.entity = entity;
		}

		#endregion

		#region Public

		public virtual void ResolveConflits( XmlNode node ) {
			XmlAttribute supplier = node.Attributes["supplier"];
			if( null != supplier ) {
				if( XmiLoader.Instance.DataTypes.ContainsKey( supplier.Value ) ) {
					( (Entity)entity ).Interfaces.Add( (EntityInterface)XmiLoader.Instance.DataTypes[supplier.Value] );
				}
			}
		}

		#endregion
	}
}
