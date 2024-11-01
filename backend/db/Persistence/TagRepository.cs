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
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public TagRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
