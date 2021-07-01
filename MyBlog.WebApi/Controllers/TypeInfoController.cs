using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IService;
using MyBlog.Model;
using MyBlog.WebApi.Utility.ApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TypeInfoController : ControllerBase
    {
        private readonly ITypeInfoService _typeInfoService;
        public TypeInfoController(ITypeInfoService typeInfoService)
        {
            this._typeInfoService = typeInfoService;
        }

        /// <summary>
        /// 获取所有文章的类型
        /// </summary>
        /// <returns></returns>
        [HttpGet("Types")]
        public async Task<ActionResult<ApiResult>> GetTypes()
        {
            var data = await _typeInfoService.QueryAsync();
            if (data != null && data.Count > 0)
            {
                return ApiResultHelper.Success(data);
            }
            else
            {
                return ApiResultHelper.Error("未查询到结果");
            }
        }

        /// <summary>
        /// 添加文章类型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<ActionResult<ApiResult>> Create(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return ApiResultHelper.Error("类型名称为空");
            }

            var typeinfo = await _typeInfoService.QueryAsync(T => T.Name == name);
            if (typeinfo != null && typeinfo.Count > 0)
            {
                return ApiResultHelper.Error("该文章类型已经存在");
            }

            TypeInfo typeInfo = new TypeInfo { Name = name };
            bool b = await _typeInfoService.CreateAsync(typeInfo);
            if (b)
            {
                return ApiResultHelper.Success(typeInfo);
            }
            else
            {
                return ApiResultHelper.Error("添加失败，服务器出现错误");
            }
        }

        /// <summary>
        /// 修改类型名称
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPut("Edit")]
        public async Task<ActionResult<ApiResult>> Edit(int id, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return ApiResultHelper.Error("文章类型为空");
            }

            var data = await _typeInfoService.FindAsync(id);
            if (data != null)
            {
                data.Name = name;
                if (await _typeInfoService.EditAsync(data))
                {
                    return ApiResultHelper.Success(data);
                }
                else
                {
                    return ApiResultHelper.Error("修改失败，服务器发生错误");
                }
            }
            else
            {
                return ApiResultHelper.Error("修改失败，该类型不存在");
            }
        }


        /// <summary>
        /// 删除指定的文章类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete")]
        public async Task<ActionResult<ApiResult>> Delete(int id)
        {
            if (await _typeInfoService.DeleteAsync(id))
            {
                return ApiResultHelper.Success(id);
            }
            else
            {
                return ApiResultHelper.Error("删除失败!");
            }
        }
    }
}
