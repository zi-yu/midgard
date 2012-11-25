using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using Genghis.Windows.Forms;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Collections.Generic;

namespace Midgard.XmlForms
{

    public delegate void StartConnection(StringButtonControl control);
    public delegate void EndConnection(StringButtonControl control);

	public class StringButtonControl : BaseProperty, IXmlProperty
	{
        /// <summary>
        /// Occurs whenever a start connection.
        /// </summary>
        public event StartConnection Start;

        /// <summary>
        /// Occurs whenever a stop connection.
        /// </summary>
        public event EndConnection Stop;

		protected System.Windows.Forms.Label labelControl;
        protected System.Windows.Forms.TextBox valueControl;
		
        private RequiredFieldValidator req;
        private string onClickFunction = null;
        private string[] onClickFunctionParams = null;
        private string[] onClickGlobalDataParams = null;
        //private OperacoesInternas opr;


        private XmlElement _definition;
        private Button lookUp;
        public bool isProcessing = false;

        public Button LookUp
        {
            get { return lookUp; }
        }
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private Container components = null;


        private Dictionary<string, string> globalData;

        public Dictionary<string, string> GlobalData
        {
            get { return globalData; }
            set { globalData = value; }
        }

		/// <summary>
		/// Initializes a new insance of the StringControl class.
		/// </summary>
		/// <param name="definition">Definition of the property.</param>
		/// <param name="mode">The current design mode.</param>
        public StringButtonControl(XmlElement definition, string mode)
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
			valueControl.Width = Property.ValueWidth-25;
			this.Height = valueControl.Height + Property.PropertyMarginBottom; 

			#endregion

            _definition = definition;

			// Initialize control
			Initialize( _definition );
            GetFunctionAndParameters(_definition);
            GetGlobalParameters(_definition);

			labelControl.Text = this.Label;
         
            valueControl.Text = "";

            /*
            * Height / number of rows.
            */
            labelControl.Height = valueControl.Height;

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
            //_form = new ProgressForm();
            globalData = new Dictionary<string, string>();
		}

        private void GetGlobalParameters(XmlElement _definition)
        {
            if (null == _definition.Attributes["globalParameters"])
                return;
            string parames = _definition.Attributes["globalParameters"].Value;
            parames = parames.Replace(" ", string.Empty);

            onClickGlobalDataParams = parames.Split(',');
            if ("" == onClickGlobalDataParams[0])
                onClickGlobalDataParams = null;
        }

