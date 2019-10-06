using ArchivesCar.BLL;
using ArchivesCar.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivesCar.DAL
{
     public class GetStorageRoomsDAL
    {
        public bool GetStorageRoom(ref object errorMsg)
        {
            try
            {
                ReturnInfo returnInfo = new ReturnInfo() { TrueOrFalse=false};
                string url = string.Format(@"http://{0}:{1}/storeroom/countingcar/selAllStore",PublicData.ServerConfig.serverIP,PublicData.ServerConfig.serverPort);
                Http http = new Http(url,null);
                var result = JToken.Parse(http.HttpGet(url));
                returnInfo.Result = result["msg"].ToString();
                if (result["state"].ToString().ToLower().Equals("true"))
                {
                    List<StorageRoomInfo> infos = new List<StorageRoomInfo>();
                    foreach (var temp in result["row"].Children())
                    {
                        infos.Add(new StorageRoomInfo() {Id=temp["id"].ToString(),Name=temp["storeName"].ToString() });
                    }
                    returnInfo.TrueOrFalse = true;
                    returnInfo.Result = infos;
                }
                errorMsg = returnInfo;
                return true;
            }
            catch (Exception ex)
            {
                errorMsg = "未连接服务器";
                return false;
            }
        }
    }
}
