using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchivesCar.Model
{
    /// <summary>
    /// 返回类
    /// </summary>
    public class ReturnInfo
    {
        /// <summary>
        /// state
        /// </summary>
        public bool TrueOrFalse { get; set; }
        /// <summary>
        /// 结果集
        /// </summary>
        public object Result { get; set; }
        /// <summary>
        /// 状态码
        /// </summary>
        public string Code { get; set; }
    }
}
