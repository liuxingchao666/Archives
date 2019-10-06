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
     public class GetDownLoadFileNamesDAL
    {
        public bool GetDownLoadFileNames(ref object errorMsg)
        {
            try
            {
                ReturnInfo returnInfo = new ReturnInfo() { TrueOrFalse=false};
                string url = string.Format(@"http://{0}:{1}/archivesmodule/TableSqlFile/sqlBack", PublicData.ServerConfig.serverIP, PublicData.ServerConfig.serverPort);
                Http http = new Http(url,null);
                var result = JToken.Parse(http.HttpGet(url));
                returnInfo.Result = result["msg"].ToString();
                returnInfo.Code = result["code"].ToString();
                if (result["state"].ToString().ToLower().Equals("true"))
                {
                    returnInfo.TrueOrFalse = true;
                    List<string> list = new List<string>();
                    foreach (var temp in result["row"].Children())
                    {
                        list.Add(temp.ToString()); 
                    }
                    returnInfo.Result = list;
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
