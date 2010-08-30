using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Reports.Infrastructure
{
    public interface IReportingSession
    {
        void Add<T>(T report) where T : class;

        void Remove<T>(T report) where T : class;

        IQueryable<T> Query<T>() where T : class;

        void Commit();
    }
}