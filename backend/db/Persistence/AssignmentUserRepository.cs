using Base.Persistence;
using Core.Contracts;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class AssignmentUserRepository : GenericRepository<AssignmentUser>, IAssignmentUserRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public AssignmentUserRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
