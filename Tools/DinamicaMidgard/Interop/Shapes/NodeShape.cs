/******************************************************************************
* $Workfile: NodeShape.cs $
*
* $Revision: 1.1 $
* $Author: pre $
*
* -----------------------------------------------------------------------------
* $History: NodeShape.cs $
* 
* *****************  Version 1  *****************
* User: Tms          Date: 5-12-06    Time: 14:22
* Created in $/dev/EditorDinamica/Tranquilidade/Shapes
* Última versão do Editor Dinâmica.
* 
* *****************  Version 1  *****************
* User: Tms          Date: 3-11-06    Time: 18:44
* Created in $/dev/EditorDinamica/Tranquilidade/Shapes
 * 
 * *****************  Version 3  *****************
 * User: Xiagpf       Date: 26-05-06   Time: 20:53
 * Updated in $/WK/FerramentasProdutividade/BaseLine/QuartzModeler/SRC/Tranquilidade/Shapes
 * 
 * *****************  Version 2  *****************
 * User: Xiagpf       Date: 16-05-06   Time: 9:06
 * Updated in $/WK/FerramentasProdutividade/BaseLine/QuartzModeler/SRC/Tranquilidade/Shapes
 * 
 * *****************  Version 1  *****************
 * User: Xiagpf       Date: 16-05-06   Time: 8:26
 * Created in $/WK/FerramentasProdutividade/BaseLine/QuartzModeler/SRC/Tranquilidade/Shapes
 * Reorganização
 * 
 * *****************  Version 1  *****************
 * User: Xiagpf       Date: 14-11-05   Time: 17:01
 * Created in $/DSV/FerramentasProdutividade/QuartzModeler/Main/ClassLibrary/ProcessModel/Shapes
* 
* *****************  Version 1  *****************
* User: Lft          Date: 5-09-05    Time: 16:28
* Created in $/dev/ProcessModeler/ProcessModel/Shapes
* 
******************************************************************************/
using System;
using System.Windows.Forms;
using System.Xml;
using Genghis.Windows.Forms;
using VisShape = Microsoft.Office.Interop.Visio.Shape;
using Midgard.XmlForms;
 

namespace Midgard.Interop.Shapes
{
	/// <summary>
	/// Shape handler for regular shapes.
	/// </summary>
	public class NodeShape : RegularShape, IShapeHandler
	{
		#region NodeIdValidator

		/// <summary>
		/// Validates the uniqueness of a Node ID across a single process.
		/// </summary>
		public class NodeIdValidator : BaseValidator
		{
			private VisShape _shape;

			/// <summary>
			/// Initializes a new instance of the NodeIdValidator class.
			/// </summary>
			/// <param name="shape">Shape being validated.</param>
			public NodeIdValidator( VisShape shape ) : base()
			{
				_shape = shape;
			}

			/// <summary>
			/// Gets wether the control is valid.
			/// </summary>
			/// <returns>True if valid, false otherwise.</returns>
			protected override bool EvaluateIsValid()
			{
				string nodeId = ControlToValidate.Text.Trim();

				// Empty value is a valid value. If the value must be specified,
				// use a required validate.
				if ( nodeId.Length == 0 )
					return true;

				// TODO: check node id stuff

				return true;
			}
		}

		#endregion


		/// <summary>
		/// Initializes a new instance of the NodeShape class.
		/// </summary>
		public NodeShape( object shape, XmlForm form, XmlElement attributes ) : base( shape, form, attributes )
		{
		}


		/// <summary>
		/// Adds shape-specific wiring to the XML-based WinForm controls. For
		/// node-shapes, adds a NodeIdValidator.
		/// </summary>
		public override void Design()
		{
			base.Design();


			Control c = (Control) Form[ "id" ];

			NodeIdValidator p = new NodeIdValidator( VisioShape );
			p.ControlToValidate = c.Controls[ 0 ];
			p.ErrorMessage = Resources.GetString( ResourceTokens.NodeIdValidator );
			p.IconPadding = 2;
		}
	}
}
