using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Drawing;

using FastDFS.Client;
namespace FastDFS
{
    class Program
    {
        static void Main(string[] args)
        {
            new Program().Test();
            Console.Read();
        }


        public void Test()
        {

            TestUploadFile();
            //TestBatchUploadFile();
            //TestQueryFile();

            //TestDownloadFile();

            //TestRemoveFile();
            //TestHttpGet();
            //TestBigFiles();
        }

        public void TestBigFiles()
        {
            string group = "group1";
            WebClient web = new WebClient();
            #region 30M
            //string re1 = UploadFile(group, @"F:\test\bigFiles\B30.rar", "rar");
            //Console.WriteLine("上传成功：" + re1);
            ////Download
            //web.DownloadFile("http://192.168.12.215:8080/group1/" + re1, @"D:\test\Download\DB30.rar");
            //Console.WriteLine("下载成功30M");
            #endregion
            #region 100M
            //100M
            //string re2 =  UploadFile(group, @"F:\test\bigFiles\B100.rar", "rar");
            //Console.WriteLine("上传成功：" + re2);
            ////Download
            //web.DownloadFile("http://192.168.12.215:8080/group1/" + re2, @"D:\test\Download\DB100.rar");
            //Console.WriteLine("下载成功100M");
            #endregion
            #region 400M
            //string re3 = UploadFile(group, @"F:\test\bigFiles\B400.rar", "rar");
            //Console.WriteLine("上传成功：" + re3);

            //Thread.Sleep(500);
            ////Download
            //web.DownloadFile("http://192.168.12.215:8080/group1/" + re3, @"D:\test\Download\DB400.rar");
            //Console.WriteLine("下载成功400M");
            #endregion
            //1000M
            #region 1G
            string re4 = UploadFile(group, @"F:\test\bigFiles\B1000.rar", "rar");
            Console.WriteLine("上传成功：" + re4);

            Thread.Sleep(500);
            //Download
            web.DownloadFile("http://192.168.12.215:8080/group1/" + re4, @"D:\test\Download\DB1000.rar");
            Console.WriteLine("下载成功1G");
            #endregion
        }


        public void TestUploadFile()
        {
            string group = "group1";
            string filePath = @"D:\FastDFS-5.01_nginx_cache 集群安装配置手册.docx";
            string re = UploadFile(group, filePath,"docx");
            Console.WriteLine(re);
        }



        public string UploadFile(string group,string oFile,string fix)
        {
            //===========================Initial========================================
            List<IPEndPoint> trackerIPs = new List<IPEndPoint>();
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.12.211"), 22122);
            IPEndPoint endPoint2 = new IPEndPoint(IPAddress.Parse("192.168.12.212"), 22122);
            trackerIPs.Add(endPoint);
            trackerIPs.Add(endPoint2);
            ConnectionManager.Initialize(trackerIPs);
            StorageNode node = FastDFSClient.GetStorageNode(group);


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
            return fileName;
           
        }

        public void TestBatchUploadFile()
        {
            List<IPEndPoint> trackerIPs = new List<IPEndPoint>();
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.12.211"), 22122);
            IPEndPoint endPoint2 = new IPEndPoint(IPAddress.Parse("192.168.12.212"), 22122);
            trackerIPs.Add(endPoint);
            trackerIPs.Add(endPoint2);
            ConnectionManager.Initialize(trackerIPs);
            StorageNode node = FastDFSClient.GetStorageNode("group1");

            byte[] content = null;
            string[] _FileEntries = Directory.GetFiles(@"D:\test", "*.jpg");
            DateTime start = DateTime.Now;

            
            foreach (string file in _FileEntries)
            {
                string name = Path.GetFileName(file);
                content = null;
                FileStream streamUpload = new FileStream(file, FileMode.Open);
                using (BinaryReader reader = new BinaryReader(streamUpload))
                {
                    content = reader.ReadBytes((int)streamUpload.Length);
                }
                //string fileName = FastDFSClient.UploadAppenderFile(node, content, "mdb");
                string fileName = FastDFSClient.UploadFile(node, content, "jpg");

                Console.WriteLine(fileName);
            }
            DateTime end = DateTime.Now;
            TimeSpan consume = ((TimeSpan)(end - start));
            double consumeSeconds = Math.Ceiling(consume.TotalSeconds);
            Console.WriteLine(consumeSeconds + "秒");
        }

