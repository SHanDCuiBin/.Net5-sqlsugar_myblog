using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Model.DTO
{
    public class BlogNewsDTO
    {
        public int Id { get; set; }

        /// <summary>
        /// 文章标题 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 文章正文
        /// </summary>
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
        /// 文章类型  
        /// </summary>
        public TypeInfoDTO TypeInfo { get; set; }

        /// <summary>
        /// 文章作者  
        /// </summary>
        public WriterDTO WriterInfo { get; set; }

    }
}
