using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivesCar.Model
{
    public class InventoryInfo
    {
        //public string Select(string state)
        //{
        //    switch (state)
        //    {
        //        case "tbt":
        //            return "待取";
        //        case "lend":
        //            return "借出";
        //        case "nots":
        //            return "未上架";
        //        case "rf":
        //            return "在架";
        //        case "loss":
        //            return "遗失";
        //        case "trans":
        //            return "移交";
        //        case "dest":
        //            return "销毁";
        //        case "tba":
        //            return "待归档";
        //        case "ita":
        //            return "归档中";
        //        case "destr":
        //            return "销毁中";
        //    }
        //    return "";
        //}
        public int order { get; set; }
        public string fkFileId { get; set; }
        public string rfid { get; set; }
        public string fkFileTypeId { get; set; }
        public string fkFileName { get; set; }
        public string isBox { get; set; }
        public string state { get; set; }
        public string locationName { get; set; }
        public string barCode { get; set; }
        public string Background { get; set; }
    }
}
