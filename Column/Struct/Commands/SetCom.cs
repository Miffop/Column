namespace Column.Struct.Commands
{
    class SetCom:Command
    {
        IExp Var;
        IExp Exp;
        public SetCom(IExp V,IExp B)
        {
            this.Var = V;
            this.Exp = B;
        }
        public override int Run(Contex c)
        {
            ColumnData V = Var.Eval(c) as ColumnData;
            
            V.Value = Exp.Eval(c);
            return Command.None;
        }
    }
}
