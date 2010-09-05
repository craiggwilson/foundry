using System.Web.Mvc;
using System.Linq;

using Sikai.EventSourcing.Domain;
using Foundry.Reports;
using Foundry.Website.Models;
using Foundry.Website.Models.Dashboard;
using Foundry.Services;

namespace Foundry.Website.Controllers
{
    [Authorize]
    public partial class DashboardController : Controller
    {
        private readonly IAuthorizationService _authorizationService;

        public DashboardController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public virtual ActionResult Index()
        {
            var currentUserId = ((FoundryUser)User).Id;

            var authInfo = _authorizationService.GetAuthorizationInformation(currentUserId);

            var model = new IndexViewModel
            {
                WritableRepositories = (from p in authInfo.GetAllAuthorizations(SubjectType.Repository, "Write")
                                       select p.SubjectName).ToList()
            };
            return View(model);
        }
    }
}