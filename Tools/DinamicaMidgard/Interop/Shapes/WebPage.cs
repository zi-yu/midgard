using System;
using System.Collections.Generic;
using System.Text;
using Midgard.XmlForms;
using System.Xml;
using System.Windows.Forms;
using System.Collections;

namespace Midgard.Interop.Shapes
{
    public class WebPage : BaseShape, IShapeHandler
    {
        /// <summary>
		/// Initializes a new instance of the NodeShape class.
		/// </summary>
        public WebPage(object shape, XmlForm form, XmlElement attributes)
            : base(shape, form, attributes)
		{
		}


        /// <summary>
        /// Performs context validation on the current shape.
        /// </summary>
        /// <param name="validationData">Context dependent validation data.</param>
        /// <returns>A non-empty string in case of error, otherwise null.</returns>
        protected override string DoContextValidation(object validationData)
        {
            StringBuilder sb = new StringBuilder();

            object[] o = (object[])validationData;
            ArrayList from = (ArrayList)o[0];
            ArrayList to = (ArrayList)o[1];

            int controlVar = 0;

            if (to != null && to.Count > 0)
                ++controlVar;

            if (from != null && from.Count > 0)
                ++controlVar;

            if( controlVar == 0)
                sb.Append(Resources.GetString(ResourceTokens.NodeShapeNotConnected));

            return sb.ToString();
        }

    }
}
