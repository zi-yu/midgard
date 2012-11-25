#region Licence Statment
// Copyright (c) Zi-Yu.com - All Rights Reserved
// http://midgard.zi-yu.com/
//
// The use and distribution terms for this software are covered by the
// LGPL (http://opensource.org/licenses/lgpl-license.php).
// By using this software in any fashion, you are agreeing to be bound by
// the terms of this license.
//
// You must not remove this notice, or any other, from this software.
#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Loki.Generic;
using Loki.Exceptions;
using DesignPatterns;
using Loki.Interfaces;
using Quartz;
using Loki.DataRepresentation.Loaders;
using System.IO;

namespace Quartz {
	public class QuartzLoader {

		#region Private Fields

		private static QuartzLoader loader = new QuartzLoader();

		private Dictionary<string, ReadTags> tags = new Dictionary<string,ReadTags>();
        private Dictionary<string, InnerTags> innerTags = new Dictionary<string, InnerTags>();

		private List<Process> dataTypes = new List<Process>();
		private Dictionary<string, List<Transiction>> unresolved = new Dictionary<string, List<Transiction>>();
        
        private delegate void ReadTags( XmlNode element);
        private delegate void InnerTags(INode node, XmlNode element);

		private IProject project;
		private string path;
		
		#endregion
		
		#region Initializer

		public void Init( IProject project ) {
			this.project = project;
			this.path = Path.Combine(project.OutputPath, "XmlLibraries/Process");

			dataTypes.Clear();
			unresolved.Clear();
			
			//Primary Tags
            tags["annotation"] = new ReadTags(Ignore);
            tags["startTransitions"] = new ReadTags(Ignore);
            tags["nodeList"] = new ReadTags(ParseNodeList);
            tags["node"] = new ReadTags(ParseNodeElement);
            tags["screenNode"] = new ReadTags(ParseScreenNodeElement);
			tags["subProcess"] = new ReadTags(ParseSubProcessNodeElement);
		
			//Inner Tags
            innerTags["transitions"] = new InnerTags(ParseTransitions);
            innerTags["transition"] = new InnerTags(ParseTransition);
            innerTags["moniker"] = new InnerTags(ParseMoniker);
            innerTags["extended"] = new InnerTags(ParseExtended);

            //Screen Specific
            innerTags["screen"] = new InnerTags(ParseScreen);
            innerTags["style"] = new InnerTags(ParseStyle);
		}
		
		#endregion

		#region Properties

		public List<Process> DataTypes {
			get { return dataTypes; }
		}

		public IProject Project {
			get { return project; }
			set { project = value; }
		}

		public static QuartzLoader Instance {
			get { return loader; }
		}

		public Process CurrentProcess {
			get { return dataTypes[dataTypes.Count-1]; }
		}

		#endregion

        #region Private

		private void LoadFile(string file) {
			XmlDocument doc = new XmlDocument();
			doc.Load(file);
			
			ParseProcess(doc);
		}

        private void FillNode(INode node, XmlNode element) { 
            if( element.Attributes["ID"] != null ) {
                node.Name = element.Attributes["ID"].Value;
            }

            if( element.Attributes["label"] != null ) {
                node.Label = element.Attributes["label"].Value;
            }
        }

        private Node CreateNode(XmlNode element) {
			Node node = new Node();
			FillNode(node, element);
            return node;
        }

        private ScreenNode CreateScreenNode(XmlNode element) {
			ScreenNode screenNode = new ScreenNode();
			FillNode(screenNode, element);
			return screenNode;
        }

		private SubProcessNode CreateSubProcessNode(XmlNode element) {
			SubProcessNode subProcessNode = new SubProcessNode();
			FillNode(subProcessNode, element);
			return subProcessNode;
		}

        #endregion

		#region Parser

        #region Utilities

        private void ParseNodes( XmlNode element ) {
            string name = element.Name;
			if (!string.IsNullOrEmpty(name) && tags.ContainsKey(name)) {
				tags[name]( element );
			} else {
				Log.Warn( "Don't know how to handle element '{0}'... ignoring it.", element.Name );
			}
		}

        private void ParseInnerNodes( INode node, XmlNode element ) {
			string name = element.Name;
			if( !string.IsNullOrEmpty( name ) && innerTags.ContainsKey( name ) ) {
				innerTags[name]( node, element );
			} else {
				Log.Warn( "Don't know how to handle child element '{0}'... ignoring it.", element.Name );
			}
		}

		private void AddNode(INode node) {
			CurrentProcess.AddNode(node);
		}


		private void ResolveUnresolved() {
			foreach (string node in unresolved.Keys) {
				if (CurrentProcess.HasNode(node)) {
					INode iNode = CurrentProcess.NodeList[node];
					List<Transiction> transictions = unresolved[node];

					foreach (Transiction transiction in transictions) {
						iNode.AddTransiction(transiction);
					}
				}
			}
		}

