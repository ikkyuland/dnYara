using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public class Tools
    {
        private static double Rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        public static void Download(string url, string path, string data, string method = "POST")
        {
            byte[] b = null;
            if (!string.IsNullOrEmpty(data))
            {
                b = Encoding.UTF8.GetBytes(data);
            }
            Stream stream = null;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = method;

            if (method == "POST")
            {
                req.ContentType = "application/x-www-form-urlencoded";
                stream = req.GetRequestStream();
                if (b == null)
                {

                }
                else
                {
                    stream.Write(b, 0, b.Length);
                }
                stream.Dispose();
            }
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            var fs = new FileStream(path, FileMode.Create);
            res.GetResponseStream().CopyTo(fs);
            fs.Close();
        }
        public static string Truncate(string data, int length)
        {
            if (data.Length > length)
            {
                return data.Substring(0, length) + "...";
            }

            return data;

        }
        public static string Req(string url, string data, Dictionary<string, object> headerDic=null, string method = "POST")
        {
            byte[] b = null;
            Stream stream = null;
            if (!string.IsNullOrEmpty(data))
            {
                b = Encoding.UTF8.GetBytes(data);
            }

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            foreach (var kv in headerDic)
            {
                req.Headers[kv.Key] = kv.Value.ToString();
            }
            req.Method = method;
            if (method == "POST")
            {
                req.ContentType = "application/json";
                stream = req.GetRequestStream();
                if (b == null)
                {

                }
                else
                {
                    stream.Write(b, 0, b.Length);
                }
                stream.Dispose();
            }
            HttpWebResponse res = null;

            res = (HttpWebResponse)req.GetResponse();

            stream = res.GetResponseStream();
            StreamReader sr = new StreamReader(stream, Encoding.GetEncoding("utf-8"));
            string retval = sr.ReadToEnd();
            sr.Dispose();
            stream.Dispose();
            res.Close();

            return retval;
        }
        public static string MD5Encrypt(Stream stream)
        {
            MD5 md5 = MD5.Create();
            var b = md5.ComputeHash(stream);
            var sb = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
            {
                sb.Append(b[i].ToString("X2"));
            }
            return sb.ToString();
        }
        public static string MD5Encrypt(string source)
        {
            MD5 md5 = MD5.Create();
            var b= md5.ComputeHash(Encoding.UTF8.GetBytes(source));
            var sb=new StringBuilder();
            for (int i = 0; i < b.Length; i++)
            {
                sb.Append(b[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public static string GetRandomString(int length, bool useNum = true, bool useLow = true, bool useUpp = true, bool useSpe = false)
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            StringBuilder sb = new StringBuilder(); string str = "";
            if (useNum) { str += "0123456789"; }
            if (useLow) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                sb.Append(str[r.Next(0, str.Length)]);
            }
            return sb.ToString();
        }

        public static int GetRandom(int min,int max)
        {
            Random r = new Random((int)DateTime.Now.Ticks);
            return r.Next(min, max);
        }

        public static string GetGuid()
        {
            return Guid.NewGuid().ToString("N");
        }
        public static DateTime TimeStampToDateTime(double ts, int type = 0)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            if (type == 0)
            {
                dtDateTime = dtDateTime.AddSeconds(ts).ToLocalTime();
            }
            else
            {
                dtDateTime = dtDateTime.AddMilliseconds(ts).ToLocalTime();
            }
            return dtDateTime;
        }
        public static string FormatDateTime(DateTime dt,int type=0)
        {
            string dtStr = "";
            if (type == 0)
            {
                dtStr = dt.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                dtStr = dt.ToString();
            }
            return dtStr;
        }
        public static string Base64Encode(byte[] content)
        {
            return Convert.ToBase64String(content);
        }
        
        public static byte[] Base64Decode(string content)
        {
            return Convert.FromBase64String(content);
        }
        public static byte[] AesEncrypt(byte[] toEncryptArray, byte[] keyArray)
        {
            SymmetricAlgorithm des = Aes.Create();
            des.Key = keyArray;
            des.Mode = CipherMode.CBC;
            des.Padding = PaddingMode.PKCS7;
            des.IV = new byte[16];
            ICryptoTransform cTransform = des.CreateEncryptor();
            return cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
        }

        public static byte[] AesDeEncrypt(byte[] toDeEncryptArray, byte[] keyArray)
        {
            SymmetricAlgorithm des = Aes.Create();
            des.Key = keyArray;
            des.Mode = CipherMode.CBC;
            des.Padding = PaddingMode.PKCS7;
            des.IV = new byte[16];
            ICryptoTransform cTransform = des.CreateDecryptor();
            return cTransform.TransformFinalBlock(toDeEncryptArray, 0, toDeEncryptArray.Length);
        }

        public static string SizeToStr(double size)
        {
            var str = "0";
            if (size>= 1024l * 1024 * 1024 * 1024)
            {
                str=size / 1024 / 1024 / 1024 / 1024 + "TB";
            }
            else if (size>= 1024l * 1024 * 1024)
            {
                str=size / 1024 / 1024 / 1024 + "G";
            }
            else if (size>= 1024l * 1024)
            {
                str=size / 1024 / 1024 + "MB";
            }
            else if (size>= 1024l)
            {
                str=size / 1024 + "KB";
            }
            else
            {
                str=size.ToString("0");
            }
            return str;
        }

        public static string SpeedToStr(double size)
        {
            var str = "0";
            if (size >= 1000l * 1000 * 1000 * 1000)
            {
                str = size / 1000 / 1000 / 1000 / 1000 + "TBps";
            }
            else if (size >= 1000l * 1000 * 1000)
            {
                str = size / 1000 / 1000 / 1000 + "GBps";
            }
            else if (size >= 1000l * 1000)
            {
                str = size / 1000 / 1000 + "MBps";
            }
            else if (size >= 1000l)
            {
                str = size / 1000 + "KBps";
            }
            else
            {
                str = size.ToString("0");
            }
            return str;
        }

    }
}
