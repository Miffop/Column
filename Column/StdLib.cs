using System;
using System.Collections.Generic;

namespace Column
{
    public class StdLib : ColumnLib
    {
        List<KeyValuePair<string,Method>> Lim;
        public StdLib()
        {
            Lim = new List<KeyValuePair<string, Method>>();
            LibConsole(Lim);
            LibString(Lim);
        }
        public override List<KeyValuePair<string, Method>> GetMeth()
        {
            return Lim;
        }
        public void LibConsole(List<KeyValuePair<string, Method>> Lim)
        {
            ColumnData Con = new ColumnData(null);
            Con["WriteLine"] = new ColumnData(new Method((arg) =>
            {
                for (int i = 0; i < arg.Length; i++)
                {
                    Console.Write(arg[i]!=null?arg[i].ToString():"null");
                }
                Console.WriteLine();
                return null;
            }));
            Con["Write"] = new ColumnData(new Method((arg) =>
            {
                for (int i = 0; i < arg.Length; i++)
                {
                    Console.Write(arg[i].ToString());
                }
                return null;
            }));
            Con["WriteChar"] = new ColumnData(new Method((arg) =>
            {
                for (int i = 0; i < arg.Length; i++)
                {
                    Console.Write((char)((int)arg[i]));
                }
                return null;
            }));
            Con["ReadLine"] = new ColumnData(new Method((arg) =>
            {
                return Console.ReadLine();
            }));
            Con["ReadKey"] = new ColumnData(new Method((arg) =>
            {
                return (int)Console.ReadKey().KeyChar;
            }));
            Lim.Add(new KeyValuePair<string, Method>("Console|WriteLine", Con["WriteLine"].Value as Method));
            Lim.Add(new KeyValuePair<string, Method>("Console|Write", Con["Write"].Value as Method));
            Lim.Add(new KeyValuePair<string, Method>("Console|WriteChar", Con["WriteChar"].Value as Method));
            Lim.Add(new KeyValuePair<string, Method>("Console|ReadLine", Con["ReadLine"].Value as Method));
            Lim.Add(new KeyValuePair<string, Method>("Console|ReadKey", Con["ReadKey"].Value as Method));
        }
        public void LibString(List<KeyValuePair<string, Method>> Lim)
        {
            ColumnData Str = new ColumnData(null);
            Str["Char"] = new ColumnData(new Method((arg) =>
             {
                 return (int)((arg[0] as string)[(int)arg[1]]);
             }));
            Str["Sub"] = new ColumnData(new Method((arg) =>
            {
                return (arg.Length == 2) ? (arg[0] as string).Substring((int)arg[1]) : (arg[0] as string).Substring((int)arg[1], (int)arg[2]);
            }));
            Str["Length"] = new ColumnData(new Method((arg) =>
            {
                return (arg[0] as string).Length;
            }));
            Lim.Add(new KeyValuePair<string, Method>("String|Char", Str["Char"].Value as Method));
            Lim.Add(new KeyValuePair<string, Method>("String|Sub", Str["Sub"].Value as Method));
            Lim.Add(new KeyValuePair<string, Method>("String|Length", Str["Length"].Value as Method));
        }
        public void LibObject(List<KeyValuePair<string, Method>> Lim)
        {
            Lim.Add(new KeyValuePair<string, Method>("Null", (arg) =>
            {
                return null;
            }));
        }

    }
}
