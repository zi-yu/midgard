using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using WinMessageBox = System.Windows.Forms.MessageBox;

namespace Midgard.Interop
{
	/// <summary>
	/// Custom MessageBox wrapper.
	/// </summary>
	[ComVisible( false )]
	public sealed class ESIMessageBox
	{
		/// <remarks>
		/// Prevent initialization.
		/// </remarks>
		private ESIMessageBox()
		{
		}


		/// <summary>
		/// Displays the message as an information box.
		/// </summary>
		/// <param name="s">String message.</param>
		public static void Show( string s )
		{
            //ESITracer.Current.LogInfo(s);
            WinMessageBox.Show(null, s, "Relatório", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}


		/// <summary>
		/// Displays a formatted message as an information box.
		/// </summary>
		/// <param name="format">Message format.</param>
		/// <param name="args">Message arguments.</param>
		public static void Show( string format, params object[] args )
		{
			string s = string.Format( CultureInfo.InvariantCulture, format, args );
            //ESITracer.Current.LogInfo(s);
            WinMessageBox.Show(null, s, "Relatório", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}


		/// <summary>
		/// Displays the message as an error box.
		/// </summary>
		/// <param name="s">String message.</param>
		public static void ShowError( string s )
		{
            //ESITracer.Current.LogError(s);
            WinMessageBox.Show(null, s, "Relatório", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}


		/// <summary>
		/// Displays the message as an error box.
		/// </summary>
		/// <param name="format">Message format.</param>
		/// <param name="args">Message arguments.</param>
		public static void ShowError( string format, params object[] args )
		{
			string s = string.Format( CultureInfo.InvariantCulture, format, args );
            //ESITracer.Current.LogError(s);
            WinMessageBox.Show(null, s, "Relatório", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}


		/// <summary>
		/// Displays the exception as an error box.
		/// </summary>
		/// <param name="ex">Unhandled exception.</param>
		public static void ShowError( Exception ex )
		{
            //ESITracer.Current.LogError(ex.StackTrace.ToString());
			ExceptionMessageBox box = new ExceptionMessageBox();
			box.ErrorMessage = String.Format("A aplicação encontrou uma excepção.\nExcepção: {0}", ex.Message);
			box.Exception = ex;
			box.ShowDialog();
		}

        /// <summary>
        /// Displays the message as a warning box.
        /// </summary>
        /// <param name="s">String message.</param>
        public static DialogResult ShowWarningOKCancel(string s)
        {
            //ESITracer.Current.LogError(s);
            return WinMessageBox.Show(null, s, "Aviso", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
        }

        public static DialogResult ShowWarningOK(string s)
        {
            //ESITracer.Current.LogError(s);
            return WinMessageBox.Show(null, s, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}