using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivesCar.PublicData
{
    public static class ServerConfig
    {
        /// <summary>
        /// 服务器ip
        /// </summary>
        public static string serverIP = ConfigurationManager.AppSettings["ServerIP"];
        /// <summary>
        /// 服务器端口
        /// </summary>
        public static string serverPort = ConfigurationManager.AppSettings["ServerPort"];
        /// <summary>
        /// 登陆后获取身份验证牌
        /// </summary>
        public static string Token { get; set; }
        /// <summary>
        /// 当前库房id
        /// </summary>
        public static string roomId = ConfigurationManager.AppSettings["RoomId"];
        /// <summary>
        /// 模式选择
        /// </summary>
        public static string model= ConfigurationManager.AppSettings["Model"];
        /// <summary>
        /// 接收uid集合
        /// </summary>
        public static Queue<string> UserList = new Queue<string>();
        /// <summary>
        /// 接收epc集合
        /// </summary>
        public static Queue<string> EpcList = new Queue<string>();
        /// <summary>
        /// 当前操作未处理的epc集合（用于去重）
        /// </summary>
        public static List<string> EpcS = new List<string>();
        /// <summary>
        /// 当前操作已处理的epc集合（用于去重）
        /// </summary>
        public static List<string> UEpcS = new List<string>();
    }
}
