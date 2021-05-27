using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.App.Extensions
{
    public static class RazorExtensions
    {
        public static string DocumentFormat(this RazorPage page, int PersonType, string document)
        {
            return PersonType == 1 ? Convert.ToUInt64(document).ToString(@"000\.000\.000\-00") : Convert.ToUInt64(document).ToString(@"00\.000\.000\/0000\-00");
        }

        public static string MarcarOpcao(this RazorPage page, int tipoPessoa, int valor)
        {
            return tipoPessoa == valor ? "checked" : "";
        }

        //public static bool IfClaim(this RazorPage page, string claimName, string claimValue)
        //{
        //    return CustomAuthorization.ValidarClaimsUsuario(page.Context, claimName, claimValue);
        //}

        //public static string IfClaimShow(this RazorPage page, string claimName, string claimValue)
        //{
        //    return CustomAuthorization.ValidarClaimsUsuario(page.Context, claimName, claimValue) ? "" : "disabled";
        //}

        //public static IHtmlContent IfClaimShow(this IHtmlContent page, HttpContext context, string claimName, string claimValue)
        //{
        //    return CustomAuthorization.ValidarClaimsUsuario(context, claimName, claimValue) ? page : null;
        //}
    }
}
