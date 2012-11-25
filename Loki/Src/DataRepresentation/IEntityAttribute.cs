using System;
using System.Collections.Generic;
using System.Text;
using Loki.DataRepresentation;

namespace Loki.DataRepresentation {
	public interface IEntityAttribute: IEntity {
		Entity Type { get;set; }
		Multiplicity Mult { get;set; }
	}
}
