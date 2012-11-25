using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using Genghis.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Midgard.XmlForms
{
	/// <summary>
	/// Base class for properties.
	/// </summary>
#if DEBUG
	public class BaseProperty : UserControl
#else
	public abstract class BaseProperty : UserControl
#endif
	{

#if DEBUG
		/// <remarks>
		/// This is required for designer support: VS expects an empty constructor.
		/// </remarks>
		public BaseProperty() {}
#endif

		#region Private Members

		private string _id;
		private string _label;
		private string _annotation;
		private ContainerValidator _validator;
        private XmlNodeList _changeShape;
        private XmlNodeList _changeByConnection;

        protected DefaultValues[] defaultValues;

        protected Regex _mask;
		#endregion

		/// <summary>
		/// Intitializes all common attributes of the property control,
		/// according to it's definition.
		/// </summary>
		/// <param name="definition">XML Definition.</param>
		protected void Initialize( XmlElement definition )
		{
			_id = definition.Attributes[ "id" ].Value;
			_label = NewLines( definition.Attributes[ "label" ].Value );
			_annotation = GetAnnotation( definition );
            _changeShape = definition.SelectNodes("changeShape", FormsNamespace.NamespaceManager );
			_validator = new ContainerValidator( this );

            XmlNode dvNode = definition.SelectSingleNode("defaultValues", FormsNamespace.NamespaceManager);
            if (dvNode != null)
                defaultValues = PrepareDefaultValues(dvNode);

            _changeByConnection = definition.SelectNodes("changeByConnection", FormsNamespace.NamespaceManager);

            dvNode = definition.SelectSingleNode("mask", FormsNamespace.NamespaceManager);
            if (dvNode != null) 
            {
                _mask = new Regex(dvNode.InnerText, RegexOptions.IgnorePatternWhitespace);
            }
		}


		#region Public Properties

		/// <summary>
		/// Gets the ID for this property. Property ID are unique across
		/// a shape.
		/// </summary>
		public string Id
		{
			get { return _id; }
		}


		/// <summary>
		/// Gets the label of the current property.
		/// </summary>
		public string Label
		{
			get { return _label; }
		}

		/// <summary>
		/// Gets wether the currently property is valid or not.
		/// </summary>
		public bool IsValid
		{
			get
			{
				return _validator.IsValid();
			}
		}

		/// <summary>
		/// Gets the annotation.
		/// </summary>
		public string Annotation
		{
			get { return _annotation; }
		}

		#endregion

		#region Helper Methods

		/// <summary>
		/// Gets the annotation for the given element definition.
		/// </summary>
		/// <param name="definition">Element definition.</param>
		/// <returns>Help string, or null if not set.</returns>
		protected string GetAnnotation( XmlElement definition )
		{
			XmlElement annotationElem = (XmlElement) definition.SelectSingleNode( "annotation", FormsNamespace.NamespaceManager );

			if ( annotationElem == null )
				return null;

			return annotationElem.InnerText;
		}


      public bool IsInvisibleInMode(XmlElement definition, string mode)
      {
         XmlElement invisibleElem = (XmlElement)definition.SelectSingleNode("invisible", FormsNamespace.NamespaceManager);
         if (invisibleElem == null)
            return false;

         XmlAttribute modesAttr = invisibleElem.Attributes["modes"];
         if (modesAttr == null)
            return true;

         string[] modes = modesAttr.Value.Split(',');
         int index = Array.IndexOf(modes, mode);

         return (index > -1);
      }

      public bool IsDisabledInMode(XmlElement definition, string mode)
      {
         XmlElement disabledElem = (XmlElement)definition.SelectSingleNode("disabled", FormsNamespace.NamespaceManager);
         if (disabledElem == null)
            return false;

         XmlAttribute modesAttr = disabledElem.Attributes["modes"];
         if (modesAttr == null)
            return true;

         string[] modes = modesAttr.Value.Split(',');
         int index = Array.IndexOf(modes, mode);

         return (index > -1);
      }

		/// <summary>
		/// Returns wether the given property definition is required for the
		/// designated mode.
		/// </summary>
		/// <param name="definition">Element definition.</param>
		/// <param name="mode">Name of the design mode.</param>
		/// <returns>True if the property is required, False otherwise.</returns>
		protected bool IsRequired( XmlElement definition, string mode )
		{
			XmlElement requiredElem = (XmlElement) definition.SelectSingleNode( "required", FormsNamespace.NamespaceManager );

			/*
			 * No <required/> element has been set. Therefore, it is *never* required.
			 */
			if ( requiredElem == null )
				return false;


			XmlAttribute modesAttr = requiredElem.Attributes[ "modes" ];
			
			/*
			 * No @modes attribute is set. Therefore, no matter what the value of mode
			 * is, this property is always required.
			 */
			if ( modesAttr == null )
				return true;


			string[] modes = modesAttr.Value.Split( ',' );
			int index = Array.IndexOf( modes, mode );


			/*
			 * This property is only required as long as mode has been found
			 * in the modes array, in which case index will be greater than -1.
			 */
			return ( index > -1 );

		}

        /// <summary>
        /// Applys the valid rules to the shape
        /// </summary>
        protected void ApplyChangeShapeRules()
        {
            if (this.Enabled)
            {
                if (this._changeShape != null && this._changeShape.Count > 0)
                {
                    if (this.ParentForm != null)
                    {
                        IXmlShape shape = (IXmlShape)this.ParentForm.Tag;
                        foreach (XmlElement el in _changeShape)
                        {
                            if (el.Attributes["checkFor"] != null && el.Attributes["checkFor"].Value == GetCheckForValue())
                            {
                                shape.ChangeShapeProperties(el.Attributes["property"].Value, el.Attributes["value"].Value);
                            }
                            if (el.Attributes["checkForDifferentThan"] != null && el.Attributes["checkForDifferentThan"].Value != GetCheckForValue())
                            {
                                shape.ChangeShapeProperties(el.Attributes["property"].Value, el.Attributes["value"].Value);
                            }
                        }
                    }
                    else
                    {
                        //TODO - In validate and export this.ParentForm comes here as null
                        //throw(new Exception);
                        //HAMMER - see why the form comes null or if needs to \
                    }

                }
            }
        }

        /// <summary>
        /// Returns the value to be compared in the changeShape rule. Must be implemented by all controls that use the rule.
        /// </summary>
        /// <returns></returns>
        protected virtual string GetCheckForValue()
        {
            throw new ApplicationException("The method GetCheckForValue must be implemented to use the changeShape tag.");
        }

        public virtual void ChangeByConnection(string connectedTo, string connectedFrom)
        {
            if (this._changeByConnection != null && this._changeByConnection.Count > 0)
            {
                IXmlShape shape = (IXmlShape)this.ParentForm.Tag;
                foreach (XmlElement el in _changeByConnection)
                {
                    string s = el.Attributes["shape"].Value;
                    string t = el.Attributes["type"].Value;
                    bool applyRule = false;
                    switch (t)
                    {
                        case "isConnectedTo":
                            applyRule = (connectedTo != null && s.IndexOf(connectedTo) >= 0);
                            break;
                        case "isNotConnectedTo":
                            applyRule = (connectedTo == null || s.IndexOf(connectedTo) < 0);
                            break;
                        case "isConnectedFrom":
                            applyRule = (connectedFrom != null && s.IndexOf(connectedFrom) >= 0);
                            break;
                        case "isNotConnectedFrom":
                            applyRule = (connectedFrom == null || s.IndexOf(connectedFrom) < 0);
                            break;
                    }
                    if (applyRule)
                    {
                        bool val = (el.Attributes["value"].Value == "false" ? false : true);
                        switch (el.Attributes["property"].Value)
                        {
                            case "enabled": this.Enabled = val; break;
                            case "disabled": this.Enabled = !val; break;
                            case "visible": this.Visible = val; break;
                            case "invisible": this.Visible = !val; break;
                        }
                    }
                }
            }
        }

		/// <summary>
		/// Displays text information associated with a control.
		/// </summary>
		/// <param name="control">The control to which the help is associated.</param>
		/// <param name="help">Help text. This argument can be null, in which case nothing
		/// will happen (as there is no text to display).</param>
		protected void SetHelpTooltip( Control control, string help )
		{
			if ( help == null )
				return;

			if ( control == null )
				throw new ArgumentNullException( "control" );


			Point local = new Point( 100, 15 );
			Point screen = control.PointToScreen( local );
			Help.ShowPopup( control, help, screen );
		}


		/// <summary>
		/// Converts the "\n" character sequence into a new-line.
		/// </summary>
		/// <param name="s">Input string.</param>
		/// <returns>Output string.</returns>
		protected string NewLines( string s )
		{
			return s.Replace( "\\n", Environment.NewLine );
		}

		private void InitializeComponent()
		{
			// 
			// BaseProperty
			// 
			this.Name = "BaseProperty";
			this.Size = new System.Drawing.Size(408, 104);

		}


		/// <summary>
		/// Appends a child element to an existing element.
		/// </summary>
		/// <param name="parent">Existint parent element.</param>
		/// <param name="elementName">Name of the child element to create.</param>
		/// <param name="innerText">Inner text of the child element.</param>
		/// <returns>Child element node.</returns>
		protected XmlElement AppendChild( XmlElement parent, string elementName, string innerText )
		{
			XmlElement element = parent.OwnerDocument.CreateElement( elementName );
			element.InnerText = innerText;
			parent.AppendChild( element );

			return element;
		}

        /// <summary>
        /// To set the value of the controls
        /// </summary>
        /// <param name="value"></param>
        public virtual void Set(string value, bool required, bool disabled, bool invisible, bool forceReplacement) { }

        /// <summary>
        /// Organize fields and values to set in a HybridDictionary
        /// </summary>
        /// <param name="defaultValues"></param>
        /// <returns></returns>
        private static DefaultValues[] PrepareDefaultValues(XmlNode defaultValues)
        {
            XmlNodeList nodes = defaultValues.ChildNodes;
            if (nodes.Count == 0)
                return null;

            DefaultValues[] arrDV = new DefaultValues[nodes.Count];
            DefaultValues dvObj;
            for (int i = 0; i < nodes.Count; i++)
            {
                dvObj = new DefaultValues();

                dvObj.ValueToCompare = (nodes[i].Attributes["value"] != null)? nodes[i].Attributes["value"].Value: null;

                if (nodes[i].Name == "equals")
                    dvObj.compareType = CompareType.EQUALS;
                else if (nodes[i].Name == "differs")
                    dvObj.compareType = CompareType.DIFFERS;
                else if (nodes[i].Name == "any")
                    dvObj.compareType = CompareType.NONE;

                if (nodes[i].ChildNodes.Count > 0)
                {
                    dvObj.FieldsToSet = new ArrayList();
                    foreach (XmlNode field in nodes[i].ChildNodes)
                    {
                        dvObj.FieldsToSet.Add(
                            new string[] {
                                field.Attributes["id"].Value,
                                field.Attributes["value"]  != null ? field.Attributes["value"].Value  : null,
                                (field.Attributes["required"] != null && field.Attributes["required"].Value.ToLower() == bool.TrueString.ToLower() ? bool.TrueString : bool.FalseString),
                                (field.Attributes["disabled"] != null && field.Attributes["disabled"].Value.ToLower() == bool.TrueString.ToLower() ? bool.TrueString : bool.FalseString),
                                (field.Attributes["invisible"] != null && field.Attributes["invisible"].Value.ToLower() == bool.TrueString.ToLower() ? bool.TrueString : bool.FalseString),
                                (field.Attributes["set"] != null ? field.Attributes["set"].Value:""),
                                null // this is the value to inspect and is set on DoManegedNodes
                            }
                        );
                    }
                }

                arrDV[i] = dvObj;
            }

            return arrDV;
        }

        protected void DoManagedNodes(string valueToInspect)
        {
            if (this.Visible && this.Enabled && defaultValues != null && this.ParentForm != null)
            {
                for (int i = 0; i < defaultValues.Length; i++)
                {
                    if ((defaultValues[i].compareType == CompareType.EQUALS && valueToInspect.ToLower() == defaultValues[i].ValueToCompare.ToLower()) ||
                        (defaultValues[i].compareType == CompareType.DIFFERS && valueToInspect.ToString().ToLower() != defaultValues[i].ValueToCompare.ToLower()) ||
                        (defaultValues[i].compareType == CompareType.NONE)
                    ) {

                        foreach (string[] fieldsToSet in defaultValues[i].FieldsToSet)
                        {
                            fieldsToSet[6] = valueToInspect;
                            SetField(fieldsToSet, this.ParentForm);
                        }

                    }

                }
            }
        
        }

        /// <summary>
        /// Search the field by ID and sets the value
        /// </summary>
        /// <param name="fieldsToSet"></param>
        /// <param name="control"></param>
        protected void SetField(string[] fieldsToSet, Control control, bool forceReplacement)
        {
            if (control.Controls.Count > 0)
            {
                foreach (Control cs in control.Controls)
                {
                    SetField(fieldsToSet, cs, forceReplacement);
                    BaseProperty b = cs as BaseProperty;
                    if (b != null && b.Id.ToLower() == fieldsToSet[0].ToLower())
                    {
                        b.Set(fieldsToSet[1], bool.Parse(fieldsToSet[2]), bool.Parse(fieldsToSet[3]), bool.Parse(fieldsToSet[4]), forceReplacement);

                        if ((fieldsToSet.Length == 6) && (fieldsToSet[5] != null) && (fieldsToSet[5].Length > 0))
                            CallObjectMember(cs, fieldsToSet[5], (fieldsToSet[1] != null && fieldsToSet[6] != null) ? fieldsToSet[1].Replace("{0}", fieldsToSet[6]) : fieldsToSet[1]);
                    }
                }
            }
        }

        protected void SetField(string[] fieldsToSet, Control control)
        {
            SetField(fieldsToSet, control, false);
        }

        protected void CallObjectMember(object obj, string memberName, string value)
        {
            Type ct = obj.GetType();
            MemberInfo[] mi = ct.GetMember(memberName);
            if ((mi != null) && (mi.Length > 0))
            {
                if (mi[0].MemberType == MemberTypes.Field) {
                    ((FieldInfo)mi[0]).SetValue(obj, value);
                } else if (mi[0].MemberType == MemberTypes.Property) {
                    ((PropertyInfo)mi[0]).SetValue(obj, value, null);
                }
            }

        }

		#endregion
	}

    /// <summary>
    /// Organize information about node values dependencies
    /// </summary>
    public class DefaultValues
    {
        public string ValueToCompare;
        public CompareType compareType;
        public ArrayList FieldsToSet;

        public DefaultValues()
        { }

        public DefaultValues(string valueTocompare, ArrayList fieldsToSet)
        {
            this.ValueToCompare = valueTocompare;
            this.FieldsToSet = fieldsToSet;
        }
    }

    public enum CompareType
    {
        NONE = 0,
        EQUALS = 1,
        DIFFERS = 2
    }

}