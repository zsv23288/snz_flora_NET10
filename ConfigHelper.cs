using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Menu_14
{
    public static class ConfigHelper
    {
        public static string GetCatSubCutFotosPath()
        {
            return ConfigurationManager.AppSettings["catSubCutFotos"] ?? string.Empty;
        }
    }
}
