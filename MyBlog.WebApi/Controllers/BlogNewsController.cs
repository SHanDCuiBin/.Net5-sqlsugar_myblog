using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IService;
using MyBlog.Model;
using MyBlog.Model.DTO;
using MyBlog.WebApi.Utility.ApiResult;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BlogNewsController : ControllerBase
    {
        private readonly IBlogNewsService _blogNewsService;
        public BlogNewsController(IBlogNewsService blogNewsService)
        {
            this._blogNewsService = blogNewsService;
        }

        /// <summary>
        /// 获取所有文章列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("BlogNews")]
        public async Task<ApiResult> GetBlogNews([FromServices] IMapper mapper)
        {
            int id = Convert.ToInt32(this.User.FindFirst("Id").Value);
            var data = await _blogNewsService.QueryAsync(c => c.WriterId == id);    //仅查询自己的博客文章

            if (data == null)
            {
                return ApiResultHelper.Error("没有更多的文章!");
            }
            else
            {
                List<BlogNewsDTO> dt = new List<BlogNewsDTO>();

                foreach (var item in data)
                {
                    dt.Add(mapper.Map<BlogNewsDTO>(item));
                }
                return ApiResultHelper.Success(dt);
            }
        }

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="title">文章标题</param>
        /// <param name="content">文章正文</param>
        /// <param name="typeid">文章类型</param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ApiResult> Creare(string title, string content, int typeid)
        {
            #region 数据合理性校验
            if (string.IsNullOrEmpty(title))
            {
                return ApiResultHelper.Error("文章标题为空!");
            }

            if (string.IsNullOrEmpty(content))
            {
                return ApiResultHelper.Error("文章正文为空!");
            }

            if (string.IsNullOrEmpty(content))
            {
                return ApiResultHelper.Error("文章类型为空!");
            }

            List<BlogNews> blogNewLis = await _blogNewsService.QueryAsync(T => T.Title == title && T.TypeId == typeid);
            if (blogNewLis != null && blogNewLis.Count > 0)
            {
                return ApiResultHelper.Error("文章类型，标题重复!");
            }

            #endregion

            BlogNews blogNews = new BlogNews
            {
                BrowseCount = 0,
                Content = content,
                LikeCount = 0,
                Time = DateTime.Now,
                Title = title,
                TypeId = typeid,
                WriterId = Convert.ToInt32(this.User.FindFirst("Id").Value)
            };

            bool b = await _blogNewsService.CreateAsync(blogNews);
            if (b)
            {
                return ApiResultHelper.Success(blogNews);
            }
            else
            {
                return ApiResultHelper.Error("添加失败，服务器发生错误!");
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">文章id</param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public async Task<ApiResult> Delete(int id)
        {
            bool b = await _blogNewsService.DeleteAsync(id);
            if (b)
            {
                return ApiResultHelper.Success(b);
            }
            else
            {
                return ApiResultHelper.Error("删除失败");
            }
        }

        /// <summary>
        /// 文章修改
        /// </summary>
        /// <param name="id">文章id</param>
        /// <param name="title">新文章标题</param>
        /// <param name="content">新文章内容</param>
        /// <param name="typeid">新文章类型</param>
        /// <returns></returns>
        [HttpPut("Edit")]
        public async Task<ApiResult> Edit(int id, string title, string content, int typeid)
        {
            #region 数据合理性校验
            if (string.IsNullOrEmpty(title))
            {
                return ApiResultHelper.Error("文章标题为空!");
            }

            if (string.IsNullOrEmpty(content))
            {
                return ApiResultHelper.Error("文章正文为空!");
            }

            if (string.IsNullOrEmpty(content))
            {
                return ApiResultHelper.Error("文章类型为空!");
            }

            BlogNews blogNew = await _blogNewsService.FindAsync(id);   //  更新要在查询的基础上，修改实体之后，再进行更新
            if (blogNew != null)
            {
                blogNew.Title = title;
                blogNew.Content = content;
                blogNew.Time = DateTime.Now;
                blogNew.TypeId = typeid;

                bool b = await _blogNewsService.EditAsync(blogNew);
                if (b)
                {
                    return ApiResultHelper.Success(blogNew);
                }
                else
                {
                    return ApiResultHelper.Error("更新失败，服务器发生错误!");
                }
            }
            else
            {
                return ApiResultHelper.Error("不存在该文章，无法修改"); ;
            }
            #endregion
        }

        [HttpGet("BlogNewsPage")]
        public async Task<ApiResult> GetBlogNewsPage([FromServices] IMapper mapper, int page, int size)
        {
            RefAsync<int> total = 0;

            var blognews = await _blogNewsService.QueryAsync(page, size, total);

            try
            {
                var data = mapper.Map<List<BlogNewsDTO>>(blognews);
                return ApiResultHelper.Success(data, total);
            }
            catch (Exception)
            {
                return ApiResultHelper.Error("映射错误");
            }
        }
    }
}
