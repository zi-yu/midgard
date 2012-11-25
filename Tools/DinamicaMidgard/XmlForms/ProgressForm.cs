using System;
using System.Windows.Forms;

namespace Midgard.XmlForms
{
	/// <summary>
	/// Displays progress on long-running operations.
	/// </summary>
	public class ProgressForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.ProgressBar progressBar;


		private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.Label labelPleaseWait;

        public System.Windows.Forms.Label LabelPleaseWait
        {
            get { return labelPleaseWait; }
        }

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Initializes a new instance of the ProgressForm class.
		/// </summary>
		public ProgressForm()
		{
			InitializeComponent();
			labelPleaseWait.Text = Resources.GetString( ResourceTokens.PleaseWait );
            Application.EnableVisualStyles();
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProgressForm));
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.progressLabel = new System.Windows.Forms.Label();
            this.labelPleaseWait = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.progressBar.Location = new System.Drawing.Point(16, 52);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(215, 20);
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.TabIndex = 0;
            this.progressBar.UseWaitCursor = true;
            // 
            // progressLabel
            // 
            this.progressLabel.Location = new System.Drawing.Point(16, 32);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.progressLabel.Size = new System.Drawing.Size(215, 16);
            this.progressLabel.TabIndex = 1;
            // 
            // labelPleaseWait
            // 
            this.labelPleaseWait.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.labelPleaseWait.Location = new System.Drawing.Point(16, 10);
            this.labelPleaseWait.Name = "labelPleaseWait";
            this.labelPleaseWait.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelPleaseWait.Size = new System.Drawing.Size(215, 16);
            this.labelPleaseWait.TabIndex = 2;
            this.labelPleaseWait.Text = "Aguarde por favor...";
            // 
            // ProgressForm
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(247, 90);
            this.ControlBox = false;
            this.Controls.Add(this.labelPleaseWait);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.progressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Progresso";
            this.ResumeLayout(false);

		}
		#endregion


		/// <summary>
		/// Displays a visual progress of the export.
		/// </summary>
		/// <param name="step">Current step.</param>
		/// <param name="totalSteps">Total number of steps.</param>
		/// <param name="message">Message to be displayed.</param>
		public void SetProgress( int step, int totalSteps, string message )
		{
			progressBar.Maximum = totalSteps;
			progressBar.Value = step;
			progressLabel.Text = message;

			// Force a visual refresh of the screen.
			this.Refresh();
		}
	}
}
