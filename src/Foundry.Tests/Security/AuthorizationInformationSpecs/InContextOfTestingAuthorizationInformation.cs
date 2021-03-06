﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Machine.Specifications;

namespace Foundry.Security.AuthorizationInformationSpecs
{
    public abstract class InContextOfTestingAuthorizationInformation : BaseSpecification<AuthorizationInformation>
    {
        protected static Guid _userId = Guid.NewGuid();
        protected static List<UserAuthorization> _userPermissions;

        Establish context = () =>
        {
            _userPermissions = new List<UserAuthorization>
            {
                new UserAuthorization { SubjectId = Guid.NewGuid(), SubjectType = "Test", Allow = false, Level = 99, Operation="Allow" },
                new UserAuthorization { SubjectId = Guid.Empty, SubjectType = "Test", Allow = true, Level = 50, Operation="Allow" },
                new UserAuthorization { SubjectId = Guid.NewGuid(), SubjectType = "Test", Allow = false, Level = 1, Operation="Allow" },

                new UserAuthorization { SubjectId = Guid.NewGuid(), SubjectType = "Test", Allow = true, Level = 99, Operation="Deny" },
                new UserAuthorization { SubjectId = Guid.Empty, SubjectType = "Test", Allow = false, Level = 50, Operation="Deny" },
                new UserAuthorization { SubjectId = Guid.NewGuid(), SubjectType = "Test", Allow = true, Level = 1, Operation="Deny" },
            };

            _subjectUnderTest = new AuthorizationInformation(_userId, _userPermissions);
        };


        protected class AuthorizableA
        {
            public Guid Id { get; set; }
        }
    }
}
