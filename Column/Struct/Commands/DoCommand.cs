namespace Column.Struct.Commands
{
    class DoCommand:Command
    {
        BlockDo Code;
        public DoCommand(BlockDo code)
        {
            this.Code = code;
        }
        public override int Run(Contex c)
        {
            return Code.Run();
        }
    }
}
