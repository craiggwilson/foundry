using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Foundry.SourceControl.GitIntegration
{
    public class GitRouteHandler : System.Web.Routing.IRouteHandler
    {
        public IHttpHandler GetHttpHandler(System.Web.Routing.RequestContext requestContext)
        {
            return new GitHttpHandler();
        }
    }
}