using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Microsoft.Office.Interop.Visio;
using Midgard.XmlForms;


namespace Midgard.Interop.Shapes
{
	/// <summary>
	/// Base class for shape handlers.
	/// </summary>
	public abstract class BaseShape : IXmlShape, IShapeHandler, IDisposable
	{
		#region Private Members
        public const string ShapePrefix = "Process-";

		private Shape _shape;
		private XmlForm _form;
		private XmlElement _attributes;
		private string _element;
		private ModelShapeType _type;

		#endregion

		/// <summary>
		/// Initializes a new instance of the BaseShape class.
		/// </summary>
		/// <param name="shape">Visio shape on which it is based.</param>
		/// <param name="form">Form </param>
		/// <param name="attributes">Configuration attributes.</param>
		public BaseShape( object shape, XmlForm form, XmlElement attributes )
		{
			_shape = (Shape) shape;
			_form = form;
			_attributes = attributes;

			_element = _attributes.Attributes[ "element" ].Value;
			_type = (ModelShapeType) Enum.Parse( typeof( ModelShapeType ), _attributes.Attributes[ "type" ].Value, true );
		}

		#region Public Properties

		/// <summary>
		/// Gets the Quartz Model shape type.
		/// </summary>
		public ModelShapeType Type
		{
			get { return _type; }
		}

		/// <summary>
		/// Gets the name of the serialization element.
		/// </summary>
		public string Element
		{
			get { return _element; }
		}

		/// <summary>
		/// Gets the associated shape.
		/// </summary>
		public Shape VisioShape
		{
			get { return _shape; }
		}

		/// <summary>
		/// Gets the associated form.
		/// </summary>
		public XmlForm Form
		{
			get { return _form; }
		}

		/// <summary>
		/// Gets the configuration attributes from the shape file.
		/// </summary>
		public XmlElement Attributes
		{
			get { return _attributes; }
		}

		#endregion


		/// <summary>
		/// Adds shape-specific wiring to the XML-based WinForm controls.
		/// </summary>
		public virtual void Design()
		{
		}

        /// <summary>
        /// Changes shape-specific properties
        /// </summary>
        public virtual void ChangeShapeProperties(string prop, string value)
        {
            VisioShape.get_Cells(prop).Formula = value;
        }

        /// <summary>
        /// Where the shape is connected to
        /// </summary>
        /// <returns>The name of the shape</returns>
        public string ConnectedTo()
        {
            if (VisioShape.Connects.Count > 1)
            {
                Microsoft.Office.Interop.Visio.Shape to = VisioShape.Connects[2].ToCell.Shape;
                return VisioUtils.GetProperty(to, "User", "QuartzShape");
            }
            return null;
        }
        /// <summary>
        /// Where the shape is connected from
        /// </summary>
        /// <returns>The name of the shape</returns>
        public string ConnectedFrom()
        {
            if (VisioShape.Connects.Count > 1)
            {
                Microsoft.Office.Interop.Visio.Shape to = VisioShape.Connects[1].ToCell.Shape;
                return VisioUtils.GetProperty(to, "User", "QuartzShape");
            }
            return null;
        }
        /// <summary>
		/// Requests that the shape process the shape command.
		/// </summary>
		/// <param name="command">Command requested by the shape.</param>
		/// <param name="context">Execution context: these are the arguments specified in the event. Please
		/// note that the command is one of the arguments.</param>
		/// <param name="settings">Display settings.</param>
		/// <returns>New display settings.</returns>
		public DisplaySettings Execute( string command, NameValueCollection context, DisplaySettings settings, bool forceChange )
		{
            bool dropEvent = (string.IsNullOrEmpty(VisioUtils.GetProperty(_shape, "Prop", "ShapeXML")));

            if (command == "drop" && !dropEvent) return settings;
			/*
			 * If the command was a "drop" (shape dragged from stencil into page)
			 * then execute it right now. This must be done before the following
			 * statement.
			 */
			if ( command == "drop" )
				OnPageDrop();


			/*
			 * 
			 */
            string shapeXml = null;
            if (forceChange && VisioUtils.ShapeCustomProps != null)
            {
                shapeXml = VisioUtils.ShapeCustomProps;
                VisioUtils.SetProperty(VisioShape, "Prop", "ShapeXML", shapeXml);
            }
            else
                shapeXml = VisioUtils.GetProperty(VisioShape, "Prop", "ShapeXML");
            Form.SetShapeXml(shapeXml);
            /*
             * Command router.
             */
			switch ( command )
			{
				case "drop":
				case "edit":
                    return OnShapeEdit(command, context, settings, forceChange);

				default:
					return OnExecuteCustom( command, context, settings );
			}		
		}


		/// <summary>
		/// Returns the XML properties for the current shape.
		/// </summary>
		/// <returns>Xml node with properties.</returns>
		public XmlNode GetProperties()
		{
			return Form.GetShapeXml().DocumentElement;
		}


