using System;
using System.Collections.Generic;

namespace Column
{
    public abstract class ColumnLib
    {
        public string Name { get; protected set; }
        public abstract List<KeyValuePair<string,Method>> GetMeth();
    }
}
