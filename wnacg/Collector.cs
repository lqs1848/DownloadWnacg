using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace wnacg
{
    class Collector
    {
        int startPage = -1;
        int endPage = -1;

        String basePath = "https://www.wnacg.org";
        //9漫画汉化 10短篇汉化
        String collectorPath = "/albums-index-page-{0}-cate-{1}.html";
        String downloadPath = "/download-index-aid-{0}.html";

        int bzType;

        public EventHandler<String> CollectorLog;  
        public EventHandler<String> DownloadList;

        SynchronizationContext _syncContext;
        HttpClient client;
       
        public Collector(SynchronizationContext formContext,int startPage,int endPage,int bzType) {
            this._syncContext = formContext;
            this.startPage = startPage;
            this.endPage = endPage;
            this.bzType = bzType;
       }


        public void Start() {
            client = new HttpClient();
            new Thread(Collect).Start();
        }

        public void Collect() {
            for(int curPage= startPage; curPage <= endPage; curPage++) { 
                _syncContext.Post(OutLog, "分析页面 page:"+curPage);
                
                string listUrl = basePath + String.Format(collectorPath, curPage,bzType);
                string listResult = client.GetStringAsync(listUrl).Result;
                Regex rgx = new Regex(@"<li class=""li gallary_item"">\s*?<div class=""pic_box"">\s*?<a href=""/photos-index-aid-(?<mgid>\d+).html""\s*title=""(?<title>.*?)""><img alt="".*?"" src=""(?<img>.*?)""");
                int bzIndex = 1;
                foreach (Match mch in rgx.Matches(listResult))
                {
                    bzIndex++;
                    string mgid = mch.Groups["mgid"].Value;
                    string title = mch.Groups["title"].Value;
                    string img = mch.Groups["img"].Value;
                    //获得下载地址
                    string downPage = client.GetStringAsync(basePath+ String.Format(downloadPath, mgid)).Result;
                    string dwUrl = new Regex(@"<a class=""down_btn"" href=""(?<url>.*?)"" target=""_blank""><span>&nbsp;本地下載一</span></a>").Match(downPage).Groups["url"].Value;

                    _syncContext.Post(OutLog, "提取 \r" + title +"");
                    
                    ExeLog.WriteLog("downloadUrl_zip.txt", dwUrl+"\\"+title+".zip\r\n");
                    _syncContext.Post(AddDwList, dwUrl + "\\" + title + ".zip\r\n");

                    ExeLog.WriteLog("downloadUrl_jpg.txt", basePath + img + "\\" + title + ".jpg\r\n");
                    Thread.Sleep(100);
                }//foreach
                if (bzIndex != 12)
                {
                    ExeLog.WriteLog("当前页面本子数量缺少:" + bzIndex + "/12\r\n页面:" + listUrl + "内容为:\r\n" + listResult);
                }
                    
                    
            }//for
            _syncContext.Post(OutLog, "任务完成");
        }//method 

        private void OutLog(object state)
        {
            //ExeLog.WriteLog("exelog.txt", state.ToString()+"\r\n");
            CollectorLog?.Invoke(this, state.ToString());
        }

        private void AddDwList(object state)
        {
            DownloadList?.Invoke(this, state.ToString());
        }
    }//class
}
