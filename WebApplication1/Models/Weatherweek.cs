using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Weatherweek
    {
        public string 地點 { get; set; }
        public List<Weatherweek_detail> Weatherweek_detail { get; set; }

    }
    public class Weatherweek_detail
    {
        public string 時間 { get; set; }
        public string 時間結 { get; set; }
        public string 平均溫度 { get; set; }
        public string 平均露點溫度 { get; set; }
        public string 平均相對濕度 { get; set; }
        public string 最高溫度 { get; set; }
        public string 最低溫度 { get; set; }
        public string 最高體感溫度 { get; set; }
        public string 最低體感溫度 { get; set; }
        public string 最大舒適度指數 { get; set; }
        public string 最小舒適度指數 { get; set; }
        public string 十二小時降雨機率 { get; set; }
        public string 風向 { get; set; }
        public string 最大風速 { get; set; }
        public string 天氣現象 { get; set; }
        public string 紫外線指數 { get; set; }
        public string 天氣預報綜合描述 { get; set; }
    }
}