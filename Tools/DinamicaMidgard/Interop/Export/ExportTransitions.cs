using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml;
using SysPath = System.IO.Path;
using Microsoft.Office.Interop.Visio;
using System.Reflection;
using System.Text;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using System.Xml.XPath;
using System.Diagnostics;
using Midgard.XmlForms;
using Midgard.Interop.Shapes;
using Midgard.Interop.ShapeObjects;
using Microsoft.Win32;

namespace Midgard.Interop.Export
{
    /// <summary>
    /// Class that handles exporting.
    /// </summary>
    public class ExportTransitions
    {
        #region Private Members

        private int _totalSteps; 
        private int _currentStep;

        #endregion


        /// <summary>
        /// Initializes a new instance of the VisioExporter class.
        /// </summary>
        public ExportTransitions()
        {

        }


        private bool _skip = false;


        #region Public Entry Point



        /// <summary>
        /// Exports a transition diagram.
        /// </summary>
        /// <param name="page">Page.</param>
        /// <param name="mode">Export mode.</param>
        public PageReport ExportPage(Page page)
        {

            _skip = false;
            _totalSteps = 1000;
            _currentStep = 0;
            OnStartStep("Obtain total number of shapes");

            _totalSteps = 6 + 3 * page.Shapes.Count;
            return DoPageExport(page, "live");
        }


        /// <summary>
        /// Validates a transition diagram.
        /// </summary>
        /// <param name="page">Page.</param>
        /// <param name="mode">Export mode.</param>
        public PageReport ValidatePage(Page page)
        {
            _totalSteps = 1000;
            _currentStep = 0;
            OnStartStep("Obtaining shapes total number...");

            _totalSteps = 2 + 3 * page.Shapes.Count;

            return DoPageValidation(page, "live");
        }

        private PageReport DoPageValidation(Page page, string mode)
        {
            Dictionary<IShapeHandler, Shape> mapping;
            List<IShapeHandler> webPages, transitions;
            Hashtable shapeHandlers;

            return DoPageValidation(page, mode, out mapping,
                                    out webPages, out transitions);

        }

        private PageReport DoPageValidation(Page page, string mode, 
                            out Dictionary<IShapeHandler, Shape> mapping, 
                            out List<IShapeHandler> webPages, out List<IShapeHandler> transitions)
        {
            PageReport report = new PageReport(page);
            Hashtable shapeHandlers = new Hashtable();
            Hashtable fromConns = new Hashtable();
            Hashtable toConns = new Hashtable();
            mapping = new Dictionary<IShapeHandler, Shape>();
            webPages = new List<IShapeHandler>();
            transitions = new List<IShapeHandler>();

            try
            {
                int processedShapes = 0;

                /*
                 * Iterate through all the shapes in the current page, generating
                 * the overall graph of the shapes. This effectively preloads all
                 * of the information into memory.
                 */
                #region Preloading, Construct Graph

                foreach (Shape shape in page.Shapes)
                {
                    string progressMessage = string.Format("Page: {0}, Shape: {1}, scanning...", page.Name, shape.Name);
                    OnStartStep(progressMessage);

                    string shapeName = VisioUtils.GetProperty(shape, "User", "Midgard");


                    if (shapeName == null)
                    {
                        OnEndStep("skip");
                        continue;
                    }

                    processedShapes++;

                    IShapeHandler shapeHandler = GetShapeHandler(shapeHandlers, shape, shapeName, mode);
                    mapping.Add(shapeHandler, shape);

                    if ("transition" == shapeHandler.Element && 2 <= shape.Connects.Count)
                    {
                        SetShapeConn(fromConns, shape.Connects[1].ToCell.Shape, shapeHandler);
                        SetShapeConn(toConns, shape.Connects[2].ToCell.Shape, shapeHandler);
                    }
                    OnEndStep("ok.");
                }

                /*
                 * If no processable shapes have been encountered, then reset the
                 * processed bit to false and quit early...
                 */
                if (processedShapes == 0)
                {
                    report.Processed = false;
                    _currentStep += page.Shapes.Count;

                    return report;
                }

                #endregion

                #region Connections Validation (client side)

                foreach (IShapeHandler h in shapeHandlers.Values)
                {
                    ShapeError error;
                    OnStartStep(string.Format("A validar localmente a shape {0}", h.Text.Replace('\n', ' ')));
                    switch (h.Element)
                    {
                        case "transition":
                            error = h.Validate(shapeHandlers);
                            transitions.Add(h);
                            break;

                        default:
                            ArrayList from = (ArrayList)fromConns[h.VisioShape];
                            ArrayList to = (ArrayList)toConns[h.VisioShape];
                            error = h.Validate(new object[] { from, to });
                            webPages.Add(h);
                            break;

                    }

                    if (error != null)
                        report.Errors.Add(error);
                }

                /*
                 * Contains errors: abort! || only clientValidation
                 */
                if (report.Errors.Count > 0)
                {
                    _currentStep += page.Shapes.Count;
                    return report;
                }

                if (0 == shapeHandlers.Count)
                {
                    report.Errors.Add(new ShapeError(Resources.GetString(ResourceTokens.NoDiagram)));
                    return report;
                }

                #endregion Connections Validation (client side)

                #region Data Validation (server side)

                if (report.Errors.Count > 0)
                {
                    _currentStep += page.Shapes.Count;
                    return report;
                }

                #endregion Data Validation (server side)

            }
            catch (Exception e)
            {
                report.Errors.Add(new ShapeError(e.StackTrace));
            }
            finally
            {
                /*
                * Avoid maintaining a reference to Interop COM objects. Release
                * all of the content of the graph to avoid memory leaks.
                */
                foreach (IShapeHandler h in shapeHandlers.Values)
                {
                    (h as IDisposable).Dispose();
                }

                shapeHandlers.Clear();
                fromConns.Clear();
                toConns.Clear();
            }

            return report;
        }


