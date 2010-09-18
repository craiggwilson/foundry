using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Messaging.Infrastructure;
using Foundry.Domain;

namespace Foundry.Messages.Handlers
{
    public class NewsFeedMessageHandler : IMessageHandler<UserRepositoryCreatedMessage>
    {
        private IBus _bus;
        private readonly IDomainRepository<NewsItem> _newsItemRepository;

        public NewsFeedMessageHandler(IBus bus, IDomainRepository<NewsItem> newsItemRepository)
        {
            _bus = bus;
            _newsItemRepository = newsItemRepository;
        }

        public void Handle(UserRepositoryCreatedMessage message)
        {
            var newsItem = new NewsItem
            {
                RepositoryId = message.RepositoryId,
                RepositoryName = message.AccountName + "/" + message.ProjectName,
                UserId = message.UserId,
                Username = message.Username,
                UserDisplayName = message.UserDisplayName,
                Event = NewsItemEventType.RepositoryCreated,
                DateTime = DateTime.UtcNow,
                Message = string.Format("created [[Repository: {0}]]", message.AccountName + "/" + message.ProjectName),
            };

            _newsItemRepository.Add(newsItem);
        }
    }
}