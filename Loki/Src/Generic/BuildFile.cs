using System;
using System.Collections.Generic;
using System.Text;

namespace Loki.Generic {
	public class BuildFile {

		#region Fields

		private string name;
		private VSFileType type = VSFileType.Compile;
		private bool isSubType = false;
		private string dependentUpon = null;
		private string component = null;

		#endregion

		#region Properties

		public string Name {
			get { return name; }
			set { name = value; }
		}

		public VSFileType Type {
			get { return type; }
			set { type = value; }
		}

		public bool IsSubType {
			get { return isSubType; }
			set { isSubType = value; }
		}
		
		public string Component {
			get { return component; }
			set { component = value; }
		}

		#endregion

		#region Constructor

		public BuildFile( string name, bool isSubType, string dependentUpon, VSFileType type ) {
			Name = name;
			IsSubType = isSubType;
			Type = type;
			this.dependentUpon = dependentUpon;
		}
		
		public BuildFile( string component, string name, bool isSubType, string dependentUpon, VSFileType type ) 
				: this(name, isSubType, dependentUpon, type) {
			Component = component;
		}

		#endregion

		#region Public

		public void DependsUpon( string file ) {
			dependentUpon = file;
		}

		public override string ToString() {
			StringBuilder builder = new StringBuilder();

			builder.AppendFormat( "<{0} Include=\"{1}\" ", type.ToString(), name );

			if( !isSubType && dependentUpon == null ) {
				builder.Append("/>");
				return builder.ToString();
			} else {
				builder.Append( ">" );
				if( isSubType ) {
					builder.AppendFormat( "<SubType>ASPXCodeBehind</SubType>" );
				}

				if( dependentUpon != null ) {
					builder.AppendFormat( "<DependentUpon>{0}</DependentUpon>", dependentUpon );
				}
			}

			builder.AppendFormat( "</{0}>", type.ToString() );

			return builder.ToString();
		}

		#endregion


	}
}
