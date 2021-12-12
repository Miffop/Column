namespace Column.Struct.Commands
{
    class RunCom:Command
    {
        IExp Com;
        public RunCom(IExp a)
        {
            this.Com = a;
        }
        public override int Run(Contex c)
        {
            Com.Eval(c);
            return Command.None;
        }
    }
}
