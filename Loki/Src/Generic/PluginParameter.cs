using System;
using System.Collections.Generic;
using System.Text;

namespace Loki.Generic {
    public class PluginParameter {
        #region Fields

        private string name;
        private string val;
        private bool needed;

        #endregion
        #region Properties

        public string Name {
            get { return name; }
            set { name = value; }
        }

        public string Value {
            get { return val; }
            set { val = value; }
        }

        public bool Needed {
            get { return needed; }
            set { needed = value; }
        }

        #endregion
        #region Ctor

        public PluginParameter( string _name, string _val, bool _needed ) {
            name = _name;
            val = _val;
            needed = _needed;
        }

        public PluginParameter( string _name, bool _needed ) : this(_name, null, _needed) {
        }

        public PluginParameter( string _name, string _val )
            : this(_name, _val, true) {
        }

        public PluginParameter( string _name )
            : this(_name, null, true) {
        }

        #endregion
    };
}
