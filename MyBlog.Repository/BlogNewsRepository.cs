using MyBlog.IRepository;
using MyBlog.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.Repository
{
    public class BlogNewsRepository : BaseRepository<BlogNews>, IBlogNewsRepository
    {
        /// <summary>
        /// 查询所有            表关联映射
        /// </summary>
        /// <returns></returns>
        public async override Task<List<BlogNews>> QueryAsync()
        {
            return await base.Context.Queryable<BlogNews>()
                .Mapper(c => c.TypeInfo, c => c.TypeId, c => c.TypeInfo.Id)
                .Mapper(c => c.WriterInfo, c => c.WriterId, c => c.WriterInfo.Id)
                .ToListAsync();
        }

        /// <summary>
        /// 自定义条件查询      表关联映射
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public async override Task<List<BlogNews>> QueryAsync(Expression<Func<BlogNews, bool>> func)
        {
            return await base.Context.Queryable<BlogNews>()
                 .Where(func)
                 .Mapper(c => c.TypeInfo, c => c.TypeId, c => c.TypeInfo.Id)
                 .Mapper(c => c.WriterInfo, c => c.WriterId, c => c.WriterInfo.Id)
                 .ToListAsync();
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public async override Task<List<BlogNews>> QueryAsync(int page, int size, RefAsync<int> total)
        {
            return await base.Context.Queryable<BlogNews>()
                 .Mapper(c => c.TypeInfo, c => c.TypeId, c => c.TypeInfo.Id)
                .Mapper(c => c.WriterInfo, c => c.WriterId, c => c.WriterInfo.Id)
                .ToPageListAsync(page, size, total);
        }

        /// <summary>
        /// 条件分页查询
        /// </summary>
        /// <param name="func"></param>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public async override Task<List<BlogNews>> QueryAsync(Expression<Func<BlogNews, bool>> func, int page, int size, RefAsync<int> total)
        {
            return await base.Context.Queryable<BlogNews>()
                .Where(func)
                .Mapper(c => c.TypeInfo, c => c.TypeId, c => c.TypeInfo.Id)
                .Mapper(c => c.WriterInfo, c => c.WriterId, c => c.WriterInfo.Id)
                .ToPageListAsync(page, size, total);
        }
    }
}
