﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public interface ISourceControlProvider
    {
        void CreateRepository(string name);

        IEnumerable<Commit> GetCommits(string name, int page, int pageCount);
    }
}