using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Column;
using Column.Parsing;

namespace ColumnTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Пример интерпретатора Column
            Console.Title = "";
            if(args.Length!=0)
            {
                StreamReader Rd;
                if(!File.Exists(args[0]))
                {
                    Console.WriteLine("File \""+args[0]+"\" doesn't exist");
                    Console.ReadKey();
                    Environment.Exit(1);
                }
                Rd = new StreamReader(args[0]);
                string Code = Rd.ReadToEnd();
                Rd.Close();
                ColumnProgram Prog = null;
                Prog = new ColumnProgram();
                try
                {
                    Prog.Assamble(Code);
                }
                catch
                {
                    Console.WriteLine("We've got some errors:");
                    while (Prog.Debug.IsError)
                    {
                        Console.WriteLine(Prog.Debug.GetError());
                    }
                    Console.ReadKey();
                    Environment.Exit(1);
                }
                try
                {
                    Prog.AddLib(new StdLib());
                }
                catch
                {
                    Console.WriteLine("We've got some errors:");
                    while (Prog.Debug.IsError)
                    {
                        Console.WriteLine(Prog.Debug.GetError());
                    }
                    Console.ReadKey();
                    Environment.Exit(1);
                }
                try
                {
                    Prog.Run();
                }
                catch
                {
                    Console.WriteLine("We've got some errors:");
                    while (Prog.Debug.IsError)
                    {
                        Console.WriteLine(Prog.Debug.GetError());
                    }
                    Console.ReadKey();
                    Environment.Exit(1);
                }
            }

        }
    }
}
