using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Association.Security
{
    public class AuthorizeRole : AuthorizeAttribute
    {

        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            var request = filterContext.HttpContext.Request;
            var url = new UrlHelper(filterContext.RequestContext);
            var urlReferer = request.UrlReferrer != null
                ? request.UrlReferrer.ToString()
                : String.Empty;
            var signInUrl = url.Action("Login", "Account", new { Area = "Account", ReturnUrl = urlReferer });
            var accessDeniedUrl = url.Action("Error", "Error");


            //Vérification si l'utilisateur est authentifié
            if (!request.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult(signInUrl);
            }
            //Vérification que l'utilisateur a le rôle
            else if (!String.IsNullOrWhiteSpace(base.Roles))
            {
                var isRoleError = true;
                var rolesAllowed = base.Roles.Split(',');
                //authenticated and we have some roles to check against
                var user = filterContext.HttpContext.User;
                if (user != null && rolesAllowed.Any())
                {
                    foreach (var role in rolesAllowed)
                    {
                        if (user.IsInRole(role))
                        {
                            isRoleError = false;
                        }
                    }
                }

                if (isRoleError)
                {
                    filterContext.Result = new RedirectResult(accessDeniedUrl);
                }

            }

        }

    }
}
