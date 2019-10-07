using ArchivesCar.BLL;
using ArchivesCar.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivesCar.DAL
{
    public class GetDataByEPCDAL
    {
        public bool GetDataByEPC(ref object errorMsg)
        {
            try
            {
                ReturnInfo returnInfo = new ReturnInfo() { TrueOrFalse=false};
                string url = string.Format(@"http://{0}:{1}/storeroom/countingcar/arcOfLay", PublicData.ServerConfig.serverIP, PublicData.ServerConfig.serverPort);
                Http http = new Http(url, JsonConvert.SerializeObject(errorMsg));
                var result = JToken.Parse(http.HttpPosts());
                returnInfo.Result = result["msg"].ToString();
                if (result["state"].ToString().ToLower().Equals("true"))
                {
                    returnInfo.TrueOrFalse = true;
                    List<InventoryInfo> infos = new List<InventoryInfo>();
                    int i = 1;
                    foreach (var temp in result["row"].Children())
                    {
                        infos.Add(new InventoryInfo() {
                            order = i,
                            fkFileId = temp["fkFileId"].ToString(),
                            rfid = temp["rfid"].ToString(),
                            fkFileTypeId = temp["fkFileTypeId"].ToString(),
                            fkFileName = temp["fkFileName"].ToString(),
                            state = getState(temp["state"].ToString()),
                            locationName = temp["locationName"].ToString(),
                            barCode = temp["barCode"].ToString(),
                            isBox = getIsBox(temp["isBox"].ToString())
                        });
                        i++;
                    }
                    returnInfo.Result = infos;
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
        string getState(string state)
        {
            switch (state)
            {
                case "tbt":
                    return "待取";
                case "lend":
                    return "借出";
                case "nots":
                    return "未上架";
                case "rf":
                    return "在架";
                case "loss":
                    return "遗失";
                case "trans":
                    return "移交";
                case "dest":
                    return "销毁";
                case "tba":
                    return "待归档";
                case "ita":
                    return "归档中";
                case "destr":
                    return "销毁中";
            }
            return "";
        }
        string getIsBox(string box)
        {
            switch (box)
            {
                case "0":
                    return "档案";
                default:
                    return "档案盒";
            }
        }
    }
}
