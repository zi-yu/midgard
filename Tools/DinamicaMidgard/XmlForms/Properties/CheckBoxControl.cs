using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Genghis.Windows.Forms;

namespace Midgard.XmlForms
{
	/// <summary>
	/// Check-box.
	/// </summary>
	public class CheckBoxControl : BaseProperty, IXmlProperty
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		private System.Windows.Forms.CheckBox valueControl;

        private XmlElement _definition;

        private RequiredFieldValidator req;

        //private OperacoesInternas opr;
        private bool userSelected = false;

		#region True/False Constants

		/// <summary>
		/// True string.
		/// </summary>
		public const string TrueString = "true";

		/// <summary>
		/// False string.
		/// </summary>
		public const string FalseString = "false";

		#endregion

		/// <summary>
		/// Initializes a new insance of the StringControl class.
		/// </summary>
		/// <param name="definition">Definition of the property.</param>
		/// <param name="mode">The current design mode.</param>
		public CheckBoxControl( ref XmlElement definition, string mode)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			#region Use Standard Sizes

			valueControl.Top =  Property.ValueTop;
			valueControl.Left = Property.ValueLeft;
			valueControl.Width = Property.ValueWidth;
			this.Height = valueControl.Height + Property.PropertyMarginBottom; 

			#endregion
            //opr = _opr;
            _definition = definition;

			// Initialize control
            Initialize(_definition);

			valueControl.Text = this.Label;

            if (_definition.Attributes["checked"] != null && _definition.Attributes["checked"].Value == "true")
                valueControl.Checked = true;

            /*
             * Check required
             */
            if (IsRequired(_definition, mode) == true)
            {
                req = new RequiredFieldValidator();
                req.ControlToValidate = this.valueControl;
                req.ErrorMessage = Resources.GetString(ResourceTokens.ValidatorRequired);
                req.IconPadding = 2;
            }
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if ( disposing )
			{
				if ( components != null )
					components.Dispose();

                if (req != null)
                {
                    req.ControlToValidate.Dispose();
                    req.Icon.Dispose();
                    req.Dispose();
                }
			}
			base.Dispose( disposing );
		}

        protected override void OnLoad(EventArgs e)
        {
            DoManagedNodes(valueControl.Checked.ToString());
        }

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.valueControl = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // valueControl
            // 
            this.valueControl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.valueControl.Location = new System.Drawing.Point(104, 0);
            this.valueControl.Name = "valueControl";
            this.valueControl.Size = new System.Drawing.Size(240, 20);
            this.valueControl.TabIndex = 2;
            this.valueControl.Text = "checkBox1";
            this.valueControl.CheckedChanged += new System.EventHandler(this.valueControl_CheckedChanged);
            // 
            // CheckBoxControl
            // 
            this.Controls.Add(this.valueControl);
            this.Name = "CheckBoxControl";
            this.Size = new System.Drawing.Size(368, 24);
            this.ResumeLayout(false);

		}

		#endregion


		/// <summary>
		/// Fills the XML form data based on the serialization data.
		/// </summary>
		/// <param name="element">Element serialization.</param>
		public void Set( XmlElement element )
		{
            Set(element.InnerText, false, false, false, false);

            bool b = false;
            if (element.Attributes["userSelected"] != null)
            {
                if (element.InnerText == TrueString)
                    b = true;
                else if (element.InnerText == FalseString)
                    b = false;

                valueControl.Checked = b;
            }
		}

        /// <summary>
        /// Fills the XML form data based on a value.
        /// </summary>
        /// <param name="value"></param>
        public override void Set(string value, bool required, bool disabled, bool invisible, bool forceReplacement)
        {
            bool b = false;

            if (!userSelected || forceReplacement)
            {
                if (value == TrueString)
                    b = true;
                else if (value == FalseString)
                    b = false;

                valueControl.Checked = b;
            }

            if (required)
            {
                req = new RequiredFieldValidator();
                req.ControlToValidate = this.valueControl;
                req.ErrorMessage = "Required.";
                req.IconPadding = 2;
            }

            this.valueControl.Enabled = !disabled;

            if (this.valueControl.Parent is CheckBoxControl)
                this.valueControl.Parent.Visible = !invisible;
            else
                this.valueControl.Visible = !invisible;
            
        }

		/// <summary>
		/// Fills the destination element with the serialization data for the
		/// property.
		/// </summary>
		/// <param name="element">Element serialization.</param>
		/// <returns>True if the element is to be append to the shape XML, false otherwise.</returns>
		public bool Fill( XmlElement element )
		{
            if (userSelected)
            {
                XmlAttribute att = element.OwnerDocument.CreateAttribute("userSelected");
                att.Value = "true";
                element.Attributes.Append(att);
            }
			element.InnerText = ( valueControl.Checked == true ) ? TrueString : FalseString;
            ApplyChangeShapeRules();
			return true;
		}


		/// <summary>
		/// Displays help, if available.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="hlpevent">Event arguments.</param>
		public void PropertyHelpRequested( object sender, HelpEventArgs hlpevent )
		{
			SetHelpTooltip( (Control) sender, this.Annotation );
		}

   	 protected override string GetCheckForValue()
        {
            return (valueControl.Checked == true) ? TrueString : FalseString;
        }

        private void valueControl_CheckedChanged(object sender, EventArgs e)
        {
            if (!userSelected)
            {
                userSelected = true;
                //XmlAttribute att = _definition.OwnerDocument.CreateAttribute("userSelected");
                //att.Value = "true";
                //_definition.Attributes.Append(att);
            }
            DoManagedNodes(valueControl.Checked.ToString());
        }
	}
}