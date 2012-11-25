using System;

namespace Midgard.Interop
{
	/// <summary>
	/// Describes a display settings set.
	/// </summary>
	public class DisplaySettings : ICloneable
	{
		/// <summary>
		/// Vertical position of frame.
		/// </summary>
		public int Top;
		
		/// <summary>
		/// Horizontal position of frame.
		/// </summary>
		public int Left;

		/// <summary>
		/// Name of currently displayed tab.
		/// </summary>
		public string TabName;
		
		#region ICloneable Members


		/// <summary>
		/// Creates a new object that is a copy of the current instance.   
		/// </summary>
		/// <returns>A new object that is a copy of this instance.</returns>
		public object Clone()
		{
			DisplaySettings ds = new DisplaySettings();
			ds.Top = this.Top;
			ds.Left = this.Left;
			ds.TabName = this.TabName;

			return ds;
		}

		#endregion
	}
}
