using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.IService;
using MyBlog.WebApi.Utility.ApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test_2_WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class Test2Controller : ControllerBase
    {
        private readonly IWriterInfoService _writerInfoService;
        public Test2Controller(IWriterInfoService writerInfoService)
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
    }
}
