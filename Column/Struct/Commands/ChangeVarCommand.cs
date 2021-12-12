namespace Column.Struct.Commands
{
    class ChangeVarCommand:Command
    {
        string VarName;
        IExp Ptr;
        public ChangeVarCommand(string vr,IExp ptr)
        {
            this.VarName = vr;
            this.Ptr = ptr;
        }
        public override int Run(Contex c)
        {
            c.SetVar(VarName,(ColumnData)Ptr.Eval(c));
            return Command.None;
        }
    }
}
