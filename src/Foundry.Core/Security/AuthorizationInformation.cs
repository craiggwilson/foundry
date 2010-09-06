using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.Security
{
    public class AuthorizationInformation
    {
        private readonly Guid _userId;
        private readonly IEnumerable<UserAuthorization> _authorizations;

        public Guid UserId
        {
            get { return _userId; }
        }

        public AuthorizationInformation(Guid userId, IEnumerable<UserAuthorization> authorizations)
        {
            _userId = userId;
            _authorizations = authorizations;
        }

        public bool IsAllowed(string subjectType, Guid subjectId, string operation)
        {
            //TODO: come back and do this...
            return true;
        }

        public IEnumerable<T> Filter<T>(IEnumerable<T> subjects, string subjectType, string operation) where T : IAuthorizable<T>
        {
            var auths = GetDistinct(subjectType, operation).OrderByDescending(x => x.Level);
            
            if (!auths.Any())
                return Enumerable.Empty<T>();

            var allowedList = new List<Guid>();
            var denyList = new List<Guid>();

            bool allowDefault = false;
            foreach (var perm in auths)
            {
                if (perm.Allow)
                {
                    if (perm.SubjectId == Guid.Empty)
                    {
                        allowDefault = true;
                        break;
                    }

                    allowedList.Add(perm.SubjectId);
                }
                else
                {
                    if (perm.SubjectId == Guid.Empty)
                    {
                        allowDefault = false;
                        break;
                    }

                    denyList.Add(perm.SubjectId);
                }
            }

            if (allowDefault)
            {
                if (denyList.Any())
                    subjects = subjects.Where(s => !denyList.Contains(s.Id));
            }
            else
            {
                if (allowedList.Any())
                    subjects = subjects.Where(s => allowedList.Contains(s.Id));
                else
                    return Enumerable.Empty<T>();
            }

            return subjects;
        }

        private IEnumerable<UserAuthorization> GetDistinct(string subjectType, string operation)
        {
            return from p in _authorizations
                   where p.SubjectType == subjectType && p.Operation == operation
                   group p by p.SubjectId into g
                   select g.OrderByDescending(x => x.Level).First();
        }
    }
}