		/// <summary>
		/// Loads the properties from the Visio shape.
		/// </summary>
		public void LoadProperties()
		{
			string shapeXml = VisioUtils.GetProperty( VisioShape, "Prop", "ShapeXML" );
			Form.SetShapeXml( shapeXml );
		}


		/// <summary>
		/// Performs validation on the current shape.
		/// </summary>
		/// <param name="validationData">Context dependent validation data.</param>
		/// <returns>An instance of the ShapeError class if more than one error occured with
		/// the current shape, otherwise null.</returns>
		public virtual ShapeError Validate( object validationData )
		{
			ShapeError error;

			string e1 = null;

			if ( Form.IsValid == false )
			{
				e1 = "Invalid properties. ";
			}

			string e2 = DoContextValidation( validationData );
		    
			string e = string.Format( "{0}{1}", e1, e2 );

			if ( e != null && e.Length > 0 )
			{
				PaintAsInvalid();
				error = new ShapeError( this, e );
			}
			else
			{
				error = null;
				if (!PaintAsValid())
                    error = new ShapeError(this, "Invalid connection.");

			}

			return error;
		}


		/// <summary>
		/// Performs context validation on the current shape.
		/// </summary>
		/// <param name="validationData">Context dependent validation data.</param>
		/// <returns>A non-empty string in case of error, otherwise null.</returns>
		protected abstract string DoContextValidation( object validationData );


		#region OnPageDrop

		/// <summary>
		/// Called when the drop occurs onto a page, performing sequence
		/// initialization.
		/// </summary>
		protected void OnPageDrop()
		{
			XmlNodeList sequenceElemts = (XmlNodeList) _attributes.SelectNodes( "sequence[ @ref ]", FormsNamespace.NamespaceManager );

			if ( sequenceElemts == null )
				return;
			StringBuilder sb = new StringBuilder("<properties>");
			foreach(XmlNode sequenceElem in sequenceElemts)
			{
				string property = sequenceElem.Attributes[ "ref" ].Value;
				string format  = sequenceElem.Attributes[ "format" ].Value;
				string pattern = string.Format( CultureInfo.InvariantCulture,
					"<{0}>{1}</{0}>", 
					property,
					sequenceElem.Attributes[ "pattern" ].Value );

				Regex regex = new Regex( pattern, RegexOptions.ExplicitCapture );
				int max = 0;

				foreach ( Shape iterShape in _shape.ContainingPage.Shapes )
				{
					string shapeName = VisioUtils.GetProperty( iterShape, "User", "QuartzShape" );
					if ( shapeName == null )
						continue;

					string shapeXml = VisioUtils.GetProperty( iterShape, "Prop", "ShapeXML" );

					Match m = regex.Match( shapeXml );
					if ( m.Success == false )
						continue;

					int index = int.Parse( m.Groups[ "index" ].Value, CultureInfo.InvariantCulture );

					if ( index > max )
						max = index;
				}

				
				sb.AppendFormat( "<{0}>", property );
				sb.AppendFormat( format, ++max );
				sb.AppendFormat( "</{0}>", property );
			}
			sb.AppendFormat( "</properties>");
			VisioUtils.SetProperty( _shape, "Prop", "ShapeXML", sb.ToString() );
		}

		#endregion

		#region OnShapeEdit

		/// <summary>
		/// Edits the properties of the designated shape.
		/// </summary>
		/// <param name="command">Command requested by the shape.</param>
		/// <param name="context">Execution context: these are the arguments specified in the event. Please
		/// note that the command is one of the arguments.</param>
		/// <param name="settings">Display settings.</param>
		/// <returns>New display settings.</returns>
        protected DisplaySettings OnShapeEdit(string command, NameValueCollection context, DisplaySettings settings, bool forceChange)
		{
			if ( settings == null )
				throw new ArgumentNullException( "settings" );

			if ( Form.PropertyCount == 0 )
				return settings;

			DisplaySettings ns = (DisplaySettings) settings.Clone();
			Form.Left = settings.Left;
			Form.Top = settings.Top;
			Form.SelectedTabName = settings.TabName;
            if (!forceChange) 
			    Form.ShowDialog();


            if (Form.DialogResult == DialogResult.OK || forceChange)
			{
                XmlDocument xml = Form.GetShapeXml();

				string shapeText = GetShapeText( xml );

                #region Channel Text
                StringBuilder channelText = new StringBuilder();
                
                foreach (XmlElement xe in xml.SelectNodes("properties/channels/channel"))
                {
                    if (channelText.Length == 0)
                        channelText.Append("[");
                    else
                        channelText.Append(", ");
                    channelText.Append(xe.InnerText);
                }
                if (channelText.Length > 0)
                    channelText.Append(']');

                #endregion

                SetShapeText( shapeText, channelText.ToString());

			    VisioUtils.SetProperty(_shape, "Prop", "ShapeXML", xml.OuterXml);

				ns.TabName = Form.SelectedTabName;
			}
			else
			{
                if (command == "drop")
                {
                    try
                    {
                        VisioShape.Delete();
                    }
                    catch (Exception) { }

                }
			}

			ns.Left = Form.Left;
			ns.Top = Form.Top;

			return ns;
		}


