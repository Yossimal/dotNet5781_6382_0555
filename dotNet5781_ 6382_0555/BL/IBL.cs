using BL.BO;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        IEnumerable<BOStation> AllStations();
        BOStation GetStation(int code);
        bool DeleteStation(int code);
        int AddStation(BOAddStation toAdd);
        BOLine GetLine(int id);
        IEnumerable<BOLine> GetAllLines(int lineNumber);
        IEnumerable<BOLine> GetAllLines();
        bool RemoveLine(int id);
        Task<int> AddLine(BOLine toAdd);
        void AddLineConnectionless(BOLine toAdd);
        BOLine AddStationToLine(int stationCode,int lineId,int index=-1);
        BOLine RemoveStationFromLine(int index);
        bool IsInternetAvailable();



    }
}
