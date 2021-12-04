using System;


namespace Column.Struct.Commands
{
    class CondCommand:Command
    {
        public Command YesYesYes, NoNoNo;
        IExp Cond;
        public CondCommand(IExp cnd, Command yes, Command no)
        {
            this.YesYesYes = yes;
            this.NoNoNo = no;
            this.Cond = cnd;
        }
        public override int Run(Contex c)
        {
            try
            {
                if (((int)Cond.Eval(c)) != 0)
                {
                    return YesYesYes.Run(c);
                }
                else if (NoNoNo != null)
                {
                    return NoNoNo.Run(c);
                }
            }
            catch
            {
                c.db.Error("Line " + Cond.Line + ": Runtime Error: " + "if execution failed");
            }
            return Command.None;
        }
    }
}
