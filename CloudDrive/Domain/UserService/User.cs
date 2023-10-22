using Domain.AccessService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UserService;

public class User
{
    public int Id { get; set; }

    public List<Access> Accesses { get; set; }
}
