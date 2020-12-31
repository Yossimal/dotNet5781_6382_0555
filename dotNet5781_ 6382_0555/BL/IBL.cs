using BL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public interface IBL
    {
        BOUser CheckUserName(BOUser user);
        int Register(BORegister register);
        IEnumerable<BOBus> AllAvelibleBuses();
        IEnumerable<BOBus> AllBuses();
    }
}
