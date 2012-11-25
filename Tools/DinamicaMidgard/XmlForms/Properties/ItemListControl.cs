using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;
using Genghis.Windows.Forms;
using Microsoft.Office.Interop.Visio;


namespace Midgard.XmlForms
{
    /// <summary>
    /// Summary description for StringControl.
    /// </summary>
    public class ItemListControl : BaseProperty, IXmlProperty
    {
        #region Constants
        private const int columnsXMin = 1;
        private const int columnsXMax = 25;
        #endregion

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components = null;
        private ListBox valueControl;
        private String ItemType = null;
        private String ItemName = "item";
        private String ComplexLabel = null;
        private String ComplexPattern = null;
        private Button add;
        private Button remove;
        private Button edit;
        private Button up;
        private Button down;
 
        private Label labelControl;
        private bool processShape = false;

        private String childPattern = null;

        private Microsoft.Office.Interop.Visio.Page page;

        private RequiredFieldValidator req;
        private string _mode;
        private XmlElement _definition;
        //private OperacoesInternas opr;

        protected Button Down
        {
            get { return down; }
        }

        protected Button Up
        {
            get { return up; }
        }

        protected Button Edit
        {
            get { return edit; }
        }

        protected Button Remove
        {
            get { return remove; }
        }

        protected Button Add
        {
            get { return add; }
        }

        public ListBox SelectionList
        {
            get { return valueControl; }
        }
        /// <summary>
        /// Initializes a new insance of the StringControl class.
        /// </summary>
        /// <param name="definition">Definition of the property.</param>
        /// <param name="mode">The current design mode.</param>
        public ItemListControl(XmlElement definition, string mode,Page _page)
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
            page = _page;
            //opr = _opr;
            _mode = mode;
            _definition = definition;
            #region Use Standard Sizes

            labelControl.Top = Property.LabelTop;
            labelControl.Left = Property.LabelLeft;
            labelControl.Width = Property.LabelWidth;
            valueControl.Top = Property.ValueTop;
            valueControl.Left = Property.ValueLeft;

            //valueControl.Width = Property.ValueWidth;
            this.Height = valueControl.Height + Property.PropertyMarginBottom;
            this.Height = valueControl.Height + Property.PropertyMarginBottom;

            #endregion

            // Initialize control
            Initialize(definition);

            XmlAttribute itemAttribute = definition.Attributes["itemType"];
            if (itemAttribute != null)
            {
                ItemType = itemAttribute.Value;
            }
            itemAttribute = definition.Attributes["itemName"];
            if (itemAttribute != null)
            {
                ItemName = itemAttribute.Value;
            }
            itemAttribute = definition.Attributes["complexLabel"];
            if (itemAttribute != null)
            {
                ComplexLabel = itemAttribute.Value;
            }
            itemAttribute = definition.Attributes["complexPattern"];
            if (itemAttribute != null)
            {
                ComplexPattern = itemAttribute.Value;
            }
            itemAttribute = definition.Attributes["processType"];
            if (itemAttribute != null)
            {
                processShape = bool.Parse(itemAttribute.Value);
            }

            itemAttribute = definition.Attributes["childPattern"];
            if (itemAttribute != null)
            {
                childPattern = itemAttribute.Value;
            }

            labelControl.Text = this.Label;
            valueControl.Text = "";

            /*
             * Height / number of rows.
             */
            labelControl.Height = valueControl.Height;

            /*
             * Check required
             */
            if (IsRequired(definition, mode) == true)
            {
                req = new RequiredFieldValidator();
                req.ControlToValidate = this.valueControl;
                req.ControlToValidate.Validating += new CancelEventHandler(ControlToValidate_Validating);
                req.ErrorMessage = "Required.";
                req.IconPadding = 2;
            }
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

