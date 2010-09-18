using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundry.Messaging.Infrastructure;
using Foundry.Domain;

namespace Foundry.Messages.Handlers
{
    public class UserPermissionMessageHandler : IMessageHandler<UserRepositoryCreatedMessage>
    {
        private IBus _bus;
        private readonly IDomainRepository<UserPermission> _userPermissionRepository;

        public UserPermissionMessageHandler(IBus bus, IDomainRepository<UserPermission> userPermissionsRepository)
        {
            _bus = bus;
            _userPermissionRepository = userPermissionsRepository;
        }

        public void Handle(UserRepositoryCreatedMessage message)
        {
            var permission = GetOwnerPermissions(message.UserId, message.RepositoryId, message.AccountName + "/" + message.ProjectName);
            _userPermissionRepository.Add(permission);

            permission = GetEveryonePermissions(message.RepositoryId, message.AccountName + "/" + message.ProjectName, message.IsPrivate);
            _userPermissionRepository.Add(permission);
        }

        private static UserPermission GetOwnerPermissions(Guid userId, Guid repositoryId, string repositoryName)
        {
            return new UserPermission
            {
                UserId = userId,
                SubjectType = SubjectType.Repository,
                SubjectId = repositoryId,
                Level = 99,
                Operation = Security.Operation.All,
                Allow = true
            };
        }

        private static UserPermission GetEveryonePermissions(Guid repositoryId, string repositoryName, bool isPrivate)
        {
            return new UserPermission
            {
                UserId = Guid.Empty,
                SubjectType = SubjectType.Repository,
                SubjectId = repositoryId,
                Level = 1,
                Operation = Security.Operation.Read,
                Allow = isPrivate
            };
        }
    }
}
