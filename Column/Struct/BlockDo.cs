using System;
using System.Collections.Generic;


namespace Column.Struct
{
    class BlockDo
    {
        public List<Command> Code;
        Contex c;
        public BlockDo(Contex con, List<Command> code)
        {
            this.Code = code;
            this.c = con;
        }
        public int Run()
        {
            c.Push();

            for (int i = 0; i < Code.Count; i++)
            {
                int ec = Code[i].Run(c);
                if (ec == Command.Break)
                {
                    return Command.Break;
                }
                else if (ec == Command.Continue)
                {
                    return Command.Continue;
                }
            }

            c.Pop();
            return Command.None;
        }
    }
}
