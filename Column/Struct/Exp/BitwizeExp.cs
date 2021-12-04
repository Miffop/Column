using System;

namespace Column.Struct.Exp
{

    class AndExp : IExp
    {
        IExp A, B;
        public AndExp(IExp a, IExp b,int ln)
        {
            this.A = a;
            this.B = b;
            this.Line = ln;
        }
        public override object Eval(Contex c)
        {
            object a = A.Eval(c);
            object b = B.Eval(c);
            try
            {
                if (a is int || b is int)
                {
                    return (int)a & (int)b;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [&]");
                throw new Exception();
            }
        }
    }
    class OrExp : IExp
    {
        IExp A, B;
        public OrExp(IExp a, IExp b,int ln)
        {
            this.A = a;
            this.B = b;
            this.Line = ln;
        }
        public override object Eval(Contex c)
        {
            object a = A.Eval(c);
            object b = B.Eval(c);
            try
            {
                if (a is int || b is int)
                {
                    return (int)a & (int)b;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [|]");
                throw new Exception();
            }
        }
    }
    class XorExp : IExp
    {
        IExp A, B;
        public XorExp(IExp a, IExp b,int ln)
        {
            this.A = a;
            this.B = b;
            this.Line = ln;
        }
        public override object Eval(Contex c)
        {
            object a = A.Eval(c);
            object b = B.Eval(c);
            try
            {
                if (a is int || b is int)
                {
                    return (int)a ^ (int)b;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [^]");
                throw new Exception();
            }
        }
    }
    class NotExp:IExp
    {
        IExp A;
        public NotExp(IExp a,int ln)
        {
            this.A = a;
            this.Line = ln;
        }
        public override object Eval(Contex c)
        {
            object a = A.Eval(c);
            try
            {
                if (a is int)
                {
                    return ~((int)a);
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [~]");
                throw new Exception();
            }
        }
    }
    class ShlExp : IExp
    {
        IExp A, B;
        public ShlExp(IExp a, IExp b,int ln)
        {
            this.A = a;
            this.B = b;
            this.Line = ln;
        }
        public override object Eval(Contex c)
        {
            object a = A.Eval(c);
            object b = B.Eval(c);
            try
            {
                if (a is int || b is int)
                {
                    return (int)a << (int)b;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [<<]");
                throw new Exception();
            }
        }
    }
    class ShrExp : IExp
    {
        IExp A, B;
        public ShrExp(IExp a, IExp b,int ln)
        {
            this.A = a;
            this.B = b;
            this.Line = ln;
        }
        public override object Eval(Contex c)
        {
            object a = A.Eval(c);
            object b = B.Eval(c);
            try
            {
                if (a is int || b is int)
                {
                    return (int)a >> (int)b;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [>>]");
                throw new Exception();
            }
        }
    }
}
