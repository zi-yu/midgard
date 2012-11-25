using System.Collections;
using Midgard.Interop;

namespace  Midgard.Interop
{
	/// <summary>
	/// Collection of errors accumulated during export/validation of a
	/// page.
	/// </summary>
	public class ShapeErrorCollection : CollectionBase
	{
		/// <summary>
		/// Initializes a new instance of the ShapeErrorCollection class.
		/// </summary>
		public ShapeErrorCollection() : base()
		{
		}


		/// <summary>
		/// Gets the item at the index position.
		/// </summary>
		public ShapeError this[ int index ]
		{
			get { return (ShapeError) this.List[ index ]; }
		}


		/// <summary>
		/// Adds a single export error, originated in the given shape.
		/// </summary>
		/// <param name="shape">Handler for the invalid shape.</param>
		/// <param name="message">Error message.</param>
		public void Add( IShapeHandler shape, string message )
		{
			ShapeError e = new ShapeError( shape, message );
			Add( e );
		}


		/// <summary>
		/// Adds an export error to the current collection.
		/// </summary>
		/// <param name="error"></param>
		public void Add( ShapeError error )
		{
			List.Add( error );
		}
	}
}
