using Base.Core.Entities;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Contracts
{
    public interface IUser
    {
        string Username { get; set; }
        string Firstname { get; set; }
        string Lastname { get; set; }
        List<Assignments> Assignments { get; set; }
    }
}
