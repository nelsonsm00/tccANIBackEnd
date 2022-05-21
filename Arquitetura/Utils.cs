using System;
using System.Text;

namespace ANI.Arquitetura
{
    public class Utils
    {

        public static string Base64Decode(string base64EncodedData)
        {
            if (base64EncodedData == null) return "";
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string Base64Encode(string base64DecodeData)
        {
            if (base64DecodeData == null) return "";
            var plainTextBytes = Encoding.UTF8.GetBytes(base64DecodeData);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}
