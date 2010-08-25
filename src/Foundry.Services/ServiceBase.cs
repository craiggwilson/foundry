using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sikai.EventSourcing.Infrastructure;
using Foundry.Reporting;

namespace Foundry.Services
{
    public abstract class ServiceBase
    {
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
