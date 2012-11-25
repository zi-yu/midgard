using System;
using System.Xml;

namespace Midgard.XmlForms
{
	/// <summary>
	/// Summary description for ListItemsObject.
	/// </summary>
	public class ListItemsObject
	{
		/// <summary>
		/// 
		/// </summary>
		private XmlElement _column;
        private string _childPattern = null;
        private string _complexLabel = null;
        private string _complexPattern = null;
		/// <summary>
		/// 
		/// </summary>
		public ListItemsObject()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public ListItemsObject(XmlElement ele)
		{
            this._column = ele;
		}

        /// <summary>
        /// 
        /// </summary>
        public ListItemsObject(XmlElement ele, string complexLabel, string complexPattern, string childPattern)
        {
            this._column = ele;
            this._complexLabel = complexLabel;
            this._complexPattern = complexPattern;
            this._childPattern = childPattern;
        }

        /// <summary>
        /// 
        /// </summary>
        public ListItemsObject(XmlElement ele, string complexLabel, string complexPattern)
        {
            this._column = ele;
            this._complexLabel = complexLabel;
            this._complexPattern = complexPattern;
        }

        public XmlElement Column
        {
            get
            {
                return _column;
            }
        }
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
            string s = "";
            if (_complexLabel == null)
            {
                if (_column.SelectSingleNode("label") == null)
                    s = _column.GetAttribute("label");
                else
                    s = _column.SelectSingleNode("label").InnerText;
            }
            else
            {
                //Construct label width value(s) of attribute complexLabel
                string[] fields = (_complexLabel.Replace(" ","")).Split(',');
                string[] fieldsValues = new string[fields.Length];

                if (null == _childPattern)
                {
                    for (int i = 0; i < fields.Length; ++i)
                    {
                        if ((fields[i] == null) || (fields[i] == "")) continue;
                        XmlNode node = null;
                        if ((node = _column.SelectSingleNode(fields[i])) != null)
                        {
                            fieldsValues[i] = node.InnerText;
                        }
                    }
                    s = string.Format(_complexPattern, fieldsValues);
                }
                else
                {
                    int parentVars = 0;
                    for (int i = 0; i < fields.Length; ++i)
                    {
                        string toFind = "{" + i + "}";
                        if(_complexPattern.Contains(toFind))
                            ++parentVars; 
                    }

                    for (int i = 0; i < parentVars; ++i)
                    {
                        if ((fields[i] == null) || (fields[i] == "")) continue;
                        XmlNode node = null;
                        if ((node = _column.SelectSingleNode(fields[i])) != null)
                        {
                            fieldsValues[i] = node.InnerText;
                        }
                    }
                    s = string.Format(_complexPattern, fieldsValues);
                    string childs = string.Empty;
                    for (int i = 0; i < _column.GetElementsByTagName(fields[parentVars]).Count; ++i)
                    {
                        for (int j = parentVars; j < fields.Length; ++j)
                        {
                            fieldsValues[j] = _column.GetElementsByTagName(fields[j])[i].InnerText;
                        }

                        childs += string.Format(_childPattern, fieldsValues);
                    }
                    s += childs;
                    
                }
            }
            return s;
		}
	}
}
