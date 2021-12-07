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
            LibObject(Lim);
            LibInt(Lim);
            LibFloat(Lim);
            LibChar(Lim);
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

            Lim.Add(new KeyValuePair<string, Method>("Console|Clear", (arg) =>
            {
                Console.Clear();
                return null;
            }));
            Lim.Add(new KeyValuePair<string, Method>("Console|Title", (arg) =>
            {
                Console.Title = (string)arg[0];
                return null;
            }));
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
            Lim.Add(new KeyValuePair<string, Method>("String", (arg) =>
            {
                return (arg[0] != null ? arg[0].ToString() : "null");
            }));
        }
        public void LibObject(List<KeyValuePair<string, Method>> Lim)
        {
            Lim.Add(new KeyValuePair<string, Method>("Null", (arg) =>
            {
                return null;
            }));
        }
        public void LibInt(List<KeyValuePair<string,Method>> Lim)
        {
            Lim.Add(new KeyValuePair<string, Method>("Int", (arg) =>
            {
                return Convert.ToInt32(arg);
            }));
            Lim.Add(new KeyValuePair<string, Method>("Int|Parse", (arg) =>
            {
                return Int32.Parse((string)arg[0]);
            }));
            Lim.Add(new KeyValuePair<string, Method>("Int|MaxValue", (arg) =>
            {
                return Int32.MaxValue;
            }));
            Lim.Add(new KeyValuePair<string, Method>("Int|MinValue", (arg) =>
            {
                return Int32.MinValue;
            }));
        }
        public void LibFloat(List<KeyValuePair<string, Method>> Lim)
        {
            Lim.Add(new KeyValuePair<string, Method>("Float", (arg) =>
            {
                return Convert.ToDouble(arg);
            }));
            Lim.Add(new KeyValuePair<string, Method>("Float|Parse", (arg) =>
            {
                return Double.Parse((string)arg[0], System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            }));
            Lim.Add(new KeyValuePair<string, Method>("Float|MaxValue", (arg) =>
            {
                return Double.MaxValue;
            }));
            Lim.Add(new KeyValuePair<string, Method>("Float|MinValue", (arg) =>
            {
                return Double.MinValue;
            }));
        }
        public void LibChar(List<KeyValuePair<string, Method>> Lim)
        {
            Lim.Add(new KeyValuePair<string, Method>("Char|IsLetter", (arg) =>
            {
                return Char.IsLetter((char)(int)arg[0]) ? 1 : 0;
            }));
            Lim.Add(new KeyValuePair<string, Method>("Char|IsDigit", (arg) =>
            {
                return Char.IsDigit((char)(int)arg[0]) ? 1 : 0;
            }));
            Lim.Add(new KeyValuePair<string, Method>("Char|IsLetterOrDigit", (arg) =>
            {
                return Char.IsLetterOrDigit((char)(int)arg[0]) ? 1 : 0;
            }));
            Lim.Add(new KeyValuePair<string, Method>("Char|IsUpper", (arg) =>
            {
                return Char.IsUpper((char)(int)arg[0]) ? 1 : 0;
            }));
            Lim.Add(new KeyValuePair<string, Method>("Char|IsLower", (arg) =>
            {
                return Char.IsLower((char)(int)arg[0]) ? 1 : 0;
            }));
            Lim.Add(new KeyValuePair<string, Method>("Char|IsSymbol", (arg) =>
            {
                return Char.IsSymbol((char)(int)arg[0]) ? 1 : 0;
            }));
            Lim.Add(new KeyValuePair<string, Method>("Char|IsWhiteSpace", (arg) =>
            {
                return Char.IsWhiteSpace((char)(int)arg[0]) ? 1 : 0;
            }));
            Lim.Add(new KeyValuePair<string, Method>("Char|ToUpper", (arg) =>
            {
                return (int)Char.ToUpper((char)(int)arg[0]);
            }));
            Lim.Add(new KeyValuePair<string, Method>("Char|ToLower", (arg) =>
            {
                return (int)Char.ToLower((char)(int)arg[0]);
            }));
        }
    }
}
