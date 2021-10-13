using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopeee.Class
{
    public static class GlobalVariables
    {
        public static readonly string webImagesPath = "https://unthinkable-surface.000webhostapp.com/images/";

        public static readonly string ftpImagesPath = "ftp://files.000webhost.com/public_html/images/";

        public static readonly string ftpServerUsername = "unthinkable-surface";

        public static readonly string ftpServerPassword = "Aa123456";

        public static readonly string[] permittedExtensions = { ".jpg", ".jpeg", ".svg", ".webp", ".bmp" };
    }
}
