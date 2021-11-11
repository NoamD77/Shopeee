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

        public static readonly string FacebookPageToken = "EAAMs4FMOuNYBAFIBqAKXC7ejoVFagGh5ZBWM2dWL6uLynBvWAxcARptit6LXiO0ynXqk2M6HJ2lUlqGuLliZBzau2CIoyKPZAVpbQIExhnhSyGZBr9nwoLPG7KK7extC6Sj26ddTWkKDKm79gAGAqAbik1KOEAE5DIrdf9oJIx1aZAZAdeE5U1FQBGwnLQQGEZD";

        public static readonly string FacebookAPI = "https://graph.facebook.com/";

        public static readonly string googleApiKey = "AIzaSyByIhhF4FzYO5yVwEJxAEfeEnWNd_EQ1FA";

        public static readonly string ExchangeRateAPI = "https://v6.exchangerate-api.com/v6/";

        public static readonly string ExchangeRateKey = "b0a24c542f3bc9d27f465bfa";

        public static readonly string baseRate = "USD";

        public static string currentRate = "USD";
    }
}