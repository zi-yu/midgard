using System;
using System.Collections;

namespace  Midgard.Interop
{
	#region Class GroupElement
	/// <summary>
	/// Summary description for GroupElement.
	/// </summary>
	public class GroupElement
	{
		/// <summary>
		/// 
		/// </summary>
		private string _group;
		/// <summary>
		/// 
		/// </summary>
		private string[] _element;
		/// <summary>
		/// 
		/// </summary>
		private int _count;
		/// <summary>
		/// 
		/// </summary>
		private int _expectedCount;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Group"></param>
		public GroupElement(string Group)
		{
			this._group=Group;
			this._element=this._group.Split('|');
			AddCount(1);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Count"></param>
        public void AddCount(int Count)
        {
        	this._count+=Count;
            if((this._count%this._element.Length)==0)
            {
            	this._expectedCount=this._count/this._element.Length;
            }
			else
            {
				this._expectedCount=-1;
            }
        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Count"></param>
		public void ReleaseCount(int Count)
		{
			this._count-=Count;
		}

		/// <summary>
		/// 
		/// </summary>
		public string[] Element
		{
			get { return this._element; }
			set { this._element=value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int Count
		{
			get { return this._count; }
		}

		/// <summary>
		/// 
		/// </summary>
		public int ExpectedCount
		{
			get { return this._expectedCount; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this._group;
		}

	}
	#endregion

	#region Class Element
	/// <summary>
	/// 
	/// </summary>
	public class Element
	{
		/// <summary>
		/// 
		/// </summary>
        private string _name;

		/// <summary>
		/// 
		/// </summary>
		private int _count;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Name"></param>
		public Element(string Name)
		{
			this._name=Name;
			AddCount(1);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Count"></param>
		public void AddCount(int Count)
		{
            this._count+=Count;			
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Count"></param>
        public void ReleaseCount(int Count)
        {
        	this._count-=Count;
        }

		/// <summary>
		/// 
		/// </summary>
		public int Count
		{
			get { return this._count; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return this._name;
		}

	}
	#endregion

	#region Class GroupElementTable
	/// <summary>
	/// 
	/// </summary>
	public class GroupElementTable
	{
		/// <summary>
		/// 
		/// </summary>
		private ArrayList _groupElementTable;

		/// <summary>
		/// 
		/// </summary>
		public GroupElementTable()
		{
            this._groupElementTable=new ArrayList();
		}

		/// <summary>
		/// 
		/// </summary>
		public void AddGroupElement(GroupElement groupElement)
		{
			bool found=false;
			for(int i=0;i<this._groupElementTable.Count;i++)
			{
				GroupElement iterator=(GroupElement)this._groupElementTable[i];
				if(iterator.ToString()==groupElement.ToString())
				{
					iterator.AddCount(1);
                    found=true;
					i=this._groupElementTable.Count;
				}
			}
			if(found==false)
			{
				this._groupElementTable.Add(groupElement);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public ArrayList GroupElement
		{
			get { return this._groupElementTable; }
		}
	}
	#endregion

	#region Class ElementTable
	/// <summary>
	/// 
	/// </summary>
	public class ElementTable
	{
		/// <summary>
		/// 
		/// </summary>
		private ArrayList _elementTable;

		/// <summary>
		/// 
		/// </summary>
		public ElementTable()
		{
			this._elementTable=new ArrayList();
		}

		/// <summary>
		/// 
		/// </summary>
		public void AddElement(Element element)
		{
			bool found=false;
			for(int i=0;i<this._elementTable.Count;i++)
			{
				Element iterator=(Element)this._elementTable[i];
				if(iterator.ToString()==element.ToString())
				{
					iterator.AddCount(1);
					found=true;
					i=this._elementTable.Count;
				}
			}
			if(found==false)
			{
				this._elementTable.Add(element);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public ArrayList Element
		{
			get { return this._elementTable; }
		}
	}
	#endregion

	/// <summary>
	/// 
	/// </summary>
	public class ShapeGroupValidation
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="groupElementTable"></param>
		/// <param name="elementTable"></param>
		/// <param name="report"></param>
		/// <returns></returns>
        public static PageReport Validate(GroupElementTable groupElementTable,ElementTable elementTable,PageReport report)
        {
			string badElementList=string.Empty;

			for(int i=0;i<groupElementTable.GroupElement.Count;i++)
        	{
				GroupElement groupElement=(GroupElement)groupElementTable.GroupElement[i];
				int expectedCount=groupElement.ExpectedCount;

                if(expectedCount<0)
                {
					badElementList+=Environment.NewLine+"'"+groupElement.ToString()+"'";
                }
				else
                {
					for(int n=0;n<groupElement.Count;n++)
					{
						string elementName=groupElement.Element[n];
						for(int l=0;l<elementTable.Element.Count;l++)
						{
							Element element=(Element)elementTable.Element[l];
							if(element.ToString()==elementName)
							{
								element.ReleaseCount(expectedCount);
							}
						}
					}
                }
        	}
			badElementList=string.Empty;

			for(int i=0;i<elementTable.Element.Count;i++)
			{
                Element element=(Element)elementTable.Element[i];
				if(element.Count!=0)
				{
					badElementList+=Environment.NewLine+"'"+element.ToString()+"'";
				}
			}

           return report;                    	
        }
	}
}