        public void TestQueryFile()
        {
             //===========================Initial========================================
            List<IPEndPoint> trackerIPs = new List<IPEndPoint>();
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.12.211"), 22122);
            IPEndPoint endPoint2 = new IPEndPoint(IPAddress.Parse("192.168.12.212"), 22122);
            trackerIPs.Add(endPoint);
            trackerIPs.Add(endPoint2);
            ConnectionManager.Initialize(trackerIPs);
            StorageNode node = FastDFSClient.GetStorageNode("group1");

            string fileName = "M00/00/00/wKgM01fFqb-ALJk9AAEIDcbQkWg629.jpg";

            FDFSFileInfo fileInfo = FastDFSClient.GetFileInfo(node, fileName);
            Console.WriteLine(string.Format("FileName:{0}", fileName));
            Console.WriteLine(string.Format("FileSize:{0}", fileInfo.FileSize));
            Console.WriteLine(string.Format("CreateTime:{0}", fileInfo.CreateTime));
            Console.WriteLine(string.Format("Crc32:{0}", fileInfo.Crc32));
        }

        public void TestAppendFile()
        {
            //略
        }

        public void TestDownloadFile()
        {
            string group = "group2";
            string fileName = "M00/00/00/wKgM01fH_zOAGyfIAC8pO3ZE_9w09.docx";
            string tFile = @"D:\test\Download\abc.docx";
            DownLoadFile(group, fileName, tFile);
            Console.WriteLine("下载成功");
        }

        public void DownLoadFile(string group,string fileName,string tFile)
        {
            //下载的问题就有问题
            //===========================Initial========================================
            List<IPEndPoint> trackerIPs = new List<IPEndPoint>();
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.12.211"), 22122);
            IPEndPoint endPoint2 = new IPEndPoint(IPAddress.Parse("192.168.12.212"), 22122);
            trackerIPs.Add(endPoint);
            trackerIPs.Add(endPoint2);
            ConnectionManager.Initialize(trackerIPs);
            StorageNode node = FastDFSClient.GetStorageNode(group);

            //string fileName = "M00/00/00/wKgM01fH_zOAGyfIAC8pO3ZE_9w09.docx";
            byte[] buffer = FastDFSClient.DownloadFile(node, fileName);
            string file = tFile;  // @"D:\test\Download\abc.docx";
            if (File.Exists(file))
                File.Delete(file);
            FileStream stream = new FileStream(file, FileMode.CreateNew);
            using (BinaryWriter write = new BinaryWriter(stream, Encoding.BigEndianUnicode))
            {
                write.Write(buffer);
                write.Close();
            }
            stream.Close();
        }

        public void TestRemoveFile()
        {

            //===========================Initial========================================
            List<IPEndPoint> trackerIPs = new List<IPEndPoint>();
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.12.211"), 22122);
            IPEndPoint endPoint2 = new IPEndPoint(IPAddress.Parse("192.168.12.212"), 22122);
            trackerIPs.Add(endPoint);
            trackerIPs.Add(endPoint2);
            ConnectionManager.Initialize(trackerIPs);


            string fileName = "M00/00/00/wKgM01fH_zOAGyfIAC8pO3ZE_9w09.docx";
            FastDFSClient.RemoveFile("group1", fileName);
        }

        public void TestHttpGet()
        {
            string url = "http://192.168.12.215:8080/group1/M00/00/00/wKgM01fFqb-ALJk9AAEIDcbQkWg629.jpg";
            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
            System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
            Image myImage = Image.FromStream(res.GetResponseStream());
            myImage.Save(@"D:\test\Download\abc1.jpg");//保存

            WebClient web = new WebClient();
            web.DownloadFile("http://192.168.12.215:8080/group1/M00/00/00/wKgM1FfIMhWAIT6LAC8pO3ZE_9w10.docx", @"D:\test\Download\abc2.docx");
            web.DownloadFile("http://192.168.12.215:8080/group1/M00/00/00/wKgM01fFqb-ALJk9AAEIDcbQkWg629.jpg", @"D:\test\Download\abc3.jpg");
        }




