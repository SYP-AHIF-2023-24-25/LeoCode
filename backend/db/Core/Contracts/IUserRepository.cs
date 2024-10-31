using Base.Core.Contracts;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User GetByUsername(string username);
    }
}
