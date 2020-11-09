using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dotNet5781_02__6382_0555
{
    class LineBusCollection:IEnumerable
    {
        private List<BusLine> lines;

        public LineBusCollection()
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
            return this.Remove(line);
        }
        public List<BusLine> GetStationLines (BusStation station)
        {
            return new List<BusLine>(lines.Where(p=>p.IsExists(station.Code)));
        }
        public List<BusLine> GetSortedLines() 
        {
            return new List<BusLine>(lines.ToArray());
        }

        public IEnumerator GetEnumerator()
        {
            return lines.GetEnumerator();
        }

        public BusLine this[int lineNum] 
        {
            get
            {
                BusLine[] lines = this.lines.Where(p => p.LineNum == lineNum).ToArray();
                if (lines.Length == 0)
                {
                    throw new IndexOutOfRangeException("Line is not exists");
                }
                return lines[0];
            }
        }
        public BusLine this[int lineNum, bool isFirst]
        {
            get
            {
                BusLine[] lines = this.lines.Where(p => p.LineNum == lineNum).ToArray();
                if (lines.Length == 0)
                {
                    throw new IndexOutOfRangeException ("Line is not exists");
                }
                return isFirst?lines[0]:lines[1];
            }
        }
    }
}