		/// <summary>
		/// Obtains the text that is to be displayed on the shape itself.
		/// </summary>
		/// <param name="xml">Shape properties XML.</param>
		/// <returns>String represenation of the text to be displayed.</returns>
		protected virtual string GetShapeText( XmlDocument xml )
		{
			XmlElement textElem = (XmlElement) _attributes.SelectSingleNode( "text", FormsNamespace.NamespaceManager );

			if ( textElem == null )
				return string.Empty;

			string pattern = textElem.Attributes[ "pattern" ].Value;
            pattern = pattern.Replace("\\n","\n");
			ArrayList list = new ArrayList();
            string[] patternList = pattern.Split('}');    
		    StringBuilder finalPattern=new StringBuilder();
            int i = 0;
			foreach ( XmlElement item in textElem.SelectNodes( "item[ @ref ]", FormsNamespace.NamespaceManager ) )
			{
				string reference = item.Attributes[ "ref" ].Value;

				XmlNode node = xml.DocumentElement.SelectSingleNode( reference );

                if (node != null)
                {
                    list.Add(node.InnerText);
                    if(item.Attributes[ "textBefore" ] != null)
                    {
                        finalPattern.Append(item.Attributes["textBefore"].Value);
                    }
                    finalPattern.Append(patternList[i]);
                    finalPattern.Append('}');
                    if (item.Attributes["textAfter"] != null)
                    {
                        finalPattern.Append(item.Attributes["textAfter"].Value);
                    }
                }
			    else
                    list.Add("");
                i++;
			}

			string[] args = (string[]) list.ToArray( typeof( string ) );
            return string.Format(CultureInfo.InvariantCulture, finalPattern.ToString(), args);
		}


		/// <summary>
		/// Sets the text that is to be displayed in the shape itself.
		/// </summary>
		/// <param name="text">String.</param>
		protected virtual void SetShapeText( string text )
		{
			VisioShape.get_Cells( "LockTextEdit" ).ResultIU = 0;
			VisioShape.Text = text;
			VisioShape.get_Cells( "LockTextEdit" ).ResultIU = 1;
		}

        /// <summary>
        /// Sets the text that is to be displayed in the shape itself.
        /// </summary>
        /// <param name="text">String.</param>
        protected virtual void SetShapeText(string text, string channels)
        {
            SetShapeText(String.Format("{0}\n{1}",text, channels));
        }

		#endregion


		/// <summary>
		/// Gets the shape text of the current shape.
		/// </summary>
		public virtual string Text
		{
			get { return VisioShape.Text; }
		}


		#region OnExecuteCustom (Abstract)

		/// <summary>
		/// Executes a custom shape command, if implemented in a derived class.
		/// </summary>
		/// <param name="command">Name of the command to execute.</param>
		/// <param name="context">The full shape command context.</param>
		/// <param name="settings">Display settings, in case visual interaction is required.</param>
		/// <returns>Change the current display settings.</returns>
		protected virtual DisplaySettings OnExecuteCustom( string command, NameValueCollection context, DisplaySettings settings )
		{
			throw new NotSupportedException( "Unsupported command: " + command );
		}

		#endregion



		/// <summary>
		/// Paints the Visio shape as an invalid shape.
		/// </summary>
		public virtual bool PaintAsInvalid()
		{
			string colorIndex = Resources.GetString( ResourceTokens.ShapeLineColorError );
			VisioUtils.PaintShape( VisioShape, colorIndex );
            return true;
		}

		/// <summary>
		/// Paints the Visio shape as a valid shape.
		/// </summary>
        public virtual bool PaintAsValid()
		{
			string colorIndex = Resources.GetString( ResourceTokens.ShapeLineColorOk );
			VisioUtils.PaintShape( VisioShape, colorIndex );
            return colorIndex != "2";
		}

        /// <summary>
        /// Paints the Visio shape as a not match shape.
        /// </summary>
        public virtual bool PaintAsNoMatch()
        {
            string colorIndex = Resources.GetString(ResourceTokens.ShapeLineColorNoMatch);
            VisioUtils.PaintShape(VisioShape, colorIndex);
            return true;
        }

		#region IDisposable Members

		/// <summary>
		/// Disposes unmanaged-resources.
		/// </summary>
		public void Dispose()
		{
		}

		#endregion
	}
}
