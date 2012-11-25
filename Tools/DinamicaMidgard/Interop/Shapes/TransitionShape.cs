using System;
using System.Collections;
using System.Text;
using System.Xml;
using Midgard.XmlForms;

namespace Midgard.Interop.Shapes
{
    /// <summary>
    /// Shape handler for event shapes.
    /// </summary>
    public class TransitionShape : BaseShape, IShapeHandler
    {
        /// <summary>
        /// Initializes a new instance of the EventShape class.
        /// </summary>
        public TransitionShape(object shape, XmlForm form, XmlElement attributes)
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

            Hashtable handlers = (Hashtable)validationData;

            if (VisioShape.Connects.Count != 2)
            {
                sb.Append(Resources.GetString(ResourceTokens.TransitionNotConnected));
            }
            else
            {
                IShapeHandler from = (IShapeHandler)handlers[VisioShape.Connects[1].ToCell.Shape];
                IShapeHandler to = (IShapeHandler)handlers[VisioShape.Connects[2].ToCell.Shape];

                if (from == null)
                    sb.Append(Resources.GetString(ResourceTokens.TransitionConnectedFromInvalidShape));
                else if (from.Type == ModelShapeType.Transition)
                    sb.Append(Resources.GetString(ResourceTokens.TransitionConnectedFromTransitionShape));

                if (to == null)
                    sb.Append(Resources.GetString(ResourceTokens.TransitionConnectedToInvalidShape));
                else if (to.Type == ModelShapeType.Transition)
                    sb.Append(Resources.GetString(ResourceTokens.TransitionConnectedToTransitionShape));
            }

            return sb.ToString();
        }

        public override bool PaintAsValid()
        {
            return VisioUtils.PaintColoredTransition(VisioShape, this.Attributes.SelectNodes("paint", FormsNamespace.NamespaceManager));
        }

        protected override void SetShapeText(string text)
        {
            VisioShape.get_Cells("LockTextEdit").ResultIU = 0;
            VisioShape.Text = text;
            VisioShape.get_Cells("LockTextEdit").ResultIU = 1;
        }

    }
}
