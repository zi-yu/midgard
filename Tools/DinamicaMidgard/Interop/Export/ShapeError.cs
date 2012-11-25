using System;
using Midgard.Interop;

namespace  Midgard.Interop
{
	/// <summary>
	/// Describes a single shape export/validation error.
	/// </summary>
	public class ShapeError
	{
		#region ExportError

		private IShapeHandler _shapeHandler;
		private string _message;

		#endregion

		/// <summary>
		/// Initializes a new instance of the ShapeError class.
		/// </summary>
		/// <param name="shapeHandler">Handler class for shape.</param>
		/// <param name="message">Error message.</param>
		public ShapeError( IShapeHandler shapeHandler, string message )
		{
			_shapeHandler = shapeHandler;
			_message = message;
		}

        public ShapeError(string message)
        {
            _shapeHandler = null;
            _message = message;
        }

		/// <summary>
		/// Gets the shape handler for the shape that has an error.
		/// </summary>
		public IShapeHandler ShapeHandler
		{
			get { return _shapeHandler; }
		}

		/// <summary>
		/// 
		/// </summary>
		public string ErrorText
		{
			get	
			{
				if(_shapeHandler==null)
					return string.Empty;
				else
					return _shapeHandler.Text;
			}
		}

		/// <summary>
		/// Gets the error message.
		/// </summary>
		public string Message
		{
			get { return _message; }
		}
	}
}
