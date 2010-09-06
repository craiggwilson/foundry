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
        private readonly IAuthorizationService _authorizationService;

        public DashboardController(IAuthorizationService authorizationService, IDomainRepository<Repository> repositoryRepository)
        {
            _authorizationService = authorizationService;
            _repositoryRepository = repositoryRepository;
        }

        public virtual ActionResult Index(FoundryUser user)
        {
            var auth = _authorizationService.GetAuthorizationInformation(user.Id);

            var model = new IndexViewModel
            {
                WritableRepositories = auth.Filter(_repositoryRepository, SubjectType.Repository, Operation.Write).ToList()
            };
            return View(model);
        }
    }
}