using CommonHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FastDFSTestTool
{
    public class LocalConfig
    {
        public static string TrackerIP1 { get { return ConfigureHelper.GetConfigureValue("TrackerIP1", "192.168.12.201"); } }
        public static string TrackerIP2 { get { return ConfigureHelper.GetConfigureValue("TrackerIP2", "192.168.12.202"); } }
        public static int TrackerPort1 { get { return ConfigureHelper.GetConfigureIntValue("TrackerPort1", 22122); } }
        public static int TrackerPort2 { get { return ConfigureHelper.GetConfigureIntValue("TrackerPort2", 22122); } }
        public static string GroupName1 { get { return ConfigureHelper.GetConfigureValue("GroupName1", "group1"); } }

        public static string DownloadUrl { get { return ConfigureHelper.GetConfigureValue("DownloadUrl", "http://192.168.12.215:8080/group1/"); } }

        public static string TongJiUrl { get { return ConfigureHelper.GetConfigureValue("TongJiUrl", "http://192.168.12.211:3000/"); } }

        public static bool IsNeedTong { get { return ConfigureHelper.GetConfigureBoolValue("IsNeedTong", true); } }
    }
}
