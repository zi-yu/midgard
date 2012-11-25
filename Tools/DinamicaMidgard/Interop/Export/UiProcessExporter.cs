using System;
using System.Text;
using Microsoft.Office.Interop.Visio;

namespace Midgard.Interop.Export
{
    /// <summary>
    /// GUI wrapper around VisioExplorer class.
    /// </summary>
    public class UiProcessExporter
    {
        #region Private Members

        private ProgressForm _form;
        private ProcessExporter _exporter;
        private ExportTransitions _transition;
        private int _maxSize;
        private string _more;
        #endregion


        /// <summary>
        /// Initializes a new instance of the UiExplorer class.
        /// </summary>
        public UiProcessExporter()
            : this("UTF-8")
        {
        }

        public UiProcessExporter(string encoding)
        {
            _form = new ProgressForm();
            _exporter = new ProcessExporter(encoding);
            _exporter.StartStep += new StepStartEventHandler(exporter_StartStep);
            _exporter.EndStep += new StepEndEventHandler(exporter_EndStep);

            _transition = new ExportTransitions();
            _transition.StartStep += new StepStartEventHandler(exporter_StartStep);
            _transition.EndStep += new StepEndEventHandler(exporter_EndStep);

            _maxSize = int.Parse(Resources.GetString(ResourceTokens.ShapeTextMaxSize));
            _more = Resources.GetString(ResourceTokens.ShapeTextTruncate);

        }

        #region Delegate Callbacks

        /// <summary>
        /// Callback to the StartStep export event.
        /// </summary>
        /// <param name="currentStep">Current step of execution.</param>
        /// <param name="totalSteps">Total steps of execution.</param>
        /// <param name="stepName">Name of current step.</param>
        private void exporter_StartStep(int currentStep, int totalSteps, string stepName)
        {
            _form.SetProgress(currentStep, totalSteps, stepName);
        }

        /// <summary>
        /// Callback to the EndStep export event.
        /// </summary>
        /// <param name="description">Description of the step result.</param>
        private void exporter_EndStep(string description)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Exports the page to DB using a WS
        /// </summary>
        /// <param name="page">Page.</param>
        /// <param name="mode">Export mode.</param>
        public void ExportTransition(Page page)
        {
            PageReport report;

            try
            {
                _form.Show();
                report = _transition.ExportPage(page);
            }
            finally
            {
                _form.Close();
            }

            ShowReport(report, true);
        }

        private void ShowReport(PageReport report, bool isExporting)
        {
            if (report.Processed == false)
            {
                string s = Resources.GetString(ResourceTokens.ExportPageNotMidgard);
                ESIMessageBox.ShowError(s);
            }
            else if (report.Errors.Count == 0)
            {
                string s;
                if (isExporting)
                {
                    s = Resources.GetString(ResourceTokens.ExportPageOk);
                }
                else
                {
                    s = Resources.GetString(ResourceTokens.ValidatePageOk);
                }
                ESIMessageBox.Show(s);
            }
            else
            {
                DisplayErrorCollection(report);
            }
        }

        public void ValidatePage(Page page)
        {
            PageReport report;
            try
            {
                _form.Show();
                report = _transition.ValidatePage(page);
            }
            finally
            {
                _form.Close();
            }

            ShowReport(report, false);
        }

        #endregion


        private void DisplayErrorCollection(PageReport report)
        {
            StringBuilder sb = new StringBuilder();

            string fmt = Resources.GetString(ResourceTokens.ExportErrorsInPage);
            string shapeFmt = Resources.GetString(ResourceTokens.ExportShapeError);

            sb.AppendFormat(fmt, report.Errors.Count, report.Page.Name);

            foreach (ShapeError e in report.Errors)
            {
                if (null != e.ShapeHandler)
                {
                    sb.AppendFormat(shapeFmt,
                        Truncate(e.ShapeHandler.Text.Replace('\n', ' ')),
                        e.Message);
                }
                else
                {
                    sb.Append(e.Message);
                    sb.Append("\n\r");
                }
            }

            ESIMessageBox.ShowError(sb.ToString());
        }



        private string Truncate(string s)
        {
            return Truncate(s, _maxSize, _more);
        }

        private string Truncate(string value, int maxSize, string truncation)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            if (value.Length <= maxSize)
                return value;

            else if (truncation != null)
                return value.Substring(0, maxSize - truncation.Length) + truncation;

            else
                return value.Substring(0, maxSize);
        }
    }
}

