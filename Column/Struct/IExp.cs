namespace Column.Struct
{
    abstract class IExp
    {
        public abstract object Eval(Contex c);
        public int Line;
    }
}