        private PageReport DoPageExport(Page page, string mode)
        {
            Dictionary<IShapeHandler, Shape> mapping;
            List<IShapeHandler> webPages, transitions;

            PageReport report = DoPageValidation(page, mode, out mapping,
                                        out webPages, out transitions);

            if (report.Errors.Count > 0)
            {
                _currentStep += page.Shapes.Count;
                return report;
            }

            OnStartStep("Getting web pages information...");
            List<WebPageObj> pages = new List<WebPageObj>(webPages.Count);

            foreach (IShapeHandler shape in webPages)
            {
                string title = shape.GetProperties()["title"].InnerText;

                if (!ContainsConditional(pages, title))
                {
                    WebPageObj webPage = new WebPageObj(title);
                    webPage.Entity = shape.GetProperties()["entity"].InnerText;
                    webPage.Operation = shape.GetProperties()["operation"].InnerText;
                    webPage.Fields = GetFields(shape);

                    pages.Add(webPage);
                }
            }
            OnEndStep("ok.");

            OnStartStep("Getting web pages transitions information...");

            List<TransitionObj> shifts = new List<TransitionObj>(transitions.Count);

            foreach (IShapeHandler ishape in transitions)
            {
                Shape transition = mapping[ishape];

                Shape temp = transition.Connects[1].ToCell.Shape;
                string start = temp.Text.Substring(0, temp.Text.IndexOf('\n'));
                temp = transition.Connects[2].ToCell.Shape;
                string end = temp.Text.Substring(0, temp.Text.IndexOf('\n'));

                TransitionObj trans = new TransitionObj();
                trans.Origin = start;
                trans.Destiny = end;
                trans.Label = ishape.GetProperties()["value"].InnerText;
                trans.Connector = ishape.GetProperties()["connector"].InnerText;

                shifts.Add(trans);

            }
            OnEndStep("ok.");

            string fileName = "\\" + page.Document.Name.Substring(0,page.Document.Name.Length-4)  + "_" + page.Name + ".xml";
            Dictionary<string, object> param = new Dictionary<string,object>();
            param.Add("pages", pages);
            param.Add("shifts", shifts);

            string guid = (string)Registry.ClassesRoot.OpenSubKey("MidgardDynamic.Interop.Connect").OpenSubKey("CLSID").GetValue("");
            string exec = (string)Registry.ClassesRoot.OpenSubKey("CLSID").OpenSubKey(guid).OpenSubKey("InprocServer32").GetValue("BaseDir");
            string template = System.IO.Path.Combine(exec, "AddIn\\Templates\\Dynamic.vtl");
            string exportDir = System.IO.Path.Combine(exec, "Export");

            FileInfo tempFile = new FileInfo(template);
            tempFile.CopyTo(Directory.GetCurrentDirectory() + "/Dynamic.vtl");
            
            OnStartStep("Creating file " + fileName + " ...");
            Templates.Generate("Dynamic.vtl", exportDir + fileName, param, false);

            tempFile = new FileInfo(Directory.GetCurrentDirectory() + "/Dynamic.vtl");
            tempFile.Delete();

            OnEndStep("ok.");
            return report;

        }

