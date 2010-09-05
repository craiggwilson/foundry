using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry
{
    public abstract class BaseSpecification<TSubjectUnderTest>
    {
        protected static TSubjectUnderTest _subjectUnderTest;
    }
}
