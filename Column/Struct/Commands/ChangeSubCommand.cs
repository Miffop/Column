using Column.Struct.Exp;

namespace Column.Struct.Commands
{
    class ChangeSubCommand:Command
    {
        IExp Var;
        IExp Ptr;
        public ChangeSubCommand(IExp vr,IExp ptr)
        {
            this.Var = vr;
            this.Ptr = ptr;
        }
        public override int Run(Contex c)
        {
            if(Var is ConstSubExp)
            {
                ConstSubExp PVar = Var as ConstSubExp;
                ColumnData V = (ColumnData)(PVar.Var.Eval(c));
                V[PVar.SubName] = (ColumnData)Ptr.Eval(c);
            }
            else if(Var is IndexSubExp)
            {
                IndexSubExp PVar = Var as IndexSubExp;
                ColumnData V = (ColumnData)(PVar.V.Eval(c));
                object Index = PVar.S.Eval(c);
                if(Index is string)
                {
                    V[(string)Index] = (ColumnData)Ptr.Eval(c);
                }
                else if (Index is int)
                {
                    V[((int)Index).ToString()] = (ColumnData)Ptr.Eval(c);
                }
                else if (Index is double)
                {
                    V[((double)Index).ToString()] = (ColumnData)Ptr.Eval(c);
                }
                else
                {
                    c.db.Error("Line " + PVar.S.Line + ": Runtime Error: " + "wrong type in the index");
                }
            }
            return Command.None;
        }
    }
}
