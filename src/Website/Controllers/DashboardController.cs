using System.Web.Mvc;

using Sikai.EventSourcing.Domain;

namespace Foundry.Website.Controllers
{
    [Authorize]
    public partial class DashboardController : Controller
    {
        private IRepository _repository;

        public DashboardController(IRepository repository)
        {
            _repository = repository;
        }

        public virtual ActionResult Index()
        {
            return View();
        }
    }
}
