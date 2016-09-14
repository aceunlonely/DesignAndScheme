using FastDFS.Client;
using log4net;
using log4net.Appender;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FastDFSTestTool
{
    class Program
    {

        private static ILog log;

        public static void AutoLog()
        {
            FileAppender app = (FileAppender)log.Logger.Repository.GetAppenders()[0];
            app.File = Path.Combine(Path.GetDirectoryName(app.File), "AutoLog" + DateTime.Now.ToString("ddHHmmssfff") + Guid.NewGuid().ToString() + ".txt");
            app.ActivateOptions();
        }
        static void Main(string[] args)
        {
            //日志
            string path = AppDomain.CurrentDomain.BaseDirectory + @"log4net.config";
            log4net.Config.XmlConfigurator.Configure(new FileInfo(path));
            log = LogManager.GetLogger("log");

            //参数
            string arg1 = string.Empty;
            string arg2 = string.Empty;
            string arg3 = string.Empty;
            if (args.Length > 0)
            {
                arg1 = args[0];
                //Console.WriteLine(args[0]);

                if (string.IsNullOrEmpty(arg1) == false && arg1.ToLower().StartsWith("a"))
                {
                    arg1 = arg1.Substring(1);
                    AutoLog();
                }
            }
            if (args.Length > 1)
            {
                arg2 = args[1];
                //Console.WriteLine(args[1]);
            }
            if (args.Length > 2)
            {
                arg3 = args[2];
            }
            //初始化
            Init();
            //选择命令
            switch (arg1.ToLower())
            {
                case "r":
                case "repeat":
                    RepeatRun(args);
                    break;
                case "upload":
                case "u":
                    Upload(new string[] { arg2 });
                    break;
                case "d":
                case "download":
                    DownLoad(arg2, arg3);
                    break;
                case "help":
                case "h":
                    help();
                    break;
                default:
                    Console.WriteLine("请输入正确参数");
                    help();
                    break;
            }


        }

        public const string HR = @"重复执行命令格式： FastDFSTestTool.exe r 1000 3000 u D:\test.jpg   ******* 命令 时间间隔 重复次数 操作命令(u/d) 文件名 ";
        public const string HU = @"上传命令格式：  FastDFSTestTool.exe u D:\FastDFS-5.01_nginx_cache 集群安装配置手册.docx";
        public const string HD = @"下载命令格式：  FastDFSTestTool.exe d M00/00/00/wKgMylfOu8iAT49KACOHqW7r-pY983.jpg D:\targeFile.jpg  ********** 命令 文件服务器路径 本地下载路径";

        public static void help()
        {
            Console.WriteLine(HR);
            Console.WriteLine(HU);
            Console.WriteLine(HD);
        }

        public static void Upload(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine(HU);
                return;
            }

            if (File.Exists(args[0]) == false)
            {
                Console.WriteLine("文件：" + args[0] + "不存在");
                return;
            }
            string filePath = args[0];
            string fix = Path.GetExtension(filePath).TrimStart(new char[] { '.' });
            try
            {
                log.Debug("开始上传文件:" + filePath);
                Console.WriteLine("开始上传文件:" + filePath);
                DateTime d1 = DateTime.Now;
                string re = UploadFile(filePath, fix);
                double s = DateTime.Now.Subtract(d1).TotalMilliseconds / 1000;
                log.Debug("上传成功！共耗时" + s + "秒");
                Console.WriteLine("上传成功！共耗时" + s + "秒");
                //加统计
                TongJi(1, s);
            }
            catch (Exception ex)
            {
                log.Error("上传失败：错误信息：" + ex.StackTrace);
                Console.WriteLine("上传失败：错误信息：" + ex.StackTrace);
            }
        }

        public static void DownLoad(string file, string targetFile)
        {
            if (string.IsNullOrEmpty(file) || string.IsNullOrEmpty(targetFile))
            {
                Console.WriteLine(HD);
                return;
            }
            //web.DownloadFile("http://192.168.12.215:8080/group1/" + re4, @"D:\test\Download\DB1000.rar");
            WebClient web = new WebClient();
            string url = LocalConfig.DownloadUrl + file;
            if (File.Exists(targetFile))
            {
                File.Delete(targetFile);
            }
            try
            {
                Console.WriteLine("开始下载文件:" + url);
                log.Debug("开始下载文件:" + url);
                DateTime d1 = DateTime.Now;
                web.DownloadFile(url, targetFile);
                double s = DateTime.Now.Subtract(d1).TotalMilliseconds / 1000;

                log.Debug("下载成功！共耗时" + s + "秒 ： " + targetFile);
                Console.WriteLine("下载成功！共耗时" + s + "秒 ： " + targetFile);
                //加统计
                TongJi(1, s);
            }
            catch (Exception ex)
            {
                log.Error("下载出错: " + ex.StackTrace);
                Console.WriteLine("下载出错: " + ex.StackTrace);
            }
        }

        /// <summary>
        /// 重复执行
        /// </summary>
        public static void RepeatRun(string[] args)
        {
            if (args.Length < 5)
            {
                Console.WriteLine(HR);
                return;
            }
            // FastDFSTestTool.exe r 1000 3000 
            int i = 0;
            int a2 = int.Parse(args[1]);
            int a3 = int.Parse(args[2]);
            string a4 = args[3];
            string a5 = args[4];
            string a6 = string.Empty;
            if (args.Length >= 6)
                a6 = args[5];

            double totoalSec = 0;
            while (i++ < a3)
            {
                Console.WriteLine("重复执行执行" + i + "次");
                log.Debug("重复执行执行" + i + "次");
                DateTime d1 = DateTime.Now;
                switch (a4.ToLower())
                {
                    case "upload":
                    case "u":
                        Upload(new string[] { a5 });
                        break;
                    case "d":
                    case "download":
                        DownLoad(a5, a6);
                        break;
                    default:
                        break;
                }
                double s = DateTime.Now.Subtract(d1).TotalMilliseconds / 1000;
                totoalSec += s;
                Console.WriteLine("重复执行" + i + "次，运行总时间" + totoalSec + "秒，平均执行时间" + (totoalSec * 1.0 / i) + "秒/次, " + (i * 1.0 / totoalSec) + "次/秒");
                log.Debug("重复执行" + i + "次，运行总时间" + totoalSec + "秒，平均执行时间" + (totoalSec * 1.0 / i) + "秒/次, " + (i * 1.0 / totoalSec) + "次/秒");
                Thread.Sleep(a2);
            }
            Console.WriteLine("任务执行完成，去除间隔时间，共花费" + totoalSec + "秒");
        }


        private static void Init()
        {
            //===========================Initial========================================
            List<IPEndPoint> trackerIPs = new List<IPEndPoint>();
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(LocalConfig.TrackerIP1), LocalConfig.TrackerPort1);
            IPEndPoint endPoint2 = new IPEndPoint(IPAddress.Parse(LocalConfig.TrackerIP2), LocalConfig.TrackerPort2);
            trackerIPs.Add(endPoint);
            trackerIPs.Add(endPoint2);

            ConnectionManager.Initialize(trackerIPs);
            node = FastDFSClient.GetStorageNode(LocalConfig.GroupName1);
        }

        static StorageNode node;

        private static string UploadFile(string oFile, string fix)
        {
            byte[] content = null;
            string filePath = oFile;
            if (File.Exists(filePath))
            {
                FileStream streamUpload = new FileStream(filePath, FileMode.Open);
                using (BinaryReader reader = new BinaryReader(streamUpload))
                {
                    content = reader.ReadBytes((int)streamUpload.Length);
                }
            }
            //string fileName = FastDFSClient.UploadAppenderFile(node, content, "mdb");
            string fileName = FastDFSClient.UploadFile(node, content, fix);

            Console.WriteLine("服务器文件名:" + fileName);
            log.Info("服务器文件名:" + fileName);
            return fileName;


        }

        /// <summary>
        /// 统计时间
        /// </summary>
        /// <param name="add"></param>
        /// <param name="total"></param>
        public static void TongJi(int add, double total)
        {
            if (LocalConfig.IsNeedTong)
            {
                try
                {
                    string url = LocalConfig.TongJiUrl + string.Format("add?add={0}&value={1}", add, total);

                    HttpWebRequest webrequest = (HttpWebRequest)HttpWebRequest.Create(url);
                    HttpWebResponse webreponse = (HttpWebResponse)webrequest.GetResponse();
                    Stream stream = webreponse.GetResponseStream();
                    byte[] rsByte = new Byte[webreponse.ContentLength];  //save data in the stream
                }
                catch
                {

                }
            }
        }
    }
}
