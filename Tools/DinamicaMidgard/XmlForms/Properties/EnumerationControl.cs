using System;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using Genghis.Windows.Forms;


namespace Midgard.XmlForms
{
	/// <summary>
	/// Allows for input of several related "items".
	/// </summary>
	public class EnumerationControl : BaseProperty, IXmlProperty
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private Container components = null;
		private System.Windows.Forms.Label labelControl;
		private System.Windows.Forms.TextBox valueControl;
		private RequiredFieldValidator required=null;

        //private OperacoesInternas opr;
		private string _itemName;

		/// <summary>
		/// Initializes a new insance of the StringControl class.
		/// </summary>
		/// <param name="definition">Definition of the property.</param>
		/// <param name="mode">The current design mode.</param>
        public EnumerationControl(XmlElement definition, string mode)
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


			/*
			 * 
			 */
			if ( definition.Attributes[ "itemName" ] != null )
				_itemName = definition.Attributes[ "itemName" ].Value;
			else if ( this.Id.EndsWith( "s" ) )
				_itemName = this.Id.Substring( 0, this.Id.Length-1 );
			else
				_itemName = this.Id;


			/*
			 * Height / number of rows.
			 */
			labelControl.Height = valueControl.Height;



			/*
			 * Check required
			 */
			if ( IsRequired( definition, mode ) == true )
			{
				required = new RequiredFieldValidator();
				required.ControlToValidate = this.valueControl;
				required.ErrorMessage = Resources.GetString( ResourceTokens.ValidatorRequired );
				required.IconPadding = 2;
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
			this.labelControl = new System.Windows.Forms.Label();
			this.valueControl = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// labelControl
			// 
			this.labelControl.Location = new System.Drawing.Point(0, 4);
			this.labelControl.Name = "labelControl";
			this.labelControl.Size = new System.Drawing.Size(100, 14);
			this.labelControl.TabIndex = 0;
			this.labelControl.Text = "(Label)";
			this.labelControl.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.PropertyHelpRequested);
			// 
			// valueControl
			// 
			this.valueControl.AcceptsReturn = true;
			this.valueControl.AcceptsTab = true;
			this.valueControl.Location = new System.Drawing.Point(104, 0);
			this.valueControl.Multiline = true;
			this.valueControl.Name = "valueControl";
			this.valueControl.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.valueControl.Size = new System.Drawing.Size(240, 64);
			this.valueControl.TabIndex = 1;
			this.valueControl.Text = "(Text)";
			this.valueControl.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.PropertyHelpRequested);
			// 
			// EnumerationControl
			// 
			this.Controls.Add(this.valueControl);
			this.Controls.Add(this.labelControl);
			this.Name = "EnumerationControl";
			this.Size = new System.Drawing.Size(368, 72);
			this.ResumeLayout(false);

		}

		#endregion


		/// <summary>
		/// Fills the XML form data based on the serialization data.
		/// </summary>
		/// <param name="element">Element serialization.</param>
		public void Set( XmlElement element )
		{
			StringBuilder sb = new StringBuilder();

			foreach ( XmlElement el in element.SelectNodes( "./" + _itemName ) )
			{
				sb.Append( el.InnerText );
				sb.Append( Environment.NewLine );
			}

			valueControl.Text = sb.ToString().TrimEnd();
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

			string[] lines = trim.Split( '\n' );

			foreach ( string line in lines )
				AppendChild( element, _itemName, line.Trim() );

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