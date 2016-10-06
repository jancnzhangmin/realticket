using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Net;
using System.IO;

namespace shishicai
{
    class PublicClass
    {
        public static List<JToken> ChongqingJson = new List<JToken>();
        public static List<JToken> XinjiangJson = new List<JToken>();
        public static int[,] chongqing_fill_lianxu;

        public static void init_trem(string code)//调用当日全部开奖记录
        {


            DateTime nowdate = new DateTime();
            string str = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            //DateTime nowdate = DateTime.Now;
            nowdate = DateTime.Now;
            HttpWebRequest request = null;
            JArray jsonstr;

            /////////////////重庆时时彩////////////////
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    request = (HttpWebRequest)WebRequest.Create("http://t.apiplus.cn/daily.do?code=" + code + "&date=" + nowdate.Year + "-" + nowdate.Month + "-" + nowdate.Day + "&format=json");
                    request.Method = "GET";
                    request.Timeout = 10000;
                    response = (HttpWebResponse)request.GetResponse();
                    i = 10;
                }
                catch { }
            }


            reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8"));
            str = reader.ReadToEnd();
            str = str.Substring(str.IndexOf('['), str.Length - str.IndexOf('[') - 1);
            jsonstr = JArray.Parse(str);

            if (code == "cqssc")
            {
                PublicClass.ChongqingJson.Clear();
            }

            for (int i = jsonstr.Count - 1; i >= 0; i--)
            {
                if (code == "cqssc")
                {
                    PublicClass.ChongqingJson.Add(jsonstr[i]);
                }
            }
            //////////////////重庆时时彩END//////////////
            int cct;
            //UI_process();
            //create_danshuang_lab();
            //create_lianxu_coor();
            //modify_last_update_time();
            //timer.Start();
          





        }





    }
}
