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
        private readonly IDomainRepository<UserNewsItem> _userNewsItemRepository;
        private readonly IDomainRepository<RepositoryNewsItem> _repositoryNewsItemRepository;
        private readonly IAuthorizationService _authorizationService;

        public DashboardController(IAuthorizationService authorizationService, IDomainRepository<Repository> repositoryRepository, IDomainRepository<UserNewsItem> userNewsItemRepository, IDomainRepository<RepositoryNewsItem> repositoryNewsItemRepository)
        {
            _authorizationService = authorizationService;
            _repositoryRepository = repositoryRepository;
            _userNewsItemRepository = userNewsItemRepository;
            _repositoryNewsItemRepository = repositoryNewsItemRepository;
        }

        public virtual ActionResult Index(FoundryUser user)
        {
            var auth = _authorizationService.GetAuthorizationInformation(user.Id);

            var userNewsItems = auth.Filter(_userNewsItemRepository.OrderByDescending(x => x.DateTime).Take(20), SubjectType.User, "Read");
            var repositoryNewsItems = auth.Filter(_repositoryNewsItemRepository.OrderByDescending(x => x.DateTime).Take(20), SubjectType.Repository, "Read");
            var repos = auth.Filter(_repositoryRepository, SubjectType.Repository, Operation.Write);

            var model = new IndexViewModel
            {
                NewsItems = userNewsItems.OfType<NewsItem>().Union(repositoryNewsItems).OrderByDescending(x => x.DateTime).ToList(),
                WritableRepositories = repos.ToList()
            };
            return View(model);
        }
    }
}