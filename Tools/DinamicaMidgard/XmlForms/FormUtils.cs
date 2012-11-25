using System;
using System.Windows.Forms;

namespace Midgard.XmlForms
{
	/// <summary>
	/// Contains utility methods for Win Forms.
	/// </summary>
	public sealed class FormUtils
	{
		/// <summary>
		/// Finds the control with the lowest tab-index in the given container.
		/// </summary>
		/// <param name="container"></param>
		/// <returns>Control with lowest tab-index, or null if no tab-stop control
		/// is found.</returns>
		public static Control LowestTabIndex( Control container )
		{
			Control first = null;

			foreach ( Control c in container.Controls )
			{
				if ( c.TabStop == false )
					continue;

				if ( first == null )
				{
					first = c;
					continue;
				}

				if ( c.TabIndex < first.TabIndex )
					first = c;
			}

			return first;
		}

	
		/// <remarks>
		/// Prevent class initialization.
		/// </remarks>
		private FormUtils() {}
	}
}
