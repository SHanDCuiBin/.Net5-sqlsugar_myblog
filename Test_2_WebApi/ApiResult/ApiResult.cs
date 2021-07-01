using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.WebApi.Utility.ApiResult
{
    public class ApiResult
    {
        /// <summary>
        /// 状态子码
        /// </summary>
        public int Code { get; set; }
        
        /// <summary>
        /// 错误信息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 分页总数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 数据 
        /// </summary>
        public dynamic Data { get; set; }
    }
}
