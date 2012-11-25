using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Visio;
using Shape = Microsoft.Office.Interop.Visio.Shape;
using VisioApplication = Microsoft.Office.Interop.Visio.Application;
using Visio = Microsoft.Office.Interop.Visio;
using System.Xml;
using System.Diagnostics;
using System.Reflection;
 
using System.Collections.Generic;
using System.Web.Services.Protocols;
using System.Net;
using System.Threading;
using Midgard.XmlForms;

namespace Midgard.Interop
{
    /// <summary> 
    /// Captures and processes events received from the Visio application.
    /// </summary>
    [ComVisible(true)]
    [ProgId("MidgardDynamic.EventSink")]
    [Guid("27CDA4FC-2FC6-4FB1-837B-A19149399EC0")]
    public sealed class EventSink : IVisEventProc, IDisposable
    {
        #region Private Members

        private bool _active = false;
        private VisioApplication _application;
        private ArrayList _events;
        private DisplaySettings _displaySettings;

        private CommandBarButton _validateButton;
        private CommandBarButton _exportXMLButton;
 
        private int _openDocuments = 0;
        private XmlElement _eventAttributes;
        private XmlElement _eventProcessAttributes;

        private bool _forceFormChange = false;

        #endregion

        #region Constants

        private const string TagValidateButton = "Midgard.ValidateTransition";
        private const string TagExportXMLButton = "Midgard.Transition";
        #endregion

        #region Protected Properties

        /// <summary>
        /// Gets the application associated with the add-in.
        /// </summary>
        private VisioApplication Application
        {
            get { return _application; }
        }

        #endregion


        /// <summary>
        /// Initialize a new instance of the EventSink class.
        /// </summary>
        /// <param name="application">Reference to current Visio instance.</param>
        public EventSink(VisioApplication application)
        {
            _application = application;
            _events = new ArrayList();
            _displaySettings = new DisplaySettings();
        }


        /// <summary>
        /// Subscribe to all relevant events.
        /// </summary>
        public void AddAdvise()
        {
            // The Sink and TargetArgs values aren't needed.
            const string sink = "";
            const string targetArgs = "";
            Event e;

            // Add marker events to the application.  This event
            // will be raised when a user double-clicks on a shape.
            e = Application.EventList.AddAdvise(VisioEvent.ShapeEventMarker, this, sink, targetArgs);
            _events.Add(e);

            // Register the Shape Connect event.
            e = Application.EventList.AddAdvise(VisioEvent.ShapeConnect, this, sink, targetArgs);
            _events.Add(e);

            // Register the Shape Disconnect event.
            e = Application.EventList.AddAdvise(VisioEvent.ShapeDisconnect, this, sink, targetArgs);
            _events.Add(e);

            _events.TrimToSize();
        }

        /// <summary>
        /// Creates a command-bar.
        /// </summary>
        public void CreateCommandBar()
        {
            string commandBarName = Resources.GetString(ResourceTokens.CommandBarName);
            Object missing = System.Reflection.Missing.Value;
            CommandBar commandBar = VisioUtils.GetCommandBar(Application, commandBarName);

            if (commandBar != null)
                return;

            //adicionar a barra ao Visio
            CommandBars applicationCommandBars = (CommandBars)Application.CommandBars;
            commandBar = applicationCommandBars.Add(commandBarName, MsoBarPosition.msoBarTop, false, true);


            // Validate transition Button
            _validateButton = (CommandBarButton)commandBar.Controls.Add(MsoControlType.msoControlButton, missing, missing, 1, false);
            _validateButton.Click += new _CommandBarButtonEvents_ClickEventHandler(this.validateTransition_Click);
            SetButtonProperties(_validateButton, ResourceTokens.ButtonValidate, ResourceTokens.ButtonValidateTooltip, TagValidateButton, "Validate.bmp", "Validate-Mask.bmp");

            // Export transition
            _exportXMLButton = (CommandBarButton)commandBar.Controls.Add(MsoControlType.msoControlButton, missing, missing, 2, false);
            _exportXMLButton.Click += new _CommandBarButtonEvents_ClickEventHandler(this.exportTransition_Click);
            SetButtonProperties(_exportXMLButton, ResourceTokens.ButtonExport, ResourceTokens.ButtonExportTooltip, TagExportXMLButton, "Export.bmp", "Export-Mask.bmp");
            

            // User cannot customize our commandbar
            commandBar.Protection = MsoBarProtection.msoBarNoCustomize;

            // Toolbar is only visible when viewing the drawing
            commandBar.Context = Convert.ToString((short)VisUIObjSets.visUIObjSetDrawing, CultureInfo.InvariantCulture) + "*";

            _application.DocumentCreated += new EApplication_DocumentCreatedEventHandler(_application_DocumentCreated);
            _application.DocumentOpened += new EApplication_DocumentOpenedEventHandler(_application_DocumentOpened);
            _application.BeforeDocumentClose += new EApplication_BeforeDocumentCloseEventHandler(_application_BeforeDocumentClose);
            _application.WindowActivated += new EApplication_WindowActivatedEventHandler(_application_WindowActivated);
            _application.ViewChanged += new EApplication_ViewChangedEventHandler(_application_ViewChanged);
        }


