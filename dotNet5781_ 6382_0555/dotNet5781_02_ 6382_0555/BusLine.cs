using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace dotNet5781_02__6382_0555
{
    class BusLine : IComparable, IEnumerable
    {
        private List<LineBusStation> path;
        private int lineNum;
        private Area area;

        public BusLine(List<LineBusStation> path, int lineNum, Area area)
        {
            this.path = path;
            this.lineNum = lineNum;
            this.Area = area;
        }

        public BusLine(int lineNum, Area area)
        {
            this.path = new List<LineBusStation>();
            this.lineNum = lineNum;
            this.Area = area;
        }

        public BusStation First { get => this.path.Count == 0 ? null : path[0]; }
        public BusStation Last { get => this.path.Count == 0 ? null : path[path.Count - 1]; }
        public int LineNum { get => lineNum; }
        internal Area Area { get => area; set => area = value; }

        public int CompareTo(object obj)
        {
            if (!(obj is BusLine))
            {

                throw new ArgumentException("Wrong object to compare");
            }
            if (obj == null)
            {
                throw new ArgumentNullException("Can't compare null");
            }

            BusLine rhs = obj as BusLine;
            if (rhs.path.Count == 0 && this.path.Count == 0)
            {
                return 0;
            }
            if (rhs.path.Count == 0)
            {
                return 1;
            }
            else if (this.path.Count == 0)
            {
                return -1;
            }
            return (int)(this.TimeBetweenStations(this.First.Code, this.Last.Code).TotalSeconds - rhs.TimeBetweenStations(rhs.First.Code, rhs.Last.Code).TotalSeconds);
        }

        public IEnumerator GetEnumerator()
        {
            return path.GetEnumerator();
        }

        public override string ToString()
        {
            string ret;
            ret = $"{LineNum,-4}{Area}\t";
            foreach (LineBusStation station in path)
            {
                ret += $"{station.Code} => ";
            }
            return ret.Remove(ret.Length - 3);
        }
        public TimeSpan TimeBetweenStations(int first, int last)
        {
            TimeSpan sum = new TimeSpan(0);
            int firstIndex = GetIndex(first);
            int lastIndex = GetIndex(last);
            if (lastIndex < 0 || firstIndex < 0)
            {
                throw new InvalidOperationException("The codes must be of a station in path");
            }

            if (lastIndex < firstIndex)
            {
                firstIndex += lastIndex;
                lastIndex = firstIndex - lastIndex;
                firstIndex -= lastIndex;
            }
            foreach (LineBusStation station in path.GetRange(firstIndex + 1, lastIndex - firstIndex))
            {
                sum += station.TimeFromPre;
            }
            return sum;
        }
        public float DistanceBetweenStations(int first, int last)
        {
            float sum = 0f;
            int firstIndex = GetIndex(first);
            int lastIndex = GetIndex(last);
            if (lastIndex < 0 || firstIndex < 0)
            {
                throw new InvalidOperationException("The codes must be of a station in path");
            }

            if (lastIndex < firstIndex)
            {
                firstIndex += lastIndex;
                lastIndex = firstIndex - lastIndex;
                firstIndex -= lastIndex;
            }
            foreach (LineBusStation station in path.GetRange(firstIndex + 1, lastIndex - firstIndex))
            {
                sum += station.DistanceFromPre;
            }
            return sum;
        }

        public BusLine SubPath(int first, int last)
        {
            int firstIndex = GetIndex(first);
            int lastIndex = GetIndex(last);
            if (lastIndex < 0 || firstIndex < 0)
            {
                throw new InvalidOperationException("The codes must be of a station in path");
            }
            if (lastIndex < firstIndex)
            {
                firstIndex += lastIndex;
                lastIndex = firstIndex - lastIndex;
                firstIndex -= lastIndex;
            }
            return new BusLine(path.GetRange(firstIndex, lastIndex - firstIndex + 1),
                this.LineNum, this.Area);
        }
        public bool Remove(int code)
        {
            LineBusStation toRemove = null;
            foreach (LineBusStation station in path)
            {
                if (station.Code == code)
                {
                    toRemove = station;
                    break;
                }
            }
            if (toRemove == null)
            {
                return false;
            }
            return path.Remove(toRemove);
        }
        public void Add(LineBusStation station)
        {
            if (station == null)
            {
                throw new ArgumentNullException("Not allowed to add null");
            }
            this.path.Add(station);

        }
        public void Add(LineBusStation station, int code)
        {
            if (station == null)
            {
                throw new ArgumentNullException("Not allowed to add null");
            }
            int index;
            for (index = 0; this.path.Count != index && this.path[index].Code != code; index++) ;
            if (this.path.Count == index)
            {
                throw new InvalidOperationException("The code must be of a station in path");
            }
            this.path.Insert(index, station);
        }
        public bool IsExists(int code)
        {
            return this.path.Exists(p => p.Code == code);
        }
        public bool IsBefore(int first, int last)
        {
            if (!this.IsExists(first) || !this.IsExists(last))
            {
                throw new InvalidOperationException("At least one station is not in line");
            }

            if (this.GetIndex(first) >= this.GetIndex(last))
            {
                return false;
            }
            return true;
        }
        private int GetIndex(int code)
        {
            int index;
            for (index = 0; code != this.path.Count && this.path[index].Code != code; index++) ;
            if (index == this.path.Count)
            {
                return -1;
            }
            return index;
        }

    }
}
