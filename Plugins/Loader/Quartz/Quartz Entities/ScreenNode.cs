using System;
using System.Collections.Generic;
using System.Text;

namespace Quartz {
    public class ScreenNode : Node {

        #region Fields

		string style = string.Empty;
		string screen = string.Empty;

        #endregion

		#region Properties

		public string Style {
			get { return style; }
			set { style = value; }
		}

		public string Screen {
			get { return screen; }
			set { screen = value; }
		}
	
		#endregion
	}
}
