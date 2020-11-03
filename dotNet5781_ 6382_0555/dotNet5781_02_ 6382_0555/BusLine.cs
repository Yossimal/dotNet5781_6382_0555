using System;
using System.Collections.Generic;
using System.Text;

namespace dotNet5781_02__6382_0555
{
    enum Area { General = 'g', North = 'n', South = 's', Center = 'c', Jerusalum = 'j' }
    class BusLine
    {
        private List<LineBusStation> path;
        private int code;
        private Area area;
        public BusStation First { get => path[0]; }
        public BusStation Last { get => path[path.Count - 1]; }
        

    }
}
