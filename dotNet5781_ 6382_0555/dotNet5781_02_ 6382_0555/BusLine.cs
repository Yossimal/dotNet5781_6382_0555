using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace dotNet5781_02__6382_0555
{
    public class BusLine : IComparable, IEnumerable
    {
        private List<LineBusStation> path;
        private int lineNum;
        private Area area;

        /// <summary>
        /// Generate new bus line
        /// </summary>
        /// <param name="path">The line path</param>
        /// <param name="lineNum">The line number</param>
        /// <param name="area">The line area</param>
        public BusLine(List<LineBusStation> path, int lineNum, Area area)
        {
            this.path = path;
            this.lineNum = lineNum;
            this.Area = area;
        }
        /// <summary>
        /// Generate new bus line with ann empty path
        /// </summary>
        /// <param name="lineNum">The line number</param>
        /// <param name="area">The line area</param>
        public BusLine(int lineNum, Area area)
        {
            this.path = new List<LineBusStation>();
            this.lineNum = lineNum;
            this.Area = area;
        }
        /// <summary>
        /// The first station in the line
        /// </summary>
        public BusStation First { get => this.path.Count == 0 ? null : path[0]; }
        /// <summary>
        /// The last station in the line
        /// </summary>
        public BusStation Last { get => this.path.Count == 0 ? null : path[path.Count - 1]; }
        public int LineNum { get => lineNum; }
        internal Area Area { get => area; set => area = value; }
        /// <summary>
        /// Comparing two lines according to the travel time
        /// </summary>
        /// <param name="obj">The other line to compare</param>
        /// <returns>The delta travel time</returns>
        /// <exception cref="ArgumentException">The given object is not a BusLine object</exception>
        /// <exception cref="ArgumentNullException">The given object is null</exception>
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
        /// <summary>
        /// Returns enumerator to move over all the line path
        /// </summary>
        /// <returns>The path enumerator</returns>
        public IEnumerator GetEnumerator()
        {
            return path.GetEnumerator();
        }
        /// <summary>
        /// </summary>
        /// <returns>String that presents the object data</returns>
        public override string ToString()
        {
            string ret;
            ret = $"Line Number: {LineNum}\t\tArea: {Area} \n\n";
            for (int i = 0; i < path.Count; i++) {//we want to use the index so we using for loop instead of foreach loop
                ret += $"{(i+1):00}. {path[i]} \n";
            }
            foreach (LineBusStation station in path)
            {
             
            }
            return ret;
        }
        /// <summary>
        /// Get the travel time between two stations in the line path
        /// </summary>
        /// <param name="first">The first station in the path</param>
        /// <param name="last">The second station in the path</param>
        /// <returns>The travel time between the stations</returns>
        /// <exception cref="InvalidOperationException">One or more of the given codes are not a code of a station in the path</exception>
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
        /// <summary>
        /// Get the distance between two stations
        /// </summary>
        /// <param name="first">The code of first station in the path</param>
        /// <param name="last">The code of last station in the path</param>
        /// <returns>The distance between those stations</returns>
        /// <exception cref="InvalidOperationException">One or more of the given codes are not of a code of a station in the path</exception>
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
        /// <summary>
        /// Get a sub path of this bus line path
        /// </summary>
        /// <param name="first">The first sub-path boundary</param>
        /// <param name="last">The second sub-path boundary</param>
        /// <returns>new bus line with the same data and in the sub-path insted of this path</returns>
        /// <exception cref="InvalidOperationException">One or more of the given codes are not of a code of a station in the path</exception>
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
        /// <summary>
        /// Remove a station from the bus line
        /// </summary>
        /// <param name="code">The code of the station that we want to remove</param>
        /// <returns> true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the BusLine.path.</returns>
        public bool Remove(int code)
        {
            if (!IsExists(code)) {
                return false;
            }
            LineBusStation toRemove = path.Find(station => station.Code == code);
            if (GetIndex(code) != this.path.Count - 1) {
                this.path[GetIndex(code) + 1].DistanceFromPre = (float)Program.rand.Next(1, 300) / (float)Program.rand.Next(1, 6);
                this.path[GetIndex(code) + 1].TimeFromPre = new TimeSpan(Program.rand.Next(0, 2), Program.rand.Next(0, 59), Program.rand.Next(1, 59));
            }
            return path.Remove(toRemove);
        }
        /// <summary>
        /// Add a station to the end of the line path
        /// </summary>
        /// <param name="station">The station that we want to add</param>
        /// <exception cref="ArgumentNullException">The station param is null</exception>
        /// <exception cref="InvalidOperationException">The station param code alredy exists in the path</exception>
        public void Add(LineBusStation station)
        {
            if (station == null)
            {
                throw new ArgumentNullException("Not allowed to add null");
            }
            if (GetIndex(station.Code)!=-1) {
                throw new InvalidOperationException("Can't add the same stations twice");
            }
            if (this.path.Count == 0)
            {//If that staion is the first station so reset the time and distanse from the previos station
                station.DistanceFromPre = 0;
                station.TimeFromPre = new TimeSpan(0);
            }
            this.path.Add(station);

        }
        /// <summary>
        /// Add a station to the line path before a given station
        /// </summary>
        /// <param name="station">The station that we want to add</param>
        /// <param name="code">The code of the station that we want to add before</param>
        /// <exception cref="ArgumentNullException">The station param is null</exception>
        /// <exception cref="InvalidOperationException">The station param code alredy exists in the path</exception>
        public void Add(LineBusStation station, int code)
        {
            if (station == null)
            {
                throw new ArgumentNullException("Not allowed to add null");
            }
            if (GetIndex(station.Code) != -1) {
                throw new InvalidOperationException("Can't add the same station twice");
            }
            int index=GetIndex(code);
            if (this.path.Count == index)
            {
                throw new InvalidOperationException("The code must be of a station in path");
            }
            if (index == 0) {//If that staion is the first station so reset the time and distanse from the previos station
                station.DistanceFromPre = 0;
                station.TimeFromPre = new TimeSpan(0);
            }
            this.path.Insert(index, station);
            this.path[index + 1].DistanceFromPre = (float)Program.rand.Next(1, 300)/(float)Program.rand.Next(1,6);
            this.path[index+1].TimeFromPre = new TimeSpan(Program.rand.Next(0,2),Program.rand.Next(0,59),Program.rand.Next(1,59));
        }
        /// <summary>
        /// Check the exsistence of a station in the line path by the station code
        /// </summary>
        /// <param name="code">The station code</param>
        /// <returns>true if the station exists, else returns false</returns>
        public bool IsExists(int code)
        {
            return this.path.Exists(p => p.Code == code);
        }
        /// <summary>
        /// Check if one station is before the other
        /// </summary>
        /// <param name="first">The station that need to be "first"</param>
        /// <param name="last">The station that need to be "last"</param>
        /// <returns>true if first is before last</returns>
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
        /// <summary>
        /// Returns the index of a station in the path according to the station code
        /// </summary>
        /// <param name="code">The code of the station that we want to get</param>
        /// <returns>The index of the station in the path</returns>
        private int GetIndex(int code)
        {
            return this.path.FindIndex(station => station.Code == code);
        }
    }
}
