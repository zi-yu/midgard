using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using Genghis.Windows.Forms;
using System.Text.RegularExpressions;


namespace Midgard.XmlForms
{
	/// <summary>
	/// Summary description for StringControl.
	/// </summary>
	public class IntegerControl : BaseProperty, IXmlProperty
	{
		private System.Windows.Forms.Label labelControl;
		private System.Windows.Forms.TextBox valueControl;
		
        private RequiredFieldValidator req;
        //private OperacoesInternas opr;
		
        IntegerValidator integer=null;

        private XmlElement _definition;

        /// <summary> 
		/// Required designer variable.
		/// </summary>
		private Container components = null;

		/// <summary>
		/// Initializes a new insance of the StringControl class.
		/// </summary>
		/// <param name="definition">Definition of the property.</param>
		/// <param name="mode">The current design mode.</param>
        public IntegerControl(XmlElement definition, string mode)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			#region Use Standard Sizes

			labelControl.Top = Property.LabelTop;
			labelControl.Left = Property.LabelLeft;
			labelControl.Width = Property.LabelWidth;
			valueControl.Top =  Property.ValueTop;
			valueControl.Left = Property.ValueLeft;
			//valueControl.Width = Property.ValueWidth;
			this.Height = valueControl.Height + Property.PropertyMarginBottom; 

			#endregion
            //opr = _opr;
            _definition = definition;

			// Initialize control
			Initialize( _definition );

			labelControl.Text = this.Label;
         
            valueControl.Text = "";
             
			integer = new IntegerValidator();
			
			integer.ControlToValidate = this.valueControl;
			integer.ErrorMessage = Resources.GetString( "Value must be a valid integer" );
			integer.IconPadding = 2;

			/*
			 * Check required
			 */
			if ( IsRequired( _definition, mode ) == true )
			{
				req = new RequiredFieldValidator();
				req.ControlToValidate = this.valueControl;
				req.ErrorMessage = Resources.GetString( ResourceTokens.ValidatorRequired );
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

				if(integer!=null)
				{
					integer.ControlToValidate.Dispose();
					integer.Icon.Dispose();
					integer.Dispose();
				}

				if(req!=null)
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
            if (valueControl.Text.Replace(" ", "").Length == 0 && _definition.Attributes["value"] != null && _definition.Attributes["value"].Value.Replace(" ", "").Length > 0)
                valueControl.Text = _definition.Attributes["value"].Value;
            DoManagedNodes(valueControl.Text);
        }

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.labelControl = new System.Windows.Forms.Label();
            this.valueControl = new System.Windows.Forms.TextBox();
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
            this.valueControl.Location = new System.Drawing.Point(104, 0);
            this.valueControl.Name = "valueControl";
            this.valueControl.Size = new System.Drawing.Size(88, 20);
            this.valueControl.TabIndex = 1;
            this.valueControl.Text = "(Text)";
            this.valueControl.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.PropertyHelpRequested);
            this.valueControl.Leave += new System.EventHandler(this.valueControl_Leave);
            this.valueControl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.valueControl_KeyPress);
            // 
            // IntegerControl
            // 
            this.Controls.Add(this.valueControl);
            this.Controls.Add(this.labelControl);
            this.Name = "IntegerControl";
            this.Size = new System.Drawing.Size(368, 32);
            this.ResumeLayout(false);
            this.PerformLayout();

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
            if (valueControl.Text.Replace(" ", "").Length <= 0 || forceReplacement)
                valueControl.Text = value;

            if (required)
            {
                req = new RequiredFieldValidator();
                req.ControlToValidate = this.valueControl;
                req.ErrorMessage = "Required.";
                req.IconPadding = 2;
            }

            this.valueControl.Enabled = !disabled;

            if (this.valueControl.Parent is IntegerControl)
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
			string trim = valueControl.Text.Trim();

			if ( trim.Length == 0 )
				return false;

			element.InnerText = trim;
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


		/// <summary>
		/// Gets the string representation of the current object.
		/// </summary>
		/// <returns>String representation.</returns>
		public override string ToString()
		{
			return valueControl.Text;
		}

        void valueControl_Leave(object sender, EventArgs e)
        {
            DoManagedNodes(valueControl.Text);
        }

        private void valueControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (_mask == null)
                _mask = new Regex("[0-9]");

            if (e.KeyChar != '\b' && !_mask.Match(e.KeyChar.ToString()).Success)
                e.Handled = true;
        }
	}
}