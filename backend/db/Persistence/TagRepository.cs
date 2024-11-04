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

        public List<Tag> CheckIfTagsExistElseCreate(string[] tags)
        {
            List<Tag> tagList = new List<Tag>();
            foreach (string tag in tags)
            {
                Tag? tag1 = _dbContext.Tags.FirstOrDefault(t => t.Name == tag);
                if (tag1 == null)
                {
                    tag1 = new Tag(tag);
                    _dbContext.Tags.Add(tag1);
                }
                tagList.Add(tag1);
            }
            return tagList;
        }

        public List<Tag> CreateTagsAndStoreInDB(string[] tags)
        {
            List<Tag> tagList = new List<Tag>();
            foreach (string tag in tags)
            {
                Tag tag1 = new Tag(tag);
                tagList.Add(tag1);
            }
            _dbContext.Tags.AddRange(tagList);
            return tagList;
        }
    }
}
