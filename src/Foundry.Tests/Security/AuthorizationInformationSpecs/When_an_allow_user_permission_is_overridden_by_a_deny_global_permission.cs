﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;

namespace Foundry.Security.AuthorizationInformationSpecs
{
    public class When_an_allow_user_permission_is_overridden_by_a_deny_global_permission : InContextOfTestingAuthorizationInformation
    {
        private static IEnumerable<AuthorizableA> _filtered;

        Because of = () =>
        {
            var items = new List<AuthorizableA> { new AuthorizableA { Id = _userPermissions[5].SubjectId } };
            _filtered = _subjectUnderTest.Filter(items, "Test", "Deny");
        };

        It should_not_allow_the_item_to_go_through = () =>
            _filtered.Count().ShouldEqual(0);
    }
}