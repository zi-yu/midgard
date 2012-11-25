using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Visio;
using Shape = Microsoft.Office.Interop.Visio.Shape;
using System.Xml;
using Application=Microsoft.Office.Interop.Visio.Application;

namespace Midgard.Interop
{
	/// <summary>This class provides all the constants and methods that are
	/// shared among the classes in the project.</summary>
	[ComVisible( false )]
    //[CLSCompliant(false)]
	public sealed class VisioUtils
	{
		#region Constants

		/// <summary>
		/// Minimum version of Visio supported: Office 2003.
		/// </summary>
		public const short MinVisioVersion = 11;

		/// <summary>
		/// Begin marker format used in the marker event context string.
		/// </summary>
		public const char ContextBeginMarker = '/';

		/// <summary>
		/// Marker for key value seperator.
		/// </summary>
		public const char ContextEquals = '=';

		/// <summary>
		/// Quotation symbol that is used in Visio Cell formulas.
		/// </summary>
		public const string FormulaQuote = "\"";

		/// <summary>
		/// 
		/// </summary>
		public const string measure="mm";

        /// <summary>
        /// 
        /// </summary>
        public const string ValidColor = "0";

		#endregion

	    // Martelada para fazer "upgrades" de versões (processos e ecrãs)
        public static string ShapeCustomProps;
	    
		#region Formula / Strings

		/// <summary>
		/// Converts the input string to a Formula for a string by replacing each 
		/// double quotation mark (") with a pair of double quotation marks ("") 
		/// and adding double quotation marks ("") around the entire string. When 
		/// this formula is assigned to the formula property of a Visio cell it 
		/// will produce a result value equal to the string, input.
		/// </summary>
		/// <param name="s">Input string that will be processed.</param>
		/// <returns>Formula for input string</returns>
		public static string StringToFormula( string s )
		{
			string result = "";

			// Replace all (") with ("").
			result = s.Replace( FormulaQuote, ( FormulaQuote + FormulaQuote ) );

			// Add ("") around the entire string.
			result = FormulaQuote + result + FormulaQuote;

			return result;
		}

		/// <summary>
		/// Converts the formula into a string, by removing the starting and
		/// trailing quote marks and by replacing any double quotes with single
		/// quotes.
		/// </summary>
		/// <param name="formula">Cell formula value.</param>
		/// <returns>Regular string value.</returns>
		public static string FormulaToString( string formula )
		{
			string result = formula;

			if ( formula.StartsWith( FormulaQuote ) && formula.EndsWith( FormulaQuote ) )
			{
				result = formula
					.Substring( 1, formula.Length-2 )
					.Replace( FormulaQuote + FormulaQuote, FormulaQuote );
			}

			return result;
		}

		#endregion

		#region Get / Set Property

		/// <summary>
		/// Gets the string value of the Value attribute fom a Shape property.
		/// </summary>
		/// <param name="shape">Visio shape.</param>
		/// <param name="section">Name of the Visio shape-sheep section.</param>
		/// <param name="property">Name of the shape-sheep property (also known as section rows).</param>
		/// <returns>The string value of the Value attribute from the designated property, or
		/// null if the property/attribute does not exist.</returns>
		public static string GetProperty( Shape shape, string section, string property )
		{
			return GetProperty( shape, section, property, "Value" );
		}


		/// <summary>
		/// Gets the string value of an attribute fom a Shape property.
		/// </summary>
		/// <param name="shape">Visio shape.</param>
		/// <param name="section">Name of the Visio shape-sheep section.</param>
		/// <param name="property">>Name of the shape-sheep property (also known as section rows).</param>
		/// <param name="attribute">Name of the shape-sheep property attribute (also known as section column).</param>
		/// <returns>The string value of the attribute from the designated property, or
		/// null if the property/attribute does not exist.</returns>
		public static string GetProperty( Shape shape, string section, string property, string attribute )
		{
			string cellLocation=string.Empty;
			if((section==string.Empty)&&(property==string.Empty))
				cellLocation=string.Format( "{0}", attribute );
			else
				cellLocation = string.Format( "{0}.{1}.{2}", section, property, attribute );
			Cell cell;

			try
			{
				cell = shape.get_Cells( cellLocation );
			}
			catch ( COMException )
			{
				return null;
			}

			
			return FormulaToString( cell.Formula );
		}

