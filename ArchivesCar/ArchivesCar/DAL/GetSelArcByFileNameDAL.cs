using ArchivesCar.BLL;
using ArchivesCar.Model;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Configuration;

namespace ArchivesCar.DAL
{
    public class GetSelArcByFileNameDAL
    {
        /// <summary>
        /// 根据题名模糊搜索当前库房内所有相关档案信息
        /// </summary>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public bool GetSelArcByFileName(ref object errorMsg)
        {
            try
            {
                ReturnInfo returnInfo = new ReturnInfo() { TrueOrFalse = false };
                string url = string.Format(@"http://{0}:{1}/storeroom/countingcar/selArcByFileName?map[storeid]={2}&map[filename]={3}", PublicData.ServerConfig.serverIP, PublicData.ServerConfig.serverPort, ConfigurationManager.AppSettings["RoomId"], errorMsg.ToString());
                Http http = new Http(url, null);
                var result = JToken.Parse(http.HttpGet(url));
                returnInfo.Result = result["msg"].ToString();
                if (result["state"].ToString().ToLower().Equals("true"))
                {
                    returnInfo.TrueOrFalse = true;
                    List<InventoryInfo> infos = new List<InventoryInfo>();
                    int i = 1;
                    if (!string.IsNullOrEmpty(result["rows"].ToString()))
                    {
                        foreach (var temp in result["rows"].Children())
                        {
                            infos.Add(new InventoryInfo()
                            {
                                order = i,
                                fkFileId = temp["fileId"].ToString(),
                                fkFileName = temp["fileName"].ToString(),
                                isBox = temp["arcOrBox"].ToString(),
                                rfid = temp["rfid"].ToString(),
                                barCode = temp["barCode"].ToString(),
                                locationName = temp["locAddress"].ToString(),
                                state = temp["state"].ToString()
                            });
                            i++;
                        }
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
    }
}
