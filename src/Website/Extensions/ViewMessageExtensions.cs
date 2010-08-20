using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;


using Foundry.Website.Controllers;
using Foundry.Website.Models;

namespace Foundry.Website
{
    public static class ViewMessageExtensions
    {
        public static void RenderViewMessage(this HtmlHelper html)
        {
            if (!html.ViewData.ContainsKey(FoundryController.VIEW_MESSAGE_KEY))
                return;

            var msg = html.ViewData[FoundryController.VIEW_MESSAGE_KEY] as ViewMessageModel;
            if (msg == null)
                return;

            html.RenderPartial("ViewMessage", msg);
        }
    }
}