		/// <summary>
		/// Gets the string value of the Value attribute fom a Shape property.
		/// </summary>
		/// <param name="shape">Visio shape.</param>
		/// <param name="section">Name of the Visio shape-sheep section.</param>
		/// <param name="property">Name of the shape-sheep property (also known as section rows).</param>
		/// <returns>The string value of the Value attribute from the designated property, or
		/// null if the property/attribute does not exist.</returns>
		public static int GetPropertyInt( Shape shape, string section, string property )
		{
			return GetPropertyInt( shape, section, property, "Value", measure );
		}

	    public static int GetPropertyInt( Shape shape, string section, string property, string attribute)
	    {
            return GetPropertyInt(shape, section, property, attribute, measure);
	    }
		/// <summary>
		/// Gets the string value of an attribute fom a Shape property.
		/// </summary>
		/// <param name="shape">Visio shape.</param>
		/// <param name="section">Name of the Visio shape-sheep section.</param>
		/// <param name="property">>Name of the shape-sheep property (also known as section rows).</param>
		/// <param name="attribute">Name of the shape-sheep property attribute (also known as section column).</param>
		/// <returns>The string value of the attribute from the designated property, or
		/// null if the property/attribute does not exist.</returns>
		public static int GetPropertyInt( Shape shape, string section, string property, string attribute, string customMesure)
		{
			string cellLocation=string.Empty;
			if((section==string.Empty)&&(property==string.Empty))
				cellLocation=string.Format( "{0}", attribute );
			else
				cellLocation = string.Format( "{0}.{1}.{2}", section, property, attribute );
			Cell cell;

			try
			{
				cell = shape.get_Cells( cellLocation );
			}
			catch ( COMException )
			{
				return -1;
			}

            return cell.get_ResultInt(customMesure, 1);
		}

