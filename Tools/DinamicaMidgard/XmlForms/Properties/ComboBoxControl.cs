using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using Genghis.Windows.Forms;
using System;


namespace Midgard.XmlForms
{
	/// <summary>
	/// Allows for configuration input.
	/// </summary>
	public class ComboBoxControl : BaseProperty, IXmlProperty
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private Container components = null;
		private ComboBox valueControl;
		private Label labelControl;

        private RequiredFieldValidator req;

        private XmlElement _definition;
        //private OperacoesInternas opr;
		/// <summary>
		/// Initializes a new insance of the ComboBoxControl class.
		/// </summary>
		/// <param name="definition">Definition of the property.</param>
		/// <param name="mode">The current design mode.</param>
        public ComboBoxControl(XmlElement definition, string mode)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            //opr = _opr;
			#region Use Standard Sizes

			labelControl.Top = Property.LabelTop;
			labelControl.Left = Property.LabelLeft;
			labelControl.Width = Property.LabelWidth;
			valueControl.Top =  Property.ValueTop;
			valueControl.Left = Property.ValueLeft;
			valueControl.Width = Property.ValueWidth;

			#endregion

            _definition = definition;

			// Initialize control
            Initialize(_definition);

			labelControl.Text = this.Label;
			valueControl.Items.Clear();

            /*
             * Check required
             */
            if (IsRequired(_definition, mode) == true)
            {
                req = new RequiredFieldValidator();
                req.ControlToValidate = this.valueControl;
                req.ControlToValidate.Validating += new CancelEventHandler(ControlToValidate_Validating);
                req.ErrorMessage = "Required.";
                req.IconPadding = 2;
                this.valueControl.BackColor = Resources.GetColor("required");
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

        private bool _dataBinded = false;
        private void EnsureDataBind()
        {
            if (!_dataBinded)
            {
                if (_definition.Attributes["xPath"] != null)
                {
                    if (_definition.Attributes["configFile"] != null)
                    {
                        ComboBoxObject[] cgi = ComboBoxUtils.Search(_definition.Attributes["xPath"].Value, _definition.Attributes["configFile"].Value);

                        if (cgi != null)
                        {
                            int i, len = cgi.Length;
                            for (i = 0; i < len; i++)
                            {
                                valueControl.Items.Add(cgi[i]);
                            }
                        } 
                    }
                }
                labelControl.Height = valueControl.Height;
                _dataBinded = true;
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            EnsureDataBind();
            int selIdx = 0;
            if (valueControl.SelectedIndex < 0 && _definition.Attributes["SelectedIndex"] != null && int.TryParse(_definition.Attributes["SelectedIndex"].Value, out selIdx))
            {
                if (valueControl.Items.Count > selIdx)
                valueControl.SelectedIndex = selIdx;
            }
            string cgiCode;
            if (valueControl.Items.Count > 0)
            {
                if (valueControl.SelectedItem == null)
                    cgiCode = ((ComboBoxObject)valueControl.Items[0]).Code;
                else
                    cgiCode = ((ComboBoxObject)valueControl.SelectedItem).Code;
                DoManagedNodes(cgiCode);
            }
        }

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.labelControl = new System.Windows.Forms.Label();
            this.valueControl = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // labelControl
            // 
            this.labelControl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelControl.Location = new System.Drawing.Point(0, 4);
            this.labelControl.Name = "labelControl";
            this.labelControl.Size = new System.Drawing.Size(100, 14);
            this.labelControl.TabIndex = 0;
            this.labelControl.Text = "(Label)";
            this.labelControl.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.PropertyHelpRequested);
            // 
            // valueControl
            // 
            this.valueControl.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.valueControl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.valueControl.Location = new System.Drawing.Point(96, 0);
            this.valueControl.Name = "valueControl";
            this.valueControl.Size = new System.Drawing.Size(256, 21);
            this.valueControl.TabIndex = 1;
            this.valueControl.SelectedIndexChanged += new System.EventHandler(this.valueControl_SelectedIndexChanged);
            this.valueControl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.valueControl_KeyPress);
            this.valueControl.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.PropertyHelpRequested);
            // 
            // ComboBoxControl
            // 
            this.Controls.Add(this.valueControl);
            this.Controls.Add(this.labelControl);
            this.Name = "ComboBoxControl";
            this.Size = new System.Drawing.Size(368, 48);
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
		}

        /// <summary>
        /// Fills the XML form data based on a value.
        /// </summary>
        /// <param name="value"></param>
        public override void Set(string value, bool required, bool disabled, bool invisible, bool forceReplacement)
        {
            EnsureDataBind();

            ComboBoxObject cgi;
            bool found = false;
            int i, len = valueControl.Items.Count;

            if (valueControl.Text.Replace(" ", "").Length == 0 || forceReplacement)
            {
                for (i = 0; i < len; i++)
                {
                    cgi = (ComboBoxObject)valueControl.Items[i];
                    if (value == cgi.Code)
                    {
                        valueControl.SelectedIndex = i;
                        i = len;
                        found = true;
                    }
                }
                if (found == false)
                {
                    valueControl.SelectedIndex = -1;
                    valueControl.Text = value;
                }
            }

            if (required)
            {
                req = new RequiredFieldValidator();
                req.ControlToValidate = this.valueControl;
                req.ControlToValidate.Validating += new CancelEventHandler(ControlToValidate_Validating);
                req.ErrorMessage = "Required.";
                req.IconPadding = 2;
            }


            this.valueControl.Enabled = !disabled;

            if (this.valueControl.Parent is ComboBoxControl)
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
			if ( valueControl.Items.Count == 0 )
				return false;

            ApplyChangeShapeRules();
			if(valueControl.SelectedIndex<0)
			{
                if (valueControl.Text != "")
                {
                    element.InnerText = valueControl.Text;
                    return true;
                }
				return false;
			} 
			else
			{
				ComboBoxObject Combo=(ComboBoxObject)valueControl.SelectedItem;
                if (Combo.Code == string.Empty)
					return false;

                element.InnerText = Combo.Code;
                element.SetAttribute("Name", Combo.Name);
			}
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

        private void ControlToValidate_Validating(object sender, CancelEventArgs e)
        {

            if (this.valueControl.Items.Count > 0 && this.valueControl.SelectedIndex < 0)
                this.valueControl.SelectedIndex = 0;
        }

        /// <summary>
        /// Evaluate and execute automatic field binding
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void valueControl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            string ComboCode;
            if (valueControl.SelectedItem == null)
                ComboCode = ((ComboBoxObject)valueControl.Items[0]).Code;
            else
                ComboCode = ((ComboBoxObject)valueControl.SelectedItem).Code;
            DoManagedNodes(ComboCode);
        }

        private void valueControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = false;
            if (e.KeyChar != '\b' && _mask != null)
            {
                if (!_mask.Match(e.KeyChar.ToString()).Success)
                    e.Handled = true;
            }
        }

        protected override string GetCheckForValue()
        {
            return (valueControl.SelectedIndex != -1) ? ((ComboBoxObject) valueControl.SelectedItem).Code : "";
        }
	}
}