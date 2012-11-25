using System;
using System.Windows.Forms;
using System.Xml;
using Genghis.Windows.Forms;

namespace Midgard.XmlForms
{
	/// <summary>
	/// 
	/// </summary>
	public class XmlTabPage
	{
		#region Private Members

		private TabPage _tab;

		#endregion


		/// <summary>
		/// Initializes a new instance of the XmlTabPage class.
		/// </summary>
		/// <param name="definition">XML definition.</param>
		internal XmlTabPage( XmlElement definition)
		{
			string name = definition.Attributes[ "name" ].Value;
			string tooltip = definition.Attributes[ "tooltip" ].Value;

			_tab = new TabPage();
			_tab.Text = name;
			_tab.ToolTipText = tooltip;
			_tab.Tag = this;
			_tab.DockPadding.All = Property.TabPadding;
            XmlAttribute _modesAttr = null;

			_invisibleElem = (XmlElement) definition.SelectSingleNode( "invisible", FormsNamespace.NamespaceManager );
			if(_invisibleElem != null)
			{
				_modesAttr = _invisibleElem.Attributes[ "modes" ];
				if(_modesAttr != null)
					_invisibleModes = _modesAttr.Value.Split( ',' );
				//Isto tem de estar aqui para não estourar mais à frente por "No registered property handler"
				definition.RemoveChild(definition.SelectSingleNode( "invisible", FormsNamespace.NamespaceManager ));
			}
            _disabledElem = (XmlElement)definition.SelectSingleNode("disabled", FormsNamespace.NamespaceManager);
             if (_disabledElem != null)
             {
                _modesAttr = _disabledElem.Attributes["modes"];
                if (_modesAttr != null)
                   _disabledModes = _modesAttr.Value.Split(',');
                //Isto tem de estar aqui para não estourar mais à frente por "No registered property handler"
                definition.RemoveChild(definition.SelectSingleNode("disabled", FormsNamespace.NamespaceManager));
             }

		}
		private XmlElement _invisibleElem = null;
        private XmlElement _disabledElem = null;
		private string[] _invisibleModes = null;
        private string[] _disabledModes = null;

		/// <summary>
		/// Gets the name of the current tab.
		/// </summary>
		public string Name
		{
			get { return _tab.Name; }
		}

      /// <summary>
      /// Verifies if a tab is invisible in the current mode
      /// </summary>
      /// <param name="mode"></param>
      /// <returns></returns>
      public bool IsInvisibleInMode(string mode)
      {
         if (_invisibleElem == null)
            return false;
         if (_invisibleModes == null)
            return true;
         int index = Array.IndexOf(_invisibleModes, mode);
         return (index > -1);
      }

      /// <summary>
      /// Verifies if a tab is disabled in the current mode
      /// </summary>
      /// <param name="mode"></param>
      /// <returns></returns>
      public bool IsDisabledInMode(string mode)
      {
         if (_disabledElem == null)
            return false;
         if (_disabledModes == null)
            return true;
         int index = Array.IndexOf(_disabledModes, mode);
         return (index > -1);
      }

      /// <summary>
		/// Gets the WinForm tab page.
		/// </summary>
		public TabPage TabPage
		{
			get { return _tab; }
		}


		/// <summary>
		/// Gets the collection of properties contained in the tab.
		/// </summary>
		/// <remarks>
		/// Please note that each property is defined as a single <see cref="Panel"/> containing
		/// any number of specific controls.
		/// </remarks>
		public Control.ControlCollection Properties
		{
			get
			{
				return TabPage.Controls;
			}
		}


		/// <summary>
		/// Places focus on the first control.
		/// </summary>
		/// <remarks>
		/// The first property is the last panel (see remarks in <see cref="ReLayout"/>).
		/// Navigate to that panel and then select the control in that container which
		/// has the lowest TabIndex.
		/// </remarks>
		public void FocusFirst()
		{
			if ( _tab.Controls.Count == 0 )
				return;

			Panel p = (Panel) _tab.Controls[ _tab.Controls.Count-1 ];

			Control c = FormUtils.LowestTabIndex( p );

			if ( c != null )
				c.Focus();
		}
		

		/// <summary>
		/// Performs re-layout of all the panels on the tab sheet.
		/// </summary>
		/// <remarks>
		/// This is necessary because the panels have been placed in the same
		/// order as the underlying properties have been specified. However, since
		/// they are being docked Top, that order is visually reversed. Hence,
		/// reverse it and they appear as specified.
		/// </remarks>
		public void ReLayout()
		{
			if ( _tab.Controls.Count == 0 )
				return;

			Control[] c = new Control[ _tab.Controls.Count ];

			for ( int i=0; i<c.Length; i++ )
				c[ i ] = _tab.Controls[ i ];

			for ( int j=0; j<c.Length; j++ )
				c[ j ].BringToFront();
		}


		/// <summary>
		/// Adds a control/property to the current tab.
		/// </summary>
		/// <param name="control">Control, created by the factory. :-)</param>
		public void AddProperty( Control control )
		{
			Panel p = new Panel();
			p.Controls.Add( control );
			p.Dock = DockStyle.Top;
			p.Height = control.Height;
           

 			//ContainerValidator cv = new ContainerValidator();
			//cv.ContainerToValidate = p;
			//p.Tag = cv;

			_tab.Controls.Add( p );
		}

	}
}
