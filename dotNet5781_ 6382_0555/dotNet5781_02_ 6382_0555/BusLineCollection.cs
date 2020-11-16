using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotNet5781_02__6382_0555
{
    class BusLineCollection:IEnumerable
    {
        private List<BusLine> lines;

        public BusLineCollection()
        {
            this.lines = new List<BusLine>();
        }
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
        public bool Remove(BusLine line)
        {
            return this.lines.Remove(line);
        }
        public List<BusLine> GetStationLines (int code)
        {
            List<BusLine> ret= new List<BusLine>(lines.Where(line => line.IsExists(code)));
            if (ret.Count == 0) {
                throw new InvalidOperationException("That station not contains any line.");
            }
            return ret;
        }
        public List<BusLine> GetSortedLines() 
        {
            return new List<BusLine>(lines.ToArray());
        }

        public IEnumerator GetEnumerator()
        {
            return lines.GetEnumerator();
        }

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
        public bool IsExists (int number)
        {
            return this.lines.Exists(p => p.LineNum == number);
        }
    }
}
