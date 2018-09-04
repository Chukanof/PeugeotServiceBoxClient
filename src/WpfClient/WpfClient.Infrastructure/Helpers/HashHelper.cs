using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Infrastructure.Helpers
{
    public static class HashHelper
    {
        public static string CalculateMD5Hash(string input)
        {
            //  calculate MD5 hash from input

            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);

                byte[] hash = md5.ComputeHash(inputBytes);

                // convert byte array to hex string

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hash.Length; i++)

                {

                    sb.Append(hash[i].ToString("X2"));

                }

                return sb.ToString();
            }
        }

    }
}
