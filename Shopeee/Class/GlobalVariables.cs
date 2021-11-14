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

        public static readonly string FacebookPageID = "293533752446566";

        public static readonly string FacebookPageToken = "EAAMs4FMOuNYBAKZCKfCjCC745elppcL0ScY9iIDVv5HlMUWnKTcZA9ZCud1MnaUVZCx3JlJZAKSfOjVOZBvymvtZCyFgcneCwGQTSdZCnlg6mGIwURBiAN247cEtkSbpGnUdgUdiz3W1gMJpGUBlemYrCK1Ukb1ZAOHMhZBmA4FJ3S7Upe3FJQZAg37";

        public static readonly string FacebookAPI = "https://graph.facebook.com/";

        public static readonly string googleApiKey = "AIzaSyByIhhF4FzYO5yVwEJxAEfeEnWNd_EQ1FA";

        public static readonly string ExchangeRateAPI = "https://v6.exchangerate-api.com/v6/";

        public static readonly string ExchangeRateKey = "b0a24c542f3bc9d27f465bfa";

        public static readonly string baseRate = "USD";

        public static string currentRate = "USD";
    }
}