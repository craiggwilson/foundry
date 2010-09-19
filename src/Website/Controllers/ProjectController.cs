using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Foundry.SourceControl;
using Foundry.Website.Models;
using Foundry.Security;
using Foundry.Domain;
using Foundry.Website.Models.Project;

namespace Foundry.Website.Controllers
{
    [Authorize]
    public partial class ProjectController : FoundryController
    {
        private readonly ISourceControlManager _sourceControlManager;
        private readonly IDomainRepository<Project> _projectRepository;

        public ProjectController(ISourceControlManager sourceControlManager, IDomainRepository<Project> projectRepository)
        {
            _sourceControlManager = sourceControlManager;
            _projectRepository = projectRepository;
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

            _sourceControlManager.CreateUserProject(user.Id, model.SelectedProviderName, user.Name, model.Name, false);

            return RedirectToAction(MVC.Dashboard.Index())
                .WithMessage(this, "Project successfully created", ViewMessageType.Info);
        }

        [HttpGet]
        public virtual ActionResult Index(string account, string repository)
        {
            var project = _projectRepository.Single(r => r.AccountName == account && r.RepositoryName == repository);

            var branches = _sourceControlManager.GetBranches(project);

            IEnumerable<Commit> commits = Enumerable.Empty<Commit>();
            
            var defaultBranch = branches.FirstOrDefault(b => b.IsCurrent);

            if(defaultBranch != null)
                commits = _sourceControlManager.GetCommits(project, defaultBranch.Name, 1, 20);

            var model = new IndexViewModel()
            {
                Project = project,
                Branches = branches,
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