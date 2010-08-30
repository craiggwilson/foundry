using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Foundry.SourceControl.GitIntegration
{
    internal static class GitSettings
    {
        public static string ExePath
        {
            get { return ConfigurationManager.AppSettings["GitPath"]; }
        }

        public static string RepositoriesPath
        {
            get 
            {
                return ConfigurationManager.AppSettings["RepositoriesPath"];
            }
        }
    }
}