using System;
using System.Collections.Generic;
using System.Text;

namespace dotNet5781_02__6382_0555
{
    public enum Area { General = 'g', North = 'n', South = 's', Center = 'c', Jerusalem = 'j' }
    public static class Actions
    {
        public const string AddLine = "Add new bus line"; 
        public const string AddStation = "Add station to a bus line"; 
        public const string DelLine = "Delete bus line"; 
        public const string DelStation = "Delete station from a bus line"; 
        public const string SearchStationLines = "Search all lines of a station"; 
        public const string SearchPathBetweenStations = "Search bus lines pathes between stations"; 
        public const string PrintAllLines = "Print all lines"; 
        public const string PrintAllSationsLines = "Print all stations and their lines"; 
        public const string Exit = "Exit"; 
        public static List<string> AllActions
        {
            get => new List<string>() { AddLine, AddStation, DelLine, DelStation, SearchStationLines,
                    SearchPathBetweenStations, PrintAllLines, PrintAllSationsLines, Exit };
        }
    }
}
