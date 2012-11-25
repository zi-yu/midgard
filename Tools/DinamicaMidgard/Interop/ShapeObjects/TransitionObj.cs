using System;
using System.Collections.Generic;
using System.Text;

namespace Midgard.Interop.ShapeObjects
{
    public class TransitionObj
    {
        private string label, connector, origin, destiny;

        public string Destiny
        {
            get { return destiny; }
            set { destiny = value; }
        }

        public string Origin
        {
            get { return origin; }
            set { origin = value; }
        }

        public string Connector
        {
            get { return connector; }
            set { connector = value; }
        }

        public string Label
        {
            get { return label; }
            set { label = value; }
        }
    }
}
