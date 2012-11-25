using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Office.Interop.Visio;
using Microsoft.Win32;

namespace  Midgard.Interop
{
	/// <summary>
	/// Export settings.
	/// </summary>
	public static class ExportSettings
	{	


		/// <summary>
		/// Gets or sets the export path.
		/// </summary>
        private static string _path = null;
        private static string _mode = null;

        public static string Path
        {
            get {
                if (_path == null)
                {
                    try
                    {
                        //ESITracer.Current.LogDebug("Before Registry Subkey");
                        RegistryKey regKeyAppRoot = Registry.CurrentUser.OpenSubKey("Software\\Classes\\DynamicMidgard.Connect");
                        //ESITracer.Current.LogDebug("Before get Registry value");
                        _path = (string)regKeyAppRoot.GetValue("ExportPath", "C:\\MidgardDynamic\\Export");
                        //ESITracer.Current.LogDebug("After get Registry value");
                    }
                    catch (Exception e)
                    {
                        _path = "C:\\MidgardDynamic\\Export";
                        ESIMessageBox.ShowError(e.StackTrace.ToString());
                    }
                }
                return _path;
            }
        }

	    public static string Mode
	    {
            get
            {
                if (_mode == null)
                {
                    try
                    {
                        //ESITracer.Current.LogDebug("Before Registry Subkey");
                        RegistryKey regKeyAppRoot = Registry.CurrentUser.OpenSubKey("Software\\Classes\\ProcessModel.Connect");
                        //ESITracer.Current.LogDebug("Before get Registry value");
                        _mode = (string)regKeyAppRoot.GetValue("Mode", "");
                        //ESITracer.Current.LogDebug("After get Registry value");
                    }
                    catch (Exception e)
                    {
                        _mode = "";
                        ESIMessageBox.ShowError(e.StackTrace.ToString());
                    }
                }
                return _mode;
            }
        }
        public static void SavePath(string path)
        {
            try
            {
                //ESITracer.Current.LogDebug("Save settings");
                SetPath(path);
                //ESITracer.Current.LogDebug("Set registry");
                RegistryKey regKeyAppRoot = Registry.CurrentUser.CreateSubKey("Software\\Classes\\ProcessModel.Connect");
                regKeyAppRoot.SetValue("ExportPath", _path);
                //ESITracer.Current.LogDebug("Set registry:done");
            }
            catch (Exception e)
            {
                ESIMessageBox.ShowError(e.StackTrace.ToString());
            }
        }

	    public static void SaveMode(string mode)
	    {
            try
            {
                //ESITracer.Current.LogDebug("Save Mode");
                SetMode(mode);
                RegistryKey regKeyAppRoot = Registry.CurrentUser.CreateSubKey("Software\\Classes\\ProcessModel.Connect");
                regKeyAppRoot.SetValue("Mode", _mode);
                //ESITracer.Current.LogDebug("Set mode:done");
            }
            catch (Exception e)
            {
                ESIMessageBox.ShowError(e.StackTrace.ToString());
            }
        }
	    
        public static void SetPath( string path )
        {
            _path = path;
        }
	    
        public static void SetMode(string mode)
        {
            _mode = mode;
        }
    }
}
