using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IService;
using MyBlog.Model;
using MyBlog.Model.DTO;
using MyBlog.WebApi.Utility.ApiResult;
using MyBlog.WebApi.Utility.Md5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WriterInfoController : ControllerBase
    {
        private readonly IWriterInfoService _writerInfoService;
        public WriterInfoController(IWriterInfoService writerInfoService)
        {
            this._writerInfoService = writerInfoService;
        }

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="name">用户名</param>
        /// <param name="username">真实姓名</param>
        /// <param name="userpwd">用户密码</param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ActionResult<ApiResult>> Create(string name, string username, string userpwd)
        {
            #region 逻辑校验

            if (string.IsNullOrEmpty(name))
            {
                return ApiResultHelper.Error("用户名为空!");
            }

            if (string.IsNullOrEmpty(username))
            {
                return ApiResultHelper.Error("真实姓名为空");
            }

            if (string.IsNullOrEmpty(userpwd))
            {
                return ApiResultHelper.Error("用户密码为空");
            }

            var data = await _writerInfoService.QueryAsync(T => T.Name == name);
            if (data != null && data.Count > 0)
            {
                return ApiResultHelper.Error("该用户名已经存在!");
            }
            #endregion

            WriterInfo writerInfo = new WriterInfo
            {
                Name = name,                                          //用户名
                UserName = username,                                  //用户真实姓名
                UserPwd = MD5Helper.MD5Encrypt32(userpwd)             //用户密码 --Md5加密
            };

            if (await _writerInfoService.CreateAsync(writerInfo))
            {
                return ApiResultHelper.Success(writerInfo);
            }
            else
            {
                return ApiResultHelper.Error("添加失败，服务器错误!");
            }
        }

        /// <summary>
        /// 修改当前用户的 姓名
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut("Edit")]
        public async Task<ActionResult<ApiResult>> Edit(string username)
        {
            int id = Convert.ToInt32(this.User.FindFirst("Id").Value);

            var writer = await _writerInfoService.FindAsync(id);
            if (writer != null)
            {
                writer.UserName = username;
                if (await _writerInfoService.EditAsync(writer))
                {
                    return ApiResultHelper.Success(username);
                }
                else
                {
                    return ApiResultHelper.Error("操作失败，服务器错误");
                }
            }
            else
            {
                return ApiResultHelper.Error("操作失败，未获取到当前用户的信息");
            }
        }

        /// <summary>
        /// 查找指定的用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("FindWriter")]
        public async Task<ActionResult<ApiResult>> FindWriter([FromServices] IMapper mapper, int id)
        {
            var writer = await _writerInfoService.FindAsync(id);
            var dto = mapper.Map<WriterDTO>(writer);

            return ApiResultHelper.Success(dto);
        }

    }
}
