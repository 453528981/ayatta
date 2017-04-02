using Ayatta;
using System;
using Ayatta.Domain;
using Ayatta.Storage;
using Ayatta.Extension;
using Ayatta.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;

namespace Ayatta.Web.Controllers
{
    [Route("account")]

    public class AccountController : BaseController
    {
        /// <summary>
        /// 用于存放登录失败次数的Session Key
        /// </summary>
        private static readonly string SignInErrorCountKey = "sign-in-error-count";

        public AccountController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger<AccountController> logger) : base(defaultStorage, defaultCache, logger)
        {
        }

        #region 登录/退出

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="redirect">登录成功后跳转Url</param>
        /// <returns></returns>
        [HttpGet("/sign-in")]
        public IActionResult Index(string redirect = null)
        {

            var model = new Model<int>();
            model.Data = GetIntSession(SignInErrorCountKey) ?? 0;
            return View(model);
        }

        /// <summary>
        /// 通用AJAX登录
        /// </summary>
        /// <param name="uid">用户名/绑定邮箱/绑定手机号码</param>
        /// <param name="pwd">登录密码</param>
        /// <param name="captcha">验证码</param>
        /// <param name="remember">记住帐号</param>
        /// <param name="redirect">登录成功后跳转Url</param>
        /// <returns></returns>
        [HttpPost("/sign-in")]
        public IActionResult Index(string uid, string pwd, string captcha = "", bool remember = false, string redirect = null)
        {
            var result = new Result<string, int>();

            if (uid.IsNullOrEmpty() || pwd.IsNullOrEmpty())
            {
                result.Message = "请输入用户名和密码！";
                return Json(result);
            }

            var count = GetIntSession(SignInErrorCountKey) ?? 0;

            if (count > 3)
            {
                if (captcha.IsNullOrEmpty())
                {
                    result.Extra = count;
                    result.Message = "请输入验证码！";
                    return Json(result);
                }
                var valid = true;// ValidateCaptcha("sign-in", captcha);//验证验证码是否正确
                if (!valid)
                {
                    result.Extra = count;//登录失败次数
                    result.Message = "验证码不正确，请核对后重新输入！";
                    return Json(result);
                }
            }
            var user = DefaultStorage.UserGet(uid, pwd);
            if (user != null)
            {
                result.Status = true;
                result.Data = redirect;

                SignIn(user);

                RemoveSession("sign-in-captcha");
                RemoveSession(SignInErrorCountKey);
            }
            else
            {
                result.Extra = count;
                result.Message = "用户名或密码不正确，请重新输入！";

                SetSession(SignInErrorCountKey, ++count);
            }
            return Json(result);
        }

