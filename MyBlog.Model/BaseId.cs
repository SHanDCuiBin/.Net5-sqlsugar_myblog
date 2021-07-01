using SqlSugar;
namespace MyBlog.Model
{
    public class BaseId
    {
        /// <summary>
        /// ID  表示 自动增长  主键
        /// </summary>
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int Id { get; set; }
    }
}
