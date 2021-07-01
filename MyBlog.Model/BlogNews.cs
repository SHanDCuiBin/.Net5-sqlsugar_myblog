using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
namespace MyBlog.Model
{
    public class BlogNews : BaseId
    {
        /// <summary>
        /// 文章标题 
        /// </summary>
        [SugarColumn(ColumnDataType = "nvarchar(100)")]           // nvarchar 带中文比较好   
        public string Title { get; set; }

        /// <summary>
        /// 文章正文
        /// </summary>
        [SugarColumn(ColumnDataType = "text")]
        public string Content { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 文章浏览量
        /// </summary>
        public int BrowseCount { get; set; }

        /// <summary>
        /// 喜欢量
        /// </summary>
        public int LikeCount { get; set; }

        /// <summary>
        /// 文章类型ID
        /// </summary>
        public int TypeId { get; set; }

        /// <summary>
        /// 作者ID
        /// </summary>
        public int WriterId { get; set; }

        /// <summary>
        /// 文章类型   不向数据库进行映射
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public TypeInfo TypeInfo { get; set; }

        /// <summary>
        /// 文章作者   不向数据库进行映射
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public WriterInfo WriterInfo { get; set; }
    }
}
