using System;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using stdole;
 

namespace Midgard.Interop
{
	/// <summary>
	/// Utility class for accessing embedded resources in the current assembly.
	/// </summary>
	public sealed class Resources
	{
		#region Private Members

		private const string ResourcesBaseName = "Midgard.Interop.Resources.StringResources";
		private const string BasePath = "Midgard.Interop.Resources.";
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


		/// <summary>This method creates a picture object from the contents
		/// of the passed in bitmap file.</summary>
		/// <remark>Note: The bitmap file is embedded as a resource in the 
		/// executable. The file is temporarily copied out to the client 
		/// file system by this function, since the OleLoadPictureFile 
		/// function requires the file to reside in the file system.</remark>
		///	<param name="resourceName">File name of the bitmap embedded as
		/// a resource in the executable</param>
		/// <returns>stdole.IPictureDisp object created from the contents 
		/// of the file.</returns>
		public static IPictureDisp GetPicture( string resourceName )
		{
			int loadPictureResult;
			Object picture = null;

			try
			{
				string temporaryFile = Path.GetTempFileName();

				// Get an input stream of the bitmap file embedded 
				// in the executable.
				Stream inStream = Assembly.GetExecutingAssembly().GetManifestResourceStream( BasePath + resourceName );

				// Create a BinaryReader from the input stream.
				BinaryReader inReader = new BinaryReader( inStream );

				// Create the output stream for the temporary file.
				FileStream outStream = new FileStream( temporaryFile, FileMode.Create );

				// Create a BinaryWriter from the output stream.
				BinaryWriter outWriter = new BinaryWriter( outStream );

				byte[] buffer = new byte[ 1000 ];
				int bytesRead = inReader.Read( buffer, 0, buffer.Length );

				while ( bytesRead > 0 )
				{
					outWriter.Write( buffer, 0, bytesRead );
					bytesRead = inReader.Read( buffer, 0, buffer.Length );
				}

				inReader.Close();
				outWriter.Close();

				// Load the bitmap from the temporary file.
				loadPictureResult = OleLoadPictureFile( temporaryFile, out picture );

				// Delete the temporary file.
				File.Delete( temporaryFile );

				if ( loadPictureResult != 0 )
					throw new COMException( "OleLoadPictureFailed", loadPictureResult );
			}
			catch ( COMException ex )
			{
                ESIMessageBox.ShowError(ex);
			}

			return (IPictureDisp) picture;
		}


		/// <summary>This is the prototype for the OleLoadPictureFile 
		/// method.</summary>
		[DllImport( "OleAut32.dll" )]
		internal static extern int OleLoadPictureFile( 
			[In] Object FileName, 
			[Out, MarshalAs( UnmanagedType.Interface )] out Object Picture );

	}
}