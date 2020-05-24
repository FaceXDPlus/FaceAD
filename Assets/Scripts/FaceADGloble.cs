using System.Collections;
using System.Collections.Generic;

namespace FaceAD
{
    public class Globle
    {
        public static string APPName = "AMGAFace";
        public static string APPVersion = "Alpha 0.1";
        public static string APPBuild = "2";
        public static string DataLog = "\n软件版本：" + APPVersion + "，构建版本：" + APPBuild;
        public static void AddDataLog(string data)
        {
            DataLog = DataLog + "\n" + data;
        }
    }
}