﻿using System;
using System.Linq;
using System.Security.Claims;

namespace Ayatta.Web.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Identity AsIdentity(this ClaimsPrincipal principal)
        {

            if (principal != null && principal.Identity.IsAuthenticated)
            {
                var id = 0;
                string guid, name;
                if (!string.IsNullOrEmpty(principal.Identity.Name))
                {
                    id = Convert.ToInt32(principal.Identity.Name);
                }
                guid = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Hash).Value;
                name = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
                
                return new Identity(id,guid, name);
            }
            return new Identity(0);
        }
    }
}