        /// <summary>
        /// Gets the string value of an attribute fom a Shape property.
        /// </summary>
        /// <param name="shape">Visio shape.</param>
        /// <param name="section">Name of the Visio shape-sheep section.</param>
        /// <param name="property">>Name of the shape-sheep property (also known as section rows).</param>
        /// <param name="attribute">Name of the shape-sheep property attribute (also known as section column).</param>
        /// <returns>The string value of the attribute from the designated property, or
        /// null if the property/attribute does not exist.</returns>
        public static double GetPropertyDouble(Shape shape, string section, string property, string attribute, string customMeasure)
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
                return -1;
            }
            return cell.get_Result(customMeasure);
        }
        public static double GetPropertyDouble(Shape shape, string section, string property, string attribute)
        {
            return GetPropertyDouble(shape, section, property, attribute, measure);
        }
        /// <summary>
		/// Gets the string value of the Value attribute fom a Shape property.
		/// </summary>
		/// <param name="shape">Visio shape.</param>
		/// <param name="section">Name of the Visio shape-sheep section.</param>
		/// <param name="property">Name of the shape-sheep property (also known as section rows).</param>
		/// <returns>The string value of the Value attribute from the designated property, or
		/// null if the property/attribute does not exist.</returns>
		public static string GetPropertyStr( Shape shape, string section, string property )
		{
			return GetPropertyStr( shape, section, property, "Value" );
		}

		/// <summary>
		/// Gets the string value of an attribute fom a Shape property.
		/// </summary>
		/// <param name="shape">Visio shape.</param>
		/// <param name="section">Name of the Visio shape-sheep section.</param>
		/// <param name="property">>Name of the shape-sheep property (also known as section rows).</param>
		/// <param name="attribute">Name of the shape-sheep property attribute (also known as section column).</param>
		/// <returns>The string value of the attribute from the designated property, or
		/// null if the property/attribute does not exist.</returns>
		public static string GetPropertyStr( Shape shape, string section, string property, string attribute )
		{
			string cellLocation=string.Empty;
			if((section==string.Empty)&&(property==string.Empty))
				cellLocation=string.Format( "{0}", attribute );
			else
				cellLocation = string.Format( "{0}.{1}.{2}", section, property, attribute );
			Cell cell;

			try
			{
				cell = shape.get_Cells( cellLocation );
			}
			catch ( COMException )
			{
				return null;
			}
			
			return FormulaToString( cell.get_ResultStr(measure) );
		}


		/// <summary>
		/// Sets the string value of an attribute fom a Shape property.
		/// </summary>
		/// <param name="shape">Visio shape.</param>
		/// <param name="section">Name of the Visio shape-sheep section.</param>
		/// <param name="property">>Name of the shape-sheep property (also known as section rows).</param>
		/// <param name="value">The value to set.</param>
		public static void SetProperty( Shape shape, string section, string property, string value )
		{
			SetProperty( shape, section, property, "Value", value );
		}

		/// <summary>
		/// Sets the string value of an attribute fom a Shape property.
		/// </summary>
		/// <param name="shape">Visio shape.</param>
		/// <param name="section">Name of the Visio shape-sheep section.</param>
		/// <param name="property">>Name of the shape-sheep property (also known as section rows).</param>
		/// <param name="attribute">Name of the shape-sheep property attribute (also known as section column).</param>
		/// <param name="value">The value to set.</param>
		public static void SetProperty( Shape shape, string section, string property, string attribute, string value )
		{
			string cellLocation = string.Empty;
			if((section==string.Empty)&&(property==string.Empty))
				cellLocation=string.Format( "{0}", attribute );
			else
				cellLocation=string.Format( "{0}.{1}.{2}", section, property, attribute );

			Cell cell;
			try
			{
				cell = shape.get_Cells( cellLocation );
				cell.Formula = StringToFormula( value );
			}
			catch ( COMException )
			{
			}
		}

		/// <summary>
		/// Sets the string value of an attribute fom a Shape property.
		/// </summary>
		/// <param name="shape">Visio shape.</param>
		/// <param name="section">Name of the Visio shape-sheep section.</param>
		/// <param name="property">>Name of the shape-sheep property (also known as section rows).</param>
		/// <param name="value">The value to set.</param>
		public static void SetFormula( Shape shape, string section, string property, string value )
		{
			SetFormula( shape, section, property, "Value", value );
		}

		/// <summary>
		/// Sets the string value of an attribute fom a Shape property.
		/// </summary>
		/// <param name="shape">Visio shape.</param>
		/// <param name="section">Name of the Visio shape-sheep section.</param>
		/// <param name="property">>Name of the shape-sheep property (also known as section rows).</param>
		/// <param name="attribute">Name of the shape-sheep property attribute (also known as section column).</param>
		/// <param name="value">The value to set.</param>
		public static void SetFormula( Shape shape, string section, string property, string attribute, string value )
		{
			string cellLocation = string.Empty;
			if((section==string.Empty)&&(property==string.Empty))
				cellLocation=string.Format( "{0}", attribute );
			else
				cellLocation=string.Format( "{0}.{1}.{2}", section, property, attribute );

			Cell cell;
			try
			{
				cell = shape.get_Cells( cellLocation );
				cell.Formula = value;
			}
			catch ( COMException )
			{
			}
		}



		#endregion

		#region Shape Operations

		/// <summary>
		/// Retrieves a Shape from a Visio application based on the Marker Event context.
		/// </summary>
		/// <param name="visioApplication">Visio application instance.</param>
		/// <param name="context">Marker event context.</param>
		/// <returns>Shape that raised marker event, or null if shape is not found.</returns>
		//[CLSCompliant( false )]
		public static Shape GetShape( Application visioApplication, string context )
		{
			NameValueCollection nvc = ParseContext( context );

			return GetShape( visioApplication, nvc );
		}


		/// <summary>
		/// Retrieves a Shape from a Visio application based on the Marker Event context.
		/// </summary>
		/// <param name="visioApplication">Visio application instance.</param>
		/// <param name="context">Parsed marker event context.</param>
		/// <returns>Shape that raised marker event, or null if shape is not found.</returns>
		//[CLSCompliant( false )]
		public static Shape GetShape( Application visioApplication, NameValueCollection context )
		{
			#region Validations

			if ( visioApplication == null )
				throw new ArgumentNullException( "visioApplication" );

			if ( context == null )
				throw new ArgumentNullException( "context" );

			#endregion

			// Standard Visio add-on command line arguments.
			const string ArgumentDoc = "doc";
			const string ArgumentPage = "page";
			const string ArgumentShape = "shape";

			//
			string contextDoc = context[ ArgumentDoc ];
			string contextPage = context[ ArgumentPage ];
			string contextShape = context[ ArgumentShape ];

			if ( contextDoc == null || contextPage == null || contextShape == null )
				return null;

			//
			int docId = Convert.ToInt16( contextDoc, CultureInfo.InvariantCulture );
			int pageId = Convert.ToInt16( contextPage, CultureInfo.InvariantCulture );
			string shapeId = contextShape;


			Shape targetShape = null;

			// If the command line arguments contains document, page, and shape
			// then look up the shape.
			if ( ( docId > 0 ) && ( pageId > 0 ) && ( shapeId.Length > 0 ) )
			{
				try
				{
					Document document = visioApplication.Documents[ docId ];
					Page page = document.Pages[ pageId ];
					targetShape = page.Shapes[ shapeId ];
				}
				catch ( COMException )
				{
					// Doc/Page/Shape not found. Silently ignore, return null.
				}
			}

			return targetShape;
		}


		/// <summary>
		/// Parses the context arguments into the several parts.
		/// </summary>
		/// <param name="context">Event-marker context</param>
		/// <returns>Collection of Name/Values pairs.</returns>
		public static NameValueCollection ParseContext( string context )
		{
			NameValueCollection nvc = new NameValueCollection();

			string[] contextParts = context.Trim().Split( VisioUtils.ContextBeginMarker );

			for ( int i=0; i<contextParts.Length; i++ )
			{
				string[] argument = contextParts[ i ].Trim().Split( new char[] { VisioUtils.ContextEquals }, 2 );

				if ( argument.Length == 1 )
					nvc.Add( argument[ 0 ], null );
				else
					nvc.Add( argument[ 0 ], argument[ 1 ] );
			}

			return nvc;
		}

		#endregion

		#region Command Bars

		/// <summary>This method gets the command bar named in 
		/// commandBarName.</summary>
		/// <param name="visioApplication">Current vision application.</param>
		/// <param name="commandBarName">Name of the command bar to be 
		/// found</param>
		/// <returns>CommandBar object if found; otherwise null.</returns>
		public static CommandBar GetCommandBar( Application visioApplication, string commandBarName )
		{
			CommandBar currentCommandBar = null;

			try
			{
				CommandBars applicationCommandBars = (CommandBars) visioApplication.CommandBars;
				currentCommandBar = applicationCommandBars[ commandBarName ];
			}
			catch ( COMException e)
			{
				// Some other COM screw-up. :/
                //ESITracer.Current.LogDebug("GetCommandBar:EXC:"+e.StackTrace.ToString());
                //if (e.InnerException != null)
                //    ESITracer.Current.LogDebug("GetCommandBar:EXC:INNER:" + e.InnerException.StackTrace.ToString());

			}
			catch ( ArgumentException e)
			{
				// The CommandBar object was not found. Ignore the error.
                //ESITracer.Current.LogDebug("GetCommandBar:EXC:" + e.StackTrace.ToString());
                //if (e.InnerException != null)
                //    ESITracer.Current.LogDebug("GetCommandBar:EXC:INNER:" + e.InnerException.StackTrace.ToString());
            }
			catch ( InvalidComObjectException e)
			{
				// The CommandBar object was not found. Ignore the error.
                /*ESITracer.Current.LogDebug("GetCommandBar:EXC:" + e.StackTrace.ToString());
                if (e.InnerException != null)
                    ESITracer.Current.LogDebug("GetCommandBar:EXC:INNER:" + e.InnerException.StackTrace.ToString());*/
            }

			return currentCommandBar;
		}


		/// <summary>This method destroys the command bar object added
		/// in the CreateCommandBar method.</summary>
		/// <param name="visioApplication">Current vision application.</param>
		/// <param name="commandBarName">Bar name to be removed.</param>
		public static void DestroyCommandBar( Application visioApplication, string commandBarName )
		{
			CommandBar commandBar = GetCommandBar( visioApplication, commandBarName );

			if ( commandBar != null )
			{
				// Delete the command bar object.
				commandBar.Delete();
				Marshal.ReleaseComObject( commandBar );
			}
		}

		#endregion

		#region PaintShape

        public static bool PaintColoredTransition(Shape shape, XmlNodeList paint)
        {
            string colorIndex="2";// por omissão é erro

            if (shape.Connects.Count > 1)
            {

                string fromShape = GetProperty(shape.Connects[1].ToCell.Shape, "User", "Midgard");
                string toShape = GetProperty(shape.Connects[2].ToCell.Shape, "User", "Midgard");
                if (fromShape != null && toShape != null)
                {

                    colorIndex = ValidColor;
                }
            }
            PaintShape(shape, colorIndex);
            return (colorIndex != "2");
        }

        public static bool PaintColoredShape(Shape shape, XmlNodeList paint)
        {
            string colorIndex="2";// por omissão é erro

            if (shape.Connects.Count > 1)
            {
                
                string fromShape = GetProperty(shape.Connects[1].ToCell.Shape, "User", "Midgard");
                string toShape = GetProperty(shape.Connects[2].ToCell.Shape, "User", "Midgard");
                if (fromShape != null && toShape != null)
                {
                    XmlDocument fromShapeDoc = GetPropertiesFromShape(shape.Connects[1].ToCell.Shape);
                    XmlDocument toShapeDoc = GetPropertiesFromShape(shape.Connects[2].ToCell.Shape);

                    string fromShapeScreen = GetElementValueFromProperties(fromShapeDoc, (fromShape == "Business" ? "screen" : "id"));
                    string toShapeScreen = GetElementValueFromProperties(toShapeDoc, (toShape == "Business" ? "screen" : "id"));

                    foreach (XmlNode p in paint)
                    {
                        if (p.Attributes["connectedTo"] != null && p.Attributes["connectedFrom"] != null)
                        {
                            string connectedTo = p.Attributes["connectedTo"].Value;
                            string connectedFrom = p.Attributes["connectedFrom"].Value;
                            bool execute = true;
                            if (p.Attributes["rule"] != null)
                            {
                                string rule = p.Attributes["rule"].Value;
                                if (rule.ToLower() == "differentscreencode")
                                {
                                    execute = (fromShapeScreen != toShapeScreen);
                                }
                            }
                            if (execute)
                            {
                                if (connectedTo.IndexOf(toShape) >= 0 && connectedFrom.IndexOf(fromShape) >= 0)
                                    colorIndex = p.InnerText;
                            }
                        }
                    }
                }
            }
            PaintShape(shape, colorIndex);
            return (colorIndex != "2");
        }
	
		/// <summary>
		/// Paints the Shape, and it's composing sub-shapes, with the
		/// given color.
		/// </summary>
		/// <param name="shape">Root shape</param>
		/// <param name="colorIndex">Color, as per index in the Visio Color Palette</param>
		public static void PaintShape( Shape shape, string colorIndex )
		{
			Cell cell = shape.get_CellsSRC( (short) VisSectionIndices.visSectionObject, (short) VisRowIndices.visRowLine, (short) VisCellIndices.visLineColor );
    		cell.FormulaU = colorIndex;


            cell = shape.get_CellsSRC((short)VisSectionIndices.visSectionCharacter, (short)VisRowIndices.visRowCharacter, (short)VisCellIndices.visCharacterColor);
            cell.FormulaU = colorIndex;

            foreach (Shape subShape in shape.Shapes)
                PaintShape(subShape, colorIndex);

		}

		#endregion

        #region Process and Screen

        public static bool IsProcess(Application app)
        {
            bool isProcess = false;
            if (app.ActivePage !=null)
            {
                foreach (Layer l in app.ActivePage.Layers)
                {
                    if (l.Name == "Process")
                    {
                        isProcess = true;
                        break;
                    }
                }
            }
                return isProcess;
        }

        public static string GetShapeType(Application app)
        {
            if (IsProcess(app))
                return "QuartzShape";
            else
                return "ScreenShape";
        }

        public static string GetShapeType(Shape shape)
        {
            string shapeType = null;
            string layerName = null;
            Application app = shape.Document.Application;
            if (!IsProcess(app))
            {
                shapeType = "QuartzShape";
                layerName = "Process";
            }
            else
            {
                shapeType = "ScreenShape";
                layerName = "Screen";
            }

            string name = GetProperty(shape, "User", shapeType);

            if (name != null)
            {
                Shape tmpShape = shape;
                tmpShape.Application.ActivePage.Layers.Add(layerName);
            }
            return name;
        }

        public static string GetShapePrefix(Application app)
        {
            if (IsProcess(app))
                return "Process-";
            else
                return "Screen-";
        }

        public static XmlDocument GetPropertiesFromShape(Shape shape)
        {
            string xml =GetProperty(shape, "Prop", "ShapeXml");

            if (xml == null)
                return null;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            return xmlDoc;
        }

        public static string GetElementValueFromProperties(XmlDocument doc, string element)
        {
            if (doc != null)
            {
                XmlElement ele = (XmlElement)doc.SelectSingleNode("properties/" + element);
                if (ele != null)
                {
                    return ele.InnerText;
                }
            }
            return null;
        }

        #endregion
    }
}