using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IService;
using MyBlog.WebApi.Utility.ApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_1_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class Test1Controller : ControllerBase
    {
        private readonly IWriterInfoService _writerInfoService;

        public Test1Controller(IWriterInfoService writerInfoService)
        {
            this._writerInfoService = writerInfoService;
        }

        /// <summary>
        /// 查找指定的用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("FindWriter")]
        public async Task<ActionResult<ApiResult>> FindWriter(int id)
        {
            var writer = await _writerInfoService.FindAsync(id);
            return ApiResultHelper.Success(writer);
        }

        /// <summary>
        /// 获取响应字符串
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public string GetTestString()
        {
            return "Hello Test_1";
        }
    }
}
