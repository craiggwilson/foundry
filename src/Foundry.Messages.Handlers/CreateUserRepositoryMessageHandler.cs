using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Messaging.Infrastructure;
using Foundry.SourceControl;
using Sikai.EventSourcing.Domain;
using Foundry.Domain;
using Sikai.EventSourcing.Infrastructure;

namespace Foundry.Messages.Handlers
{
    public class CreateUserRepositoryMessageHandler : IMessageHandler<CreateUserRepositoryMessage>
    {
        private readonly IDomainSession _domainSession;
        private readonly IEnumerable<Lazy<ISourceControlProvider, ISourceControlProviderMetadata>> _sourceControlProviders;

        public CreateUserRepositoryMessageHandler(IDomainSession domainSession, IEnumerable<Lazy<ISourceControlProvider, ISourceControlProviderMetadata>> sourceControlProviders)
        {
            _domainSession = domainSession;
            _sourceControlProviders = sourceControlProviders;
        }

        public void Handle(CreateUserRepositoryMessage message)
        {
            var provider = _sourceControlProviders.Single(x => x.Metadata.Name == message.SourceControlProvider);

            provider.Value.CreateRepository(message.RepositoryName);

            var domainRepository = new DomainRepository(_domainSession);

            var repo = new CodeRepository(message.SourceControlProvider, message.RepositoryName);
            domainRepository.Add(repo);

            var user = domainRepository.GetById<User>(message.UserId);
            user.AddRepository(repo.Id);

            _domainSession.Commit();
        }
    }
}