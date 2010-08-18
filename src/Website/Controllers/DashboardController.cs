using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Sikai.EventSourcing.Domain;

namespace Foundry.Website.Controllers
{
    public class DashboardController : Controller
    {
        private IRepository _repository;

        public DashboardController(IRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}
