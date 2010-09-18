using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundry.SourceControl;
using Foundry.Website.Models.Repository;
using Foundry.Website.Models;
using Foundry.Security;
using Foundry.Domain;

namespace Foundry.Website.Controllers
{
    [Authorize]
    public partial class RepositoryController : FoundryController
    {
        private readonly ISourceControlManager _sourceControlManager;
        private readonly IDomainRepository<Repository> _repoRepository;

        public RepositoryController(ISourceControlManager sourceControlManager, IDomainRepository<Repository> repoRepository)
        {
            _sourceControlManager = sourceControlManager;
            _repoRepository = repoRepository;
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
        public virtual ActionResult Create(CreateViewModel model, FoundryUser user)
        {
            if (!ModelState.IsValid)
            {
                model.ProviderNames = _sourceControlManager.ProviderNames.Select(x => new SelectListItem { Text = x, Value = x, Selected = x == model.SelectedProviderName });
                return View(model);
            }

            _sourceControlManager.CreateUserRepository(user.Id, model.SelectedProviderName, user.Name, model.Name);

            return RedirectToAction(MVC.Dashboard.Index())
                .WithMessage(this, "Repository successfully created", ViewMessageType.Info);
        }

        [HttpGet]
        public virtual ActionResult Index(string account, string project)
        {
            var repo = _repoRepository.Single(r => r.AccountName == account && r.ProjectName == project);

            var commits = _sourceControlManager.GetCommits(repo.SourceControlProvider, repo.AccountName, repo.ProjectName, 1, 20);

            var model = new IndexViewModel()
            {
                AccountName = account,
                ProjectName = project,
                Commits = commits
            };

            return View(model);
        }

        [HttpGet]
        public virtual ActionResult Source(string account, string project)
        {
            return View();
        }
    }
}