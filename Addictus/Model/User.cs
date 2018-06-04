using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Addictus
{

    public class User
    {
        public string Username { get; set; }
        public int Password { get; set; }

        public User(string username)
        {
            Username = username;
        }

    }

}