        private string[] GetFields(IShapeHandler shape)
        {
            if (null == shape.GetProperties()["fields"])
                return new string[0];

            string fieldInfo = shape.GetProperties()["fields"].InnerXml;
            int begin = 0, end = 0, pointer = 0;

            List<string> toReturn = new List<string>();
            while (true)
            {
                begin = fieldInfo.IndexOf("<value>", pointer);

                if (-1 == begin)
                    break;

                pointer = begin;

                end = fieldInfo.IndexOf("</value>", pointer);
                pointer = end;

                toReturn.Add(fieldInfo.Substring(begin+7,end-(begin+7)));

            }
            return toReturn.ToArray();
        }

        private bool ContainsConditional(List<WebPageObj> pages, string title)
        {
            foreach (WebPageObj page in pages)
            {
                if (title == page.Title)
                    return true;
            }
            return false;
        }

        #endregion

        #region Events

        /// <summary>
        /// Raises the <see cref="StartStep"/> event.
        /// </summary>
        /// <param name="name">Name of the step.</param>
        private void OnStartStep(string name)
        {
            if (StartStep != null)
                StartStep(_currentStep, _totalSteps, name);

            _currentStep++;
        }

        /// <summary>
        /// Raises the <see cref="EndStep"/> event, providing a formatted step 
        /// ending description.
        /// </summary>
        /// <param name="format">Description formatter of the step result.</param>
        /// <param name="args">Description arguments.</param>
        private void OnEndStep(string format, params object[] args)
        {
            string description = string.Format(CultureInfo.InvariantCulture, format, args);

            if (EndStep != null)
                EndStep(description);
        }

        /// <summary>
        /// Raises the <see cref="EndStep"/> event, providing a step ending description.
        /// </summary>
        /// <param name="description">Description of the step result.</param>
        private void OnEndStep(string description)
        {
            if (EndStep != null)
                EndStep(description);
        }

        /// <summary>
        /// Raises the <see cref="EndStep"/> event, without a concluding description.
        /// </summary>
        private void OnEndStep()
        {
            if (EndStep != null)
                EndStep(null);
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


        #region Private Methods

        private void SetShapeConn(Hashtable connections, Shape shape, IShapeHandler shapeHandler)
        {
            ArrayList list = (ArrayList)connections[shape];

            if (list == null)
            {
                list = new ArrayList();
                connections[shape] = list;
            }

            list.Add(shapeHandler);
        }

        private IShapeHandler GetShapeHandler(Hashtable dictionary, Shape shape, string shapeName, string mode)
        {
            IShapeHandler shapeHandler = (IShapeHandler)dictionary[shape];

            if (shapeHandler != null)
                return shapeHandler;

            XmlForm form = new XmlForm();
            form.LoadDefinition(shapeName, shape);
            form.SkipDesign = _skip;

            if (!_skip)
                form.Design(mode);

            shapeHandler = (IShapeHandler)form.Tag;
            shapeHandler.LoadProperties();

            dictionary[shape] = shapeHandler;

            return shapeHandler;
        }

        #endregion Private Methods

    }
}
