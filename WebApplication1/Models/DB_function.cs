using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;

namespace WebApplication1.Models
{
    public class DB_function
    {
       public static JArray getjson(string url)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url); //request請求
            req.Timeout = 10000; //request逾時時間
            req.Method = "GET"; //request方式
            HttpWebResponse respone = (HttpWebResponse)req.GetResponse(); //接收respone
            StreamReader streamReader = new StreamReader(respone.GetResponseStream(), Encoding.UTF8); //讀取respone資料
            string result = streamReader.ReadToEnd(); //讀取到最後一行
            respone.Close();
            streamReader.Close();
            JObject jsondata = JsonConvert.DeserializeObject<JObject>(result); //將資料轉為json物件
            return (JArray)jsondata["cwbopendata"]["dataset"]["locations"]["location"];
        }
    }
}