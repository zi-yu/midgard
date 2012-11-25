using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Xml;
using Midgard.XmlForms;
using SysPath = System.IO.Path;
using Microsoft.Office.Interop.Visio;
using System.Reflection;
using System.Text;
using Midgarg.Interop;

namespace Midgard.Interop
{
	/// <summary>
	/// Class that handles exporting of Process data to XML.
	/// </summary>
	public class ProcessExporter
	{
		#region Private Members

		private int _totalSteps;
		private int _currentStep;
        private const string _INVALID = "INVALID";
        private string _encoding;
		#endregion


		#region Constructors
		/// <summary>
		/// Initializes a new instance of the VisioExporter class.
		/// </summary>
		public ProcessExporter(string encoding)
		{
            _encoding = encoding;
		}


		#endregion

		#region Public Entry Point



		/// <summary>
		/// Exports a Midgard Dynamic page.
		/// </summary>
		/// <param name="page">Page.</param>
		/// <param name="mode">Export mode.</param>
		public PageReport ExportPage( Page page, string mode )
		{

			_totalSteps = 1000;
			_currentStep = 0;
			OnStartStep( "Obtain total number of shapes" );

			_totalSteps = 1 + 2*page.Shapes.Count;

			return DoPageExport( page, mode );
		}

		/// <summary>
		/// Validates a single Quartz page.
		/// </summary>
		/// <param name="page">Page.</param>
		/// <param name="mode">Export mode.</param>
		public PageReport ValidatePage( Page page, string mode )
		{
			_totalSteps = 1000;
			_currentStep = 0;
			OnStartStep( "Obtain total number of shapes" );

			_totalSteps = 1 + page.Shapes.Count;

			return DoPageExport( page, mode, false );
		}

		#endregion

		#region Events

		/// <summary>
		/// Raises the <see cref="StartStep"/> event.
		/// </summary>
		/// <param name="name">Name of the step.</param>
		private void OnStartStep( string name )
		{
			if ( StartStep != null )
				StartStep( _currentStep, _totalSteps, name );
	
			_currentStep++;
		}

		/// <summary>
		/// Raises the <see cref="EndStep"/> event, providing a formatted step 
		/// ending description.
		/// </summary>
		/// <param name="format">Description formatter of the step result.</param>
		/// <param name="args">Description arguments.</param>
		private void OnEndStep( string format, params object[] args )
		{
			string description = string.Format( CultureInfo.InvariantCulture, format, args );

			if ( EndStep != null )
				EndStep( description );
		}

		/// <summary>
		/// Raises the <see cref="EndStep"/> event, providing a step ending description.
		/// </summary>
		/// <param name="description">Description of the step result.</param>
		private void OnEndStep( string description )
		{
			if ( EndStep != null )
				EndStep( description );
		}

		/// <summary>
		/// Raises the <see cref="EndStep"/> event, without a concluding description.
		/// </summary>
		private void OnEndStep()
		{
			if ( EndStep != null )
				EndStep( null );
		}



		/// <summary>
		/// Occurs whenever a step is started.
		/// </summary>
		public event StepStartEventHandler StartStep;

		/// <summary>
		/// Occurs whenever a step ends.
		/// </summary>
		public event StepEndEventHandler EndStep;

		#endregion

		#region Shape utils

		private void MarkInvalid(Page page)
		{
			foreach(Shape shape in page.Shapes)
			{
				string shapeName=VisioUtils.GetProperty(shape,"User","ScreenShape");
				if(shapeName==null)
					continue;
				shape.Data1=_INVALID;
			}
		}

		private PageReport PaintInvalid(Page page,PageReport report,string mode)
		{
			int invalidCount=0;
			Hashtable shapeHandlers=new Hashtable();
			CgoConfig[] Cgo=CgoConfigs.Search();

			GroupElementTable groupElementTable=new GroupElementTable();
			ElementTable elementTable=new ElementTable();

			foreach(Shape shape in page.Shapes)
			{
				string shapeName=VisioUtils.GetProperty(shape,"User","ScreenShape");
				if(shapeName==null)
					continue;

				IShapeHandler shapeHandler=GetShapeHandler(shapeHandlers,shape,shapeName,mode);
				if(shape.Data1==_INVALID)
				{
					shapeHandler.PaintAsInvalid();
				}
				else
				{
					if(Cgo!=null)
					{
						XmlNode shapeProperties=shapeHandler.GetProperties();
						XmlNode cgoNode=shapeProperties.SelectSingleNode("cgoName");
						if(cgoNode!=null)
						{
							string cgoCode=cgoNode.InnerText;
							if(!CgoConfigs.Exists(Cgo,cgoCode))
							{
							}
						}
					}
				}
				invalidCount++;

				string groupElementName=VisioUtils.GetProperty(shape,"Prop","Group");
				string elementName=VisioUtils.GetProperty(shape,"Prop","Element");
				if((groupElementName==null)||(elementName==null))
					continue;

				GroupElement groupElement=new GroupElement(groupElementName);
				groupElementTable.AddGroupElement(groupElement);

				Element element=new Element(elementName);
				elementTable.AddElement(element);
			}
			report=ShapeGroupValidation.Validate(groupElementTable,elementTable,report);

			return report;
		}
		#endregion


		private PageReport DoPageExport(Page page,string mode)
		{
			return DoPageExport( page, mode, true );
		}

		private PageReport DoPageExport(Page page, string mode, bool export)
		{

			PageReport report=new PageReport(page);
			return report;
		}

		private IShapeHandler GetShapeHandler( Hashtable dictionary, Shape shape, string shapeName, string mode )
		{
			IShapeHandler shapeHandler = (IShapeHandler) dictionary[ shape ];

			if ( shapeHandler != null )
				return shapeHandler;

			XmlForm form = new XmlForm( );
			form.LoadDefinition( shapeName, shape );
			form.Design( mode );
			form.Close();
			form.Dispose();

			shapeHandler = (IShapeHandler) form.Tag;
			shapeHandler.LoadProperties();

			dictionary[ shape ] = shapeHandler;

			return shapeHandler;
		}

		private string GetFilename( string documentPath, string relativePath, string processId )
		{
			string filenameFormat = Resources.GetString( ResourceTokens.ProcessFilenameFormat );
			string file = string.Format( filenameFormat, processId );

			string path;

			if ( documentPath == string.Empty )
			{
				path = PathEx.Combine( 
					Environment.GetFolderPath( Environment.SpecialFolder.DesktopDirectory ),
					file );
			}
			else
			{
				path = PathEx.Combine( 
					documentPath,
					relativePath,
					file );
			}

			FileInfo fileInfo = new FileInfo( path );
			return fileInfo.FullName;
		}
	}
}
