using System;
using System.IO;
using System.Reflection;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml;
using Genghis.Windows.Forms;
using Microsoft.Office.Interop.Visio;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;
using Midgard.XmlForms.Configuration;


namespace Midgard.XmlForms
{
    /// <summary>
    /// Form which supports run-time design through XML file.
    /// </summary>
    public class XmlForm : Form, IDisposable
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TabControl tabControl;
        private Genghis.Windows.Forms.ContainerValidator containerValidator;
        private int _formHeight = 479;
        private int _formWidth = 410;
        private int _tabHeightOffSet = 42;
        private int _tabWidhtOffSet = 14;
        private int _btnsHeightOffSet = 31;
        private int _btnOkWidhtOffSet = 170;
        private int _btnCancelWidhtOffSet = 81;

        private List<StringButtonControl> lookUpControls;
        ProgressForm _form;
        private System.ComponentModel.BackgroundWorker backgroundWorker;

        //private OperacoesInternas opr;

        #region Private Members

        private XmlDocument _definition;
        private ArrayList _fields;
        private string _prefix;
        private string _filename;
        #endregion

        /// <summary>
        /// Initializes a new instance of the XmlForm class.
        /// </summary>
        public XmlForm()
        {
            // Required for Windows Form Designer support
            InitializeComponent();
   
            // Create the field collection.
            _fields = new ArrayList();
            _form = new ProgressForm();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();
                containerValidator.Dispose();
                buttonOK.Dispose();
                buttonCancel.Dispose();
                tabControl.Dispose();
            }

