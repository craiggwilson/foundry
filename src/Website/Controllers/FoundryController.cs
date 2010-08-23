using System.Web.Mvc;
using Sikai.EventSourcing.Infrastructure;
using Foundry.Reporting;
using System;

namespace Foundry.Website.Controllers
{
    public abstract class FoundryController : Controller
    {
        public const string VIEW_MESSAGE_KEY = "Message";

        public IUnitOfWork UnitOfWork { get; set; }

        public IReportingUnitOfWork ReportingUnitOfWork { get; set; }

        protected ITransaction BeginTransaction()
        {
            return new Transaction(UnitOfWork, ReportingUnitOfWork);
        }

        protected void WithTransaction(Action action)
        {
            using (var tran = BeginTransaction())
            {
                action();
                tran.Commit();
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            ProcessViewMessage();
        }

        private void ProcessViewMessage()
        {
            if (TempData.ContainsKey(VIEW_MESSAGE_KEY))
                ViewData[VIEW_MESSAGE_KEY] = TempData[VIEW_MESSAGE_KEY];
        }

        public interface ITransaction : IDisposable
        {
            void Commit();
        }

        private class Transaction : ITransaction
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IReportingUnitOfWork _reportingUnitOfWork;

            public Transaction(IUnitOfWork unitOfWork, IReportingUnitOfWork reportingUnitOfWork)
            {
                _unitOfWork = unitOfWork;
                _reportingUnitOfWork = reportingUnitOfWork;
            }

            public void Commit()
            {
                _unitOfWork.Commit();
                _reportingUnitOfWork.Commit();
            }

            public void Dispose()
            {
                //nothing to do...
            }
        }
    }
}