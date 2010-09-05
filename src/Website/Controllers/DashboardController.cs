using System.Web.Mvc;
using System.Linq;

using Sikai.EventSourcing.Domain;
using Foundry.Reports;
using Foundry.Website.Models;
using Foundry.Website.Models.Dashboard;
using Foundry.Services;
using Foundry.Security;

namespace Foundry.Website.Controllers
{
    [Authorize]
    public partial class DashboardController : Controller
    {
        private readonly IReportingRepository<RepositoryReport> _repositoryRepository;
        private readonly IAuthorizationService _authorizationService;

        public DashboardController(IAuthorizationService authorizationService, IReportingRepository<RepositoryReport> repositoryRepository)
        {
            _authorizationService = authorizationService;
            _repositoryRepository = repositoryRepository;
        }

        public virtual ActionResult Index()
        {
            var currentUserId = ((FoundryUser)User).Id;

            var auth = _authorizationService.GetAuthorizationInformation(currentUserId);

            var model = new IndexViewModel
            {
                WritableRepositories = auth.Filter(_repositoryRepository, SubjectType.Repository, "Write").ToList()
            };
            return View(model);
        }
    }
}