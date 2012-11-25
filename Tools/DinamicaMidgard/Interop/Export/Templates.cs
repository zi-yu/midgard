using System;
using System.Collections.Generic;
using System.Text;
using NVelocity.App;
using NVelocity;
using System.IO;
using NVelocity.Exception;
using System.Diagnostics;
using System.Reflection;

namespace Midgard.Interop.Export
{
    public class Templates
    {

        public static void Generate(string templateFile, string outputFile, Dictionary<string, object> param, bool toAppend)
        {
            Generate(null, templateFile, outputFile, param, toAppend); 
        }

        public static void Generate(string propertiesFile, string templateFile, string outputFile, Dictionary<string, object> param, bool toAppend)
        {
            if (!string.IsNullOrEmpty(propertiesFile))
            {
                Velocity.Init(propertiesFile);
            }
            else
            {
                Velocity.Init();
            }

            VelocityContext context = GetContextFromDictionary(param);
            Template template = null;

            string file = templateFile;

            try
            {
                template = Velocity.GetTemplate(file);
                if (!toAppend)
                {
                    using (TextWriter stream = GetOutputStream(outputFile))
                    {
                        template.Merge(context, stream);
                    }
                }
                else
                {
                    using (TextWriter stream = GetOutputStreamAppend(outputFile))
                    {
                        template.Merge(context, stream);
                    }
                }
            }
            catch (ResourceNotFoundException rfe)
            {
                throw rfe;
            }
            catch (ParseErrorException pee)
            {
                throw pee;
            }
            catch (NotSupportedException nse)
            {
                throw nse;
            }
        }

        private static TextWriter GetOutputStream(string outputFile)
        {
            if (outputFile == null)
            {
                return new StringWriter();
            }
            return new StreamWriter(outputFile);
        }

        private static TextWriter GetOutputStreamAppend(string outputFile)
        {
            if (outputFile == null)
            {
                return new StringWriter();
            }
            return new StreamWriter(outputFile, true);
        }

        private static VelocityContext GetContextFromDictionary(Dictionary<string, object> param)
        {
            VelocityContext context = new VelocityContext();
            foreach (KeyValuePair<string, object> pair in param)
            {
                context.Put(pair.Key, pair.Value);
            }
            return context;
        } 
    }
}
