﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Foundry.SourceControl
{
    public interface ISourceFile : ISourceObject
    {
        string Extension { get; }

        byte[] Content { get; }
    }
}