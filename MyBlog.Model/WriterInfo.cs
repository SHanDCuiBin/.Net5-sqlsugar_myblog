using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace MyBlog.Model
{
    public class WriterInfo : BaseId
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar(12)")]
        public string Name { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar(16)")]
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码   MD5加密
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar(64)")]
        public string UserPwd { get; set; }
    }
}
