using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivesCar.Model
{
    public class PositionInfo
    {
        /// <summary>
        /// 库房id
        /// </summary>
        public string fkStoreId { get; set; }
        /// <summary>
        /// 库房名称
        /// </summary>
        public string fkStoreName { get; set; }
        /// <summary>
        /// 区id
        /// </summary>
        public string fkRegionId { get; set; }
        /// <summary>
        /// 区号
        /// </summary>
        public string fkRegionNum { get; set; }
        /// <summary>
        /// 区名
        /// </summary>
        public string fkRegionName { get; set; }
        /// <summary>
        /// 列
        /// </summary>
        public string colNum { get; set; }
        /// <summary>
        /// 节
        /// </summary>
        public string divNum { get; set; }
        /// <summary>
        /// 层
        /// </summary>
        public string laysNum { get; set; }
        /// <summary>
        /// 方向
        /// </summary>
        public string direction { get; set; }
        /// <summary>
        /// 位置
        /// </summary>
        public string position { get; set; }
        /// <summary>
        /// 离线位置
        /// </summary>
        public string positionOffLine { get; set; }
        /// <summary>
        /// 主动放弃分页
        /// </summary>
        public int currentPage { get; set; } = 1;
        /// <summary>
        /// 显示条数
        /// </summary>
        public int pageSize { get; set; } = 99999;
    }
}