        public void all()
        {

            //===========================Initial========================================
            List<IPEndPoint> trackerIPs = new List<IPEndPoint>();
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.12.215"), 22122);
            trackerIPs.Add(endPoint);
            ConnectionManager.Initialize(trackerIPs);
            StorageNode node = FastDFSClient.GetStorageNode("group1");
            //===========================UploadFile=====================================
            byte[] content = null;
            string filePath = @"D:\test.xml";
            if (File.Exists(filePath))
            {
                FileStream streamUpload = new FileStream(filePath, FileMode.Open);
                using (BinaryReader reader = new BinaryReader(streamUpload))
                {
                    content = reader.ReadBytes((int)streamUpload.Length);
                }
            }
            //string fileName = FastDFSClient.UploadAppenderFile(node, content, "mdb");
            string fileName = FastDFSClient.UploadFile(node, content, "xml");

            //===========================BatchUploadFile=====================================
            string[] _FileEntries = Directory.GetFiles(@"E:\fastimage\三维", "*.jpg");
            DateTime start = DateTime.Now;
            foreach (string file in _FileEntries)
            {
                string name = Path.GetFileName(file);
                content = null;
                FileStream streamUpload = new FileStream(file, FileMode.Open);
                using (BinaryReader reader = new BinaryReader(streamUpload))
                {
                    content = reader.ReadBytes((int)streamUpload.Length);
                }
                //string fileName = FastDFSClient.UploadAppenderFile(node, content, "mdb");
                fileName = FastDFSClient.UploadFile(node, content, "jpg");
            }
            DateTime end = DateTime.Now;
            TimeSpan consume = ((TimeSpan)(end - start));
            double consumeSeconds = Math.Ceiling(consume.TotalSeconds);
            //===========================QueryFile=======================================
            fileName = "M00/00/00/wKhR6U__-BnxYu0eAxRgAJZBq9Q180.mdb";
            FDFSFileInfo fileInfo = FastDFSClient.GetFileInfo(node, fileName);
            Console.WriteLine(string.Format("FileName:{0}", fileName));
            Console.WriteLine(string.Format("FileSize:{0}", fileInfo.FileSize));
            Console.WriteLine(string.Format("CreateTime:{0}", fileInfo.CreateTime));
            Console.WriteLine(string.Format("Crc32:{0}", fileInfo.Crc32));
            //==========================AppendFile=======================================
            FastDFSClient.AppendFile("group1", fileName, content);
            FastDFSClient.AppendFile("group1", fileName, content);

            //===========================DownloadFile====================================
            fileName = "M00/00/00/wKhR6U__-BnxYu0eAxRgAJZBq9Q180.mdb";
            byte[] buffer = FastDFSClient.DownloadFile(node, fileName, 0L, 0L);
            if (File.Exists(@"D:\SZdownload.mdb"))
                File.Delete(@"D:\SZdownload.mdb");
            FileStream stream = new FileStream(@"D:\SZdownload.mdb", FileMode.CreateNew);
            using (BinaryWriter write = new BinaryWriter(stream, Encoding.BigEndianUnicode))
            {
                write.Write(buffer);
                write.Close();
            }
            stream.Close();
            //===========================RemoveFile=======================================
            FastDFSClient.RemoveFile("group1", fileName);

            //===========================Http测试，流读取=======================================
            string url = "http://img13.360buyimg.com/da/g5/M02/0D/16/rBEDik_nOJ0IAAAAAAA_cbJCY-UAACrRgMhVLEAAD-J352.jpg";
            System.Net.HttpWebRequest req = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(url);
            System.Net.HttpWebResponse res = (System.Net.HttpWebResponse)req.GetResponse();
            Image myImage = Image.FromStream(res.GetResponseStream());
            myImage.Save("c:\\fast.jpg");//保存
            //===========================Http测试，直接下载=======================================
            WebClient web = new WebClient();
            web.DownloadFile("http://img13.360buyimg.com/da/g5/M02/0D/16/rBEDik_nOJ0IAAAAAAA_cbJCY-UAACrRgMhVLEAAD-J352.jpg", "C:\\abc.jpg");
            web.DownloadFile("http://192.168.81.233/M00/00/00/wKhR6VADbNr5s7ODAAIOGO1_YmA574.jpg", "C:\\abc.jpg");

            Console.WriteLine("Complete");
            Console.Read();
        }
    }
}