        #endregion Utilities

        #region Primary Tags

		private void ParseProcess(XmlDocument doc) {
			string process = doc.DocumentElement.Attributes["ID"].Value;
			string label = doc.DocumentElement.Attributes["label"].Value;
			dataTypes.Add(new Process(process,label));

			foreach (XmlNode child in doc.DocumentElement) {
				if( child.NodeType == XmlNodeType.Element ) {
					ParseNodes( child );
				}
			}
		}

        /// <summary>
        /// Parse ao elemento principal nodeList
        /// </summary>
        /// <param name="element"></param>
        private void ParseNodeList( XmlNode element ) {
			foreach( XmlNode child in element.ChildNodes ) {
				if( child.NodeType == XmlNodeType.Element ) {
					ParseNodes( child );
				}
			}
		}

        /// <summary>
        /// Parse ao filho de nodeList, node
        /// </summary>
        /// <param name="element"></param>
        private void ParseNodeElement( XmlNode element ) {
            Node node = CreateNode(element);
			foreach( XmlNode child in element.ChildNodes ) {
				if( child.NodeType == XmlNodeType.Element ) {
                    ParseInnerNodes(node, child);
				}
			}
			AddNode(node);
			ResolveUnresolved();
		}

		/// <summary>
        /// Parse ao filho de nodeList, sceenNode
        /// </summary>
        /// <param name="element"></param>
        private void ParseScreenNodeElement( XmlNode element ) {
            ScreenNode node = CreateScreenNode(element);
			foreach( XmlNode child in element.ChildNodes ) {
				if( child.NodeType == XmlNodeType.Element ) {
                    ParseInnerNodes(node, child);
				}
			}
			AddNode(node);
			ResolveUnresolved();
		}


		/// <summary>
		/// Parse ao filho de nodeList, subProcess
        /// </summary>
        /// <param name="element"></param>
		private void ParseSubProcessNodeElement(XmlNode element) {
			SubProcessNode subProcessNode = CreateSubProcessNode(element);
			foreach( XmlNode child in element.ChildNodes ) {
				if( child.NodeType == XmlNodeType.Element ) {
					ParseInnerNodes(subProcessNode, child);
				}
			}
			AddNode(subProcessNode);
			ResolveUnresolved();
		}

        private void Ignore(XmlNode element) { }

		#endregion

		#region Inner Tags

		private void ParseTransitions( INode node, XmlNode element ) {
			foreach (XmlNode child in element.ChildNodes) {
				if (child.NodeType == XmlNodeType.Element ) {
					ParseInnerNodes(node, child);
				}
			}
        }
        
        private void ParseTransition( INode node, XmlNode element ) {
			string eventName = element.Attributes["eventName"].Value;
			string label = element.Attributes["label"].Value;
			string nodeRef = element.Attributes["ref"].Value;
			
			Transiction transiction = new Transiction(eventName, label);
			if (CurrentProcess.HasNode(nodeRef)) {
				CurrentProcess.GetNode(nodeRef).AddTransiction(transiction);
			}else{
				if (!unresolved.ContainsKey(nodeRef)) {
					unresolved[nodeRef] = new List<Transiction>();
				}
				unresolved[nodeRef].Add( transiction );
			}
        }

		private void ParseMoniker(INode node, XmlNode element) {
			node.SetMoniker(element.InnerText);
        }

		private void ParseExtended(INode node, XmlNode element) {
			if (!string.IsNullOrEmpty(element.InnerText)) {
				string[] values = element.InnerText.Split(';');

				foreach (string value in values) {
					string[] pair = value.Split('=');
					node.AddExtended(pair[0], pair[1]);
				}
			}
        }

        //Screen Specific
		private void ParseScreen(INode node, XmlNode element) {
			if (node is ScreenNode) {
				((ScreenNode)node).Screen = element.InnerText;
			}
        }

		private void ParseStyle(INode node, XmlNode element) {
			if (node is ScreenNode) {
				((ScreenNode)node).Style = element.InnerText;
			}
        }

		#endregion

		#endregion

		#region Public

		public void Load() {
			try {
				if( tags.Count == 0 && innerTags.Count == 0 ) {
					Log.Error( "Loader init is required!" );
					return;
				}

				Log.Info("Start loading path '{0}'.", path);

				string[] files = Directory.GetFiles(path, "*.qzp");
				foreach (string file in files) {
					LoadFile(file);
				}

				AddDataTypesToProject();

				Log.Info("Path '{0}' loaded successufuly!", path);
			} catch(Exception ex) {
				Log.Fatal(ex.ToString());
				throw;
			}
		}

		private void AddDataTypesToProject() {
			project.Generic.Add("Quartz", dataTypes);
		}

		#endregion

		#region Constructor

        private QuartzLoader() { }

		#endregion

	};
}