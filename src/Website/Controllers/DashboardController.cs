using System.Web.Mvc;
using System.Linq;

using Foundry.Website.Models;
using Foundry.Website.Models.Dashboard;
using Foundry.Services;
using Foundry.Security;
using Foundry.Domain;

namespace Foundry.Website.Controllers
{
    [Authorize]
    public partial class DashboardController : FoundryController
    {
        private readonly IDomainRepository<Project> _projectRepository;
        private readonly IDomainRepository<NewsItem> _newsItemRepository;

        public DashboardController(IDomainRepository<Project> projectRepository, IDomainRepository<NewsItem> newsItemRepository)
        {
            _projectRepository = projectRepository;
            _newsItemRepository = newsItemRepository;
        }

        public virtual ActionResult Index(FoundryUser user)
        {
            var auth = this.AuthorizationService.GetAuthorizationInformation(user.Id);

            var userNewsItems = auth.Filter(_newsItemRepository, u => u.UserId, SubjectType.User, "Read");
            var projects = auth.Filter(_projectRepository, r => r.Id, SubjectType.Project, Operation.Write);

            var model = new IndexViewModel
            {
                NewsItems = userNewsItems.Take(20).OrderByDescending(x => x.DateTime).ToList(),
                WritableProjects = projects.ToList()
            };
            return View(model);
        }

        public virtual ActionResult Yours(FoundryUser user)
        {
            return View();
        }
    }
}