            base.Dispose(disposing);
        }

        private void ManageShapeSize()
        {
            this.tabControl.Size = new System.Drawing.Size(_formWidth - _tabWidhtOffSet, _formHeight - _tabHeightOffSet);
            this.buttonOK.Location = new System.Drawing.Point(_formWidth - _btnOkWidhtOffSet, _formHeight - _btnsHeightOffSet);
            this.buttonCancel.Location = new System.Drawing.Point(_formWidth - _btnCancelWidhtOffSet, _formHeight - _btnsHeightOffSet);
            this.ClientSize = new System.Drawing.Size(_formWidth, _formHeight);
        }
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(XmlForm));

            this.tabControl = new System.Windows.Forms.TabControl();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.containerValidator = new Genghis.Windows.Forms.ContainerValidator();
            ((System.ComponentModel.ISupportInitialize)(this.containerValidator)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Location = new System.Drawing.Point(8, 8);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.ShowToolTips = true;

            this.tabControl.TabIndex = 0;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // buttonOK
            // 

            this.buttonOK.Name = "buttonOK";
            this.buttonOK.TabIndex = 1;
            this.buttonOK.Text = "OK";
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            this.buttonOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonOK.UseVisualStyleBackColor = true;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // containerValidator
            // 
            this.containerValidator.ContainerToValidate = this;
            // 
            // XmlForm
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "XmlForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "XmlForm";
            this.Activated += new System.EventHandler(this.XmlForm_Activated);
            ((System.ComponentModel.ISupportInitialize)(this.containerValidator)).EndInit();
            ManageShapeSize();
            this.ResumeLayout(false);

        }

        #endregion


        /// <summary>
        /// Loads the shape definition file.
        /// </summary>
        /// <param name="name">Name of shape.</param>
        /// <param name="data">Data that is passed to the shape object (not the form, the
        /// object that aids the Form).</param>
        public void LoadDefinition(string name, object data)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            _filename = FormConfigs.GetShapeFile(name);

            _definition = new XmlDocument(FormsNamespace.NamespaceManager.NameTable);
            _definition.Load(_filename);

            XmlElement attributesElem = (XmlElement)_definition.SelectSingleNode("/shape/attributes", FormsNamespace.NamespaceManager);

            if (attributesElem == null)
                throw new ArgumentException("Missing attributes element.", "doc");

            XmlNode formSize = (XmlElement)_definition.SelectSingleNode("/shape/properties/size", FormsNamespace.NamespaceManager);
            if (formSize != null)
            {
                if (formSize.Attributes["width"] != null && formSize.Attributes["width"].Value != null && formSize.Attributes["width"].Value.ToString().Length > 0)
                    _formWidth = int.Parse(formSize.Attributes["width"].Value.ToString());
                if (formSize.Attributes["height"] != null && formSize.Attributes["height"].Value != null && formSize.Attributes["height"].Value.ToString().Length > 0)
                    _formHeight = int.Parse(formSize.Attributes["height"].Value.ToString());
                ManageShapeSize();
            }
            string moniker = attributesElem.Attributes["moniker"].Value;
            object[] ctorArgs = new object[] { data, this, attributesElem };

            this.Tag = TypeFactory.Create(moniker, ctorArgs);
        }


        /// <summary>
        /// Designs the form in run-time.
        /// </summary>
        /// <param name="mode">Current editing mode.</param>
        public void Design(string mode)
        {
            if (_definition == null)
                throw new Exception("Cannot .Design if no definition has ben .Load()'ed.");
            /*
             * Parse definition properties. 
             */
            #region Properties

            XmlElement properties = (XmlElement)_definition.SelectSingleNode("/shape/properties", FormsNamespace.NamespaceManager);

            if (properties == null)
                throw new ArgumentException("Missing properties element.", "doc");

            this.Text = properties.Attributes["title"].Value;

            #endregion

            IXmlShape shape = null;
            if (this.Tag is IXmlShape)
                shape = (IXmlShape)this.Tag;

            #region Create Tabs

            XmlNodeList tabNodeList = properties.SelectNodes("tab", FormsNamespace.NamespaceManager);
            foreach (XmlElement tabElement in tabNodeList)
            {
                /*
                 * Iterate through tab list. Please note that since each Control is
                 * being added with Dock.Top, they will visually appear in reverse order.
                 * Therefore, once all have been added, we perform a ReLayout!
                 */

                XmlTabPage tab = new XmlTabPage(tabElement);
                if (tab.IsInvisibleInMode(mode))//Se tab invisível, não é adicionado ao form
                    continue;
                if (tab.IsDisabledInMode(mode))
                    ((Control)tab.TabPage).Enabled = false;

                tabControl.TabPages.Add(tab.TabPage);
                XmlNodeList fieldNodeList = tabElement.SelectNodes("*");

                foreach (XmlElement fieldElement in fieldNodeList)
                {
                    /*
                     * Iterate through each property. Note how we do not include in the _fields
                     * bag controls that are not IXmlProperty!
                     */
                    if (fieldElement.Name == "include")
                    {
                        string filename, xPath, excludedProperties = null;
                        string[] excludedPropertiesList = null;
                        string prefix = null;

                        if ((fieldElement.Attributes["file"] != null))
                            filename = fieldElement.Attributes["file"].Value;
                        else
                            throw (new Exception("Element include in shape definition doesn´t have the attribute 'file'!"));

                        if ((fieldElement.Attributes["xPath"] != null))
                            xPath = fieldElement.Attributes["xPath"].Value;
                        else
                            throw (new Exception("Element include in shape definition doesn´t have the attribute 'xPath'!"));

                        if ((fieldElement.Attributes["prefix"] != null))
                            prefix = fieldElement.Attributes["prefix"].Value;

                        if ((fieldElement.Attributes["excludedProperties"] != null))
                        {
                            excludedProperties = fieldElement.Attributes["excludedProperties"].Value;
                            excludedPropertiesList = excludedProperties.Replace(" ", "").Split(',');
                        }

                        XmlDocument xdoc = LoadPropertiesFromFile(filename);
                        foreach (XmlElement fieldElem in xdoc.SelectNodes(xPath))
                        {
                            if ((excludedPropertiesList != null) && checkIfExcludedNode(fieldElem.Attributes["id"].Value, excludedPropertiesList))
                                continue;

                            if (prefix != null)
                                fieldElem.Attributes["id"].Value = string.Format("{0}_{1}", prefix, fieldElem.Attributes["id"].Value);

                            Control control = XmlFormFactory.CreateProperty(fieldElem, mode, null);
                            BaseProperty tmpControl = control as BaseProperty;

                            if (control is IXmlProperty)
                            {
                                IXmlProperty property = (IXmlProperty)control;
                                _fields.Add(property);
                            }

                            if (tmpControl != null)
                            {
                                if (tmpControl.IsInvisibleInMode(fieldElem, mode))
                                    continue;
                                if (tmpControl.IsDisabledInMode(fieldElem, mode))
                                    control.Enabled = false;
                            }

                            if (control.Visible)
                                tab.AddProperty(control);

                            if (shape != null && tmpControl != null)
                                tmpControl.ChangeByConnection(shape.ConnectedTo(), shape.ConnectedFrom());
                        }
                    }
                    else
                    {
                        Control control = XmlFormFactory.CreateProperty(fieldElement, mode, null);
                        BaseProperty tmpControl = control as BaseProperty;

                        if (control is IXmlProperty)
                        {
                            IXmlProperty property = (IXmlProperty)control;
                            _fields.Add(property);
                        }

                        if (tmpControl != null)
                        {
                            if (tmpControl.IsInvisibleInMode(fieldElement, mode))
                                continue;
                            if (tmpControl.IsDisabledInMode(fieldElement, mode))
                                control.Enabled = false;
                        }

                        if (control.Visible)
                            tab.AddProperty(control);

                        if (shape != null && tmpControl != null)
                            tmpControl.ChangeByConnection(shape.ConnectedTo(), shape.ConnectedFrom());
                    }
                }

                tab.ReLayout();
            }

            #endregion

            #region Shape Specific AddOns
            if (shape != null)
            {
                shape.Design();
            }
            #endregion
        }

        public void Design(string mode, Microsoft.Office.Interop.Visio.Page page)
        {
            if (_definition == null)
                throw new Exception("Cannot .Design if no definition has ben .Load()'ed.");
            /*
             * Parse definition properties. 
             */
            #region Properties

            XmlElement properties = (XmlElement)_definition.SelectSingleNode("/shape/properties", FormsNamespace.NamespaceManager);

            if (properties == null)
                throw new ArgumentException("Missing properties element.", "doc");

            this.Text = properties.Attributes["title"].Value;

            #endregion

            IXmlShape shape = null;
            if (this.Tag is IXmlShape)
                shape = (IXmlShape)this.Tag;

            #region Create Tabs

            XmlNodeList tabNodeList = properties.SelectNodes("tab", FormsNamespace.NamespaceManager);
            foreach (XmlElement tabElement in tabNodeList)
            {
                /*
                 * Iterate through tab list. Please note that since each Control is
                 * being added with Dock.Top, they will visually appear in reverse order.
                 * Therefore, once all have been added, we perform a ReLayout!
                 */

                XmlTabPage tab = new XmlTabPage(tabElement);
                if (tab.IsInvisibleInMode(mode))//Se tab invisível, não é adicionado ao form
                    continue;
                if (tab.IsDisabledInMode(mode))
                    ((Control)tab.TabPage).Enabled = false;

                tabControl.TabPages.Add(tab.TabPage);
                XmlNodeList fieldNodeList = tabElement.SelectNodes("*");

                foreach (XmlElement fieldElement in fieldNodeList)
                {
                    /*
                     * Iterate through each property. Note how we do not include in the _fields
                     * bag controls that are not IXmlProperty!
                     */
                    if (fieldElement.Name == "include")
                    {
                        string filename, xPath, excludedProperties = null;
                        string[] excludedPropertiesList = null;
                        string prefix = null;

                        if ((fieldElement.Attributes["file"] != null))
                            filename = fieldElement.Attributes["file"].Value;
                        else
                            throw (new Exception("Element include in shape definition doesn´t have the attribute 'file'!"));

                        if ((fieldElement.Attributes["xPath"] != null))
                            xPath = fieldElement.Attributes["xPath"].Value;
                        else
                            throw (new Exception("Element include in shape definition doesn´t have the attribute 'xPath'!"));

                        if ((fieldElement.Attributes["prefix"] != null))
                            prefix = fieldElement.Attributes["prefix"].Value;

                        if ((fieldElement.Attributes["excludedProperties"] != null))
                        {
                            excludedProperties = fieldElement.Attributes["excludedProperties"].Value;
                            excludedPropertiesList = excludedProperties.Replace(" ", "").Split(',');
                        }

                        XmlDocument xdoc = LoadPropertiesFromFile(filename);
                        foreach (XmlElement fieldElem in xdoc.SelectNodes(xPath))
                        {
                            if ((excludedPropertiesList != null) && checkIfExcludedNode(fieldElem.Attributes["id"].Value, excludedPropertiesList))
                                continue;

                            if (prefix != null)
                                fieldElem.Attributes["id"].Value = string.Format("{0}_{1}", prefix, fieldElem.Attributes["id"].Value);

                            Control control = XmlFormFactory.CreateProperty(fieldElem, mode, page);
                            BaseProperty tmpControl = control as BaseProperty;

                            if (control is IXmlProperty)
                            {
                                IXmlProperty property = (IXmlProperty)control;
                                _fields.Add(property);
                            }

                            if (tmpControl != null)
                            {
                                if (tmpControl.IsInvisibleInMode(fieldElem, mode))
                                    continue;
                                if (tmpControl.IsDisabledInMode(fieldElem, mode))
                                    control.Enabled = false;
                            }

                            if (control.Visible)
                                tab.AddProperty(control);

                            if (shape != null && tmpControl != null)
                                tmpControl.ChangeByConnection(shape.ConnectedTo(), shape.ConnectedFrom());
                        }
                    }
                    else
                    {
                        Control control = XmlFormFactory.CreateProperty(fieldElement, mode, page);
                        BaseProperty tmpControl = control as BaseProperty;

                        if (control is IXmlProperty)
                        {
                            IXmlProperty property = (IXmlProperty)control;
                            _fields.Add(property);
                        }

                        if (tmpControl != null)
                        {
                            if (tmpControl.IsInvisibleInMode(fieldElement, mode))
                                continue;
                            if (tmpControl.IsDisabledInMode(fieldElement, mode))
                                control.Enabled = false;
                        }

                        if (control.Visible)
                            tab.AddProperty(control);

                        if (shape != null && tmpControl != null)
                            tmpControl.ChangeByConnection(shape.ConnectedTo(), shape.ConnectedFrom());
                    }
                }

                tab.ReLayout();
            }

            #endregion

            #region Shape Specific AddOns
            if (shape != null)
            {
                shape.Design();
            }
            #endregion
        }

        void OnDoWork(object sender, DoWorkEventArgs doWorkArgs)
        {
            Thread.CurrentThread.Priority = ThreadPriority.Highest;
            object arg = doWorkArgs.Argument;
            StringButtonControl control = (StringButtonControl)arg;
            while (control.isProcessing)
            {
                _form.Refresh();
                Thread.Sleep(90);
            }
            _form.Hide();
           
        }

        void XmlForm_Stop(StringButtonControl control)
        {    
        }

        void XmlForm_Start(StringButtonControl control)
        {
            _form.Show();
            _form.Refresh();
            backgroundWorker.RunWorkerAsync(control);
            Thread.Sleep(10);
        }

        private Dictionary<string, string> GetGlobalData(string props)
        {
            Dictionary<string, string> toReturn = new Dictionary<string, string>();
            string data = props.Substring( props.IndexOf('>') + 1, props.Length - ((props.IndexOf('>')+1)*2 + 1) );
            string[] splitData = data.Split('<');
            for(int i = 1; i< splitData.Length; )
            {
                string[] keyValue = splitData[i].Split('>');
                toReturn.Add(keyValue[0], keyValue[1]);
                i += 2;
            }
            return toReturn;
        }

        private bool checkIfExcludedNode(string strToFind, string[] strList)
        {
            foreach (string str in strList)
            {
                if (str == strToFind)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets or sets the selected tab by name. If setting selected tab
        /// by name, and no tab exists by that name no exception will be raised
        /// (and selection will remain unaltered).
        /// </summary>
        public string SelectedTabName
        {
            get { return tabControl.SelectedTab.Text; }

            set
            {
                string f = value;
                foreach (TabPage p in tabControl.TabPages)
                {
                    if (p.Text == f)
                    {
                        tabControl.SelectedTab = p;
                        return;
                    }
                }
            }
        }




        private bool _skip;
        private XmlDocument _doc;

        public bool SkipDesign
        {
            get { return _skip; }
            set { _skip = value; }
        }


        /// <summary>
        /// Sets the Shape XML.
        /// </summary>
        /// <param name="xml"></param>
        public void SetShapeXml(string xml)
        {
            if (xml.Length == 0)
                return;

            XmlDocument doc = new XmlDocument();

            try
            {
                doc.LoadXml(xml);
            }
            catch (XmlException)
            {
                // Silently ignore any mal-formed XML errors: consider this
                // an "upgrade" :)
            }

            foreach (IXmlProperty prop in _fields)
            {
                string xpath = string.Format("/properties/{0}", prop.Id);
                XmlElement def = (XmlElement)doc.SelectSingleNode(xpath);

                if (def == null)
                    continue;

                prop.Set(def);
            }

            if (_skip == true)
                _doc = doc;
        }



        /// <summary>
        /// Retrieves the Shape XML from all of the properties.
        /// </summary>
        /// <returns>XML of the properties, as filled in the form.</returns>
        public XmlDocument GetShapeXml(string elementName)
        {
            if (_skip == true)
                return _doc;

            if (null == elementName)
                return null;

            XmlDocument doc = new XmlDocument();

            XmlElement root = doc.CreateElement(elementName);
            doc.AppendChild(root);

            XmlElement sf = doc.CreateElement("shapeFile");
            sf.InnerText = _filename.Substring(_filename.LastIndexOf('\\') + 1);
            root.AppendChild(sf);

            foreach (IXmlProperty prop in _fields)
            {
                XmlElement def = doc.CreateElement(prop.Id);

                bool append = prop.Fill(def);

                if (append == true)
                    root.AppendChild(def);
            }

            return doc;
        }


        /// <summary>
        /// Retrieves the Shape XML from all of the properties.
        /// </summary>
        /// <returns>XML of the properties, as filled in the form.</returns>
        public XmlDocument GetShapeXml()
        {
            return GetShapeXml("properties");
        }


        /// <summary>
        /// Gets wether the current form is valid.
        /// </summary>
        public bool IsValid
        {
            get
            {
                containerValidator.Validate();

                return containerValidator.IsValid();
            }
        }

        /// <summary>
        /// Gets the number of properties on a shape.
        /// </summary>
        public int PropertyCount
        {
            get { return _fields.Count; }
        }

        /// <summary>
        /// Gets the property at the designated position.
        /// </summary>
        public IXmlProperty this[int index]
        {
            get { return (IXmlProperty)_fields[index]; }
        }

        /// <summary>
        /// Gets the property with the given name.
        /// </summary>
        public IXmlProperty this[string id]
        {
            get
            {
                foreach (IXmlProperty prop in _fields)
                {
                    if (prop.Id == id)
                        return prop;
                }

                return null;
            }
        }


        #region Form Event Callbacks

        /// <summary>
        /// Event callback for the Form activation. Sets focus to the first control
        /// on the current tab.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XmlForm_Activated(object sender, System.EventArgs e)
        {
            XmlTabPage ft = (XmlTabPage)tabControl.SelectedTab.Tag;
            ft.FocusFirst();
        }


        /// <summary>
        /// Event callback whenever the tab is changed, which will attempt to set 
        /// focus to the first control on the sheet.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event arguments.</param>
        private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            XmlTabPage tab = (XmlTabPage)tabControl.TabPages[tabControl.SelectedIndex].Tag;
            tab.FocusFirst();
        }

        /// <summary>
        /// Event callback when the user attempts to dismiss the form and commit
        /// the values: this requires that all properties have valid values.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event arguments.</param>
        private void buttonOK_Click(object sender, System.EventArgs e)
        {
            if (this.IsValid)
            {
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                FocusFirstError();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Places the focus on the first element with an error.
        /// </summary>
        private void FocusFirstError()
        {
            foreach (Control control in _fields)
            {
                IXmlProperty property = (IXmlProperty)control;

                if (property.IsValid == false)
                {
                    Control firstControl = FormUtils.LowestTabIndex(control);

                    if (firstControl != null)
                    {
                        Debug.Assert(control.Parent.Parent.GetType() == typeof(TabPage));
                        tabControl.SelectedTab = (TabPage)control.Parent.Parent;

                        firstControl.Focus();
                        return;
                    }
                }
            }
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            if (this.Tag is IDisposable)
            {
                IDisposable d = (IDisposable)this.Tag;
                d.Dispose();
            }
        }

        #endregion

        #region load Properties from xml file
        private XmlDocument LoadPropertiesFromFile(string file)
        {
            FileInfo info = new FileInfo(Assembly.GetExecutingAssembly().Location);
            string filename = System.IO.Path.Combine(info.DirectoryName, file);
            XmlDocument doc = null;

            try
            {
                XmlTextReader reader = new XmlTextReader(filename);
                XmlReaderSettings readerSettings = new XmlReaderSettings();
                readerSettings.ValidationType = ValidationType.Schema;
                XmlReader objXmlReader = XmlReader.Create(reader, readerSettings);

                doc = new XmlDocument();

                // TODO: Add validation
                doc.Load(objXmlReader);
            }
            catch (Exception)
            {
                doc = null;
            }

            return doc;
        }
        #endregion

        private string GetProperty(Shape shape, string section, string property, string attribute)
        {
            string cellLocation = string.Empty;
            if ((section == string.Empty) && (property == string.Empty))
                cellLocation = string.Format("{0}", attribute);
            else
                cellLocation = string.Format("{0}.{1}.{2}", section, property, attribute);
            Cell cell;

            try
            {
                cell = shape.get_Cells(cellLocation);
            }
            catch (COMException)
            {
                return null;
            }

            return FormulaToString(cell.Formula);
        }

        private string FormulaToString(string formula)
        {
			string result = formula;
            string FormulaQuote = "\"";


			if ( formula.StartsWith( FormulaQuote ) && formula.EndsWith( FormulaQuote ) )
			{
				result = formula
					.Substring( 1, formula.Length-2 )
					.Replace( FormulaQuote + FormulaQuote, FormulaQuote );
			}

			return result;
		}

        public void AddGlobalData(string key, string value)
        {
            foreach (StringButtonControl control in lookUpControls)
                control.GlobalData.Add(key, value);
        }

    }
}