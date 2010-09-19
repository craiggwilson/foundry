using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Messaging.Infrastructure;
using Foundry.Domain;
using Foundry.SourceControl;

namespace Foundry.Messages.Handlers
{
    public class CreateUserRepositoryMessageHandler : IMessageHandler<CreateUserProjectMessage>
    {
        private readonly IBus _bus;
        private readonly IDomainRepository<User> _userRepository;
        private readonly IDomainRepository<Project> _projectRepository;
        private readonly IEnumerable<Lazy<ISourceControlProvider, ISourceControlProviderMetadata>> _sourceControlProviders;

        public CreateUserRepositoryMessageHandler(IBus bus, IDomainRepository<User> userRepository, IDomainRepository<Project> projectRepository, IEnumerable<Lazy<ISourceControlProvider, ISourceControlProviderMetadata>> sourceControlProviders)
        {
            _bus = bus;
            _userRepository = userRepository;
            _projectRepository = projectRepository;
            _sourceControlProviders = sourceControlProviders;
        }

        public void Handle(CreateUserProjectMessage message)
        {
            var provider = _sourceControlProviders.Single(x => x.Metadata.Name == message.SourceControlProvider);

            var project = new Project()
            {
                Id = Guid.NewGuid(),
                AccountId = message.UserId,
                IsPrivate = message.IsPrivate,
                AccountName = message.AccountName,
                RepositoryName = message.RepositoryName,
                SourceControlProvider = message.SourceControlProvider
            };

            _projectRepository.Add(project);

            var user = _userRepository.Single(x => x.Id == message.UserId);

            provider.Value.CreateRepository(project);

            _bus.Send(new UserProjectCreatedMessage { ProjectId = project.Id, AccountName = project.AccountName, RepositoryName = project.RepositoryName, UserId = user.Id, UserDisplayName = user.DisplayName, Username = user.Username, SourceControlProvider = project.SourceControlProvider, IsPrivate = project.IsPrivate });
        }
    }
}