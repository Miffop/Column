using System;
using System.Collections.Generic;


namespace Column.Struct
{
    class Block
    {
        List<Command> Code;
        string[] Args;
        Contex c;
        public Block(Contex con,List<Command> code,string[] arg)
        {
            this.Code = code;
            this.Args = arg;
            this.c = con;
            ToLib = new List<Tuple<string, ColumnData>>();
        }
        List<Tuple<string, ColumnData>> ToLib;
        public void LibVar(string Name,ColumnData val)
        {
            ToLib.Add(new Tuple<string, ColumnData>(Name, val));
        }
        public object Run(params object[] arg)
        {
            c.Push();
            c.StateVar("Result", new ColumnData(null));
            for (int i = 0; i < Args.Length; i++)
            {
                c.StateVar(Args[i], new ColumnData(arg[i]));
            }
            for(int i = 0; i < ToLib.Count; i++)
            {
                c.StateVar(ToLib[i].Item1, ToLib[i].Item2);
            }

            for(int i = 0; i < Code.Count; i++)
            {
                int ec = Code[i].Run(c);
                if(ec==Command.Break)
                {
                    break;
                }
                else if(ec==Command.Continue)
                {
                    c.db.Error("Runtime Error: " + "Cannot \"continue\" the function");
                }
            }

            object ret = c.GetVar("Result").Value;
            c.Pop();
            /*
            if(ret is ColumnData)
            {
                c.db.Error("Runtime error: " + "Method cannot return a reference");
            }
            */
            return ret;
        }
        public Method ToMethod()
        {
            return Run;
        }
    }
}
