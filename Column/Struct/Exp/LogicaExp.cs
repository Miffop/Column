using System;

namespace Column.Struct.Exp
{
    class LAndExp : IExp
    {
        IExp A, B;
        public LAndExp(IExp a, IExp b, int ln)
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
                    return ((int)a != 0) && ((int)b != 0) ? 1 : 0;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [&&]");
                throw new Exception();
            }
        }
    }
    class LOrExp : IExp
    {
        IExp A, B;
        public LOrExp(IExp a, IExp b, int ln)
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
                    return ((int)a != 0) || ((int)b != 0) ? 1 : 0;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [||]");
                throw new Exception();
            }
        }
    }
    class LXorExp : IExp
    {
        IExp A, B;
        public LXorExp(IExp a, IExp b, int ln)
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
                    return ((int)a != 0) ^ ((int)b != 0) ? 1 : 0;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [^^]");
                throw new Exception();
            }
        }
    }
    class LNotExp : IExp
    {
        IExp A;
        public LNotExp(IExp a, int ln)
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
                    return ((int)a != 0) ? 0 : 1;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [~~]");
                throw new Exception();
            }
        }
    }
}
