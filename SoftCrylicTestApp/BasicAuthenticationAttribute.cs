using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace SoftCrylicTestApp
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute, IDisposable
    {
        ~BasicAuthenticationAttribute() { Dispose(); }

        public virtual void Dispose()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.SuppressFinalize(this);
        }

        public override async System.Threading.Tasks.Task OnAuthorizationAsync(HttpActionContext context, System.Threading.CancellationToken token)
        {
            try
            {
                KeyValuePair<string, IEnumerable<string>> authHeader = context.Request.Headers.SingleOrDefault(x => x.Key == "Authorization");
                if (!string.IsNullOrWhiteSpace(authHeader.Key))
                {
                    string authToken = Convert.ToString(authHeader.Value.SingleOrDefault());
                    if (authToken.Equals("org1_abcd123"))
                    {
                        System.Security.Principal.GenericPrincipal principal = new System.Security.Principal.GenericPrincipal(new System.Security.Principal.GenericIdentity("org1_abcd123"), null);
                        System.Threading.Thread.CurrentPrincipal = principal;
                        await base.OnAuthorizationAsync(context, new System.Threading.CancellationToken());
                    }
                    else
                        context.Response = new HttpResponseMessage((HttpStatusCode)(int)HttpStatusCode.Unauthorized) { ReasonPhrase = "Unauthorized" };
                }
                else
                    context.Response = new HttpResponseMessage((HttpStatusCode)(int)HttpStatusCode.Forbidden) { ReasonPhrase = "Forbidden" };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }
    }
}