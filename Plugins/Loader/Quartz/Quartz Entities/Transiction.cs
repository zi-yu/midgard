using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Quartz {
	public class Transiction {

		#region Fields

		protected string eventName = string.Empty;
		protected string label = string.Empty;

		#endregion

		#region Properties

		public string EventName {
			get { return eventName; }
			set { eventName = value; }
		}

		public string Label {
			get { return label; }
			set { label = value; }
		}

        public string HtmlFormatedLabel {
            get { return Label.Replace("\n", "<br/>\n\t/// "); }
        }

        public string FriendlyEventName
        {
            get
            {
                string[] words = eventName.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                string friendly = string.Empty;
                for (int i = 1; i < words.Length; ++i)
                {
                    friendly += Format(words[i]);
                }
                return friendly;
            }
        }

		#endregion

        #region Private

        private string Format(string l)
        {
            return char.ToUpper(l[0]) + l.Substring(1, l.Length - 1).ToLower();
        }

        #endregion

		#region Constructor

		public Transiction(string eventName, string label) {
			this.eventName = eventName;
			this.label = label;
		}

		#endregion
	}
}
