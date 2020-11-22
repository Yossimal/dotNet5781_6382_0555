using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotNet5781_02__6382_0555
{
    /// <summary>
    /// A collection of bus lines
    /// </summary>
    public class BusLineCollection:IEnumerable
    {
        private List<BusLine> lines;
        /// <summary>
        /// Generate empty collection
        /// </summary>
        public BusLineCollection()
        {
            this.lines = new List<BusLine>();
        }
        /// <summary>
        /// Add a bus line to the collection
        /// </summary>
        /// <param name="line">The bus line to add</param>
        /// <exception cref="ArgumentNullException">The given line is null</exception>
        /// <exception cref="ArgumentException">The given line alredy exists in the collection</exception>
        public void Add(BusLine line)
        {
            if (line == null)
            {
                throw new ArgumentNullException();
            }
            if (lines.Where(p => p.LineNum == line.LineNum).Count() >= 2)
            {
                throw new ArgumentException("Line is already exists twice");
            }
            lines.Add(line);
            lines.Sort();
        }
        /// <summary>
        /// Remove a line from the collection
        /// </summary>
        /// <param name="line">The line to remove from the collection</param>
        /// <returns> true if item is successfully removed; otherwise, false. This method also returns false if item was not found in the BusLine.path.</returns>
        public bool Remove(BusLine line)
        {
            return this.lines.Remove(line);
        }
        /// <summary>
        /// Get all the lines in a given station
        /// </summary>
        /// <param name="code">The code of the station that we want to get all the lines of it</param>
        /// <returns>System.Collections.Generic.List that contains al the lines that have the given station in their path (by code)</returns>
        /// <exception cref="InvalidOperationException">If there is no lines that have the given station in their path</exception>
        public List<BusLine> GetStationLines (int code)
        {
            List<BusLine> ret= new List<BusLine>(lines.Where(line => line.IsExists(code)));
            if (ret.Count == 0) {
                throw new InvalidOperationException("That station not contains any line.");
            }
            return ret;
        }
        /// <summary>
        /// Get a sorted list of the collection
        /// </summary>
        /// <returns>System.Collection.Generic.List of all the lines in the collection, sorted by their travel duration</returns>
        public List<BusLine> GetSortedLines() 
        {
            return new List<BusLine>(lines.ToArray());//The collection is sorted in the Add method
        }
        /// <summary>
        /// Get enumerator for iterating the list
        /// </summary>
        /// <returns>The enumerator of the lines list</returns>
        public IEnumerator GetEnumerator()
        {
            return lines.GetEnumerator();
        }
        /// <summary>
        /// indexer by the line number
        /// </summary>
        /// <param name="lineNum">The line number</param>
        /// <returns>Array that contains all the lines that have the given line number</returns>
        /// <exception cref="IndexOutOfRangeException">The given line number do's not exists in the collection</exception>
        public BusLine[] this[int lineNum] 
        {
            get
            {
                BusLine[] lines = this.lines.Where(p => p.LineNum == lineNum).ToArray();
                if (lines.Length == 0)
                {
                    throw new IndexOutOfRangeException("Line is not exists");
                }
                return lines;
            }
        }
        /// <summary>
        /// Indexer
        /// </summary>
        /// <param name="lineNum">The line number</param>
        /// <param name="lineNumIndex">Index in the list</param>
        /// <returns>returns bus line from the collection by the given line number and index (the lines sorted according to their duration)</returns>
        /// <exception cref="IndexOutOfRangeException">The given line dont exists in the collection OR the index is not in range</exception>
        public BusLine this[int lineNum, int lineNumIndex]
        {
            get
            {
                if (lineNumIndex < 0 || lineNumIndex >= this[lineNum].Length)
                {
                    throw new IndexOutOfRangeException("Line is not exists");
                }
                return this[lineNum][lineNumIndex];
            }
        }
        /// <summary>
        /// Check if a bus line is in the collection 
        /// </summary>
        /// <param name="number">The line number</param>
        /// <returns>true if the line exists in the collection. else return false</returns>
        public bool IsExists (int number)
        {
            return this.lines.Exists(p => p.LineNum == number);
        }
    }
}
