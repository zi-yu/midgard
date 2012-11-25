using System;
using System.Collections.Generic;
using System.Text;
using Odin.Core;
using NUnit.Framework;
using Loki.DataRepresentation;
using Loki.DataRepresentation.Loaders;
using System.IO;
using Loki.Generic;

namespace Midgard.Tests {

	[TestFixture()]
	public class ConfigLoaderTest {

		private Project project = new Project("Sms", "../../Tools/Examples/Sms/Project/", "Midgard","");

		public ConfigLoaderTest() {
			//project.Load( "../../Tools/Examples/Sms/Project/Sms.xml" );
		}

		[Test]
		public void EntityCount() {
			
		}

	}
}