        /// <summary>
        /// Event handler for subscribed events.
        /// </summary>
        /// <param name="eventCode">Code of the captured event.</param>
        /// <param name="source">Source object. In this case, it will always be the Visio application.</param>
        /// <param name="eventID">?</param>
        /// <param name="eventSequenceNumber">?</param>
        /// <param name="subject">The object which caused the event. This parameter is not set for marker events (which are raised by Visio).</param>
        /// <param name="moreInfo">?</param>
        /// <returns>Always null.</returns>
        public object VisEventProc(short eventCode, object source, int eventID, int eventSequenceNumber, object subject, object moreInfo)
        {
            VisioApplication visioApplication = null;
            try
            {
                visioApplication = (VisioApplication)source;

                if (_active == false)
                    return null;

                if (visioApplication.IsUndoingOrRedoing == true)
                    return null;

                /*
                 * Test eventCode.
                 * Yes, it's _so_ retarded to have to use if/else, but we can't switch
                 * since VisioEvent.* aren't constants. :/
                 */


                /*
                 * Called whenever one shape connects to another. Filter out
                 * connects which are not raised by QuartzProcess shapes.
                 */
                #region Shape Connect

                if (eventCode == VisioEvent.ShapeConnect)
                {
                    Connects connects = (Connects)subject;
                    HandleShapeConnect(connects);
                }

                #endregion


                /*
				 * Called whenever a shape is disconnected from another. Filter
				 * out disconnects which are not raised by QuartzProcess shapes.
				 */
                #region Shape Disconnect

                else if (eventCode == VisioEvent.ShapeDisconnect)
                {
                    Connects connects = (Connects)subject;
                    HandleShapeDisconnect(connects);
                }

                #endregion


                /*
				 * Called whenever a QueueMarkerEvent command is executed from
				 * the shape. Filter out events which are not raised by 
				 * QuartzProcess app.
				 */
                #region Marker Events

                else if (eventCode == VisioEvent.ShapeEventMarker)
                {

                    string context = visioApplication.get_EventInfo(VisioEvent.IdMostRecent);

                    if (context == null)
                        return null;

                    NameValueCollection nvc = VisioUtils.ParseContext(context);

                    HandleMarkerEvent(nvc);
                }

                #endregion
            }
            catch (Exception ex)
            {
                ESIMessageBox.ShowError(ex);
            }

            return null;
        }


        #region IDisposable

        /// <summary>
        /// Releases unmanaged resources help by the event sink.
        /// </summary>
        public void Dispose()
        {
            foreach (Event e in _events)
            {
                e.Delete();
                Marshal.ReleaseComObject(e);
            }

            _events = null;


            string commandBarName = Resources.GetString(ResourceTokens.CommandBarName);
            CommandBar commandBar = VisioUtils.GetCommandBar(Application, commandBarName);

            if (commandBar != null)
            {
                commandBar.Delete();
                Marshal.ReleaseComObject(_exportXMLButton);
                Marshal.ReleaseComObject(_validateButton);

                _validateButton = null;
                _exportXMLButton = null;
            }
        }

        #endregion

        #region Visio Event Callbacks

        /// <summary>
        /// Handles the shape connect event.
        /// </summary>
        /// <param name="connection">Created connection.</param>
        private void HandleShapeConnect(Connects connection)
        {
            // connection.FromSheet is always the connector shape.
            // connection.ToSheet is always the shape to which the connector was just connected.
           /*  if (Application.ActivePage != null)
            {
                string shapeName = VisioUtils.GetProperty(connection.FromSheet, "User", VisioUtils.GetShapeType(Application));


                if (shapeName == null)
                    return;

                if (shapeName == "Event")
                {
                    if (_eventAttributes != null)
                        VisioUtils.PaintColoredShape(connection.FromSheet, _eventAttributes.SelectNodes("paint", FormsNamespace.NamespaceManager));
                }
                else
                    if (shapeName == "EventProcess")
                    {
                        if (_eventProcessAttributes != null)
                            VisioUtils.PaintColoredShape(connection.FromSheet, _eventProcessAttributes.SelectNodes("paint", FormsNamespace.NamespaceManager));
                    }
            }*/
        }

        /// <summary>
        /// Handles the shape disconnect event.
        /// </summary>
        /// <param name="connection">Disconnection event.</param>
        private void HandleShapeDisconnect(Connects connection)
        {
            // connection.FromSheet is always the connector shape.
            // connection.ToSheet is always the shape to which the connector was previously connected.

            Shape connect = connection.FromSheet;
            string shapeName = VisioUtils.GetProperty(connect, "User", VisioUtils.GetShapeType(Application));

            if (shapeName == null)
                return;

            if (shapeName == "Event" || shapeName == "EventProcess")
                VisioUtils.PaintShape(connection.FromSheet, "2");
        }

