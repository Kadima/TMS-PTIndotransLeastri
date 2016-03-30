using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;  

namespace WebApi.ServiceModel
{
    public class Auth
    {
        public ISecretKey SecretKey { get; set; }
        public bool AuthResult(string[] token, string uri)
        {
            return true;
            //if (token != null)
            //{
            //    string md5Key = "";
            //    try
            //    {
            //        string key = uri + SecretKey.strValue.Replace("-", "");
            //        byte[] result = Encoding.Default.GetBytes(key);
            //        MD5 md5 = new MD5CryptoServiceProvider();
            //        byte[] output = md5.ComputeHash(result);
            //        md5Key = BitConverter.ToString(output).Replace("-", "");
            //    }
            //    catch { throw; }
            //    if (token[0].ToString() == md5Key.ToLower()) { return true; }
            //    else { return false; }    
            //}
            //return false;
        }
    }
}
