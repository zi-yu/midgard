using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Genghis.Windows.Forms;

namespace Midgard.Interop
{
	/// <summary>
	/// Form to configure export settings.
	/// </summary>
	public class SettingsForm : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.GroupBox groupExportBox;
		private System.Windows.Forms.Label labelPath;
		private Genghis.Windows.Forms.FolderNameDialog pathDialog;
		private System.Windows.Forms.Button buttonPath;
		private System.Windows.Forms.TextBox textBoxPath;
		private Genghis.Windows.Forms.RequiredFieldValidator requiredField;
		private FolderExistsValidator folderExists;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private Genghis.Windows.Forms.ContainerValidator containerValidator;
        private Button button1;
		private string _documentPath;


		/// <summary>
		/// Initializes a new instance of the Settings class.
		/// </summary>
		/// <param name="documentPath">Path of Visio document.</param>
		public SettingsForm( string documentPath )
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			_documentPath = documentPath;

			// 
			// requiredField
			// 
			this.folderExists = new FolderExistsValidator();
			this.folderExists.ControlToValidate = this.textBoxPath;
			this.folderExists.ErrorMessage = "Folder does not exist.";
			this.folderExists.IconPadding = 40;
		}

		#region Dispose

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

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.pathDialog = new Genghis.Windows.Forms.FolderNameDialog();
            this.groupExportBox = new System.Windows.Forms.GroupBox();
            this.buttonPath = new System.Windows.Forms.Button();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.labelPath = new System.Windows.Forms.Label();
            this.requiredField = new Genghis.Windows.Forms.RequiredFieldValidator();
            this.containerValidator = new Genghis.Windows.Forms.ContainerValidator();
            this.button1 = new System.Windows.Forms.Button();
            this.groupExportBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.requiredField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.containerValidator)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(239, 74);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 0;
            this.buttonOK.Text = "OK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(329, 74);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            // 
            // pathDialog
            // 
            this.pathDialog.BrowseForComputer = false;
            this.pathDialog.BrowseForEverything = false;
            this.pathDialog.BrowseForPrinter = false;
            this.pathDialog.Description = "Choose a folder";
            this.pathDialog.DontTranslateTarget = false;
            this.pathDialog.NewUI = true;
            this.pathDialog.NoNewFolderButton = false;
            this.pathDialog.RestrictToDomain = false;
            this.pathDialog.RestrictToFilesystem = true;
            this.pathDialog.RestrictToSubfolders = false;
            this.pathDialog.ShowHint = false;
            this.pathDialog.ShowShareable = false;
            this.pathDialog.ShowStatusText = false;
            this.pathDialog.ShowTextBox = false;
            this.pathDialog.StartLocation = Genghis.Windows.Forms.FolderNameDialog.FolderNameFolder.Desktop;
            this.pathDialog.ValidResultsOnly = true;
            // 
            // groupExportBox
            // 
            this.groupExportBox.Controls.Add(this.buttonPath);
            this.groupExportBox.Controls.Add(this.textBoxPath);
            this.groupExportBox.Controls.Add(this.labelPath);
            this.groupExportBox.Location = new System.Drawing.Point(8, 8);
            this.groupExportBox.Name = "groupExportBox";
            this.groupExportBox.Size = new System.Drawing.Size(392, 60);
            this.groupExportBox.TabIndex = 5;
            this.groupExportBox.TabStop = false;
            this.groupExportBox.Text = "Configuração";
            // 
            // buttonPath
            // 
            this.buttonPath.Location = new System.Drawing.Point(336, 24);
            this.buttonPath.Name = "buttonPath";
            this.buttonPath.Size = new System.Drawing.Size(24, 20);
            this.buttonPath.TabIndex = 7;
            this.buttonPath.Text = "...";
            this.buttonPath.Click += new System.EventHandler(this.buttonPath_Click);
            // 
            // textBoxPath
            // 
            this.textBoxPath.Location = new System.Drawing.Point(104, 24);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(224, 20);
            this.textBoxPath.TabIndex = 6;
            // 
            // labelPath
            // 
            this.labelPath.Location = new System.Drawing.Point(13, 26);
            this.labelPath.Name = "labelPath";
            this.labelPath.Size = new System.Drawing.Size(80, 16);
            this.labelPath.TabIndex = 5;
            this.labelPath.Text = "Caminho:";
            // 
            // requiredField
            // 
            this.requiredField.ControlToValidate = this.textBoxPath;
            this.requiredField.ErrorMessage = "Required.";
            this.requiredField.Icon = ((System.Drawing.Icon)(resources.GetObject("requiredField.Icon")));
            this.requiredField.IconPadding = 40;
            // 
            // containerValidator
            // 
            this.containerValidator.ContainerToValidate = this;
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(12, 74);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(159, 22);
            this.button1.TabIndex = 6;
            this.button1.Text = "Actualizar ficheiros";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(410, 103);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupExportBox);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Configuração";
            this.groupExportBox.ResumeLayout(false);
            this.groupExportBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.requiredField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.containerValidator)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#region Form Callbacks

		private void buttonOK_Click(object sender, System.EventArgs e)
		{
			// Further form-wide validation to manually check
			// the validity of your form fields
			containerValidator.Validate();

			if ( containerValidator.IsValid() )
			{
				this.DialogResult = DialogResult.OK;
				Close();
			}
		}

		private void buttonPath_Click(object sender, System.EventArgs e)
		{
			DialogResult result = pathDialog.ShowDialog( this );

			if ( result == DialogResult.OK )
			{
				string absolute = pathDialog.DirectoryPath;
                textBoxPath.Text = absolute;
			}
		}

		#endregion


		/// <summary>
		/// Gets the absolute path of the Visio document being configured.
		/// </summary>
		public string DocumentPath
		{
			get { return _documentPath; }
		}


		/// <summary>
		/// Loads the form values with the given export settings.
		/// </summary>
		public void LoadSettings()
		{
            //textBoxPath.Text = ExportSettings.Path;
		}

		/// <summary>
		/// Loads the form values with the given export settings.
		/// </summary> 
		public string GetSettings()
		{
            return textBoxPath.Text;
		}

		#region FolderExistsValidator

		/// <summary>
		/// Custom validator to validate the folder path, _relative_ to the
		/// base path of the current document.
		/// </summary>
		public sealed class FolderExistsValidator : BaseValidator
		{
			/// <summary>
			/// Initializes a new instance of the FolderExistsValidator class.
			/// </summary>
			public FolderExistsValidator()
			{
			}

			/// <summary>
			/// Determines wether the control being validated is a valid
			/// folder name (relative to the document path).
			/// </summary>
			/// <returns>True if valid, False otherwise.</returns>
			protected override bool EvaluateIsValid()
			{
				string relative = this.ControlToValidate.Text;
				string absolute = ( (SettingsForm) this.ControlToValidate.FindForm() ).DocumentPath;

				string folder = Path.Combine( absolute, relative );
				DirectoryInfo folderInfo = new DirectoryInfo( folder );

				return folderInfo.Exists;
			}
		}

		#endregion

        private void button1_Click(object sender, EventArgs e)
        {

        }
	}
}
