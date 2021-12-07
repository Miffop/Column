using System;

namespace Column.Struct.Exp
{
    class SumExp : IExp
    {
        IExp A, B;
        public SumExp(IExp a, IExp b,int ln)
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
                    if (a is int)
                    {
                        char cha = (char)(int)a;
                        return cha + b.ToString();
                    }
                    else if (b is int)
                    {
                        char chb = (char)(int)b;
                        return a.ToString() + chb;
                    }
                    else
                    {
                        return a.ToString() + b.ToString();
                    }
                }
                else if (a is double || b is double)
                {
                    return Convert.ToDouble(a) + Convert.ToDouble(b);
                }
                else
                {
                    return (int)a + (int)b;
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [+]");
                throw new Exception();
            }
        }
    }
    class DiffExp : IExp
    {
        IExp A, B;
        public DiffExp(IExp a, IExp b,int ln)
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
                    return Convert.ToDouble(a) - Convert.ToDouble(b);
                }
                else
                {
                    return (int)a - (int)b;
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [-]");
                throw new Exception();
            }
        }
    }
    class MultExp : IExp
    {
        IExp A, B;
        public MultExp(IExp a, IExp b,int ln)
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
                    return Convert.ToDouble(a) * Convert.ToDouble(b);
                }
                else
                {
                    return (int)a * (int)b;
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [*]");
                throw new Exception();
            }
        }
    }
    class DivExp : IExp
    {
        IExp A, B;
        public DivExp(IExp a, IExp b,int ln)
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
                    return Convert.ToDouble(a) / Convert.ToDouble(b);
                }
                else
                {
                    return (int)a / (int)b;
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [/]");
                throw new Exception();
            }
        }
    }
    class ModExp : IExp
    {
        IExp A, B;
        public ModExp(IExp a, IExp b,int ln)
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
                    double ret= Convert.ToDouble(a) % Convert.ToDouble(b);
                    if (ret < 0 && Convert.ToDouble(b) > 0)
                    {
                        ret += Convert.ToDouble(b);
                    }
                    else if (ret > 0 && Convert.ToDouble(b) < 0)
                    {
                        ret += Convert.ToDouble(b);
                    }
                    return ret;
                }
                else
                {
                    int ret= (int)a % (int)b;
                    if(ret<0 && (int)b>0)
                    {
                        ret += (int)b;
                    }
                    else if(ret>0 && (int)b<0)
                    {
                        ret += (int)b;
                    }
                    return ret;
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [%]");
                throw new Exception();
            }
        }
    }
    class PowExp : IExp
    {
        IExp A, B;
        public PowExp(IExp a, IExp b,int ln)
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
                    return Math.Pow(Convert.ToDouble(a), Convert.ToDouble(b));
                }
                else
                {
                    int Base = (int)a;
                    int Pow = (int)b;
                    int Res = 1;

                    while (Pow > 0)
                    {
                        if ((Pow & 1) == 1)
                        {
                            Res *= Base;
                        }
                        Base *= Base;
                        Pow >>= 1;
                    }
                    return Res;
                }
            }
            catch
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [**]");
                throw new Exception();
            }
        }
    }

    class NegExp : IExp
    {
        IExp A;
        public NegExp(IExp a,int ln)
        {
            this.A = a;
            this.Line = ln;
        }
        public override object Eval(Contex c)
        {
            object a = A.Eval(c);
            if (a is double)
            {
                return -((double)a);
            }
            else if (a is int)
            {
                return -((int)a);
            }
            else
            {
                c.db.Error("Line: " + this.Line + " :Runtime error: " + "can't do operation [_]");
                throw new Exception();
            }
        }
    }
}
