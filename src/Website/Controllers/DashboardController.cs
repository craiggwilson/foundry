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
        private readonly IDomainRepository<NewsItem> _newsItems;
        private readonly IAuthorizationService _authorizationService;

        public DashboardController(IAuthorizationService authorizationService, IDomainRepository<Repository> repositoryRepository, IDomainRepository<NewsItem> newsItems)
        {
            _authorizationService = authorizationService;
            _repositoryRepository = repositoryRepository;
            _newsItems = newsItems;
        }

        public virtual ActionResult Index(FoundryUser user)
        {
            var auth = _authorizationService.GetAuthorizationInformation(user.Id);

            var userNewsItems = auth.Filter(_newsItems, u => u.UserId, SubjectType.User, "Read");
            var repos = auth.Filter(_repositoryRepository, r => r.Id, SubjectType.Repository, Operation.Write);

            var model = new IndexViewModel
            {
                NewsItems = userNewsItems.OfType<NewsItem>().Take(20).OrderByDescending(x => x.DateTime).ToList(),
                WritableRepositories = repos.ToList()
            };
            return View(model);
        }
    }
}