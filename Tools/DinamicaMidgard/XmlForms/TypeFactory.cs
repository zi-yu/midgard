using System;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting;
using System.Security;

namespace Midgard.XmlForms
{
	/// <summary>
	/// Provides utility methods to create type.
	/// </summary>
	public sealed class TypeFactory
	{
		/// <remarks>
		/// Prevent class initialization.
		/// </remarks>
		private TypeFactory()
		{
		}


		/// <summary>
		/// Creates an instance of a .NET type from it's moniker.
		/// </summary>
		/// <param name="moniker">
		/// .NET type moniker. This moniker must have the standard Type,Assembly format.
		/// </param>
		/// <returns>Object instance.</returns>
		public static object Create( string moniker )
		{
			if ( moniker == null )
				throw new ArgumentNullException( "moniker" );

			string[] parts = moniker.Split( ',' );

			if ( parts.Length < 2 )
				throw new ArgumentException( string.Format( "'{0}' isn't recognized as a valid .NET moniker.", moniker ), "moniker" );

			return Create( parts[ 0 ].Trim(), parts[ 1 ].Trim() );
		}


		/// <summary>
		/// Creates an instance of a .NET type from it's moniker.
		/// </summary>
		/// <param name="moniker">
		/// .NET type moniker. This moniker must have the standard Type,Assembly format.
		/// </param>
		/// <param name="args">Activation arguments.</param>
		/// <returns>Object instance.</returns>
		public static object Create( string moniker, object[] args )
		{
			if ( moniker == null )
				throw new ArgumentNullException( "moniker" );

			string[] parts = moniker.Split( ',' );

			if ( parts.Length < 2 )
				throw new ArgumentException( string.Format( "'{0}' isn't recognized as a valid .NET moniker.", moniker ), "moniker" );

			return Create( parts[ 0 ].Trim(), parts[ 1 ].Trim(), args );
		}
	
		/// <summary>
		/// Creates a new instance of the .NET type.
		/// </summary>
		/// <param name="type">Type name.</param>
		/// <param name="assembly">Assembly name.</param>
		/// <returns>Object instance.</returns>
		public static object Create( string type, string assembly )
		{
			return Create( type, assembly, null );			
		}

		/// <summary>
		/// Creates a new instance of the .NET type.
		/// </summary>
		/// <param name="type">Type name.</param>
		/// <param name="assemblyName">Assembly name.</param>
		/// <param name="args">Activation arguments.</param>
		/// <returns>Object instance.</returns>
		/// <exception cref="QuartzFrameworkException">If there was an error creating
		/// the instance of the .NET type.</exception>
		public static object Create( string type, string assemblyName, object[] args )
		{
			ObjectHandle objHandle;
				
			try
			{
				objHandle = Activator.CreateInstance( assemblyName, type, false, 
					BindingFlags.Default, null, args, null, null, null );
			}
			catch ( FileNotFoundException ex )
			{
				throw new Exception( string.Format( "Exception creating instance of type '{0},{1}'.", type, assemblyName ), ex );
			}
			catch ( MissingMethodException ex )
			{
				throw new Exception( string.Format( "Exception creating instance of type '{0},{1}'.", type, assemblyName ), ex );
			}
			catch ( TypeLoadException ex )
			{
				throw new Exception( string.Format( "Exception creating instance of type '{0},{1}'.", type, assemblyName ), ex );
			}
			catch ( MethodAccessException ex )
			{
				throw new Exception( string.Format( "Exception creating instance of type '{0},{1}'.", type, assemblyName ), ex );
			}
			catch ( SecurityException ex )
			{
				throw new Exception( string.Format( "Exception creating instance of type '{0},{1}'.", type, assemblyName ), ex );
			}

			return objHandle.Unwrap();
		}
	}
}
