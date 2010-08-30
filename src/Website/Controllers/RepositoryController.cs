using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundry.SourceControl;
using Foundry.Website.Models.Repository;

namespace Foundry.Website.Controllers
{
    public partial class RepositoryController : Controller
    {
        private readonly ISourceControlManager _sourceControlManager;

        public RepositoryController(ISourceControlManager sourceControlManager)
        {
            _sourceControlManager = sourceControlManager;
        }

        [HttpGet]
        public virtual ActionResult Create()
        {
            var model = new CreateViewModel()
            {
                ProviderNames = _sourceControlManager.ProviderNames.Select(x => new SelectListItem { Text = x, Value = x })
            };

            return View(model);
        }



    }
}