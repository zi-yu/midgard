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
	public class SeperatorControl : BaseProperty, IXmlProperty
	{
		private HeaderGroupBox labelControl;
        //private OperacoesInternas opr;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private Container components = null;

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
        public SeperatorControl(XmlElement definition, string mode)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
            //opr = _opr;
			#region Use Standard Sizes

			labelControl.Width = Property.LabelWidth + Property.ValueWidth;
			this.Height = labelControl.Height + Property.PropertyMarginBottom; 

			#endregion

			// Initialize control
			Initialize( definition );

			labelControl.Text = this.Label;
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
			this.labelControl = new Genghis.Windows.Forms.HeaderGroupBox();
			this.SuspendLayout();
			// 
			// labelControl
			// 
			this.labelControl.Location = new System.Drawing.Point(0, 8);
			this.labelControl.Name = "labelControl";
			this.labelControl.Padding = 0;
			this.labelControl.Size = new System.Drawing.Size(352, 16);
			this.labelControl.TabIndex = 0;
			this.labelControl.TabStop = false;
			this.labelControl.Text = "(Label)";
			// 
			// SeperatorControl
			// 
			this.Controls.Add(this.labelControl);
			this.Name = "SeperatorControl";
			this.Size = new System.Drawing.Size(368, 32);
			this.ResumeLayout(false);

		}

		#endregion


		/// <summary>
		/// Fills the XML form data based on the serialization data.
		/// </summary>
		/// <param name="element">Element serialization.</param>
		public void Set( XmlElement element )
		{
		}

		/// <summary>
		/// Fills the destination element with the serialization data for the
		/// property.
		/// </summary>
		/// <param name="element">Element serialization.</param>
		/// <returns>True if the element is to be append to the shape XML, false otherwise.</returns>
		public bool Fill( XmlElement element )
		{
			return false;
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