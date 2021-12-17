using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Column.Struct.Exp
{
    class HasExp:IExp
    {
        IExp Ptr;
        IExp Sub;
        public HasExp(IExp ptr,IExp sub,int ln)
        {
            this.Ptr = ptr;
            this.Sub = sub;
            this.Line = ln;
        }
        public override object Eval(Contex c)
        {
            ColumnData Var = Ptr.Eval(c) as ColumnData;
            object subName = Sub.Eval(c);

            if(Var!=null)
            {
                return Var.Exist(IndexSubExp.GetSubVariableName(subName, this.Line, c.db)) ? 1 : 0;
            }
            else
            {
                c.db.Error("Line " + this.Line + ": Runtime Error: " + "value is not a pointer");
                throw new Exception();
            }
        }
    }
}
