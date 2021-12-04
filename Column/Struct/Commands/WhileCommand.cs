using System;

namespace Column.Struct.Commands
{
    class WhileCommand:Command
    {
        IExp A;
        BlockDo Loop;
        public WhileCommand(IExp cnd,BlockDo loop)
        {
            this.A = cnd;
            this.Loop = loop;
        }
        public override int Run(Contex c)
        {

            while((int)A.Eval(c)!=0)
            {
                if(Loop.Run()==Command.Break)
                {
                    break;
                }
            }
            return Command.None;
        }
    }
}
