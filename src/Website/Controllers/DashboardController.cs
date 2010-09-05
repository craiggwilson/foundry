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
        private readonly IReportingRepository<RepositoryReport> _codeRepositoryRepository;
        private readonly IAuthorizationService _authorizationService;

        public DashboardController(IAuthorizationService authorizationService, IReportingRepository<RepositoryReport> codeRepositoryRepository)
        {
            _authorizationService = authorizationService;
            _codeRepositoryRepository = codeRepositoryRepository;
        }

        public virtual ActionResult Index()
        {
            var currentUserId = ((FoundryUser)User).Id;

            var repos = new RepositoryReport[0];

            var model = new IndexViewModel
            {
                WritableRepositories = repos.Select(x => x.Name).ToList()
            };
            return View(model);
        }
    }
}