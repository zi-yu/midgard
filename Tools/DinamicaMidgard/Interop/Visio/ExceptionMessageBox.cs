using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Midgard.Interop
{
	/// <summary>
	/// Custom container for displaying unhandled exceptions.
	/// </summary>
	public class ExceptionMessageBox : Form
	{
		private PictureBox pictureBox;
		private Button buttonOK;
		private Button buttonToggle;
		private TextBox textException;
		private Button buttonCopyClipboard;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private Container components = null;
		private Label labelError;

		private bool isFullDetail = false;
		private Exception _exception = null;


		/// <summary>
		/// Initializes a new instance of the ExceptionMessageBox class.
		/// </summary>
		public ExceptionMessageBox()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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


		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionMessageBox));
            this.textException = new System.Windows.Forms.TextBox();
            this.labelError = new System.Windows.Forms.Label();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonToggle = new System.Windows.Forms.Button();
            this.buttonCopyClipboard = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // textException
            // 
            this.textException.Location = new System.Drawing.Point(8, 96);
            this.textException.Multiline = true;
            this.textException.Name = "textException";
            this.textException.ReadOnly = true;
            this.textException.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textException.Size = new System.Drawing.Size(456, 144);
            this.textException.TabIndex = 0;
            // 
            // labelError
            // 
            this.labelError.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelError.Location = new System.Drawing.Point(56, 10);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(408, 38);
            this.labelError.TabIndex = 1;
            this.labelError.Text = "(Error Message)";
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonOK.Location = new System.Drawing.Point(388, 56);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 2;
            this.buttonOK.Text = "OK";
            // 
            // buttonToggle
            // 
            this.buttonToggle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonToggle.Location = new System.Drawing.Point(302, 56);
            this.buttonToggle.Name = "buttonToggle";
            this.buttonToggle.Size = new System.Drawing.Size(75, 23);
            this.buttonToggle.TabIndex = 2;
            this.buttonToggle.Text = "&More >>>";
            this.buttonToggle.Click += new System.EventHandler(this.buttonToggle_Click);
            // 
            // buttonCopyClipboard
            // 
            this.buttonCopyClipboard.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonCopyClipboard.Location = new System.Drawing.Point(216, 56);
            this.buttonCopyClipboard.Name = "buttonCopyClipboard";
            this.buttonCopyClipboard.Size = new System.Drawing.Size(75, 23);
            this.buttonCopyClipboard.TabIndex = 3;
            this.buttonCopyClipboard.Text = "&Copy";
            this.buttonCopyClipboard.Click += new System.EventHandler(this.buttonCopyClipboard_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(10, 10);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(32, 32);
            this.pictureBox.TabIndex = 4;
            this.pictureBox.TabStop = false;
            // 
            // ExceptionMessageBox
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.buttonOK;
            this.ClientSize = new System.Drawing.Size(474, 90);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.buttonCopyClipboard);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.labelError);
            this.Controls.Add(this.textException);
            this.Controls.Add(this.buttonToggle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExceptionMessageBox";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Application Error";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion


		/// <summary>
		/// Gets or sets the main error message.
		/// </summary>
		public string ErrorMessage
		{
			get { return labelError.Text; }
			set { labelError.Text = value; }
		}


		/// <summary>
		/// Gets or sets the exception being shown.
		/// </summary>
		public Exception Exception
		{
			get { return _exception; }
			set
			{
				_exception = value;
				textException.Text = "";
                
				if ( _exception != null )
				{
                    string innerex = (_exception.InnerException != null ? String.Format("{1}{0}StackTrace:{0}{2}", Environment.NewLine, _exception.InnerException.Message, _exception.InnerException.StackTrace) : "");
                    string text = String.Format("Exception: {1}{0}{0}InnerException:{2}{0}{0}Outer StackTrace:{0}{3}", Environment.NewLine, _exception.Message, innerex, _exception.StackTrace.ToString());
                    textException.Text = text;
				}
			}
		}


		private void buttonCopyClipboard_Click( object sender, EventArgs e )
		{
			Clipboard.SetDataObject( textException.Text, false );
		}


		private void buttonToggle_Click( object sender, EventArgs e )
		{
			if ( isFullDetail )
			{
				isFullDetail = false;
				buttonToggle.Text = "More >>>";
				this.Height = 115;
			}
			else
			{
				isFullDetail = true;
				buttonToggle.Text = "Less <<<";
				this.Height = 280;
			}
		}
	}
}