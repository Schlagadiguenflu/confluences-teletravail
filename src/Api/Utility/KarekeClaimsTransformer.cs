using Api.Models;
using IdentityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Utility
{
    public class KarekeClaimsTransformer : Microsoft.AspNetCore.Authentication.IClaimsTransformation
    {

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity.IsAuthenticated)
            {
                Claim userId = principal.FindFirst("sub");

                if (principal.FindFirst("role") == null && userId != null)
                {

                    using (var context = new ConfluencesContext())
                    {
                        // Faire depuis API
                        var roles = context.AspNetUserRoles.Where(a => a.UserId == userId.Value).Select(r => r.RoleId);
                        foreach (var role in roles)
                        {
                            ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(JwtClaimTypes.Role, role,
                                "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"));
                        }
                    }

                }
            }
            return await Task.FromResult(principal);
        }
    }
}
