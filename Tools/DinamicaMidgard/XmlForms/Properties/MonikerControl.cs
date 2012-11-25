using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using Genghis.Windows.Forms;


namespace Midgard.XmlForms
{
	/// <summary>
	/// Input of a .NET Type Moniker.
	/// </summary>
	public class MonikerControl : BaseProperty, IXmlProperty
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private Container components = null;
		private System.Windows.Forms.Label labelControl;
		private System.Windows.Forms.TextBox valueControl;
		private Genghis.Windows.Forms.RegularExpressionValidator moniker=null;
		private RequiredFieldValidator required=null;

        //private OperacoesInternas opr;

		/// <summary>
		/// Initializes a new insance of the StringControl class.
		/// </summary>
		/// <param name="definition">Definition of the property.</param>
		/// <param name="mode">The current design mode.</param>
        public MonikerControl(XmlElement definition, string mode)
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
			this.Height = valueControl.Height + Property.PropertyMarginBottom; 

			#endregion

			// Initialize control
			Initialize( definition );

			// 
			labelControl.Text = this.Label;
			valueControl.Text = "";
			moniker.ErrorMessage = Resources.GetString( ResourceTokens.ValidatorTypeMoniker );


			/*
			 * Check required
			 */
			if ( IsRequired( definition, mode ) == true )
			{
				required = new RequiredFieldValidator();
				required.ControlToValidate = this.valueControl;
				required.ErrorMessage = Resources.GetString( ResourceTokens.ValidatorRequired );
				required.IconPadding = 2;
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

				if(moniker!=null)
				{
					moniker.ControlToValidate.Dispose();
					moniker.Icon.Dispose();
					moniker.Dispose();
				}

				if(required!=null)
				{
					required.ControlToValidate.Dispose();
					required.Icon.Dispose();
					required.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MonikerControl));
			this.labelControl = new System.Windows.Forms.Label();
			this.valueControl = new System.Windows.Forms.TextBox();
			this.moniker = new Genghis.Windows.Forms.RegularExpressionValidator();
			((System.ComponentModel.ISupportInitialize)(this.moniker)).BeginInit();
			this.SuspendLayout();
			// 
			// labelControl
			// 
			this.labelControl.Location = new System.Drawing.Point(0, 3);
			this.labelControl.Name = "labelControl";
			this.labelControl.Size = new System.Drawing.Size(100, 13);
			this.labelControl.TabIndex = 0;
			this.labelControl.Text = "(Label)";
			this.labelControl.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.PropertyHelpRequested);
			// 
			// valueControl
			// 
			this.valueControl.Location = new System.Drawing.Point(104, 0);
			this.valueControl.Name = "valueControl";
			this.valueControl.Size = new System.Drawing.Size(240, 20);
			this.valueControl.TabIndex = 1;
			this.valueControl.Text = "textBox1";
			this.valueControl.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.PropertyHelpRequested);
			// 
			// moniker
			// 
			this.moniker.ControlToValidate = this.valueControl;
			this.moniker.ErrorMessage = "Value is not a valid type moniker.";
			this.moniker.Icon = ((System.Drawing.Icon)(resources.GetObject("moniker.Icon")));
			this.moniker.IconPadding = 2;
			this.moniker.Required = false;
			this.moniker.ValidationExpression = @"^(|(?:[A-Za-z]\w*\.)*[A-Za-z]\w*,(?:[A-Za-z]\w*\.)*[A-Za-z]\w*)$";
			// 
			// MonikerControl
			// 
			this.Controls.Add(this.valueControl);
			this.Controls.Add(this.labelControl);
			this.Name = "MonikerControl";
			this.Size = new System.Drawing.Size(368, 30);
			((System.ComponentModel.ISupportInitialize)(this.moniker)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion


		/// <summary>
		/// Fills the XML form data based on the serialization data.
		/// </summary>
		/// <param name="element">Element serialization.</param>
		public void Set( XmlElement element )
		{
			valueControl.Text = element.InnerText;
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
	}
}