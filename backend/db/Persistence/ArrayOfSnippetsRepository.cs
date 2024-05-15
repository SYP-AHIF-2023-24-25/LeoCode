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
    public class ArrayOfSnippetsRepository : GenericRepository<ArrayOfSnippets>, IArrayOfSnippets
    {
        private readonly ApplicationDbContext _dbContext;
        public ArrayOfSnippetsRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
