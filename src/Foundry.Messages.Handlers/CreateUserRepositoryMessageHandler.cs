using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Messaging.Infrastructure;
using Foundry.Domain;
using Foundry.SourceControl;

namespace Foundry.Messages.Handlers
{
    public class CreateUserRepositoryMessageHandler : IMessageHandler<CreateUserRepositoryMessage>
    {
        private readonly IBus _bus;
        private readonly IDomainRepository<User> _userRepository;
        private readonly IDomainRepository<Repository> _repositoryRepository;
        private readonly IEnumerable<Lazy<ISourceControlProvider, ISourceControlProviderMetadata>> _sourceControlProviders;

        public CreateUserRepositoryMessageHandler(IBus bus, IDomainRepository<User> userRepository, IDomainRepository<Repository> repositoryRepository, IEnumerable<Lazy<ISourceControlProvider, ISourceControlProviderMetadata>> sourceControlProviders)
        {
            _bus = bus;
            _userRepository = userRepository;
            _repositoryRepository = repositoryRepository;
            _sourceControlProviders = sourceControlProviders;
        }

        public void Handle(CreateUserRepositoryMessage message)
        {
            var provider = _sourceControlProviders.Single(x => x.Metadata.Name == message.SourceControlProvider);

            var repo = new Repository()
            {
                Id = Guid.NewGuid(),
                OwnerId = message.UserId,
                IsPrivate = message.IsPrivate,
                AccountName = message.AccountName,
                ProjectName = message.ProjectName,
                SourceControlProvider = message.SourceControlProvider
            };

            _repositoryRepository.Add(repo);

            var user = _userRepository.Single(x => x.Id == message.UserId);

            _bus.Send(new UserRepositoryCreatedMessage { RepositoryId = repo.Id, AccountName = repo.AccountName, ProjectName = repo.ProjectName, UserId = user.Id, UserDisplayName = user.DisplayName, Username = user.Username, SourceControlProvider = repo.SourceControlProvider, IsPrivate = repo.IsPrivate });

            provider.Value.CreateRepository(repo.Name);
        }
    }
}