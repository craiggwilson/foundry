using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundry.SourceControl;
using Foundry.Website.Models.Repository;
using Foundry.Website.Models;

namespace Foundry.Website.Controllers
{
    [Authorize]
    public partial class RepositoryController : FoundryController
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

        [HttpPost]
        public virtual ActionResult Create(CreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.ProviderNames = _sourceControlManager.ProviderNames.Select(x => new SelectListItem { Text = x, Value = x, Selected = x == model.SelectedProviderName });
                return View(model);
            }

            _sourceControlManager.CreateUserRepository(FoundryUser.Id, model.SelectedProviderName, FoundryUser.Name + "/" + model.Name);

            return RedirectToAction(MVC.Dashboard.Index())
                .WithMessage(this, "Repository successfully created", ViewMessageType.Info);
        }
    }
}