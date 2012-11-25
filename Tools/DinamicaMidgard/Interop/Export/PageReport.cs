using System;
using Microsoft.Office.Interop.Visio;

namespace  Midgard.Interop
{
	/// <summary>
	/// Describes the result of exporting/validating a single page.
	/// </summary>
	public class PageReport
	{
		#region Private Members

		private Page _page;
		private bool _processed;
        private bool _haveGlobalData;
        private bool _haveDuplicatedData;
		private ShapeErrorCollection _errors;
		private ShapeErrorCollection _warnings;

		#endregion

		/// <summary>
		/// Initializes a new instance of the PageReport class.
		/// </summary>
		/// <param name="page">Page to which this report refers to.</param>
		public PageReport( Page page )
		{
			_page = page;
			_processed = true;
            _haveGlobalData = true;
            _haveDuplicatedData = false;
			_errors = new ShapeErrorCollection();
			_warnings = new ShapeErrorCollection();

		}

		/// <summary>
		/// Gets the Visio page for which this report is relative to.
		/// </summary>
		public Page Page
		{
			get { return _page; }
		}

		/// <summary>
		/// Gets or sets wether the current page was an object of processing
		/// for Quartz processes.
		/// </summary>
		public bool Processed
		{
			get { return _processed; }
			set { _processed = value; }
		}

        /// <summary>
        /// Gets or sets wether the current page have global data
        /// </summary>
        public bool HaveGlobalData
        {
            get { return _haveGlobalData; }
            set { _haveGlobalData = value; }
        }

        /// <summary>
        /// Gets or sets wether the current page have duplicated global data
        /// </summary>
        public bool HaveDuplicatedData
        {
            get { return _haveDuplicatedData; }
            set { _haveDuplicatedData = value; }
        }

		/// <summary>
		/// Gets the collection of errors accumulated when processing this page.
		/// If the page has been skipped, the errors collection will always be empty.
		/// </summary>
		public ShapeErrorCollection Errors
		{
			get { return _errors; }
		}

		/// <summary>
		/// 
		/// </summary>
		public ShapeErrorCollection Warnings
		{
			get { return _warnings; }
		}
	}
}
