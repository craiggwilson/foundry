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
        private readonly IDomainRepository<Repository> _repositoryRepository;
        private readonly IDomainRepository<NewsItem> _newsItemRepository;

        public DashboardController(IDomainRepository<Repository> repositoryRepository, IDomainRepository<NewsItem> newsItemRepository)
        {
            _repositoryRepository = repositoryRepository;
            _newsItemRepository = newsItemRepository;
        }

        public virtual ActionResult Index(FoundryUser user)
        {
            var auth = this.AuthorizationService.GetAuthorizationInformation(user.Id);

            var userNewsItems = auth.Filter(_newsItemRepository, u => u.UserId, SubjectType.User, "Read");
            var repos = auth.Filter(_repositoryRepository, r => r.Id, SubjectType.Repository, Operation.Write);

            var model = new IndexViewModel
            {
                NewsItems = userNewsItems.OfType<NewsItem>().Take(20).OrderByDescending(x => x.DateTime).ToList(),
                WritableRepositories = repos.ToList()
            };
            return View(model);
        }

        public virtual ActionResult Yours(FoundryUser user)
        {
            return View();
        }
    }
}