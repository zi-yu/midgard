using System;
using System.IO;
using System.Reflection;

namespace Midgard.XmlForms.Configuration
{
	/// <summary>
	/// Utility class for Assemblies to obtain the path to their configuration
	/// file.
	/// </summary>
	/// <remarks>
	/// This is necessary for assemblies which participate in a application domain
	/// in which no configuration file exists. Whenever registering classes for
	/// COM Interop, thereby leaving that application domain without a centralized
	/// configuration file. Hence, each assembly must retrieve configuration
	/// settings from it's own file.
	/// </remarks>
	public sealed class AssemblyConfiguration
	{
		/// <remarks/>
		private AssemblyConfiguration()
		{
		}

		#region GetConfigurationFile

		private const string ConfigurationExtension = ".config";

		/// <summary>
		/// Obtains the location of the configuration file for the assembly.
		/// </summary>
		/// <param name="assembly">Assembly.</param>
		/// <returns>Filename location.</returns>
		public static string GetConfigurationFile( Assembly assembly )
		{
			if ( assembly == null )
				throw new ArgumentNullException( "assembly" );

			return GetConfigurationFile( assembly.Location, ConfigurationExtension );
		}


		/// <summary>
		/// Obtains the location of the configuration file for the assembly.
		/// </summary>
		/// <param name="assembly">Assembly.</param>
		/// <param name="configurationExtension">The filename extension of the configuration file.</param>
		/// <returns>Filename location.</returns>
		public static string GetConfigurationFile( Assembly assembly, string configurationExtension )
		{
			if ( assembly == null )
				throw new ArgumentNullException( "assembly" );

			return GetConfigurationFile( assembly.Location, configurationExtension );
		}


		/// <summary>
		/// Obtains the location of the configuration file for the assembly.
		/// </summary>
		/// <param name="assembly">Location of the assembly.</param>
		/// <returns>Filename location.</returns>
		public static string GetConfigurationFile( string assembly )
		{
			return GetConfigurationFile( assembly, ConfigurationExtension );
		}

		/// <summary>
		/// Obtains the location of the configuration file for the assembly.
		/// </summary>
		/// <param name="assembly">Location of the assembly.</param>
		/// <param name="configurationExtension">The filename extension of the configuration file.</param>
		/// <returns>Filename location.</returns>
		public static string GetConfigurationFile( string assembly, string configurationExtension )
		{
			#region Validations

			if ( assembly == null )
				throw new ArgumentNullException( "dll" );

			if ( configurationExtension == null )
				throw new ArgumentNullException( "configurationExtension" );

			#endregion

			FileInfo info = new FileInfo( assembly );

			string config = System.IO.Path.Combine( 
				info.DirectoryName,
				info.Name.Substring( 0, info.Name.Length - info.Extension.Length ) + configurationExtension );

			return config;
		}

		#endregion
	}
}
