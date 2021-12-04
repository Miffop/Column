using System;

namespace Column.Struct.Commands
{
    class FlagCommand:Command
    {
        int Flag;
        public FlagCommand(int flag)
        {
            this.Flag = flag;
        }
        public override int Run(Contex c)
        {
            return Flag;
        }
    }
}
