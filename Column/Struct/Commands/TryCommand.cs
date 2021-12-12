namespace Column.Struct.Commands
{
    class TryCommand:Command
    {
        Command Try, Catch;
        public TryCommand(Command trye,Command catche)
        {
            this.Try = trye;
            this.Catch = catche;
        }
        public override int Run(Contex c)
        {
            try
            {
                return Try.Run(c);
            }
            catch
            {
                c.db.ClearErrors();
                if(Catch!=null)
                {
                    return Catch.Run(c);
                }
                return Command.None;
            }
        }
    }
}
