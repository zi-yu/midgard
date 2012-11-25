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
using System.IO;
using System.Collections.Generic;
using System.Text;
using Loki.Interfaces;

namespace Loki.Generic {

    public class Globals {

        #region Directory Properties

        public static string BaseDirectory
        {
            get {
                /*Uri uri = new Uri(typeof(Globals).Assembly.CodeBase);
                FileInfo file = new FileInfo(uri.AbsolutePath);
                Console.WriteLine("************* " + file.Directory.FullName);
                return file.Directory.FullName;*/
                return ".";
            }
        }

        public static string DataDirectory
        {
            get { return Path.Combine(BaseDirectory, "Data"); }
        }

        public static string TemplateDirectory {
            get { return Path.Combine(DataDirectory, "Templates"); }
        }

        public static string PluginDirectory
        {
            get { return Path.Combine(BaseDirectory, "Plugins"); }
        }

        public static string CodeGeneratorDirectory
        {
            get { return Path.Combine(PluginDirectory, "CodeGenerator"); }
        }

		public static string BuildGeneratorDirectory {
			get { return Path.Combine( PluginDirectory, "BuildGenerator" ); }
		}

		public static string LoadGeneratorDirectory {
			get { return Path.Combine( PluginDirectory, "Loader" ); }
		}

        #endregion

		#region Utilities

		public static string GetResource(IProject project, IPlugin plugin, string file)
		{
			string baseFileName = Path.Combine(plugin.Name, file);
			string fromProjectDir = Path.Combine(project.ProjectPath, baseFileName);
			if (File.Exists(fromProjectDir)) {
				return fromProjectDir;
			}
			
			return Path.Combine(DataDirectory, baseFileName);
		}

		public static string GetResource( IProject project, string dirName, string file ) {
			string baseFileName = Path.Combine( dirName, file );
			string fromProjectDir = Path.Combine( project.ProjectPath, baseFileName );
			if( File.Exists( fromProjectDir ) ) {
				return fromProjectDir;
			}

			return Path.Combine( DataDirectory, baseFileName );
		}

		#endregion

	};

}

