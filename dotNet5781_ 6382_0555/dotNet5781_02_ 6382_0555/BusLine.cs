using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace dotNet5781_02__6382_0555
{
    enum Area { General = 'g', North = 'n', South = 's', Center = 'c', Jerusalum = 'j' }
    class BusLine : IComparable, IEnumerable
    {
        private List<LineBusStation> path;
        private int lineNum;
        private Area area;
        public BusStation First { get => path[0]; }
        public BusStation Last { get => path[path.Count - 1]; }

        public int CompareTo(object obj)
        {
            if (!(obj is BusLine) && obj == null)
            {
                throw new ObjectDisposedException("Wrong object to compare");
            }
            BusLine rhs = obj as BusLine;
            TimeSpan delta = this.TimeBetweenStations(this.First.Code, this.Last.Code) - rhs.TimeBetweenStations(rhs.First.Code, rhs.Last.Code);
            return (int)Math.Round(delta.TotalSeconds);
        }

        public IEnumerator GetEnumerator()
        {
            return path.GetEnumerator();
        }

        public override string ToString()
        {
            string ret;
            ret = $"{lineNum}\t{area}\t";
            foreach (LineBusStation station in path)
            {
                ret += $"{station.Code}=>";
            }
            return ret.Remove(ret.Length - 3);
        }
        public TimeSpan TimeBetweenStations(int first, int last)
        {
            TimeSpan sum = new TimeSpan(0);
            int stationIndex = 0;
            for (; this.path.Count > stationIndex && this.path[stationIndex].Code != first
                && this.path[stationIndex].Code != last; stationIndex++) ;
            stationIndex++;
            for (; this.path.Count > stationIndex && this.path[stationIndex].Code != first 
                && this.path[stationIndex].Code != last; stationIndex++)
            {
                sum += this.path[stationIndex].TimeFromPre;
            }
            if (stationIndex >= this.path.Count)
            {
                throw new IndexOutOfRangeException("One or more stations are not in list");
            }
            return sum + this.path[stationIndex].TimeFromPre;
        }
        public bool Remove(int code)
        {
            LineBusStation toRemove = null;
            foreach (LineBusStation station in path )
            {
                if (station.Code==code)
                {
                    toRemove = station;
                    break;
                }
            }
            if (toRemove==null)
            {
                return false;
            }
            return path.Remove(toRemove);
        }
    }
}
