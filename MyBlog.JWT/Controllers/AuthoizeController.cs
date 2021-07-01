using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyBlog.IService;
using MyBlog.JWT.Utility.ApiResult;
using MyBlog.JWT.Utility.Md5;
using MyBlog.Model;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyBlog.JWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthoizeController : ControllerBase
    {
        private readonly IWriterInfoService _writerInfoService;
        public AuthoizeController(IWriterInfoService writerInfoService)
        {
            _writerInfoService = writerInfoService;
        }
        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="name"></param>
        /// <param name="userpwd"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<ActionResult<ApiResult>> Login(string name, string userpwd)
        {
            #region 数据校验
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(userpwd))
            {
                return ApiResultHelper.Error("用户名或者密码错误！");
            }
            #endregion

            //加密之后的密钥
            string pwd = MD5Helper.MD5Encrypt32(userpwd);

            var writer = await _writerInfoService.QueryAsync(T => T.Name == name && T.UserPwd == pwd);
            if (writer != null && writer.Count == 1)
            {
                var claims = new Claim[]
                                      {
                                          //此处不可以存放敏感信息
                                          new Claim("Id", writer[0].Id.ToString()),
                                          new Claim("username", writer[0].UserName),
                                          new Claim(ClaimTypes.Name, writer[0].Name)
                                      };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SDMC-CJAS1-SAD-DFSFA-SADHJVF-VF"));
                //issuer代表颁发Token的Web应用程序，audience是Token的受理者
                var token = new JwtSecurityToken(
                    issuer: "http://localhost:6060",
                    audience: "http://localhost:5000",
                    claims: claims,
                    notBefore: DateTime.Now,
                    expires: DateTime.Now.AddHours(1),            //过期时间
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return ApiResultHelper.Success(jwtToken);
            }
            else
            {
                return ApiResultHelper.Error("当前用户不存在！");
            }
        }
    }
}
