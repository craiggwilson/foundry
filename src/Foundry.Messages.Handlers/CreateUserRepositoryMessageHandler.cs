using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Messaging.Infrastructure;
using Sikai.EventSourcing.Domain;
using Foundry.Domain;
using Sikai.EventSourcing.Infrastructure;
using Foundry.SourceControl;

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

            var domainRepository = new DomainRepository(_domainSession);

            var repo = new CodeRepository(message.UserId, message.SourceControlProvider, message.RepositoryName, message.Private);
            domainRepository.Add(repo);

            provider.Value.CreateRepository(message.RepositoryName);

            _domainSession.Commit();
        }
    }
}