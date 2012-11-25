using System.Globalization;
using System.Collections.Specialized;
using System.Collections;

namespace Midgard.Interop
{
	/// <summary>
	/// Describes a single CgoConfig.
	/// </summary>
	public class CgoConfig
	{
		#region Private Members

		private string _code;
		private string _name;
        private string _id;
        private string _label;
        private string _summary;
        private string _type;
        private string _size;
        private string _lovType;

		#endregion


		/// <summary>
		/// Initializes a new instance of the EaiService class.
		/// </summary>
		/// <param name="code">Code of service.</param>
		public CgoConfig( string code )
		{
			_code = code;
			_name = "Unknown Cgo Config.";
		}

		/// <summary>
		/// Initializes a new instance of the CgoConfig class.
		/// </summary>
		/// <param name="code">Code of service.</param>
		/// <param name="name">Name of service.</param>
		public CgoConfig( string code, string name )
		{
			_code = code;
			_name = name;
		}

        /// <summary>
        /// Initializes a new instance of the CgoConfig class.
        /// </summary>
        /// <param name="code">Code of service.</param>
        /// <param name="name">Name of service.</param>
        /// <param name="name">Associated Id.</param>
        /// <param name="name">Associated Label.</param>
        /// <param name="name">Associated summary.</param>
        /// <param name="name">Associated type</param>
        /// <param name="name">Associated size</param>
        /// <param name="name">Associated lovType</param>
        public CgoConfig(string code, string name, string id, string label, string summary, string type, string size, string lovType)
        {
            _code = code;
            _name = name;
            _id = id;
            _label = label;
            _summary = summary;
            _type = type;
            _size = size;
            _lovType = lovType;
        }

		/// <summary>
		/// Gets the CgoConfig code.
		/// </summary>
		public string Code
		{
			get { return _code; }
		}

		/// <summary>
		/// Gets the CgoConfig name.
		/// </summary>
		public string Name
		{
			get { return _name; }
		}

        /// <summary>
        /// Gets the CgoConfig id.
        /// </summary>
        public string Id
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets the CgoConfig label.
        /// </summary>
        public string Label
        {
            get { return _label; }
        }

        /// <summary>
        /// Gets the CgoConfig description.
        /// </summary>
        public string Summary
        {
            get { return _summary; }
        }

        /// <summary>
        /// Gets the CgoConfig type.
        /// </summary>
        public string Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Gets the CgoConfig size.
        /// </summary>
        public string Size
        {
            get { return _size; }
        }

        /// <summary>
        /// Gets the CgoConfig lovType.
        /// </summary>
        public string LOVType
        {
            get { return _lovType; }
        }

		/// <summary>
		/// Returns the string representation of the current instance.
		/// </summary>
		/// <returns>String representation of the object.</returns>
		public override string ToString()
		{
			return string.Format( CultureInfo.InvariantCulture, "{0}", _name );
		}
	}
}