using System;
using System.Collections.Generic;

namespace Column
{
    public class ColumnData
    {
        public object Value { get; set; }
        Dictionary<string, ColumnData> Element;

        
        public ColumnData(object val)
        {
            this.Value = val;
            this.Element = new Dictionary<string, ColumnData>();
        }
        public ColumnData this[string name]
        {
            get
            {
                ColumnData Res;
                if(Element.TryGetValue(name, out Res))
                {
                    return Res;
                }
                else
                {
                    ColumnData Mn = new ColumnData(null);
                    Element.Add(name, Mn);
                    return Mn;
                }

            }
            set
            {
                try
                {
                    Element[name] = value;
                }
                catch
                {
                    Element.Add(name, value);
                }
            }
        }
        public bool Exist(string name)
        {
            ColumnData res;
            return Element.TryGetValue(name, out res);
        }
    }
}
