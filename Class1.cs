using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_MS
{
    public class Class1
    {
        public static string GetRandomPassword(int length)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyz123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            string password = string.Empty;
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                int x = random.Next(1, chars.Length);

                if (!password.Contains(chars.GetValue(x).ToString()))
                {
                    password += chars.GetValue(x);
                }
                else
                {
                    i = i - 1;
                }
                
            }
            return password;
        }
    }
}
