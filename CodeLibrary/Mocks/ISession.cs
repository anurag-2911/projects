using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLibrary
{
    public interface ISession
    {
        string sessionID { get; set; }
        bool IsUserSession();

    }
}
