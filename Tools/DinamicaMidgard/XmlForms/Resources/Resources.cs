using System;
using System.IO;
using System.Reflection;
using System.Resources;

namespace Midgard.XmlForms
{
	/// <summary>
	/// Utility class for accessing embedded resources in the current assembly.
	/// </summary>
	internal sealed class Resources
	{
		#region Private Members

		private const string ResourcesBaseName = "Midgard.XmlForms.Resources.StringResources";
		private static ResourceManager _rm = null;

		#endregion

		private Resources()
		{
		}


		/// <summary>
		/// Retrieves a string value from the resource file.
		/// </summary>
		/// <param name="s">Key.</param>
		/// <returns>String value, or null if not found.</returns>
		public static string GetString( string s )
		{
			if ( _rm == null )
				_rm = new ResourceManager( ResourcesBaseName, Assembly.GetExecutingAssembly() );

			return _rm.GetString( s );
		}

        /// <summary>
        /// Retrieves a color value from the resource file.
        /// </summary>
        /// <param name="s">Key.</param>
        /// <returns>Color value, or white if not found.</returns>
        public static System.Drawing.Color GetColor(string s)
        {
            if (s == "required")
                return System.Drawing.Color.PapayaWhip;
            else
                return System.Drawing.Color.White;
        }
    }
}