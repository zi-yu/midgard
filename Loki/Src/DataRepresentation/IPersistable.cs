using System;
using System.Collections.Generic;
using System.Text;

namespace Loki.DataRepresentation {
	public interface IPersistable<T> {
		IPersistance<T> PersistanceManager { get; }
	}
}
