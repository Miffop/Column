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
            int res=Command.None;
            while ((int)A.Eval(c)!=0)
            {
                res = Loop.Run();
                if (res>=Command.Break)
                {
                    break;
                }
            }
            return (res == Command.Return) ? res : Command.None;
        }
    }
}
