using System;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Visio;

namespace Midgard.Interop
{
	/// <summary>
	/// Summary description for VisioEvents.
	/// </summary>
	[ComVisible( false )]
	public class VisioEvent
	{
		/// <remarks>
		/// Declare visEvtAdd as a 2-byte value to avoid a run-time overflow
		/// error.
		/// </remarks>
		private const short VisEvtAdd = -32768;

		/// <summary>
		/// Gets the event code for the event marker event.
		/// </summary>
		public static readonly short ShapeEventMarker = (short) VisEventCodes.visEvtApp + (short) VisEventCodes.visEvtMarker;

		/// <summary>
		/// Gets the event code for the shape connect event.
		/// </summary>
		public static readonly short ShapeConnect = (short) ( VisEventCodes.visEvtConnect + VisEvtAdd );
		
		/// <summary>
		/// Gets the event code for the shape disconnect event.
		/// </summary>
		public static readonly short ShapeDisconnect = (short) VisEventCodes.visEvtDel + (short) VisEventCodes.visEvtConnect;

		
		/// <summary>
		/// Gets the code to obtain event marker information.
		/// </summary>
		public static readonly int IdMostRecent = (int) VisEventCodes.visEvtIdMostRecent;
	}
}
