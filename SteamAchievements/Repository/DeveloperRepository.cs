using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Models;

namespace Repository
{
    public class DeveloperRepository : RepositoryBase<Developer>, IDeveloperRepository
    {
        public DeveloperRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }
    }
}
