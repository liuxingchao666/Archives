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
    public class GetSelArcByEpc
    {
        /// <summary>
        /// 根据标签查询单条相关数据（层架标签除外）
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool SelArcByEpc(ref object errorMsg)
        {
            try
            {
                ReturnInfo returnInfo = new ReturnInfo()
                {
                    TrueOrFalse = false
                };
                string url = string.Format(@"http://{0}:{1}/storeroom/countingcar/selArcByEpc?rfid={2}", PublicData.ServerConfig.serverIP, PublicData.ServerConfig.serverPort, errorMsg.ToString());
                Http http = new Http(url, null);
                var result = JToken.Parse(http.HttpGet(url));
                returnInfo.Result = result["msg"].ToString();
                if (result["state"].ToString().ToLower().Equals("true"))
                {
                    InventoryInfo inventoryInfo = new InventoryInfo()
                    {
                        fkFileId = result["row"]["fileId"].ToString(),
                    fkFileName = result["row"]["fileName"].ToString(),
                    isBox = result["row"]["arcOrBoz"].ToString(),
                    rfid = result["row"]["rfid"].ToString(),
                    locationName = result["row"]["locAddress"].ToString(),
                    state = result["row"]["state"].ToString()
                    };
                    returnInfo.TrueOrFalse = true;
                    returnInfo.Result = inventoryInfo;
                }
                errorMsg = returnInfo;
                return true;
            }
            catch
            {
                errorMsg = "连接服务器失败";
                return false;
            }
        }
    }
}
