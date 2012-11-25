using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Loki.Exceptions;
using Loki.DataRepresentation;

namespace Loader.Xmi {
	internal class EntityAttributeResolver : EntityResolver {

		#region Constructor

		public EntityAttributeResolver( IEntityAttribute entity ) : base( entity )
			{}

		#endregion

		#region Public

		public override void ResolveConflits( XmlNode node ) {
			XmiLoader.Instance.ResolveType( node, (IEntityAttribute)entity, false );
			if( null == ( (IEntityAttribute)entity ).Type ) {
				throw new UnknownTypeException( ( (EntityParameter)entity ).Name );
			}
			entity.Resolved = true;
		}

		#endregion
	}
}
