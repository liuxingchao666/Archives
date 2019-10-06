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
     public class BindRFIDDAL
    {
        public bool BindRFID(ref object errorMsg)
        {
            try
            {
                ReturnInfo returnInfo = new ReturnInfo() { TrueOrFalse=false};
                string url = string.Format(@"http://{0}:{1}/storeroom/countingcar/bindRfid", PublicData.ServerConfig.serverIP, PublicData.ServerConfig.serverPort);
                Dictionary<string, object> valuePairs = errorMsg as Dictionary<string, object>;
                Http http = new Http(url, valuePairs);
                var result = JToken.Parse(http.HttpPosts());
                returnInfo.Result = result["msg"].ToString();
                if (result["state"].ToString().ToLower().Equals("true"))
                {
                    returnInfo.TrueOrFalse = true;
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
