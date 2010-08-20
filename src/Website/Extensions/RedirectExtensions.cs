using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Foundry.Website.Controllers;
using Foundry.Website.Models;

namespace Foundry.Website
{
    public static class RedirectExtensions
    {
        public static RedirectResult WithMessage(this RedirectResult result, Controller from, string text, ViewMessageType type)
        {
            from.TempData[FoundryController.VIEW_MESSAGE_KEY] = new ViewMessageModel(text, type);
            return result;
        }

        public static RedirectToRouteResult WithMessage(this RedirectToRouteResult result, Controller from, string text, ViewMessageType type)
        {
            from.TempData[FoundryController.VIEW_MESSAGE_KEY] = new ViewMessageModel(text, type);
            return result;
        }
    }
}