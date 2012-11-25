using System;
using System.Collections;
using System.IO;
using System.Text;

namespace Midgarg.Interop
{
	/// <summary>
	/// Additional methods for Path manipulation.
	/// </summary>
	public sealed class PathEx
	{
		/// <remarks>
		/// Private constructor to prevent initialization.
		/// </remarks>
		private PathEx()
		{
		}

		#region Combine

		/// <summary>
		/// Combines two path strings.  
		/// </summary>
		/// <param name="path1">The first path.</param>
		/// <param name="path2">The second path.</param>
		/// <returns>A string containing the combined paths. If one of the specified paths is 
		/// a zero-length string, this method returns the other path. If path2 contains an 
		/// absolute path, this method returns path2.</returns>
		public static string Combine( string path1, string path2 )
		{
			return Path.Combine( path1, path2 );
		}

		/// <summary>
		/// Combines three path strings.  
		/// </summary>
		/// <param name="path1">The first path.</param>
		/// <param name="path2">The second path.</param>
		/// <param name="path3">The third path.</param>
		/// <returns>A string containing the combined paths. If path2/path3 contains an 
		/// absolute path, this method returns path2/path3.</returns>
		public static string Combine( string path1, string path2, string path3 )
		{
			return Path.Combine( path1, Path.Combine( path2, path3 ) );
		}

		/// <summary>
		/// Combines any-number of path strings.
		/// </summary>
		/// <param name="paths">Collection of paths.</param>
		/// <returns>A string containing the combined paths. If an N-th item is
		/// an absolute path, then all previous path items will be ignored, where
		/// as all following (relative) path items will be combined.</returns>
		public static string Combine( params string[] paths )
		{
			if ( paths == null )
				throw new ArgumentNullException( "paths" );

			if ( paths.Length == 0 )
				throw new ArgumentException( "paths" );


			int length = paths.Length;

			if ( length == 1 )
				return paths[ 0 ];
			else if ( length == 2 )
				return Path.Combine( paths[ 0 ], paths[ 1 ] );

			string accPath = Path.Combine( paths[ length-2 ], paths[ length-1 ] );

			for ( int i=length-3; i>-1; i-- )
				accPath = Path.Combine( paths[ i ], accPath	);

			return accPath;
		}

		#endregion
	
		#region Relative
		
		/// <summary>
		/// Returns the relative path required in order to navigate from the
		/// source directory to the desgination directory.
		/// </summary>
		/// <param name="from">Source directory.</param>
		/// <param name="to">Destination directory.</param>
		/// <returns>Relative path between source and destination directories. If they
		/// do not share a common path, a null string will be returned.</returns>
		public static string Relative( string from, string to )
		{
			#region Validations

			if ( from == null )
				throw new ArgumentNullException( "from" );

			if ( to == null )
				throw new ArgumentNullException( "to" );

			#endregion

			DirectoryInfo fromInfo = new DirectoryInfo( from );
			DirectoryInfo toInfo = new DirectoryInfo( to );

			return Relative( fromInfo, toInfo );
		}


		/// <summary>
		/// Returns the relative path required in order to navigate from the
		/// source directory to the desgination directory.
		/// </summary>
		/// <param name="fromInfo">Source directory.</param>
		/// <param name="toInfo">Destination directory.</param>
		/// <returns>Relative path between source and destination directories. If they
		/// do not share a common path, a null string will be returned.</returns>
		public static string Relative( DirectoryInfo fromInfo, DirectoryInfo toInfo )
		{
			#region Validations

			if ( fromInfo == null )
				throw new ArgumentNullException( "fromInfo" );

			if ( toInfo == null )
				throw new ArgumentNullException( "toInfo" );

			#endregion


			// If the two directories do _NOT_ share the same root, then they're
			// on different "namespaces" and are therefore not "comparable".
			if ( fromInfo.Root.FullName != toInfo.Root.FullName )
				return null;


			/*
			 * Create a directory stack for both the From and To paths,
			 * along the ancestor-and-self::* axis. Please note that since
			 * we're navigating from the folder up, the items will be in
			 * reverse order in the stack :D
			 */
			#region Directory Stacks

			Stack fromStack = new Stack();
			Stack toStack = new Stack();

			DirectoryInfo iter;

			iter = fromInfo;
			do
			{
				fromStack.Push( iter );
				iter = iter.Parent;
			} while ( iter != null );


			iter = toInfo;
			do
			{
				toStack.Push( iter );
				iter = iter.Parent;
			} while ( iter != null );

			#endregion


			/*
			 * Pop() items from both stacks if the peek'ed element points
			 * to the same directory. Please note that you have to compare
			 * the .FullName, comparing object instances is moot.
			 */
			#region Discard Common Path

			while ( true )
			{
				if ( toStack.Count == 0 || fromStack.Count == 0 )
					break;

				DirectoryInfo fpeek = (DirectoryInfo) fromStack.Peek();
				DirectoryInfo tpeek = (DirectoryInfo) toStack.Peek();

				if ( tpeek.FullName != fpeek.FullName )
					break;

				fromStack.Pop();
				toStack.Pop();
			}

			#endregion


			/*
			 * 
			 */
			#region Build Relative Path

			StringBuilder sb = new StringBuilder();

			// If From path is directly along the ancestor axis of To, then
			// include the ./ sequence.
			if ( fromStack.Count == 0 )
			{
				sb.Append( "." );
				sb.Append( Path.DirectorySeparatorChar );
			}

			// Descent from From path down to the common path...
			while ( fromStack.Count > 0 )
			{
				iter = (DirectoryInfo) fromStack.Pop();
				sb.Append( ".." );
				sb.Append( Path.DirectorySeparatorChar );
			}

			// And ascend into To path!
			while ( toStack.Count > 0 )
			{
				iter = (DirectoryInfo) toStack.Pop();
				sb.Append( iter.Name );
				sb.Append( Path.DirectorySeparatorChar );
			}

			#endregion

			return sb.ToString();
		}

		#endregion
	}
}
