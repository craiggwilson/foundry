using System.Linq;

namespace Foundry.Reporting
{
    public interface IReportingRepository<T> : IQueryable<T> where T : class
    {
        void Add(T report);

        void Remove(T report);
    }
}