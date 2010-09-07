﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Security;

namespace Foundry.Domain
{
    public abstract class NewsItem
    {
        public int Id { get; set; }

        public string Event { get; set; }

        public DateTime DateTime { get; set; }

        public string Message { get; set; }
    }

    public class RepositoryNewsItem : NewsItem
    {
        public Guid RepositoryId { get; set; }

        public string RepositoryName { get; set; }
    }

    public class UserNewsItem : NewsItem
    {
        public Guid UserId { get; set; }

        public string Username { get; set; }

        public string UserDisplayName { get; set; }
    }
}