        /// <summary>
        /// 登录验证码
        /// </summary>
        [HttpGet("/sign-in/captcha")]
        public void SignInCaptcha()
        {
            //var key = "sign-in-captcha";

            //RenderCaptcha("sign-in-captcha");
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="redirect">退出后跳转Url</param>
        /// <returns></returns>
        [HttpGet("/sign-out")]
        public IActionResult SignOut(string redirect = null)
        {

            base.SignOut();
            return Redirect(!string.IsNullOrEmpty(redirect) ? redirect : "/");
        }
        #endregion

        #region 第三方账号登录
        /// <summary>
        /// 使用第三方账号登录
        /// </summary>
        /// <param name="id">第三方站点OAuthId</param>
        /// <param name="redirect">登录成功后跳转Url</param>
        /// <returns></returns>
        [HttpGet("/oauth/{id}")]
        public IActionResult OAuth(string id = "weixin", string redirect = null)
        {
            var provider = GetAuthProvider(id);

            if (provider != null)
            {
                var url = provider.GetLoginUri(redirect);
                return Redirect(url);
            }
            Logger.LogWarning("OAuthProvider is null");
            return Redirect("/");
        }

        /// <summary>
        /// 用户在第三方站点登录成功并授权完成后会重定向到该地址
        /// </summary>
        /// <param name="id">第三方站点OAuthId</param>
        /// <returns></returns>
        [HttpGet("/oauth/callback/{id}")]
        public IActionResult Callback(string id = "weixin")
        {
            var provider = GetAuthProvider(id);
            if (provider != null)
            {
                var result = provider.Callback(Request.Query);
                if (result && result.Data.Id > 0)
                {
                    var user = DefaultStorage.UserGet(result.Data.Id);
                    if (user != null)
                    {
                        /*
                        if (user.Mobile.IsEmpty())
                        {

                            return View();//绑定
                        }
                        else
                        {
                            SignIn(user);
                        }
                        */
                        SignIn(user);
                    }

                    var redirect = provider.State;
                    return Redirect(!string.IsNullOrEmpty(redirect) ? redirect : "/");
                }

                Logger.LogError(1, "OAuth {provider} Callback 处理失败 {message}", result.Data.Provider, result.Message);

                return Redirect("/oauth/error");
            }
            return NotFound();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("/oauth/error")]
        public IActionResult Result(string code = null)
        {
            return Content(code);
        }

        /// <summary>
        /// 第三方帐号绑定手机号
        /// </summary>            
        /// <returns></returns>
        [HttpGet("bind")]
        public IActionResult Bind(string redirect = null)
        {
            return View();
        }

        /// <summary>
        /// 第三方帐号绑定手机号
        /// </summary>            
        /// <returns></returns>
        [HttpPost("bind")]
        public IActionResult Bind(string uid, string captcha)
        {
            return View();
        }

        #endregion

        #region 注册
        /// <summary>
        /// 使用手机号码注册
        /// </summary>
        [HttpGet("/sign-up")]
        public IActionResult SignUp()
        {
            return View();
        }

        /// <summary>
        /// 使用手机号码注册
        /// </summary>
        /// <param name="uid">手机号</param>
        /// <param name="pwd">登录密码</param>        
        /// <param name="captcha">短信验证码</param>
        /// <param name="traceCode">跟踪代码</param>
        /// <returns></returns>
        [HttpPost("/sign-up")]
        public IActionResult SignUp(string uid, string pwd, string captcha, string traceCode = "")
        {
            var time = DateTime.Now;
            var result = new Result<string>();
            if (!uid.IsMobile())
            {
                result.Message = "请输入正确的手机号码！";
                return Json(result);
            }
            if (!pwd.IsPassword())
            {
                result.Message = "6-18个字符（至少包含两个字母、两个数字、一个特殊字符）！";
                return Json(result);
            }
            /*
            var auth = Extra.Value.AuthSmsGet(authId, mobile, 0, authCode);
            if (auth == null || auth.Status || mobile != auth.Mobile)
            {
                result.Message = "短信验证码不正确或已过期！";
                return Json(result);
            }
            */
            var userId = DefaultStorage.UserIdGet(uid);
            if (userId > 0)
            {
                result.Message = "该手机号码已被使用，请换一个！";
                return Json(result);
            }


            var user = new User();
            var profile = new UserProfile();
            //var extra = new UserExtra();

            user.Guid = "adfafadf";
            user.Name = uid;
            user.Email = string.Empty;
            user.Mobile = uid;
            user.Nickname = string.Empty;
            user.Password = pwd;

            user.Role = UserRole.Buyer;
            user.Grade = UserGrade.None;
            user.Limitation = UserLimitation.None;
            user.Permission = UserPermission.None;

            user.Avatar = string.Empty;
            user.Status = UserStatus.Normal;

            user.AuthedOn = null;
            user.CreatedBy = string.Empty;
            user.CreatedOn = time;
            user.ModifiedBy = string.Empty;
            user.ModifiedOn = time;

            profile.Code = string.Empty;
            profile.Name = string.Empty;
            profile.Gender = Gender.Secrect;
            profile.Marital = Marital.Secrect;
            profile.Birthday = null;
            profile.Phone = string.Empty;
            profile.Mobile = string.Empty;
            profile.RegionId = string.Empty;
            profile.Street = string.Empty;

            profile.SignUpIp = string.Empty;
            profile.SignUpBy = 1;
            profile.TraceCode = traceCode;
            profile.LastSignInIp = string.Empty;
            profile.LastSignInOn = time;

            user.Profile = profile;
            //user.Extra = extra;

            var id = DefaultStorage.UserCreate(user);
            if (id > 0)
            {
                user.Id = id;
                result.Status = true;
                result.Data = id.ToString();
                SignIn(user);
            }
            else
            {
                result.Message = "注册失败，请稍后再试！";
            }

            return Json(result);
        }

        #endregion       

        #region 确认帐号
        /// <summary>
        /// 密码重置-确认帐号
        /// </summary>            
        /// <returns></returns>
        [HttpGet("confirm")]
        public IActionResult Confirm()
        {
            return View();
        }

        /// <summary>
        /// 密码重置-确认帐号
        /// </summary>
        /// <param name="uid">用户名/绑定邮箱/绑定手机号码</param>     
        /// <param name="captcha">验证码</param>            
        /// <returns></returns>
        [HttpPost("confirm")]
        public IActionResult Confirm(string uid, string captcha)
        {
            var result = new Status<string>();
            var captchaValid = false;// ValidateCaptcha("account-confirm", captcha);//验证验证码是否正确
            if (!captchaValid)
            {
                result.Code = 1;
                result.Message = "验证码不正确，请核对后重新输入。";
                return Json(result);
            }
            var id = DefaultStorage.UserIdGet(uid);//验证输入帐号是否存在
            if (id < 1)
            {
                result.Code = 2;
                result.Message = "您输入的账号不存在，请核对后重新输入。";
                return Json(result);
            }
            var user = DefaultStorage.UserGet(id);
            if (user != null && user.Email.IsNullOrEmpty() && user.Mobile.IsNullOrEmpty())
            {
                result.Code = 3;
                result.Message = "您输入的账号未绑定任何邮箱或手机号，无法重置密码。";
                return Json(result);
            }
            SetSession("account-current", id);//设置session

            result.Code = 0;
            return Json(result);
        }

        [HttpGet("confirm/captcha")]
        public void SignUpCaptcha()
        {
            //RenderCaptcha("account-confirm");
        }
        #endregion

        #region 验证身份
        /// <summary>
        /// 密码重置-验证身份
        /// </summary>            
        /// <returns></returns>
        [HttpGet("verify")]
        public IActionResult Verify()
        {
            var model = new Model<User>();
            var id = GetIntSession("account-current");
            if (id.HasValue && id.Value > 0)
            {
                model.Data = DefaultStorage.UserGet(id.Value);
            }
            return View(model);
        }

        /// <summary>
        /// 密码重置-验证身份
        /// </summary>
        /// <param name="uid">用户名/绑定邮箱/绑定手机号码</param>     
        /// <param name="captcha">验证码</param>            
        /// <returns></returns>
        [HttpPost("verify")]
        public IActionResult Verify(string uid, string captcha)
        {
            var result = new Status<string>();

            return Json(result);
        }

        /// <summary>
        /// 密码重置-验证身份 发送邮箱/手机验证码
        /// </summary>
        /// <returns></returns>

        [HttpPost("/account/verify/captcha")]
        public IActionResult SendCaptcha(bool toMobile)
        {
            var result = new Result<string>();
            var id = GetIntSession("account-current");
            if (id.HasValue && id.Value > 0)
            {
                var user = DefaultStorage.UserGet(id.Value);
                if (toMobile)
                {
                    if (user.Mobile.IsNullOrEmpty())
                    {
                        result.Message = "未绑定有效手机号码，请选择其它方式进行身份验证。";
                    }
                    // todo 发短信
                }
                else
                {
                    if (user.Email.IsNullOrEmpty())
                    {
                        result.Message = "未绑定有效邮箱帐号，请选择其它方式进行身份验证。";
                    }

                    // todo 发邮件
                }
            }
            else
            {
                result.Message = "停留时间过长，请重新进行身份验证。";
            }
            return Json(result);
        }

        #endregion

        #region 设置新密码
        /// <summary>
        /// 密码重置-设置新密码
        /// </summary>            
        /// <returns></returns>
        [HttpGet("pwd-reset")]
        public IActionResult Reset()
        {
            return View();
        }

        /// <summary>
        ///  密码重置-设置新密码
        /// </summary>
        /// <param name="pwd">新密码</param>            
        /// <returns></returns>
        [HttpPost("pwd-reset")]
        public IActionResult Reset(string pwd)
        {
            var result = new Result<string>();
            /*
                var id = DefaultStorage.UserIdGet(uid);
                if (id<0)
                {
                    result.Message = "您输入的账号不存在，请核对后重新输入。";
                    return Json(result);
                }
                var user=DefaultStorage.UserGet(id);
                if(user!=null && (user.Email.IsNullOrEmpty()||user.Mobile.IsNullOrEmpty()))
                {
                    result.Message = "您输入的账号未绑定任何邮箱或手机号，无法重置密码。";
                    return Json(result);
                }   
                result.Status=true;    
                */
            return Json(result);
        }
        #endregion

        /// <summary>
        /// 检查用户名/绑定手机/绑定邮箱是否已存在
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpPost("uid-exist")]
        public IActionResult UidExist(string uid)
        {
            var exist = DefaultStorage.UserIdGet(uid) > 0;
            return Json(exist);
        }
    }
}