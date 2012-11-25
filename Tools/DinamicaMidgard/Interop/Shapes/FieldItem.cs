using System;
using System.Windows.Forms;
using System.Xml;
using VisShape = Microsoft.Office.Interop.Visio.Shape;
using Midgard.XmlForms;
 

namespace Midgard.Interop.Shapes
{
	/// <summary>
	/// Shape handler for Sub Process shapes.
	/// </summary>
	public class FieldItem : NodeShape, IShapeHandler
	{
		#region Constants
        private string labelSpanDefault = "3";

		private const int columnsXMax=12;
		private const int columnsXFactor=2;

		private const int columnsYMin=5;
		private const int columnsYMax=5;
		private const int columnsYFactor=1;
		#endregion

		/// <summary>
		/// Initializes a new instance of the SubProcessShape class.
		/// </summary>
        public FieldItem(object shape, XmlForm form, XmlElement attributes)
            : base(shape, form, attributes)
		{
		}
         
		/// <summary>
		/// Adds shape-specific wiring to the XML-based WinForm controls. For
		/// node-shapes, adds a NodeIdValidator.
		/// </summary>
		public override void Design()
		{
			base.Design();
		}

		/// <summary>
		/// Sets the shape text on a Tab shape.
		/// </summary>
		/// <param name="text">Text to show.</param>
		protected override void SetShapeText( string text )
		{
			try
			{
			    XmlNode labelSpanNode=null;
			    string labelSpanValue=labelSpanDefault;
			    int labelSpan;
			    if(base.GetProperties()!=null)
			    {
                    labelSpanNode=base.GetProperties().SelectSingleNode("//labelSpan");
				    if(labelSpanNode!=null)
					    labelSpanValue=labelSpanNode.InnerText;
				    try
				    {
					    labelSpan=int.Parse(labelSpanValue);
					    if(labelSpan<1)
                            labelSpanValue=labelSpanDefault;
				    } catch(Exception)
				    {
					     labelSpanValue=labelSpanDefault;
				    }
    				
			    }

				bool writeText=false;

				// BEGIN_HAMMER
				Control c=(Control)Form["columns"];
				if(c==null)
				{
					c=(Control)Form["items"];
					if(c!=null)
						writeText=true;
				}
				if(c==null)
					c=(Control)Form["tabs"];
				// END_HAMMER

				ListBox lb=(ListBox)c.Controls[5];
                int count=lb.Items.Count;
				int index,min,max;
                
                // Mostra por defeito 3 elementos (para o caso de ser populado por uma lov)
                int cnt = lb.Items.Count;
                if (cnt == 0) cnt = 3;

				VisioUtils.SetProperty( VisioShape, "Prop", "ValidItems", lb.Items.Count.ToString() );
				min=count+1;
				max=columnsXMax+1;
				for(index=min;index<max;index++)
				{
					VisShape center=VisioShape.Shapes[index];
					center.get_Cells("LockTextEdit").ResultIU=0;
					center.Text=string.Empty;
					center.get_Cells("LockTextEdit").ResultIU=1;
				}

				for(index=1;index<min;index++)
				{
					VisShape center=VisioShape.Shapes[index];
					center.get_Cells("LockTextEdit").ResultIU=0;
					Object obj=lb.Items[index-1];
					center.Text=obj.ToString();
					center.get_Cells("LockTextEdit").ResultIU=1;
				}

				if(writeText)
				{
					for(int i=1;i<=VisioShape.Shapes.Count;i++)
					{
						VisShape center = VisioShape.Shapes[ i ];
						int isInput=VisioUtils.GetPropertyInt(center,"Prop","Label");
						if(isInput>0)
						{
                            if (labelSpanValue != null)
                                VisioUtils.SetProperty(center, "User", "LabelLen", labelSpanValue);

							center.get_Cells( "LockTextEdit" ).ResultIU = 0;
							center.Text = text;
							center.get_Cells( "LockTextEdit" ).ResultIU = 1;
							i=VisioShape.Shapes.Count+1;
						}
					}
				}
			} catch(FormatException)
			{
                ESIMessageBox.ShowError(Resources.GetString(ResourceTokens.ShapeTableColumns));                                				
			}
		}

		/// <summary>
		/// Gets the shape text of the current shape.
		/// </summary>
		public override string Text
		{
			get
			{
				for(int i=1;i<=VisioShape.Shapes.Count;i++)
				{
					VisShape center = VisioShape.Shapes[ i ];
					int isInput=VisioUtils.GetPropertyInt(center,"Prop","Label");
					if(isInput>0)
					{
						return center.Text;
					}
				}
				return "";
			}
		}
	}
}
