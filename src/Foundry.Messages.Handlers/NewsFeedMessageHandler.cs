using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Messaging.Infrastructure;
using Foundry.Domain;

namespace Foundry.Messages.Handlers
{
    public class NewsFeedMessageHandler : IMessageHandler<UserProjectCreatedMessage>
    {
        private IBus _bus;
        private readonly IDomainRepository<NewsItem> _newsItemRepository;

        public NewsFeedMessageHandler(IBus bus, IDomainRepository<NewsItem> newsItemRepository)
        {
            _bus = bus;
            _newsItemRepository = newsItemRepository;
        }

        public void Handle(UserProjectCreatedMessage message)
        {
            var newsItem = new NewsItem
            {
                ProjectId = message.ProjectId,
                ProjectFullName = message.AccountName + "/" + message.RepositoryName,
                UserId = message.UserId,
                Username = message.Username,
                UserDisplayName = message.UserDisplayName,
                Event = NewsItemEventType.ProjectCreated,
                DateTime = DateTime.UtcNow,
                Message = string.Format("created [[Project: {0}]]", message.AccountName + "/" + message.RepositoryName),
            };

            _newsItemRepository.Add(newsItem);
        }
    }
}