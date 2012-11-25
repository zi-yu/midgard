using System;
using System.Collections.Generic;
using System.Text;

namespace Quartz {
    public interface INode {
        string Name { get;set; }
        string Label { get;set; }
		string NameSpace { get; }
		string Assembly { get; }
		List<Transiction> Transictions { get; }
		Dictionary<string, string> Extended { get; }
			
		void SetMoniker( string moniker );
		void AddExtended(string key, string value);
		void AddTransiction(Transiction t);
    }
}
