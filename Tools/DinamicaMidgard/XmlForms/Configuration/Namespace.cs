using System;
using System.Xml;

namespace Midgard.XmlForms
{
	/// <summary>
	/// Describes a namespace declaration.
	/// </summary>
	public sealed class Namespace
	{
		#region Private Members

		private string _prefix;
		private string _uri;

		#endregion

		/// <summary>
		/// Gets the string prefix.
		/// </summary>
		public string Prefix
		{
			get { return _prefix; }
		}

		/// <summary>
		/// Gets the namespace URI.
		/// </summary>
		public string Uri
		{
			get { return _uri; }
		}


		/// <summary>
		/// Initializes a new instance of the Namespace class.
		/// </summary>
		/// <param name="prefix">Namespace prefix.</param>
		/// <param name="uri">Nameprefix URI.</param>
		internal Namespace( string prefix, string uri )
		{
			#region Validations

			if ( prefix == null )
				throw new ArgumentNullException( "prefix" );

			if ( uri == null )
				throw new ArgumentNullException( "uri" );

			if ( prefix.Length == 0 )
				throw new ArgumentNullException( "prefix", "Empty prefix." );

			if ( uri.Length == 0 )
				throw new ArgumentNullException( "uri", "Empty uri." );

			if ( prefix == FormsNamespace.Prefix )
				throw new ArgumentException( "prefix", "Xml Forms prefix is reserved for Xml Forms namespace." );

            if ( uri == FormsNamespace.Uri )
                throw new ArgumentException( "uri", "Xml Forms Namespace is always added." );

			#endregion

			_prefix = prefix;
			_uri = uri;
		}
	}
}