using System.Windows.Forms;
using Genghis.Windows.Forms;

namespace Midgard.XmlForms
{
	/// <summary>
	/// Custom validator to validate the folder path, _relative_ to the
	/// base path of the current document.
	/// </summary>
	public sealed class ListValidator : BaseValidator
	{
		/// <summary>
		/// Initializes a new instance of the FolderExistsValidator class.
		/// </summary>
		public ListValidator()
		{
		}

		/// <summary>
		/// Determines wether the control being validated is a valid
		/// folder name (relative to the document path).
		/// </summary>
		/// <returns>True if valid, False otherwise.</returns>
		protected override bool EvaluateIsValid()
		{
			ListBox valueControl = (ListBox)this.ControlToValidate;
			return valueControl.Items.Count!=0;
		}
	}
}