        private void GetFunctionAndParameters(XmlElement _definition)
        {
            if (null == _definition.Attributes["onClick"])
                return;

            string func = _definition.Attributes["onClick"].Value;
            func = func.Replace(" ", string.Empty);
            string[] funcParts = func.Split('(');
            if(2 != funcParts.Length)
                throw new Exception("Função do botão " + this.Label + " mal formatada");

            onClickFunction = funcParts[0];
            string parames = funcParts[1].Remove(funcParts[1].Length - 1);
            onClickFunctionParams = parames.Split(',');
            if ("" == onClickFunctionParams[0])
                onClickFunctionParams = null;
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
            this.lookUp = new System.Windows.Forms.Button();
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
            this.valueControl.Location = new System.Drawing.Point(104, 0);
            this.valueControl.Name = "valueControl";
            this.valueControl.Size = new System.Drawing.Size(210, 20);
            this.valueControl.TabIndex = 1;
            this.valueControl.Text = "(Text)";
            this.valueControl.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.PropertyHelpRequested);
            this.valueControl.Leave += new System.EventHandler(this.valueControl_Leave);
            this.valueControl.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.valueControl_KeyPress);
            // 
            // lookUp
            // 
            this.lookUp.Font = new System.Drawing.Font("Bernard MT Condensed", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lookUp.Location = new System.Drawing.Point(338, 3);
            this.lookUp.Name = "lookUp";
            this.lookUp.Size = new System.Drawing.Size(27, 17);
            this.lookUp.TabIndex = 2;
            this.lookUp.Text = "\'\'\'";
            this.lookUp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lookUp.UseVisualStyleBackColor = true;
            this.lookUp.Click += new System.EventHandler(this.lookUp_Click);
            // 
            // StringButtonControl
            // 
            this.Controls.Add(this.lookUp);
            this.Controls.Add(this.valueControl);
            this.Controls.Add(this.labelControl);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "StringButtonControl";
            this.Size = new System.Drawing.Size(368, 32);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion


		/// <summary>
		/// Fills the XML form data based on the serialization data.
		/// </summary>
		/// <param name="element">Element serialization.</param>
        public void Set(XmlElement element)
        {
            Set(element.InnerText, false, false, false, false);
        }

        /// <summary>
        /// Fills the XML form data based on a value.
        /// </summary>
        /// <param name="value"></param>
        public override void Set(string value, bool required, bool disabled, bool invisible, bool forceReplacement)
        {
            if (valueControl.Text.Replace(" ", "").Length <= 0 ||
                (valueControl.Text.Replace(" ", "").Length > 0 && this.Id == "id") || forceReplacement)
                valueControl.Text = value;

            if (required)
            {
                req = new RequiredFieldValidator();
                req.ControlToValidate = this.valueControl;
                req.ErrorMessage = "Required.";
                req.IconPadding = 2;
                this.valueControl.BackColor = Resources.GetColor("required");
            }

            this.valueControl.Enabled = !disabled;

            if (this.valueControl.Parent is StringControl)
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
            ApplyChangeShapeRules();

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

        void valueControl_Leave(object sender, EventArgs e)
        {
            DoManagedNodes(valueControl.Text);
        }

		/// <summary>
		/// Gets the string representation of the current object.
		/// </summary>
		/// <returns>String representation.</returns>
		public override string ToString()
		{
			return valueControl.Text;
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

        /// <summary>
        /// Returns the value to be compared in the changeShape rule. Must be implemented by all controls that use the rule.
        /// </summary>
        /// <returns></returns>
        protected override string GetCheckForValue()
        {
            return this.valueControl.Text;
        }

        private void lookUp_Click(object sender, EventArgs e)
        {
            if (null == onClickFunction)
                return;

            isProcessing = true;
            if (Start != null)
                Start(this);

            ClickFunction functions = new ClickFunction();
            Type type = typeof(ClickFunction);
            MethodInfo method = type.GetMethod(onClickFunction);
            string[] lookUp = null;

            try
            {
                if (null == onClickFunctionParams && null == onClickGlobalDataParams)
                {
                    lookUp = (string[])method.Invoke(functions, null);
                }
                else
                {
                    string[] paramValues;
                    if (null != onClickFunctionParams)
                    {

                        Control c = ((Button)sender).Parent.Parent.Parent.Parent.Parent;
                        XmlDocument doc = ((XmlForm)c).GetShapeXml();
                        paramValues = new string[onClickFunctionParams.Length];

                        for (int i = 0; i < onClickFunctionParams.Length; ++i)
                        {
                            if (null != doc.DocumentElement[onClickFunctionParams[i]])
                                paramValues[i] = doc.DocumentElement[onClickFunctionParams[i]].InnerText;
                        }
                        
                    }
                    else 
                    {
                        paramValues = new string[onClickGlobalDataParams.Length];
                        for (int i = 0; i < onClickGlobalDataParams.Length; ++i)
                        {
                            paramValues[i] = globalData[onClickGlobalDataParams[i]];
                        }
                    
                    }
                    lookUp = (string[])method.Invoke(functions, paramValues);
                }
            }
            catch (Exception ex)
            {
                isProcessing = false;
                if (Stop != null)
                    Stop(this);
                //_form.Hide();
                if(null != ex.InnerException)
                    MessageBox.Show(null, ex.InnerException.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show(null, "É necessário definir primeiro o produto composto.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            isProcessing = false;
            //_form.Hide();
            if (Stop != null)
                Stop(this);

            string xml = WriteXml(lookUp);
            using (XmlForm form = new XmlForm())
            {
                form.LoadDefinition("LookUp", null);
                form.Tag = this.FindForm().Tag;
                form.Design("");
                form.SetShapeXml(xml);
                DialogResult result = form.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    XmlDocument xdoc = form.GetShapeXml();
                    //valor selecionado na ListBox
                    if (null != ((ListBox)form.Controls[1].Controls[0].Controls[0].Controls[0].Controls[5]).SelectedItem)
                    {
                        string text = ((ListBox)form.Controls[1].Controls[0].Controls[0].Controls[0].Controls[5]).SelectedItem.ToString();

                        if ("GetStates" != onClickFunction)
                        {
                            valueControl.Text = text;
                        }
                        else
                        {
                            string[] values = text.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries);
                            string temp = values[0].Substring(9);
                            valueControl.Text = temp.Replace(" ", string.Empty);
                            if (2 == values.Length)
                            {
                                ((Button)sender).Parent.Parent.Parent.Controls[0].Controls[0].Controls[1].Text = values[1].Substring(13);
                            }
                        }
                    }

                }
            }
        }

        private string WriteXml(string[] lookUp)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<properties><shapeFile>Process-LookUp.shape</shapeFile><selections>");
            foreach(string val in lookUp)
            {
                sb.Append("<selection><item>");
                sb.Append(val);
                sb.Append("</item></selection>");
            }
            sb.Append("</selections></properties>");
            return sb.ToString();
        }
	}
}