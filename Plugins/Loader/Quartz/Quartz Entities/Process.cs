using System;
using System.Collections.Generic;
using System.Text;

namespace Quartz {
	public class Process {
		
		#region Fields

		protected string id = string.Empty;
		protected string label = string.Empty;
		protected Dictionary<string, INode> nodeList = new Dictionary<string, INode>();

		#endregion Fields

		#region Properties

		public string Name {
			get { return id; }
			set { id = value; }
		}

		public string Label {
			get { return label; }
			set { label = value; }
		}

		public Dictionary<string, INode> NodeList {
			get { return nodeList; }
			set { nodeList = value; }
		}

		#endregion Properties

		#region Public

		public void AddNode(INode node) {
			nodeList.Add(node.Name, node);
		}

		public bool HasNode(string node) {
			return nodeList.ContainsKey(node);
		}

        public INode GetNode(string node)
        {
            return nodeList[node];
        }

        public IEnumerable<INode> GetBusinessNodeList()
        {
            List<INode> list = new List<INode>(NodeList.Values);
            return list.FindAll(delegate(INode node) { return !(node is ScreenNode) && !(node is SubProcessNode); });
        }

		#endregion Public

		#region Contructor

		public Process(string id, string label) {
			this.id = id;
			this.label = label;
		}

		#endregion Contructor

    };
}
