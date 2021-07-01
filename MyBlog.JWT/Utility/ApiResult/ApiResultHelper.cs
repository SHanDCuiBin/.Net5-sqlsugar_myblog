using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.JWT.Utility.ApiResult
{
    public static class ApiResultHelper
    {
        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="data">获取数据</param>
        /// <returns></returns>
        public static ApiResult Success(dynamic data)
        {
            return new ApiResult
            {
                Code = 200,
                Msg = "操作成功！",
                Data = data,
                Total = 0
            };
        }

        /// <summary>
        /// 操作成功
        /// </summary>
        /// <param name="data">获取数据</param>
        /// <param name="total">若分页查询，分页总数</param>
        /// <returns></returns>
        public static ApiResult Success(dynamic data, RefAsync<int> total)
        {
            return new ApiResult
            {
                Code = 200,
                Msg = "操作成功！",
                Data = data,
                Total = total
            };
        }

        /// <summary>
        /// 操作错误
        /// </summary>
        /// <param name="msg">错误信息描述</param>
        /// <returns></returns>
        public static ApiResult Error(string msg)
        {
            return new ApiResult
            {
                Code = 500,
                Msg = msg,
                Data = null,
                Total = 0
            };
        }
    }
}
