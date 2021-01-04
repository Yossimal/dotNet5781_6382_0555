using BL.BO;
using System.Collections.Generic;

namespace BL
{
    public interface IBL
    {
        BOUser CheckUserName(BOUser user);
        int Register(BORegister register);
        IEnumerable<BOBus> AllAvelibleBuses();
        IEnumerable<BOBus> AllBuses();
        BOBus RefuelBus(int licenseNumber);
        BOBus CareBus(int licenseNumber);
        int AddBus(BOBus toAdd);
        bool DeleteBus(int licenseNumber);
        BOBus GetBus(int licenseNumber);


    }
}
