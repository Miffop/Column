using System;


namespace Column.Struct.Exp
{
    class FuncNameFoundationExp:IExp
    {
        string Foundation;
        public FuncNameFoundationExp(string f,int ln)
        {
            this.Foundation = f;
            this.Line=ln;
        }
        public override object Eval(Contex c)
        {
            return this.Foundation;
        }
    }
    class FuncNameSubConstExp:IExp
    {
        IExp Prev;
        string Sub;
        public FuncNameSubConstExp(IExp prev,string sub, int ln)
        {
            this.Prev = prev;
            this.Sub = sub;
            this.Line = ln;
        }
        public override object Eval(Contex c)
        {
            return (Prev.Eval(c) as string) +"|"+ Sub;
        }
    }
    class FuncNameSubDynamicExp : IExp
    {

        IExp Prev;
        IExp Sub;
        public FuncNameSubDynamicExp(IExp prev, IExp sub, int ln)
        {
            this.Prev = prev;
            this.Sub = sub;
            this.Line = ln;
        }
        public override object Eval(Contex c)
        {
            string prev = Prev.Eval(c) as string + "|";
            object sb = Sub.Eval(c);
            if(sb is string)
            {
                return prev + (sb as string);
            }
            else if (sb is double)
            {
                return prev + ((double)sb);
            }
            else if (sb is int)
            {
                return prev + ((int)sb);
            }
            else
            {
                c.db.Error("Line " + this.Line + ": Runtime Error: " + "wrong type in the index");
                throw new Exception();
            }
        }
    }
    class FuncExp : IExp
    {
        IExp LeMethod;
        IExp[] Args;
        public FuncExp(IExp lm, IExp[] args, int ln)
        {
            this.LeMethod = lm;
            this.Args = args;
            this.Line = ln;
        }
        public override object Eval(Contex c)
        {
            string Fname=null;
            try
            {
                Fname = LeMethod.Eval(c) as string;
            }
            catch
            {
                c.db.Error("Line: " + this.Line + ": Runtime error: " + "cannot get method's name");
            }
            Method M = c.GetFunc(Fname);
            if(M==null)
            {
                c.db.Error("Line: " + this.Line + ": Runtime error: " + "method "+Fname+" doesn't exist");
            }
            object[] A = new object[Args.Length];
            for (int i = 0; i < Args.Length; i++) A[i] = Args[i].Eval(c);
            try
            {
                return M(A);
            }
            catch
            {
                c.db.Error("Line: " + this.Line + ": Runtime error: " + "method execution failed");   
                throw new Exception();
            }
        }
    }
}
