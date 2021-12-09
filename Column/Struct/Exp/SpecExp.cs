using System;

namespace Column.Struct.Exp
{

    class ObjExp : IExp
    {
        object NIGA;//None Interactive General Abstraction 
        public ObjExp(object niga,int ln)
        {
            this.NIGA = niga;
            this.Line = ln;
        }
        public override object Eval(Contex c)
        {
            return NIGA;
        }
    }
    class VarExp : IExp
    {
        string Name;
        public VarExp(string Vn,int ln)
        {
            this.Name = Vn;
            this.Line = ln;
        }
        public override object Eval(Contex c)
        {
            ColumnData V = c.GetVar(Name);
            return V;
        }
    }
    class ConstSubExp:IExp
    {
        IExp Var;
        string SubName;
        public ConstSubExp(IExp v,string Sn,int ln)
        {
            this.Var = v;
            this.SubName = Sn;
            this.Line = ln;
        }
        public override object Eval(Contex c)
        {
            return (Var.Eval(c) as ColumnData)[SubName];
        }
    }
    class IndexSubExp:IExp
    {
        IExp V, S;
        public IndexSubExp(IExp v,IExp s,int ln)
        {
            this.V = v;
            this.S = s;
            this.Line = ln;
        }
        public override object Eval(Contex c)
        {
            object sub = S.Eval(c);
            if(sub is string)
            {
                return (V.Eval(c) as ColumnData)[sub as string];
            }
            else if(sub is int)
            {
                return (V.Eval(c) as ColumnData)[((int)sub).ToString()];
            }
            else if(sub is float)
            {
                return (V.Eval(c) as ColumnData)[((float)sub).ToString()];
            }
            else
            {
                c.db.Error("Line " + this.Line + ": Runtime Error: " + "wrong type in the index");
                throw new Exception();
            }
            
        }
    }
    class EvalVariableExp:IExp
    {
        IExp VarPtr;
        public EvalVariableExp(IExp vp,int ln)
        {
            this.VarPtr = vp;
            this.Line = ln;
        }
        public override object Eval(Contex c)
        {
            return (VarPtr.Eval(c) as ColumnData).Value;
        }
    }
}
