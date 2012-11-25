using System;
using System.Xml;

namespace Midgard.XmlForms
{
	/// <summary>
	/// Describes a property.
	/// </summary>
	public sealed class Property
	{
		#region Constants

		/// <summary>
		/// Gets the Top offset for the label part of a property.
		/// </summary>
		public const int LabelTop = 3;

		/// <summary>
		/// Gets the Left offset for the label part of a property.
		/// </summary>
		public const int LabelLeft = 0;

		/// <summary>
		/// Gets the Width for the label part of a property.
		/// </summary>
		public const int LabelWidth = 100;

		
		/// <summary>
		/// Gets the Top offset for the value part of a property.
		/// </summary>
		public const int ValueTop = 0;

		/// <summary>
		/// Gets the Left offset for the value part of a property.
		/// </summary>
		public const int ValueLeft  = 104;

		/// <summary>
		/// Gets the Width for the value part of a property.
		/// </summary>
		public const int ValueWidth = 240;


		/// <summary>
		/// Gets the padding value for the Tab page.
		/// </summary>
		public const int TabPadding = 10;


		/// <summary>
		/// Gets the vertical distance that seperates every property in the
		/// same tab.
		/// </summary>
		public const int PropertyMarginBottom = 10;


		/// <summary>
		/// Gets the vertical offset from the owner control to the respective
		/// help tooltip.
		/// </summary>
		public const int HelpOffsetTop = 15;

		/// <summary>
		/// Gets the horizontal offset from the owner control to the respective
		/// help tooltip.
		/// </summary>
		public const int HelpOffsetLeft = 100;

		#endregion

		#region Private Members

		private string _type;
		private string _moniker;

		#endregion

		/// <summary>
		/// Gets the type of the property.
		/// </summary>
		public string Type
		{
			get { return _type; }
		}

		/// <summary>
		/// Gets the .NET type-moniker.
		/// </summary>
		public string Moniker
		{
			get { return _moniker; }
		}


		/// <summary>
		/// Initializes a new instance of the Property class.
		/// </summary>
		/// <param name="type">Name of property</param>
		/// <param name="moniker"></param>
		internal Property( string type, string moniker )
		{
			_type = type;
			_moniker = moniker;
		}
	}
}