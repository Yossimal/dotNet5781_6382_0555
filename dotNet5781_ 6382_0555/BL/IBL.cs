using BL.BO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL
{
    public interface IBL
    {
        BOUser CheckUserName(BOUser user);
        int Register(BORegister register);
        IEnumerable<BOBus> AllAvailableBuses();
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
        Task<BOLine> AddStationToLine(int stationCode,int lineId,int index=-1);
        Task<BOLine> RemoveStationFromLine(int index);
        bool IsInternetAvailable();
        IEnumerable<string> GetAllAreas();
        BOLineStation GetLineStationFromStationAndLine(int lineId, int stationId,out BOStation next,out BOStation prev,bool fullLine=false);
        void UpdateNearStations(int fromCode,int toCode,double distance=-1,TimeSpan? time=null);


    }
}
