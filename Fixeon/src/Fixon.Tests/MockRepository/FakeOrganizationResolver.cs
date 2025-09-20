using Fixeon.Shared.Core.Interfaces;
using Fixeon.Shared.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixon.Tests.MockRepository
{
    internal class FakeOrganizationResolver : IOrganizationResolver
    {
        public Task<OrganizationResolverView> GetOrganization(Guid organizationId)
        {
            throw new NotImplementedException();
        }

        public Task<List<SLAResolverView>> GetSLAByOrganization(Guid organizationId)
        {
            throw new NotImplementedException();
        }
    }
}
