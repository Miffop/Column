using System;

namespace Column.Struct.Exp
{
    class EqualExp : IExp
    {
        IExp A, B;
        public EqualExp(IExp a,IExp b,int ln)
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
                if (a is string || b is string)
                {
                    return (string)a == (string)b ? 1 : 0;
                }
                else if (a is double || b is double)
                {
                    return Convert.ToDouble(a) == Convert.ToDouble(b) ? 1 : 0;
                }
                else if (a is int && b is int)
                {
                    return (int)a == (int)b ? 1 : 0;
                }
                else
                {
                    return a == b ? 1 : 0;
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [==]");
                throw new Exception();
            }
        }
    }
    class NotEqualExp : IExp
    {
        IExp A, B;
        public NotEqualExp(IExp a, IExp b,int ln)
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
                if (a is string || b is string)
                {
                    return (string)a != (string)b ? 1 : 0;
                }
                else if (a is double || b is double)
                {
                    return Convert.ToDouble(a) != Convert.ToDouble(b) ? 1 : 0;
                }
                else if (a is int && b is int)
                {
                    return (int)a != (int)b ? 1 : 0;
                }
                else
                {
                    return a != b ? 1 : 0;
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [!=]");
                throw new Exception();
            }
        }
    }
    class GreaterExp : IExp
    {
        IExp A, B;
        public GreaterExp(IExp a, IExp b,int ln)
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
                if (a is double || b is double)
                {
                    return Convert.ToDouble(a) > Convert.ToDouble(b) ? 1 : 0;
                }
                else if (a is int || b is int)
                {
                    return (int)a > (int)b ? 1 : 0;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [>]");
                throw new Exception();
            }
        }
    }
    class LessExp : IExp
    {
        IExp A, B;
        public LessExp(IExp a, IExp b,int ln)
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
                if (a is double || b is double)
                {
                    return Convert.ToDouble(a) < Convert.ToDouble(b) ? 1 : 0;
                }
                else if (a is int || b is int)
                {
                    return (int)a < (int)b ? 1 : 0;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [<]");
                throw new Exception();
            }
        }
    }
    class GreaterEqualExp : IExp
    {
        IExp A, B;
        public GreaterEqualExp(IExp a, IExp b,int ln)
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
                if (a is double || b is double)
                {
                    return Convert.ToDouble(a) >= Convert.ToDouble(b) ? 1 : 0;
                }
                else if (a is int || b is int)
                {
                    return (int)a >= (int)b ? 1 : 0;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [>=]");
                throw new Exception();
            }
        }
    }
    class LessEqualExp : IExp
    {
        IExp A, B;
        public LessEqualExp(IExp a, IExp b,int ln)
        {
            this.A = a;
            this.B = b;
        }
        public override object Eval(Contex c)
        {
            object a = A.Eval(c);
            object b = B.Eval(c);
            try
            {
                if (a is double || b is double)
                {
                    return Convert.ToDouble(a) <= Convert.ToDouble(b) ? 1 : 0;
                }
                else if (a is int || b is int)
                {
                    return (int)a <= (int)b ? 1 : 0;
                }
                else
                {
                    throw new Exception();
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [<=]");
                throw new Exception();
            }
        }
    }


}
