using System;
using System.Xml;
using Midgard.XmlForms.Configuration;

namespace Midgard.XmlForms
{
	/// <summary>
	/// Encapsulates namespace knowledge for Xml Forms documents.
	/// </summary>
	public sealed class FormsNamespace
	{
		#region Constants

		/// <summary>
		/// Gets the Xml Forms namespace prefix.
		/// </summary>
		public const string Prefix = "xf";

		/// <summary>
		/// Gets the Xml Forms namespace URI.
		/// </summary>
		public const string Uri = "http://www.tranquilidade.pt/XmlForms/";

		#endregion

		#region Private Members

		private static XmlNamespaceManager _manager;

		#endregion


		/// <remarks>
		/// Prevent class initialization.
		/// </remarks>
		private FormsNamespace()
		{
		}


		/// <summary>
		/// Gets the namespace manager for an XML Forms application.
		/// </summary>
		public static XmlNamespaceManager NamespaceManager
		{
			get
			{
				if ( _manager == null )
				{
					NameTable nt = new NameTable();

					_manager = new XmlNamespaceManager( nt );
					//_manager.AddNamespace( FormsNamespace.Prefix, FormsNamespace.Uri );

                    foreach (Namespace n in FormConfigs.Namespaces)
						_manager.AddNamespace( n.Prefix, n.Uri );
				}

				return _manager;
			}
		}
	}
}
