using System.Globalization;

namespace Midgard.XmlForms
{
	/// <summary>
	/// Describes a single ComboBoxObject.
	/// </summary>
	public class ComboBoxObject
	{
		#region Private Members

		private string _code;
		private string _name;

		#endregion


		/// <summary>
        /// Initializes a new instance of the ComboBoxObject class.
		/// </summary>
		/// <param name="code">Code of service.</param>
		public ComboBoxObject( string code )
		{
			_code = code;
			_name = "Unknown Object Config.";
		}

		/// <summary>
		/// Initializes a new instance of the ComboBoxObject class.
		/// </summary>
		/// <param name="code">Code of service.</param>
		/// <param name="name">Name of service.</param>
		public ComboBoxObject( string code, string name )
		{
			_code = code;
			_name = name;
		}

		/// <summary>
		/// Gets the ComboBoxObject code.
		/// </summary>
		public string Code
		{
			get { return _code; }
		}

		/// <summary>
		/// Gets the ComboBoxObject name.
		/// </summary>
		public string Name
		{
			get { return _name; }
		}

		/// <summary>
		/// Returns the string representation of the current instance.
		/// </summary>
		/// <returns>String representation of the object.</returns>
		public override string ToString()
		{
			if((_code==string.Empty)&&(_name==string.Empty))
				return string.Empty;
			else
				return string.Format( CultureInfo.InvariantCulture, "{0}", _name );
		}
	}
}
