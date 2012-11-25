using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using Genghis.Windows.Forms;


namespace Midgard.XmlForms
{
    /// <summary>
    /// Summary description for StringControl.
    /// </summary>
    public class LookUpControl : ItemListControl
    {

        /// <summary>
        /// Initializes a new insance of the StringControl class.
        /// </summary>
        /// <param name="definition">Definition of the property.</param>
        /// <param name="mode">The current design mode.</param>
        public LookUpControl(XmlElement definition, string mode)
            : base(definition, mode, null)
        {
            Add.Visible = false;
            Edit.Visible = false;
            Remove.Visible = false;
            Up.Visible = false;
            Down.Visible = false;
            this.Height = 200;
            SelectionList.Width += 30;
            SelectionList.Height = this.Height - 10;
            this.SelectionList.DoubleClick -= new System.EventHandler(this.Edit_Click);

            //SelectionList.Items.Add("AAA");
            //SelectionList.Items.Add("BBB");

        }
/*
        public override bool Fill(XmlElement element)
        {
            if (SelectionList.Items.Count == 0)
                return false;
            
            foreach (string column in SelectionList.Items)
            {
                element.AppendChild(element.OwnerDocument.ImportNode(column.Column, true));
            }
             * 
            return true;
        }*/
    }


}