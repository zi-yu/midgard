using System;

namespace Midgard.Interop
{
	/// <summary>
	/// Enumeration of the possible Screen model shape types.
	/// </summary>
	public enum ModelShapeType
	{
        /// <summary>
        /// Start shape. Only one such shape may exist per process.
        /// </summary>
        Start,

        /// <summary>
        /// End shape, signalling the end of executing the current process.
        /// </summary>
        End,

        /// <summary>
        /// Regular node.
        /// </summary>
        Node,

        /// <summary>
        /// Transition between any of the previous nodes.
        /// </summary>
        Transition,
        
        /// <summary>
		/// normal
		/// </summary>
		normal,
		/// <summary>
		/// popUp
		/// </summary>
		popUp,
		/// <summary>
		/// panel
		/// </summary>
		panel,
		/// <summary>
		/// title
		/// </summary>
		title,
		/// <summary>
		/// tabStrip
		/// </summary>
		tabStrip,
		/// <summary>
		/// tree
		/// </summary>
		tree,
		/// <summary>
		/// dataGrid
		/// </summary> 
		dataGrid,
		/// <summary>
		/// button
		/// </summary>
		button,
		/// <summary>
        /// ItemList
		/// </summary>
        ItemList,
        /// <summary>
		/// GlobalData
		/// </summary>
        GlobalData,
        /// <summary>
        /// State
        /// </summary>
        State,
        /// <summary>
        /// Final State
        /// </summary>
        FinalState,
        /// <summary>
        /// State Dependency
        /// </summary>
        StateDependency,
        /// <summary>
        /// Alternative Transition
        /// </summary>
        AlternativeTransition,

        Screen
	}
}
