using System;
using System.Globalization;
using System.Runtime.InteropServices;
using Extensibility;
using VisioApplication = Microsoft.Office.Interop.Visio.Application;
using System.Reflection;
using System.Diagnostics;

namespace Midgard.Interop
{
	/// <summary>
	/// Summary description for Connect.
	/// </summary>
	[ComVisible( true )]
    [Guid("881A6BD4-5CED-4C43-8BB6-BF1F2C2D41B0")]
	[ProgId( "MidgardDynamic.Connect" )]
	public class Connect : Object, IDTExtensibility2
	{
		#region Private Members

    	private VisioApplication _visioApplication;
		private EventSink _eventSink;

		#endregion

		/// <summary>
		/// Gets the Visio application.
		/// </summary>
		protected VisioApplication Application
		{
			get { return _visioApplication; }
		}

		/// <summary>
		/// Gets the event sink.
		/// </summary>
		protected EventSink EventSink
		{
			get { return _eventSink; }
		}


		/// <summary>
		/// Initializes a new instance of the Connect class.
		/// </summary>
		public Connect()
		{

		}


		/// <summary>
		/// Called when Visio loads the add-in.
		/// </summary>
		/// <param name="application">Reference to the Visio Application object.</param>
		/// <param name="connectMode">Describes how the add-in is started.</param>
		/// <param name="addInInst">Reference to the add-in.</param>
		/// <param name="custom">Array of additional parameters for the add-in.</param>
		public void OnConnection( object application, ext_ConnectMode connectMode, object addInInst, ref Array custom )
		{
 			try
			{
				if ( _visioApplication == null )
				{
					_visioApplication = (VisioApplication) application;

					/*
					 * Check visio version.
					 */
					int majorVersion = (int) Convert.ToDouble( _visioApplication.Version, CultureInfo.InvariantCulture );

					if (   ( majorVersion == 0 )
						|| ( majorVersion < VisioUtils.MinVisioVersion ) )
					{
						return;
					}


					/*
					 * Start listening to the desired events.
					 */
					_eventSink = new EventSink( Application );
					EventSink.AddAdvise();
					EventSink.CreateCommandBar();
					EventSink.Activate();
				}
			}
			catch ( Exception ex )
			{
				ESIMessageBox.ShowError( ex );
			}
		}


		/// <summary>
		/// This method is called when the add-in is about to be unloaded. The 
		/// add-in will be unloaded when Visio is shutting down or when a user 
		/// has removed or deselected the add-in from the "COM Add-Ins" dialog.
		/// </summary>
		/// <param name="removeMode">Tells the add-in why Visio is disconnecting.</param>        
		/// <param name="custom">Array of additional parameters for the add-in, not used in this case.</param>
		public void OnDisconnection( ext_DisconnectMode removeMode, ref Array custom )
		{
			try
			{
				if ( _eventSink != null )
					_eventSink.Dispose();
			}
			catch ( Exception ex )
			{
				ESIMessageBox.ShowError( ex );
			}
		}


		#region Empty Implementations

		/// <summary>
		/// This method implements the OnAddInsUpdate method of the IDTExtensibility2 
		/// interface. It receives notification that the collection of add-ins has changed.
		/// </summary>
		/// <param name="custom">Array of parameters that are host application specific.</param>
		public void OnAddInsUpdate( ref Array custom )
		{
		}

		/// <summary>
		/// This method implements the OnBeginShutdown method of the IDTExtensibility2 
		/// interface. It receives notification that the host application is being 
		/// unloaded.
		/// </summary>
		/// <param name="custom">Array of parameters that are host application specific.</param>
		public void OnBeginShutdown( ref Array custom )
		{
		}

		/// <summary>
		/// This method implements the OnStartupComplete method of the IDTExtensibility2 
		/// interface. It receives notification that the host application has completed 
		/// loading.
		/// </summary>
		/// <param name="custom">Array of parameters that are host application specific.</param>
		public void OnStartupComplete( ref Array custom )
		{
		}

		#endregion
	}
}