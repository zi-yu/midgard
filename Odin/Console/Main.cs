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

using System.IO;
using Loki.Generic.Factories;
using Odin.Core;
using System;
using Loki.Interfaces;
using Loki.Generic;
using System.Collections.Generic;

namespace Odin {

	public class MainClass {

        #region Static Error Handling

        static MainClass()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine(e.ExceptionObject);
            Console.WriteLine("*** Error: Unhandled Exception");
        }

        #endregion Static Error Handling

        #region PluginManager

        private static void PluginManagerTest() {
            try {

                PluginManager<ICodeGenerator> p = new PluginManager<ICodeGenerator>(typeof(CodeGeneratorFactory), "../../../Odin/bin/Plugins/CodeGenerator");
                //PrintRegisteredPlugins(p);
                PrintFirstPlugin(p);

            } catch(Exception e) {

                Log.Error(e);

            };
        }



        private static void PrintFirstPlugin( PluginManager<ICodeGenerator> p ) {

            ICodeGenerator readmeFile = p.Get(p.Registered[0].Name);

            Console.WriteLine(readmeFile.Name);

        }



        private static void PrintRegisteredPlugins( PluginManager<ICodeGenerator> p ) {

            ICodeGenerator[] plugs = p.Registered;

            Console.WriteLine("----");

            foreach(ICodeGenerator plug in plugs) {

                Console.WriteLine("** " + plug.Name);
            }
        }

        private static void OldMain()
        {
            //PluginManagerTest();
            IProject project = new Project("Projecto Sapus", "c:/Midgard/Sapus/", "Sapus Corp", "");
            Dictionary<string, string> pluginParameter = new Dictionary<string, string>();
            pluginParameter.Add("SourcePath", Path.Combine(Globals.DataDirectory, "Readme.txt"));
            pluginParameter.Add("DestinyPath", "Readme.txt");
            project.AddPluginParameter("Odin.CopyFile", pluginParameter);
            Generator generator = new Generator(project);
            generator.Process();
        }

        #endregion

        #region OdinConsole Main

        static void Main(string[] args) {
            CommandLine options = new CommandLine();
            if( args.Length == 0 ) {
                args = DefaultArgs;
            }
            options.ProcessArgs(args);
        }

        #endregion

        #region Utilities

        public static string[] DefaultArgs {
            get {
				//return new string[] { "--to-dataset:Data/Examples/Sms.xml" };
				//return new string[] { "-l" };
				//return new string[] { "-project:D:/Documents/Games/KW/Project/KnightWorld_MySql.xml" };
				return new string[] { "-project:c:/Documents and Settings/nas/My Documents/Midgard/TestProjects/Quartz/Project/Quartz.xml" };
                //return new string[] { "--test-project" };
                //return new string[] { "--to-wiki:ListaDePlugins.wiki" };
				//return new string[] { "--help" };
            }
        }

        #endregion
    
    };

}



