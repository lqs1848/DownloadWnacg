using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace wnacg
{
    class Download
    {
        public EventHandler<String> DownloadLog;
        public EventHandler<String> DownloadStart;
        public EventHandler<String> DownloadSpeed;

        SynchronizationContext _syncContext;

        private string dlListStr;
        private Queue<string> dlTaskStrs;
        //线程数
        const int cycleNum = 2;

        public Download(SynchronizationContext formContext, string str)
        {
            this._syncContext = formContext;
            this.dlListStr = str;

             this.dlTaskStrs = new Queue<string>(dlListStr.Split(new string[] { "\r\n" }, StringSplitOptions.None));
        }

        private void OutLog(object state)
        {
            DownloadLog?.Invoke(this, state.ToString());
        }

        private void DlTaskStart(object state)
        {
            DownloadStart?.Invoke(this, state.ToString());
        }

        private void DlTaskSchedule(object state)
        {
            DownloadSpeed?.Invoke(this, state.ToString());
        }

        public void Start()
        {
            new Thread(TaskMStart).Start();
        }

        private void TaskStart()
        {   
            string logpath = AppDomain.CurrentDomain.BaseDirectory;
            string dirPath = logpath + "download" + "\\";
            string[] strArr = dlListStr.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            foreach (string str in strArr)
            {
                string[] l = str.Split(new string[] { "\\" }, StringSplitOptions.None);
                try
                {
                    this.HttpDownloadFile(l[0], dirPath, l[1]);
                }
                catch (Exception ex)
                {
                    ExeLog.WriteLog("downloadErrorList.txt", str + "\r\n");
                    ExeLog.WriteLog("downloadErrorLog.txt", str+"\r\n" +ex.Message + "\r\n");
                }
            }//for
        }

        private void TaskMStart()
        {
            ThreadPool.SetMinThreads(cycleNum, cycleNum);
            ThreadPool.SetMaxThreads(cycleNum, cycleNum);
            for (int i = 1; i <= cycleNum; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(DownloadFun));
            }
        }

        private void DownloadFun(object obj)
        {
            Random random = new Random();
            string logpath = AppDomain.CurrentDomain.BaseDirectory;
            string dirPath = logpath + "download" + "\\";

            while (dlTaskStrs.Count > 0)
            {
                string str = dlTaskStrs.Dequeue();
                if(str!=null && str != "")
                {
                    string[] l = str.Split(new string[] { "\\" }, StringSplitOptions.None);
                    this.HttpDownloadFile(l[0], dirPath, l[1]);
                    //Thread.Sleep(random.Next(1000));
                }
            }
        }

        public void HttpDownloadFile(string url, string path, string fileName)
        {
            HttpDownloadFile(url, path, fileName, 0);
        }

        public void HttpDownloadFile(string url, string path,string fileName,int deep)
        {
            //url = http://wnacg.download/down/0548/89cf592c299da111da870286b8834d31.zip
            int keyStart = url.LastIndexOf('/');
            int keyEnd = url.LastIndexOf('.');
            //提取 89cf592c299da111da870286b8834d31 作为控件name
            string key = url.Substring(keyStart, keyEnd - keyStart);

            if (deep == 0 && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (File.Exists(path + fileName)) {
                _syncContext.Post(OutLog, "文件存在:" + fileName + " 跳过\r\n");
                return;
            }

            string historyPath = path + "history\\";
            if (deep == 0 && !Directory.Exists(historyPath)) 
            {
                Directory.CreateDirectory(historyPath);
            }

            if (File.Exists(historyPath + fileName))
            {
                _syncContext.Post(OutLog, "曾经下载过:" + fileName + " 跳过\r\n");
                return;
            }
            

            _syncContext.Post(OutLog, "开始任务:"+fileName+ "\r\n");
            try
            {
                if(deep==0)
                    _syncContext.Post(DlTaskStart, key + "|" + fileName);

                //File.Create(path + fileName + ".temp.wnacg").Close();
                
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                double dataLengthToRead = response.ContentLength;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();

                _syncContext.Post(DlTaskSchedule, key + "|创建文件中");

                //创建本地文件写入流
                Stream stream = new FileStream(path+ fileName + ".temp.wnacg", FileMode.Create);
                byte[] bArr = new byte[1024 * 512];
                double count = 0;
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    
                    stream.Write(bArr, 0, size);

                    count += size;
                    //_syncContext.Post(DlTaskSchedule, key + "|" + count + "-" + dataLengthToRead);
                    double bfb = count / dataLengthToRead * 100;
                    _syncContext.Post(DlTaskSchedule, key + "|" + bfb.ToString("0.00"));

                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                stream.Close();
                responseStream.Close();
                
                
                _syncContext.Post(DlTaskSchedule, key + "|重命名中");
                File.Move(path + fileName + ".temp.wnacg", path + fileName);
                _syncContext.Post(DlTaskSchedule, key + "|完成");
                File.Create(historyPath + fileName).Close();
            }
            catch (Exception ex)
            {
                //Console.Out.Write(ex.StackTrace);

                //重试3次
                if (deep > 3)
                {
                    _syncContext.Post(DlTaskSchedule, key + "|无法下载");
                    
                }
                else
                {
                    deep++;
                    _syncContext.Post(OutLog, " re"+ deep);
                    _syncContext.Post(DlTaskSchedule, key + "|重连 "+deep);
                    Thread.Sleep(1000);
                    HttpDownloadFile(url, path, fileName, deep);
                    return;
                }
            }
            
        }//method
    }//class
}//namespace
