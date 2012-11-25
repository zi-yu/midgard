using System.Xml;

namespace Loader.Xmi {
	internal interface IXmiResolver {
		void ResolveConflits( XmlNode node );
	}
}
