using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundry.Security;

namespace Foundry.Website.Extensions
{
    public class AuthorizationInformationModelBinder : IModelBinder
    {
        private IAuthorizationService _authorizationService;

        public AuthorizationInformationModelBinder(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var user = controllerContext.HttpContext.User as FoundryUser;
            if (user == null)
                return null;

            return _authorizationService.GetAuthorizationInformation(user.Id);
        }
    }
}