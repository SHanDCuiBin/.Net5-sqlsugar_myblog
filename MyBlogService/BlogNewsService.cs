using MyBlog.IRepository;
using MyBlog.IService;
using MyBlog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBlogService
{
    public class BlogNewsService : BaseService<BlogNews>, IBlogNewsService
    {
        //ctor+tab  构造函数快捷键
        // ~ +tab   析构函数的快捷键


        private readonly IBlogNewsRepository _blogNewsRepository;
        public BlogNewsService(IBlogNewsRepository blogNewsRepository)
        {
            base._iBaseRepository = blogNewsRepository;
            _blogNewsRepository = blogNewsRepository;
        }
    }
}
