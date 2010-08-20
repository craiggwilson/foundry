using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Foundry.Website.Controllers;
using Foundry.Website.Models;

namespace Foundry.Website
{
    public static class ViewExtensions
    {
        public static ViewResult WithMessage(this ViewResult result, Controller from, string text, ViewMessageType type)
        {
            from.ViewData[FoundryController.VIEW_MESSAGE_KEY] = new ViewMessageModel(text, type);
            return result;
        }
    }
}