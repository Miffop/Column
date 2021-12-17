using System;
using System.Collections.Generic;

namespace Column.Parsing
{
    class SudoCode
    {
        List<Label> Code;
        public Label this[int index]
        {
            get
            {
                return Code[index];
            }
            set
            {
                Code[index] = value;
            }
        }
        public int Length
        {
            get
            {
                return Code.Count;
            }
        }
        int Line;
        public SudoCode()
        {
            Line = 1;
            Code = new List<Label>();
        }
        public void Ln()
        {
            Line++;
        }
        public void Add(Label l)
        {
            Code.Add(l);
        }
        public void Add(string com, string arg)
        {
            Code.Add(new Label(com, arg, Line));
        }
        public SudoCode SubCode(int StartIndex, int Length)
        {
            SudoCode SC = new SudoCode();
            for (int i = 0; i < Length; i++)
            {
                SC.Add(Code[StartIndex + i]);
            }
            return SC;
        }
        public SudoCode SubCode(int StartIndex)
        {
            SudoCode SC = new SudoCode();
            for (int i = StartIndex; i < Code.Count; i++)
            {
                SC.Add(Code[i]);
            }

            return SC;
        }
        public Label Last
        {
            get
            {
                return this.Code[this.Code.Count-1];
            }
        }
        public static SudoCode Parse(string Code, Debugger db)
        {

            SudoCode Res = new SudoCode();
            for (int i = 0; i < Code.Length; i++)
            {
                int CurrentLine = Res.Line;
                try
                {
                    if (Code[i] == '/')
                    {
                        if (Code[i + 1] == '/')
                        {
                            Res.Add("Brace", "/");
                            i++;
                        }
                        else
                        {
                            Res.Add("Oper", "/");
                        }
                    }
                    else if (Code[i] == '\\')
                    {
                        if (Code[i + 1] == '\\')
                        {
                            Res.Add("Brace", "\\");
                            i++;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                    else if (Code[i] == '(')
                    {
                        Res.Add("Brace", "(");
                    }
                    else if (Code[i] == ')')
                    {
                        Res.Add("Brace", ")");
                    }
                    else if (Code[i] == ';')
                    {
                        Res.Add(";", "");
                    }
                    else if (Code[i] == ',')
                    {
                        Res.Add(",", "");
                    }
                    else if (Code[i] == ':')
                    {
                        i++;
                        if (Code[i] == ':')
                        {
                            Res.Add("::", "");
                        }
                        else if (Char.IsLetterOrDigit(Code[i]))
                        {
                            string name = "";
                            while (Char.IsLetterOrDigit(Code[i]))
                            {
                                name += Code[i];
                                i++;
                            }
                            i--;
                            Res.Add("Ptr", name);
                        }
                        else
                        {
                            throw new Exception();
                        }

                    }
                    else if (Code[i] == '$')
                    {
                        i++;
                        string name = "";
                        while (Char.IsLetterOrDigit(Code[i]) || Code[i] == '|')
                        {
                            name += Code[i];
                            i++;
                        }
                        i--;
                        Res.Add("Meth", name);
                    }
                    else if (Code[i] == '#')
                    {
                        i++;
                        string name = "";
                        while (Char.IsLetterOrDigit(Code[i]))
                        {
                            name += Code[i];
                            i++;
                        }
                        Res.Add("Var", name);
                        i--;
                    }
                    else if (Code[i] == '@')
                    {
                        i++;
                        string name = "";
                        while (Char.IsLetterOrDigit(Code[i]))
                        {
                            name += Code[i];
                            i++;
                        }
                        Res.Add("Run", name);
                        i--;
                    }
                    else if (Code[i] == '!')
                    {
                        if (Code[i + 1] == '=')
                        {
                            Res.Add("Oper", "!=");
                            i++;
                        }
                        else
                        {
                            i++;
                            string name = "";
                            while (Char.IsLetterOrDigit(Code[i]))
                            {
                                name += Code[i];
                                i++;
                            }
                            Res.Add("Ctrl", name);
                            i--;
                        }
                    }
                    else if (Code[i] == '|')
                    {
                        i++;
                        string name = "";
                        while (Char.IsLetterOrDigit(Code[i]))
                        {
                            name += Code[i];
                            i++;
                        }
                        Res.Add("Sub", name);
                        i--;
                    }/*
                    else if (Code[i] == '[')
                    {
                        i++;
                        string name = "";
                        while (Code[i] != ']')
                        {
                            name += Code[i];
                            if (Code[i] == '\n')
                            {
                                Res.Ln();
                            }
                            i++;
                        }
                        Res.Add("Oper", name);
                    }*/
                    else if (Code[i] == '"')
                    {
                        i++;
                        string name = "";
                        while (Code[i] != '"')
                        {
                            name += Code[i];
                            if (Code[i] == '\n')
                            {
                                Res.Ln();
                            }
                            i++;
                        }
                        name = name.Replace("\\n", "\n");
                        name = name.Replace("\\a", "\'");
                        name = name.Replace("\\b", "\\");
                        name = name.Replace("\\d", "\"");
                        Res.Add("String", name);
                    }
                    else if (Code[i] == '\'')
                    {
                        i++;
                        string name = "";
                        while (Code[i] != '\'')
                        {
                            name += Code[i];
                            if (Code[i] == '\n')
                            {
                                Res.Ln();
                            }
                            i++;
                        }
                        name = name.Replace("\\n", "\n");
                        name = name.Replace("\\a", "\'");
                        name = name.Replace("\\b", "\\");
                        name = name.Replace("\\d", "\"");
                        if (name.Length != 1)
                        {
                            db.Error("Line " + Res.Line + ": Parsing Error: " + "cannot parse character");
                        }
                        Res.Add("Int", ((int)name[0]).ToString());
                    }
                    else if (Char.IsDigit(Code[i]))
                    {
                        string Num = "";
                        bool IsFloat = false;
                        while (Char.IsDigit(Code[i]) || Code[i] == '.')
                        {
                            if (Code[i] == '.') IsFloat = true;
                            Num += Code[i];
                            i++;
                        }
                        i--;
                        Res.Add(IsFloat ? "Float" : "Int", Num);
                    }
                    else if (Code[i] == '{')
                    {
                        i++;
                        while (Code[i] != '}')
                        {
                            if (Code[i] == '\n')
                            {
                                Res.Ln();
                            }
                            i++;
                        }
                    }
                    else if (Char.IsLetter(Code[i]))
                    {
                        //i++;
                        string name = "";
                        while (Char.IsLetterOrDigit(Code[i]) || Code[i] == '|')
                        {
                            name += Code[i];
                            if (Code[i] == '\n')
                            {
                                Res.Ln();
                            }
                            i++;
                        }
                        i--;
                        if (Res.Last.Command == "Ctrl" && Res.Last.Args == "meth")
                        {
                            Res.Add("Meth", name);
                        }
                        else if(name=="has")
                        {
                            Res.Add("Oper", "has");
                        }
                        else if(name=="sub")
                        {
                            Res.Add("Oper", "sub");
                        }
                        else
                        {
                            db.Error("Line " + Res.Line + ": Parsing Error: " + "unexpected lexis");
                        }
                        //Res.Add("Word", name);
                    }
                    //Operations
                    else if (Code[i] == '+')
                    {
                        Res.Add("Oper", "+");
                    }
                    else if (Code[i] == '-')
                    {
                        Res.Add("Oper", "-");
                    }
                    else if (Code[i] == '*')
                    {
                        if (Code[i + 1] == '*')
                        {
                            Res.Add("Oper", "**");
                            i++;
                        }
                        else
                        {
                            Res.Add("Oper", "*");
                        }
                    }
                    else if (Code[i] == '%')
                    {
                        Res.Add("Oper", "%");
                    }
                    else if (Code[i] == '=')
                    {
                        if (Code[i + 1] == '=')
                        {
                            Res.Add("Oper", "==");
                            i++;
                        }
                        else if (Char.IsLetterOrDigit(Code[i + 1]))
                        {
                            i++;
                            string name = "";
                            while (Char.IsLetterOrDigit(Code[i]))
                            {
                                name += Code[i];
                                i++;
                            }
                            i--;
                            Res.Add("Val", name);
                        }
                        else
                        {
                            Res.Add("Oper", "=");
                        }
                    }
                    else if (Code[i] == '>')
                    {
                        if (Code[i + 1] == '=')
                        {
                            Res.Add("Oper", ">=");
                            i++;
                        }
                        else if (Code[i + 1] == '>')
                        {
                            Res.Add("Oper", ">>");
                            i++;
                        }
                        else
                        {
                            Res.Add("Oper", ">");
                        }
                    }
                    else if (Code[i] == '<')
                    {
                        if (Code[i + 1] == '=')
                        {
                            Res.Add("Oper", "<=");
                            i++;
                        }
                        else if (Code[i + 1] == '<')
                        {
                            Res.Add("Oper", "<<");
                            i++;
                        }
                        else
                        {
                            Res.Add("Oper", "<");
                        }
                    }
                    else if (Code[i] == '&')
                    {
                        if (Code[i + 1] == '&')
                        {
                            Res.Add("Oper", "&&");
                            i++;
                        }
                        else
                        {
                            Res.Add("Oper", "&");
                        }
                    }
                    else if (Code[i] == '.')
                    {
                        if (Code[i + 1] == '.')
                        {
                            Res.Add("Oper", "||");
                            i++;
                        }
                        else
                        {
                            Res.Add("Oper", "|");
                        }
                    }
                    else if (Code[i] == '^')
                    {
                        if (Code[i + 1] == '^')
                        {
                            Res.Add("Oper", "^^");
                            i++;
                        }
                        else
                        {
                            Res.Add("Oper", "^");
                        }
                    }
                    else if (Code[i] == '~')
                    {
                        if (Code[i + 1] == '~')
                        {
                            Res.Add("Oper", "~~");
                            i++;
                        }
                        else
                        {
                            Res.Add("Oper", "~");
                        }
                    }
                    else if (Code[i] == '_')
                    {
                        Res.Add("Oper", "_");
                    }
                    else if (Code[i] == '\n')
                    {
                        Res.Ln();
                    }
                }
                catch
                {
                    db.Error("Line " + CurrentLine + ": Parsing Error: " + "Analisys failed");
                }
            }
            return Res;
        }
    }
}