        /// <summary>
        /// Handles the marker events.
        /// </summary>
        /// <param name="context">Parsed event context.</param>
        private void HandleMarkerEvent(NameValueCollection context)
        {
            Shape shape = VisioUtils.GetShape(_application, context);

            if (shape == null)
                return;

            string shapeName = VisioUtils.GetProperty(shape, "User", "Midgard");

            if (shapeName == null)
            {
                // se for um visio desenhado com a versão antiga (sem os layers)
                shapeName = VisioUtils.GetShapeType(shape);
                if (shapeName == null)
                    return;
            }
            string command = context["cmd"];
            if (command == null)
                return;

            /*
             * 
             */
            string mode = "live";// GetMode();
            
            using (XmlForm form = new XmlForm())
            {
                form.LoadDefinition(shapeName, shape);
                form.Design(mode, Application.ActivePage);

                IShapeHandler shapeHandler = (IShapeHandler)form.Tag;
                DisplaySettings newSettings = shapeHandler.Execute(command, context, _displaySettings, _forceFormChange);
                if (newSettings != null)
                    _displaySettings = newSettings;
            }
        }

        private string GetElement(string shapeXml, string element)
        {
            string beginEle = "<" + element + ">";
            string endEle = "</" + element + ">";

            int begIndex = shapeXml.IndexOf(beginEle);

            if (-1 == begIndex)
                return null;

            int endIndex = shapeXml.IndexOf(endEle);

            return shapeXml.Substring(begIndex + beginEle.Length, endIndex - (begIndex + beginEle.Length));

        }

        #endregion

        #region Visio CommandBar Callbacks

        private void exportTransition_Click(CommandBarButton sender, ref bool CancelDefault)
        {
            _forceFormChange = false;
            if (Application.ActiveDocument == null)
                return;

            try
            {          

                Midgard.Interop.Export.UiProcessExporter exporter = new Midgard.Interop.Export.UiProcessExporter();
                exporter.ExportTransition(Application.ActivePage);
            }
            catch (Exception ex)
            {
                ESIMessageBox.ShowError(ex);
            }
        }

        private void validateTransition_Click(CommandBarButton sender, ref bool CancelDefault)
        {
            _forceFormChange = false;
            if (Application.ActivePage == null)
                return;

            try
            {
                Midgard.Interop.Export.UiProcessExporter exporter = new Midgard.Interop.Export.UiProcessExporter();
                exporter.ValidatePage(Application.ActivePage);
            }
            catch (Exception ex)
            {
                ESIMessageBox.ShowError(ex);
            }
        }

        #endregion

        #region Helpers

        private CommandBarControl GetControl(CommandBar bar, string tag)
        {
            Object missing = Missing.Value;
            return bar.FindControl(missing, missing, tag, missing, missing);
        }

        private CommandBar GetCommandBar()
        {
            string commandBarName = Resources.GetString(ResourceTokens.CommandBarName);
            return VisioUtils.GetCommandBar(Application, commandBarName);
        }


        /// <summary>This method sets the button properties for the
        /// command bar button that is passed in.</summary>
        /// <param name="button">Command bar button for which the
        /// properties are to be set</param>
        /// <param name="caption">Caption of the button.</param>
        /// <param name="tooltip">Tool tip of the button.</param>
        /// <param name="tag">Tag of the button.</param>
        /// <param name="iconPath">Filename of the icon to be set.</param>
        /// <param name="maskPath">Filename of the mask to be set.</param>
        private void SetButtonProperties(
            CommandBarButton button,
            string caption,
            string tooltip,
            string tag,
            string iconPath,
            string maskPath)
        {
            // Use the Tag property for context switching and for
            // use with the FindControl method.
            button.Tag = tag;

            button.Caption = Resources.GetString(caption);
            button.TooltipText = Resources.GetString(tooltip);
            button.Picture = Resources.GetPicture(iconPath);
            button.Mask = Resources.GetPicture(maskPath);
            button.Enabled = false;
        }

        #endregion

        #region Command Bar State

        private void SetCommandBarState(bool activate)
        {
            bool enabled;

            if (activate && _openDocuments == 1)
                enabled = true;
            else if (activate == false && _openDocuments == 0)
                enabled = false;
            else
                return;

            CommandBar bar = GetCommandBar();

            GetControl(bar, TagValidateButton).Enabled = enabled;
            GetControl(bar, TagExportXMLButton).Enabled = enabled;
         }

        private void _application_BeforeDocumentClose(Document doc)
        {

            _openDocuments--;
            SetCommandBarState(false);
        }

        private void _application_DocumentCreated(Document doc)
        {
            _openDocuments++;
            SetCommandBarState(true);
        }

        private void _application_DocumentOpened(Document doc)
        {
            _openDocuments++;
            SetCommandBarState(true);
        }

        void _application_WindowActivated(Window Window)
        {
        }

        void _application_ViewChanged(Window Window)
        {
            double hei = _application.ActiveDocument.get_PaperHeight("mm");
            double wid = _application.ActiveDocument.get_PaperWidth("mm");

            //verificar se a página alterou e se sim mudar o tamanho da shape de global data
        }
        #endregion


        /// <summary>
        /// Signals that all preloading has been executed successfully, and that
        /// the AddIn is now ready for processing the registered events.
        /// </summary>
        public void Activate()
        {
            _active = true;
        }
    }
}