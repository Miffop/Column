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
        public string Name { get; private set; }
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
        public IExp Var { get; private set; }
        public string SubName { get; private set; }
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
        public IExp V { get; private set; }
        public IExp S { get; private set; }
        public IndexSubExp(IExp v,IExp s,int ln)
        {
            this.V = v;
            this.S = s;
            this.Line = ln;
        }
        public override object Eval(Contex c)
        {
            object sub = S.Eval(c);
            return (V.Eval(c) as ColumnData)[GetSubVariableName(sub, this.Line, c.db)];
        }
        public static string GetSubVariableName(object sub,int Line,Debugger db)
        {
            if (sub is string || sub is int || sub is double)
            {
                return sub.ToString();
            }
            else
            {
                db.Error("Line " + Line + ": Runtime Error: " + "wrong type in the index");
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
