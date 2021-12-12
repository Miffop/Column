using Column.Parsing;
using Column.Struct;
using System;
using System.Collections.Generic;

namespace Column
{
    public class ColumnProgram
    {
        Block Code;
        public Debugger Debug { get; private set; }
        Contex Meta;
        public ColumnProgram()
        {
            Meta = new Contex(null,Debug);
            Debug = new Debugger();
        }
        public void Assamble(string code)
        {
            try
            {
                this.Code = Parser.Parse(code, Debug, Meta);
            }
            catch
            {
                if (!Debug.IsError) Debug.Error("Parsing error: unknown");
                else throw new Exception();
            }
        }
        public void AddLib(ColumnLib Lib)
        {
            if (Code != null)
            {
                try
                {
                    List<KeyValuePair<string, Method>> lib = Lib.GetMeth();
                    for(int i=0;i<lib.Count;i++)
                    {
                        Meta.StateFunc(lib[i].Key, lib[i].Value);
                    }
                }
                catch
                {
                    Debug.Error("Can't load library");
                }
            }
            else
            {
                Debug.Error("Can't load library to unassambled program");
            }
        }
        public void Run()
        {
            if (Code != null)
            {
                try
                {
                    Code.Run();
                }
                catch
                {
                    if (!Debug.IsError) Debug.Error("Runtime error: unknown");
                    else throw new Exception();
                }
            }
            else
            {
                Debug.Error("Can't run unassambled program");
            }
        }
    }
}
