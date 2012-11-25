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
using System.Collections;
using log4net;
using log4net.Core;
using log4net.Config;
using log4net.Repository;
using log4net.Appender;
using log4net.Filter;

namespace Loki.Generic {

	public class Log {

		#region Log
 
 		private static ILog log = LogManager.GetLogger("Loki");

		public static ILog Log4Net {
            get { return log; }
			set { log = value; }
		}

        public static ILoggerRepository Repository {
            get { return Log4Net.Logger.Repository; }
        }

		#endregion

		#region Static

        static Log() {
            ILoggerRepository rep = LogManager.GetRepository(typeof(Log).Assembly);
            BasicConfigurator.Configure(rep, new AdvancedAppender());
            ToNormalLevel();
            Debug("Log System Initialized");
        }

		public static void Debug(object message) {
			log.Debug(message);
		}

		public static void Debug(IList list) {
			foreach (object obj in list) {
				Debug(obj);
			}
		}

		public static void Debug(string message, params object[] param) {
			log.Debug(string.Format(message,param));
		}

    	public static void Info(object message) {
            log.Info(message);
		}
        
		public static void Info(string message, params object[] param) {
            log.Info(string.Format(message,param));
		}
        
		public static void Warn(object message) {
			log.Warn(message);
		}

		public static void Warn(string message, params object[] param) {
			log.Warn(string.Format(message,param));
        }
        
		public static void Error(object message) {
            log.Error(message);
		}

        public static void Error(string message, params object[] param) {
			log.Error(string.Format(message,param));
		}

		public static void Fatal(object message) {
			log.Fatal(message);
		}

		public static void Fatal(string message, params object[] param) {
			log.Error(string.Format(message,param));
		}

		public static bool IsDebugEnabled {
			get { return log.IsDebugEnabled; }
		}

		public static bool IsInfoEnabled {
			get { return log.IsInfoEnabled; }
		}

		public static bool IsWarnEnabled {
			get { return log.IsWarnEnabled; }
		}

		public static bool IsErrorEnabled {
            get { return log.IsErrorEnabled; }
		}

		public static bool IsFatalEnabled {
			get { return log.IsFatalEnabled; }
		}

		public static ILogger Logger {
			get { return log.Logger; }
		}

		#endregion

        #region Utilities

        public static void ToDebugLevel()
        {
            Repository.Threshold = Level.Verbose; 
        }

        public static void ToWarnLevel()
        {
            Repository.Threshold = Level.Info;
        }

        public static void ToNormalLevel()
        {
            Repository.Threshold = Level.Fatal;
        }

        #endregion

    };

    internal class AdvancedAppender  : IAppender {

        #region Fields

        private string name = "AdvancedConsoleAppender";

        #endregion

        #region IAppender Members

        public void Close() {

        }
        
        public void DoAppend( LoggingEvent e ) {
            if( e.Repository.Threshold.Value > e.Level.Value ) {
                return;
            }

#if PRETTY_CONSOLE_LOG
            ConsoleColor color = ConsoleColor.White;
            ConsoleColor original = Console.ForegroundColor;
            switch(e.Level.Name) {
                case "FATAL": color = ConsoleColor.Red; break;
                case "ERROR": color = ConsoleColor.DarkYellow; break;
                case "WARN": color = ConsoleColor.Yellow; break;
                case "INFO": color = ConsoleColor.Cyan; break;
                default: color = ConsoleColor.White; break;
            }
            Console.ForegroundColor = color;
#endif
            Console.WriteLine(e.MessageObject);

#if PRETTY_CONSOLE_LOG
            Console.ForegroundColor = original;
#endif
        }

        public string Name {
            get {
                return name;
            }
            set {
                name = value;
            }
        }

        #endregion

    };

}
