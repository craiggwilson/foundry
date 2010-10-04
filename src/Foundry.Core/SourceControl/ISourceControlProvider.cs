﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Domain;

namespace Foundry.SourceControl
{
    public interface ISourceControlProvider
    {
        void CreateRepository(Project project);

        IEnumerable<IBranch> GetBranches(Project project);

        IEnumerable<IHistoricalItem> GetHistory(Project project, string id);

        ICommit GetCommit(Project project, string id);

        ISourceObject GetSourceObject(Project project, string treeId, string path);
    }
}