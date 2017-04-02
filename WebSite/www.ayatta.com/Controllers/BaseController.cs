using System;
using System.Linq;
using Ayatta.OAuth;
using Ayatta.Domain;
using Ayatta.Storage;
using System.Security.Claims;
using System.Threading.Tasks;
using Ayatta.Web.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Ayatta.Web.Controllers
{

    public abstract class BaseController : AbstractController
    {
        public new Identity User
        {
            get { return base.User.AsIdentity(); }
        }
        protected BaseController(DefaultStorage defaultStorage, IDistributedCache defaultCache, ILogger logger) : base(defaultStorage, defaultCache, logger)
        {
        }

        #region SignIn/SignOut
        protected void SignIn(User user)
        {

            var ci = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            ci.AddClaim(new Claim(ClaimTypes.Name, user.Id.ToString()));
            ci.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Name));

            var cp = new ClaimsPrincipal(ci);

            HttpContext.Authentication.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, cp);
        }

        protected void SignOut()
        {
            HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        #endregion

        #region OAuthProvider
        protected IAuthProvider GetAuthProvider(string id)
        {
            var providers = DefaultStorage.OAuthProviderList();
            var provider = providers.FirstOrDefault(x => x.Id == id);

            if (provider == null) return null;

            var p = AuthProviderFactory.Create(provider);
            if (p != null)
            {
                p.Authorized += Authorized;
            }
            return p;
        }

        /// <summary>
        /// 用户在第三方平台登录成功并授权后触发
        /// 生成系统帐号
        /// </summary>
        /// <param name="o"></param>
        /// <returns>用户Id</returns>
        private int Authorized(UserOAuth o)
        {
            int userId;
            var openUser = DefaultStorage.UserOAuthGet(o.Provider, o.OpenId);
            if (openUser != null)
            {
                userId = openUser.Id;
                Task.Factory.StartNew(() =>
                {
                    o.Id = userId;
                    o.CreatedOn = DateTime.Now;

                    try
                    {
                        DefaultStorage.UserOAuthUpdate(o);
                    }
                    catch (Exception e)
                    {
                        Logger.LogError(1, "OAuth {provider} Authorized UserOAuthUpdate 失败 UserId({uid}} {msg}", o.Provider, userId, e.Message);
                    }
                });

            }
            else
            {
                userId = DefaultStorage.UserIdGet(o.OpenId, o.Provider);
                if (userId == 0)
                {
                    var user = new User();
                    var profile = new UserProfile();

                    var now = DateTime.Now;

                    user.Guid = Guid.NewGuid().ToString("N");
                    user.Name = o.OpenId;
                    user.Email = string.Empty;
                    user.Mobile = string.Empty;
                    user.Nickname = o.OpenName ?? "";
                    user.Password = string.Empty;
                    user.Role = UserRole.Buyer;
                    user.Grade = UserGrade.One;
                    user.Limitation = UserLimitation.None;
                    user.Permission = UserPermission.None;
                    user.Avatar = string.Empty;
                    user.Status = UserStatus.Normal;
                    user.AuthedOn = null;
                    user.CreatedBy = o.Provider;
                    user.CreatedOn = now;
                    user.ModifiedBy = "";
                    user.ModifiedOn = now;

                    profile.Code = string.Empty;
                    profile.Name = string.Empty;
                    profile.Gender = Gender.Secrect;
                    profile.Marital = Marital.Secrect;
                    profile.Birthday = null;
                    profile.Phone = string.Empty;
                    profile.Mobile = string.Empty;
                    profile.RegionId = string.Empty;
                    profile.Street = string.Empty;
                    profile.SignUpIp = "";
                    profile.SignUpBy = 0;
                    profile.TraceCode = "";
                    profile.LastSignInIp = "";
                    profile.LastSignInOn = now;

                    user.Profile = profile;

                    userId = DefaultStorage.UserCreate(user);
                }
                if (userId > 0)
                {
                    Task.Factory.StartNew(() =>
                    {
                        o.Id = userId;
                        o.CreatedOn = DateTime.Now;
                        try
                        {
                            DefaultStorage.UserOAuthCreate(o);
                        }
                        catch (Exception e)
                        {
                            Logger.LogError(1, "OAuth {provider} Authorized UserOAuthCreate 失败 UserId({uid}} {msg}", o.Provider, userId, e.Message);
                        }
                    });
                }
            }
            return userId;
        }
        #endregion
    }

}