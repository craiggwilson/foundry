﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public interface ISourceControlProviderMetadata
    {
        string Name { get; }

        bool CommitsHaveParents { get; }
    }
}
