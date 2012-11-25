using System;
using System.Collections.Generic;
using Loki.Generic;

namespace Loki.Interfaces {
	public interface ILoader : IPlugin {
		void Init( IProject project, IDependencyManager dependencies );
		void Load();
	}
}
