using System;

namespace Midgard.Interop
{
    /// <summary>
    /// Resources for Process Model.
    /// </summary>
    public sealed class ResourceTokens
    {
        /// <remarks/>
        private ResourceTokens() { }

        /// <summary>
        /// Name of the command-bar.
        /// </summary>
        public const string CommandBarName = "CommandBarName";

        /// <summary>
        /// Text for validate button.
        /// </summary>
        public const string ButtonValidate = "ButtonValidate";

        /// <summary>
        /// Tooltip for validate button.
        /// </summary>
        public const string ButtonValidateTooltip = "ButtonValidateTooltip";

        /// <summary>
        /// Text for export transition button.
        /// </summary>
        public const string ButtonExport = "ButtonExport";

        /// <summary>
        /// Tooltip for export transition button.
        /// </summary>
        public const string ButtonExportTooltip = "ButtonExportTooltip";

        /// <summary>
        /// Message shown in the Export: Please Wait.
        /// </summary>
        public const string ExportPleaseWait = "ExportPleaseWait";

        /// <summary>
        /// Settings depend on the location of the Visio file.
        /// </summary>
        public const string SettingsPleaseSave = "SettingsPleaseSave";

        /// <summary>
        /// Ok Color.
        /// </summary>
        public const string ShapeLineColorOk = "ShapeLineColorOk";

        /// <summary>
        /// Error color.
        /// </summary>
        public const string ShapeLineColorError = "ShapeLineColorError";

        /// <summary>
        /// When trying to export/validate a non-Quartz sheet.
        /// </summary>
        public const string ExportPageNotMidgard = "ExportPageNotMidgard";

        /// <summary>
        /// 
        /// </summary>
        public const string ExportPageIsValid = "ExportPageIsValid";

        /// <summary>
        /// 
        /// </summary>
        public const string ExportErrorsInPage = "ExportErrorsInPage";

        /// <summary>
        /// 
        /// </summary>
        public const string ExportShapeError = "ExportShapeError";

        /// <summary>
        /// 
        /// </summary>
        public const string ExportPageOk = "ExportPageOk";

        /// <summary>
        /// Validate Page sucessfully
        /// </summary>
        public const string ValidatePageOk = "ValidatePageOk";

         /// <summary>
        /// Validate Page sucessfully
        /// </summary>
        public const string ValidatePageOnServerOk = "ValidatePageOnServerOk";

        /// <summary>
        /// 
        /// </summary>
        public const string ExportNoPagesProcessed = "ExportNoPagesProcessed";

        /// <summary>
        /// 
        /// </summary>
        public const string ExportAllOk = "ExportAllOk";

        /// <summary>
        /// 
        /// </summary>
        public const string ExportErrorsInPages = "ExportErrorsInPages";

        /// <summary>
        /// 
        /// </summary>
        public const string ExportPageShapeError = "ExportPageShapeError";

        /// <summary>
        /// Maximum number of errors to display.
        /// </summary>
        public const string ExportErrorLimit = "ExportErrorLimit";

        /// <summary>
        /// Message to display when more errors occured.
        /// </summary>
        public const string ExportErrorLimitMessage = "ExportErrorLimitMessage";

        /// <summary>
        /// Formatter t obtain process filename. 0th index is the Process ID.
        /// </summary>
        public const string ProcessFilenameFormat = "ProcessFilenameFormat";

        /// <summary>
        /// Duplicate Process ID.
        /// </summary>
        public const string ProcessIdValidator = "ProcessIdValidator";

        /// <summary>
        /// Duplicate Node ID.
        /// </summary>
        public const string NodeIdValidator = "NodeIdValidator";

        /// <summary>
        /// Event must be connected on both ends.
        /// </summary>
        public const string EventNotConnected = "EventNotConnected";

        /// <summary>
        /// Invalid from connection.
        /// </summary>
        public const string EventConnectedFromInvalidShape = "EventConnectedFromInvalidShape";

        /// <summary>
        /// Invalid from connection, cannot be an End shape.
        /// </summary>
        public const string EventConnectedFromEndShape = "EventConnectedFromEndShape";

        /// <summary>
        /// Invalid To connection.
        /// </summary>
        public const string EventConnectedToInvalidShape = "EventConnectedToInvalidShape";

        /// <summary>
        /// Invalid To connection, cannot be a Start shape.
        /// </summary>
        public const string EventConnectedToStartShape = "EventConnectedToStartShape";

        /// <summary>
        /// Invalid from connection.
        /// </summary>
        public const string EventConnectedFromEventShape = "EventConnectedFromEventShape";

        /// <summary>
        /// Invalid To connection, cannot be an Event shape.
        /// </summary>
        public const string EventConnectedToEventShape = "EventConnectedToEventShape";

        /// <summary>
        /// No incoming connections to this shape.
        /// </summary>
        public const string NodeShapeNotConnected = "NodeShapeNotConnected";

        /// <summary>
        /// No outgoing connections from this shape.
        /// </summary>
        public const string StartShapeNotConnected = "StartShapeNotConnected";

        /// <summary>
        /// Invalid from connection.
        /// </summary>
        public const string TransitionConnectedFromInvalidShape = "TransitionConnectedFromInvalidShape";

        /// <summary>
        /// Invalid transition from connection.
        /// </summary>
        public const string TransitionConnectedFromTransitionShape = "TransitionConnectedFromTransitionShape";

        /// <summary>
        /// Invalid to connection.
        /// </summary>
        public const string TransitionConnectedToInvalidShape = "TransitionConnectedToInvalidShape";

        /// <summary>
        /// Invalid transition to connection.
        /// </summary>
        public const string TransitionConnectedToTransitionShape = "TransitionConnectedToTransitionShape";

        
        /// <summary>
        /// Invalid component code.
        /// </summary>
        public const string InvalidTransition = "InvalidTransition";

        /// <summary>
        /// Transition not connected.
        /// </summary>
        public const string TransitionNotConnected = "TransitionNotConnected";

       
        /// <summary>
        /// Invalid column count
        /// </summary>
        public const string ShapeTableColumns = "ShapeTableColumns";
        
        /// <summary>
        /// Shape do not match value
        /// </summary>
        public const string ShapeLineColorNoMatch = "ShapeLineColorNoMatch";

        /// <summary>
        /// Shape do not match value
        /// </summary>
        public const string CantMatch = "CantMatch";

        /// <summary>
        /// Stop export
        /// </summary>
        public const string ExportInterrupted = "ExportInterrupted";

        /// <summary>
        /// miss transitions Id
        /// </summary>
        public const string MissTransitionId = "MissTransitionId";

        /// <summary>
        /// No diagram drawed
        /// </summary>
        public const string NoDiagram = "NoDiagram";

        /// <summary>
        /// Continue the export action
        /// </summary>
        public const string ContinueExport = "ContinueExport";

        public const string ShapeTextMaxSize = "ShapeTextMaxSize";

        public const string ShapeTextTruncate = "ShapeTextTruncate";

    }
}

