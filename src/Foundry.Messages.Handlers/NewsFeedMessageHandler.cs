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
            var newsItem = new UserNewsItem
            {
                UserId = message.UserId,
                Username = message.Username,
                UserDisplayName = message.UserDisplayName,
                Event = "Repository-Created",
                DateTime = DateTime.UtcNow,
                Message = string.Format("created [[Repository: {0}]]", message.Name),
            };

            _newsItemRepository.Add(newsItem);
        }
    }
}