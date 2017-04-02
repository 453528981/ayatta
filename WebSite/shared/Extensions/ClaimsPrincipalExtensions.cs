using System;
using System.Linq;
using System.Security.Claims;

namespace Ayatta.Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Identity AsIdentity(this ClaimsPrincipal principal)
        {
            if (principal.Identity.IsAuthenticated)
            {
                var id = 0;
                string name;
                if (!string.IsNullOrEmpty(principal.Identity.Name))
                {
                    id = Convert.ToInt32(principal.Identity.Name);
                }
                name = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;

                return new Identity(id, name);
            }
            return new Identity(0);
        }
    }
}