                if (req != null)
                {
                    req.ControlToValidate.Dispose();
                    req.Icon.Dispose();
                    req.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelControl = new System.Windows.Forms.Label();
            this.valueControl = new System.Windows.Forms.ListBox();
            this.add = new System.Windows.Forms.Button();
            this.remove = new System.Windows.Forms.Button();
            this.edit = new System.Windows.Forms.Button();
            this.up = new System.Windows.Forms.Button();
            this.down = new System.Windows.Forms.Button();
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
            this.valueControl.HorizontalScrollbar = true;
            this.valueControl.Location = new System.Drawing.Point(104, 0);
            this.valueControl.Name = "valueControl";
            this.valueControl.Size = new System.Drawing.Size(184, 121);
            this.valueControl.TabIndex = 1;
            this.valueControl.DoubleClick += new System.EventHandler(this.Edit_Click);
            // 
            // add
            // 
            this.add.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.add.Location = new System.Drawing.Point(0, 48);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(80, 23);
            this.add.TabIndex = 2;
            this.add.Text = "&Add ...";
            this.add.Click += new System.EventHandler(this.Add_Click);
            // 
            // remove
            // 
            this.remove.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.remove.Location = new System.Drawing.Point(0, 96);
            this.remove.Name = "remove";
            this.remove.Size = new System.Drawing.Size(80, 23);
            this.remove.TabIndex = 4;
            this.remove.Text = "&Remove";
            this.remove.Click += new System.EventHandler(this.Remove_Click);
            // 
            // edit
            // 
            this.edit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.edit.Location = new System.Drawing.Point(0, 72);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(80, 23);
            this.edit.TabIndex = 3;
            this.edit.Text = "&Edit ...";
            this.edit.Click += new System.EventHandler(this.Edit_Click);
            // 
            // up
            // 
            this.up.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.up.Location = new System.Drawing.Point(296, 0);
            this.up.Name = "up";
            this.up.Size = new System.Drawing.Size(59, 23);
            this.up.TabIndex = 5;
            this.up.Text = "&Up";
            this.up.Click += new System.EventHandler(this.Up_Click);
            // 
            // down
            // 
            this.down.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.down.Location = new System.Drawing.Point(296, 24);
            this.down.Name = "down";
            this.down.Size = new System.Drawing.Size(59, 23);
            this.down.TabIndex = 6;
            this.down.Text = "&Down";
            this.down.Click += new System.EventHandler(this.Down_Click);
            // 
            // ItemListControl
            // 
            this.Controls.Add(this.down);
            this.Controls.Add(this.up);
            this.Controls.Add(this.edit);
            this.Controls.Add(this.remove);
            this.Controls.Add(this.add);
            this.Controls.Add(this.valueControl);
            this.Controls.Add(this.labelControl);
            this.Name = "ItemListControl";
            this.Size = new System.Drawing.Size(368, 136);
            this.ResumeLayout(false);

        }

        #endregion

        /// <summary>
        /// Fills the XML form data based on the serialization data.
        /// </summary>
        /// <param name="element">Element serialization.</param>
        public void Set(XmlElement element)
        {
            valueControl.Items.Clear();

            foreach (XmlElement el in element.SelectNodes(this.ItemName))
            {
                valueControl.Items.Add(new ListItemsObject(el, ComplexLabel, ComplexPattern, childPattern));
            }

            if (IsRequired(_definition, _mode))
            {
                req = new RequiredFieldValidator();
                req.ControlToValidate = this.valueControl;
                req.ControlToValidate.Validating += new CancelEventHandler(ControlToValidate_Validating);
                req.ErrorMessage = "Required.";
                req.IconPadding = 2;
            }
        }

        /// <summary>
        /// Fills the destination element with the serialization data for the
        /// property.
        /// </summary>
        /// <param name="element">Element serialization.</param>
        /// <returns>True if the element is to be append to the shape XML, false otherwise.</returns>
        public virtual bool Fill(XmlElement element)
        {
            if (valueControl.Items.Count == 0)
                return false;

            foreach (ListItemsObject column in valueControl.Items)
            {
                element.AppendChild(element.OwnerDocument.ImportNode(column.Column, true));
            }
            return true;
        }


