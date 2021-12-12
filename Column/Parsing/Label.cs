namespace Column.Parsing
{
    public class Label
    {
        public string Command { get; set; }
        public string Args { get; set; }
        public int Line { get; set; }
        public Label(string c,string a)
        {
            this.Command = c;
            this.Args = a;
        }
        public Label(string c,string a,int ln):this(c,a)
        {
            this.Line = ln;
        }
        public override string ToString()
        {
            return Command + ", " + Args + ";";
        }
    }
}
