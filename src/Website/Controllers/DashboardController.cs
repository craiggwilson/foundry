using System.Web.Mvc;
using System.Linq;

using Sikai.EventSourcing.Domain;
using Foundry.Reports;
using Foundry.Website.Models;
using Foundry.Website.Models.Dashboard;

namespace Foundry.Website.Controllers
{
    [Authorize]
    public partial class DashboardController : Controller
    {
        private readonly IReportingRepository<UserCodeRepositoryReport> _userCodeRepositories;

        public DashboardController(IReportingRepository<UserCodeRepositoryReport> userCodeRepositories)
        {
            _userCodeRepositories = userCodeRepositories;
        }

        public virtual ActionResult Index()
        {
            var currentUserId = ((FoundryUser)User).Id;

            var model = new IndexViewModel { UserCodeRepositories = _userCodeRepositories.Where(x => x.UserId == currentUserId).ToList() };
            return View(model);
        }
    }
}