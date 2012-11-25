using System;
using System.Windows.Forms;
using Genghis.Windows.Forms;

namespace Midgard.XmlForms
{
	/// <summary>
	/// Custom validator to validate the folder path, _relative_ to the
	/// base path of the current document.
	/// </summary>
	public sealed class IntegerValidator : BaseValidator
	{
		/// <summary>
		/// Initializes a new instance of the FolderExistsValidator class.
		/// </summary>
		public IntegerValidator()
		{
		}

		/// <summary>
		/// Determines wether the control being validated is a valid
		/// folder name (relative to the document path).
		/// </summary>
		/// <returns>True if valid, False otherwise.</returns>
		protected override bool EvaluateIsValid()
		{
			TextBox valueControl = (TextBox)this.ControlToValidate;

			if((valueControl.Text==null)||(valueControl.Text.Trim()==string.Empty))
				return true;
			try
			{
				int.Parse(valueControl.Text);
				return true;
			} catch(Exception)
			{
				return false;
			}
		}
	}
}
