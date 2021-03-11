using FlightCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace FlightCenterWebApi.Controllers
{
    public class AirlineCompanyAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.Request.Headers.Authorization.Parameter == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                return;
            }

            string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
            string decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
            string[] userInfo = decodedAuthenticationToken.Split(':');
            string username = userInfo[0];
            string password = userInfo[1];

            LoginToken<AirlineCompany> user = FlyingCenterSystem.GetFlyingCenterSystem().AttemptLoginAirlineCompany(username, password);

            if (user != null)
            {
                actionContext.Request.Properties["User"] = user;
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            }
        }
    }
}