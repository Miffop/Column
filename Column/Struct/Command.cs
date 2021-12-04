using System;


namespace Column.Struct
{
    abstract class Command
    {
        public abstract int Run(Contex c);

        public const int None = 0;
        public const int Break = 1;
        public const int Continue = 2;
    }
}
