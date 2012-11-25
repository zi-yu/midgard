using System;

namespace  Midgard.Interop
{
	/// <summary>
	/// Called whenever a step is started.
	/// </summary>
	public delegate void StepStartEventHandler( int currentStep, int totalSteps, string stepName );

	/// <summary>
	/// Called whenever a step is completed successfully.
	/// </summary>
	public delegate void StepEndEventHandler( string description );

    /// <summary>
	/// Called whenever an importer start drawing
	/// </summary>
	public delegate void StartDrawingEventHandler();

    /// <summary>
    /// Called whenever an importer stop drawing
    /// </summary>
    public delegate void StopDrawingEventHandler();
}