        /// <summary>
        /// Displays help, if available.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="hlpevent">Event arguments.</param>
        public void PropertyHelpRequested(object sender, HelpEventArgs hlpevent)
        {
            SetHelpTooltip((Control)sender, this.Annotation);
        }

        private void Add_Click(object sender, EventArgs e)
        {
            if (valueControl.Items.Count < columnsXMax)
            {
                using (XmlForm form = new XmlForm())
                {
                    form.LoadDefinition(this.ItemType, null);
                    form.Tag = this.FindForm().Tag;
                    form.Design("", page);
                    if ("ParameterItem" == this.ItemType)
                        form.AddGlobalData("ruleId", Parent.Parent.Controls[3].Controls[0].Controls[1].Text);
                    DialogResult result = form.ShowDialog(this);

                    if (result == DialogResult.OK)
                    {
                        XmlDocument xdoc = form.GetShapeXml(this.ItemName);

                        valueControl.Items.Add(new ListItemsObject(xdoc.DocumentElement, ComplexLabel, ComplexPattern, childPattern));
                    }
                }
            }
        }

        private void Remove_Click(object sender, EventArgs e)
        {
            if (valueControl.SelectedIndex >= 0)
            {
                valueControl.Items.RemoveAt(valueControl.SelectedIndex);
            }
        }

        protected void Edit_Click(object sender, EventArgs e)
        {
            if (valueControl.SelectedIndex >= 0)
            {
                int index = valueControl.SelectedIndex;
                using (XmlForm form = new XmlForm())
                {
                    form.LoadDefinition(this.ItemType, null);
                    form.Tag = this.FindForm().Tag;
                    form.Design("", page);
                    if ("ParameterItem" == this.ItemType)
                        form.AddGlobalData("ruleId", Parent.Parent.Controls[1].Controls[0].Controls[1].Text);
                    ListItemsObject dgo = (ListItemsObject)valueControl.Items[index];
                    form.SetShapeXml(String.Format("<properties>{0}</properties>", dgo.Column.InnerXml));
                    DialogResult result = form.ShowDialog(this);

                    if (result == DialogResult.OK)
                    {
                        XmlDocument xdoc = form.GetShapeXml(this.ItemName);

                        valueControl.Items[index] = new ListItemsObject(xdoc.DocumentElement, ComplexLabel, ComplexPattern, childPattern);
                    }
                }
            }
        }

        private void Up_Click(object sender, EventArgs e)
        {
            if (valueControl.SelectedIndex >= 0)
            {
                int index = valueControl.SelectedIndex;
                int max = valueControl.Items.Count;
                ListItemsObject obj = (ListItemsObject)valueControl.Items[index];
                valueControl.Items.RemoveAt(index);
                if (index == 0)
                {
                    valueControl.Items.Insert(max - 1, obj);
                    valueControl.SelectedIndex = max - 1;
                }
                else
                {
                    valueControl.Items.Insert(index - 1, obj);
                    valueControl.SelectedIndex = index - 1;
                }
            }
        }

        private void Down_Click(object sender, EventArgs e)
        {
            if (valueControl.SelectedIndex >= 0)
            {
                int index = valueControl.SelectedIndex;
                int max = valueControl.Items.Count;
                ListItemsObject obj = (ListItemsObject)valueControl.Items[index];
                valueControl.Items.RemoveAt(index);
                if (index == max - 1)
                {
                    valueControl.Items.Insert(0, obj);
                    valueControl.SelectedIndex = 0;
                }
                else
                {
                    valueControl.Items.Insert(index + 1, obj);
                    valueControl.SelectedIndex = index + 1;
                }
            }
        }

        private void ControlToValidate_Validating(object sender, CancelEventArgs e)
        {

            if (this.valueControl.Items.Count > 0 && this.valueControl.SelectedIndex < 0)
                this.valueControl.SelectedIndex = 0;
        }
    }
}