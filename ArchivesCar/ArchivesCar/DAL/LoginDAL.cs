using ArchivesCar.BLL;
using ArchivesCar.Model;
using ArchivesCar.PublicData;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivesCar.DAL
{
     public class LoginDAL
    {
        public bool login(ref object errorMsg)
        {
            try
            {
                ReturnInfo returnInfo = new ReturnInfo() { TrueOrFalse = false};
                string url = string.Format(@"http://{0}:{1}/storeroom/countingcar/userLogin", ServerConfig.serverIP,ServerConfig.serverPort);
                Dictionary<string, object> keyValuePairs = errorMsg as Dictionary<string, object>;
                Http http = new Http(url, keyValuePairs);
                var result = JToken.Parse(http.HttpPosts());
                returnInfo.Result = result["msg"].ToString();
                if (result["state"].ToString().ToLower().Contains("true"))
                {
                    returnInfo.TrueOrFalse = true;
                    PublicData.ServerConfig.Token = result["row"].ToString();
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
