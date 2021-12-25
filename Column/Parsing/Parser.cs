using Column.Struct;
using Column.Struct.Commands;
using Column.Struct.Exp;
using System;
using System.Collections.Generic;

namespace Column.Parsing
{
    class Parser
    {
        public static Block Parse(string Code,Debugger db,Contex outer)
        {
            SudoCode SC = SudoCode.Parse(Code,db);
            Contex C = new Contex(outer, db);
            return new Block(C, ParseBlock(SC, C,db), new string[0]);
        }
        private static List<Command> ParseBlock(SudoCode SC, Contex c, Debugger db)
        {
            List<Command> Code = new List<Command>();
            for (int i = 0; i < SC.Length; i++)
            {
                if (SC[i].Command == "Var")
                {
                    SudoCode Sub = SC.SubCode(i);

                    int j;
                    IExp VarExp = ParseVar(Sub, c, db, out j, false);
                    i += j;
                    if (SC[i].Command != "::" && SC[i].Command != "OperOn")
                    { 
                        db.Error("Line " + SC[i].Line + ": Parsing Error: " + "'::' expected"); 
                    }
                    Label Oper = SC[i];
                    i++;
                    int BraceCounter = 0;
                    j = 0;
                    int Idx = i;
                    while (BraceCounter != 0 || SC[i].Command != ";")
                    {
                        if (SC[i].Command == "Brace")
                        {
                            if (SC[i].Args == "/" || SC[i].Args == "(") BraceCounter++;
                            else if (SC[i].Args == "\\" || SC[i].Args == ")") BraceCounter--;
                        }
                        i++;
                        j++;
                    }
                    IExp ValExp = null;
                    if (j != 0)
                    {
                        ValExp = ParseExp(SC.SubCode(Idx, j), c, db);
                    }
                    if (Oper.Command!="::")
                    {
                        bool IsUn;
                        bool IsNull = (ValExp == null);
                        ValExp = ParseOperation(Oper, new EvalVariableExp(VarExp, VarExp.Line), ValExp, c, db,out IsUn);
                        if(IsNull != IsUn)
                        {
                            db.Error("Line " + Oper.Line + ": Parsing Error: " + "';' expected");
                        }
                    }
                    Code.Add(new SetCom(VarExp, ValExp));
                    //db.Error("Line " + SC[i].Line + ": Parsing Error: " + "'::' expected");
                }
                else if(SC[i].Command=="Ptr")
                {
                    SudoCode Sub = SC.SubCode(i);
                    int j;
                    IExp VarExp = ParseVar(Sub, c, db, out j, false);
                    i += j;
                    if (SC[i].Command != "::")
                    {
                        db.Error("Line " + SC[i].Line + ": Parsing Error: " + "'::' expected");
                    }
                    i++;
                    int BraceCounter = 0;
                    j = 0;
                    int Idx = i;
                    while (BraceCounter != 0 || SC[i].Command != ";")
                    {
                        if (SC[i].Command == "Brace")
                        {
                            if (SC[i].Args == "/" || SC[i].Args == "(") BraceCounter++;
                            else if (SC[i].Args == "\\" || SC[i].Args == ")") BraceCounter--;
                        }
                        i++;
                        j++;
                    }
                    IExp ValExp = ParseExp(SC.SubCode(Idx, j), c, db);
                    //Code.Add(new SetCom(VarExp, ValExp));
                    if(VarExp is VarExp)
                    {
                        string N = (VarExp as VarExp).Name;
                        Code.Add(new ChangeVarCommand(N, ValExp));
                    }
                    else
                    {
                        Code.Add(new ChangeSubCommand(VarExp, ValExp));
                    }
                }
                else if (SC[i].Command == "Run")
                {
                    int j = i;
                    int BraceCounter = 0;
                    try
                    {
                        while (SC[i].Command != ";" || BraceCounter != 0)
                        {
                            if (SC[i].Command == "Brace")
                            {
                                if (SC[i].Args == "/" || SC[i].Args == "(") BraceCounter++;
                                else if (SC[i].Args == "\\" || SC[i].Args == ")") BraceCounter--;
                            }
                            i++;
                        }
                        Code.Add(new RunCom(ParseExp(SC.SubCode(j, i - j), c, db)));
                    }
                    catch
                    {
                        db.Error("Line " + SC[j].Line + ": Parsing Error: " + "can't understand the command");
                    }
                }
                else if(SC[i].Command=="Ctrl")
                {
                    if (SC[i].Args=="if")
                    {
                        i++;
                        if (SC[i].Command != "Brace" || SC[i].Args != "(")
                        { 
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "'(' expected");
                        }
                        //Parse Condition
                        int Idx = i + 1;
                        int j = FindSizeOfCodeInBraces(SC, ref i);
                        IExp Cond=null;
                        try
                        {
                            Cond= ParseExp(SC.SubCode(Idx, j), c, db);
                        }
                        catch
                        {
                            db.Error("Line " + SC[Idx].Line + ": Parsing Error: " + "cannot parse condition");
                        }
                        //Parse Body
                        Idx = i+1;
                        j= FindSizeOfCodeInBraces(SC,ref i);
                        BlockDo Yes = null;
                        try
                        {
                            Contex ctx = new Contex(c, db);
                            Yes = new BlockDo(ctx, ParseBlock(SC.SubCode(Idx, j), ctx, db));
                        }
                        catch
                        {
                            db.Error("Line " + SC[Idx].Line + ": Parsing Error: " + "cannot parse codeblock");
                        }
                        Code.Add(new CondCommand(Cond, new DoCommand(Yes), null));
                        i--;
                    }
                    else if(SC[i].Args=="else")
                    {
                        CondCommand CurrentIf = Code[Code.Count - 1] as CondCommand;
                        if (CurrentIf==null)
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "\"else\" is not expected");
                        }
                        while(CurrentIf.NoNoNo!=null)
                        {
                            CurrentIf = CurrentIf.NoNoNo as CondCommand;
                            if (CurrentIf == null)
                            {
                                db.Error("Line " + SC[i].Line + ": Parsing Error: " + "\"else\" is not expected");
                            }
                        }
                        i++;
                        if (SC[i].Command == "Ctrl" && SC[i].Args == "if")//WET
                        {
                            i++;
                            if (SC[i].Command != "Brace" || SC[i].Args != "(")
                            {
                                db.Error("Line " + SC[i].Line + ": Parsing Error: " + "'(' expected");
                            }
                            //Parse Condition
                            int Idx = i + 1;
                            int j = FindSizeOfCodeInBraces(SC, ref i);
                            IExp Cond = null;
                            try
                            {
                                Cond = ParseExp(SC.SubCode(Idx, j), c, db);
                            }
                            catch
                            {
                                db.Error("Line " + SC[Idx].Line + ": Parsing Error: " + "cannot parse condition");
                            }
                            //Parse Body
                            Idx = i + 1;
                            j = FindSizeOfCodeInBraces(SC, ref i);
                            BlockDo Yes = null;
                            try
                            {
                                Contex ctx = new Contex(c, db);
                                Yes = new BlockDo(ctx, ParseBlock(SC.SubCode(Idx, j), ctx, db));
                            }
                            catch
                            {
                                db.Error("Line " + SC[Idx].Line + ": Parsing Error: " + "cannot parse codeblock");
                            }
                            CurrentIf.NoNoNo=new CondCommand(Cond, new DoCommand(Yes), null);
                        }
                        else if (SC[i].Command == "Brace" && SC[i].Args == "/")
                        {
                            int Idx = i + 1;
                            int j = FindSizeOfCodeInBraces(SC, ref i);
                            BlockDo code = null;
                            try
                            {
                                Contex ctx = new Contex(c, db);
                                code = new BlockDo(ctx, ParseBlock(SC.SubCode(Idx, j), ctx, db));
                            }
                            catch
                            {
                                db.Error("Line " + SC[Idx].Line + ": Parsing Error: " + "cannot parse codeblock");
                            }
                            CurrentIf.NoNoNo = new DoCommand(code);
                        }
                        else
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "'/' expected");
                        }
                        i--;
                    }
                    else if (SC[i].Args == "while")
                    {
                        i++;
                        if (SC[i].Command != "Brace" || SC[i].Args != "(")
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "'(' expected");
                        }
                        //Parse Condition
                        int Idx = i + 1;
                        int j = FindSizeOfCodeInBraces(SC, ref i);
                        IExp Cond = null;
                        try
                        {
                            Cond = ParseExp(SC.SubCode(Idx, j), c, db);
                        }
                        catch
                        {
                            db.Error("Line " + SC[Idx].Line + ": Parsing Error: " + "cannot parse condition");
                        }
                        //Parse Body
                        Idx = i + 1;
                        j = FindSizeOfCodeInBraces(SC, ref i);
                        BlockDo DoLoop = null;
                        try
                        {
                            Contex ctx = new Contex(c, db);
                            DoLoop = new BlockDo(ctx, ParseBlock(SC.SubCode(Idx, j), ctx, db));
                        }
                        catch
                        {
                            db.Error("Line " + SC[Idx].Line + ": Parsing Error: " + "cannot parse codeblock");
                        }
                        Code.Add(new WhileCommand(Cond, DoLoop));
                        i--;
                    }
                    else if(SC[i].Args=="meth")
                    {
                        i++;
                        if (SC[i].Command != "Meth")
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "method's name expected");
                        }
                        string MethName = SC[i].Args;
                        i++;
                        if (SC[i].Command != "Brace" || SC[i].Args != "(")
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "'(' expected");
                        }
                        //Parse input
                        i++;
                        List<string> ArgNames = new List<string>();
                        while(SC[i].Command!="Brace")
                        {
                            if(SC[i].Command=="Var")
                            {
                                ArgNames.Add(SC[i].Args);
                            }
                            else if(SC[i].Command!=",")
                            {
                                db.Error("Line " + SC[i].Line + ": Parsing Error: " + " variable name or ',' expected");
                            }
                            i++;
                        }
                        if (SC[i].Command != "Brace" || SC[i].Args != ")")
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "')' expected");
                        }
                        i++;
                        if (SC[i].Command != "Brace" || SC[i].Args != "/")
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "'//' expected");
                        }
                        int Idx = i + 1;
                        int j = FindSizeOfCodeInBraces(SC, ref i);
                        
                        Block fun = null;
                        try
                        {
                            Contex ctx = new Contex(c, db);
                            fun = new Block(ctx, ParseBlock(SC.SubCode(Idx,j), ctx, db), ArgNames.ToArray());
                        }
                        catch
                        {
                            db.Error("Line " + SC[Idx].Line + ": Parsing Error: " + "cannot parse codeblock");
                        }
                        c.StateFunc(MethName, fun.ToMethod());
                        i--;
                    }
                    else if(SC[i].Args=="break")
                    {
                        i++;
                        if(SC[i].Command!=";")
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "';' expected");
                        }
                        Code.Add(new FlagCommand(Command.Break));
                    }
                    else if(SC[i].Args=="continue")
                    {
                        i++;
                        if (SC[i].Command != ";")
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "';' expected");
                        }
                        Code.Add(new FlagCommand(Command.Continue));
                    }
                    else if(SC[i].Args=="nop")
                    {
                        i++;
                        if (SC[i].Command != ";")
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "':' expected");
                        }
                        Code.Add(new FlagCommand(Command.None));
                    }
                    else if (SC[i].Args == "ret")
                    {
                        i++;
                        if (SC[i].Command != ";")
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "';' expected");
                        }
                        Code.Add(new FlagCommand(Command.Return));
                    }
                    else if(SC[i].Args=="for")
                    {
                        i++;
                        if (SC[i].Command != "Brace" || SC[i].Args!="(")
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "'(' expected");
                        }
                        i++;
                        //Parsing the counter init
                        int BraceCounter = 0;
                        int j = 1;
                        int Idx = i;
                        while (BraceCounter != 0 || SC[i].Command != ";")
                        {
                            if (SC[i].Command == "Brace")
                            {
                                if (SC[i].Args == "/" || SC[i].Args == "(") BraceCounter++;
                                else if (SC[i].Args == "\\" || SC[i].Args == ")") BraceCounter--;
                            }
                            i++;
                            j++;
                        }
                        try
                        {
                            List<Command> PreInit = ParseBlock(SC.SubCode(Idx, j), c, db);
                            if (PreInit.Count > 1)
                            {
                                throw new Exception();
                            }
                            Code.AddRange(PreInit);
                        }
                        catch
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "cannot parse \"for\" loop's counter initialisation");
                        }
                        //Parse Condition
                        i++;
                        j = 0;
                        Idx = i;
                        while (BraceCounter != 0 || SC[i].Command != ";")
                        {
                            if (SC[i].Command == "Brace")
                            {
                                if (SC[i].Args == "/" || SC[i].Args == "(") BraceCounter++;
                                else if (SC[i].Args == "\\" || SC[i].Args == ")") BraceCounter--;
                            }
                            i++;
                            j++;
                        }
                        IExp Cond=null;
                        try
                        {
                            Cond= ParseExp(SC.SubCode(Idx, j), c, db);
                        }
                        catch
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "cannot parse \"for\" loop's condition");
                        }
                        i++;
                        //Parse Increment
                        j = 1;
                        Idx = i;
                        while (BraceCounter != 0 || SC[i].Command != ";")
                        {
                            if (SC[i].Command == "Brace")
                            {
                                if (SC[i].Args == "/" || SC[i].Args == "(") BraceCounter++;
                                else if (SC[i].Args == "\\" || SC[i].Args == ")") BraceCounter--;
                            }
                            i++;
                            j++;
                        }
                        List<Command> Inc=null;
                        try
                        {
                            Inc = ParseBlock(SC.SubCode(Idx, j), c, db);
                            if (Inc.Count > 1)
                            {
                                throw new Exception();
                            }
                        }
                        catch
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "cannot parse \"for\" loop's increment");
                        }
                        i++;
                        if (SC[i].Command != "Brace" || SC[i].Args != ")")
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "')' expected");
                        }
                        i++;
                        if (SC[i].Command != "Brace" || SC[i].Args != "/")
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "'//' expected");
                        }
                        Idx = i + 1;
                        j = FindSizeOfCodeInBraces(SC, ref i);
                        BlockDo block=null;
                        try
                        {
                            Contex ctx = new Contex(c, db);
                            block = new BlockDo(ctx, ParseBlock(SC.SubCode(Idx, j), ctx, db));
                        }
                        catch
                        {
                            db.Error("Line " + SC[Idx].Line + ": Parsing Error: " + "cannot parse codeblock");
                        }
                        block.Code.AddRange(Inc);
                        Code.Add(new WhileCommand(Cond,block));
                        i--;
                    }
                    else if(SC[i].Args=="try")
                    {
                        i++;
                        if(SC[i].Command!="Brace" || SC[i].Args!="/")
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "// expected");
                        }
                        int Idx = i + 1;
                        int j = FindSizeOfCodeInBraces(SC, ref i);
                        Command TryBlock=null;
                        try
                        {
                            Contex ctx = new Contex(c, db);
                            TryBlock = new DoCommand(new BlockDo(ctx, ParseBlock(SC.SubCode(Idx, j), ctx, db)));
                        }
                        catch
                        {
                            db.Error("Line " + SC[Idx].Line + ": Parsing Error: " + "cannot parse codeblock");
                        }
                        Command CatchBlock = null;
                        if(SC[i].Command=="Ctrl" && SC[i].Args=="catch")
                        {
                            i++;
                            if (SC[i].Command != "Brace" || SC[i].Args != "/")
                            {
                                db.Error("Line " + SC[i].Line + ": Parsing Error: " + "// expected");
                            }
                            Idx = i + 1;
                            j = FindSizeOfCodeInBraces(SC, ref i);
                            try
                            {
                                Contex ctx = new Contex(c, db);
                                CatchBlock = new DoCommand(new BlockDo(ctx, ParseBlock(SC.SubCode(Idx, j), ctx, db)));
                            }
                            catch
                            {
                                db.Error("Line " + SC[Idx].Line + ": Parsing Error: " + "cannot parse codeblock");
                            }
                        }
                        Code.Add(new TryCommand(TryBlock, CatchBlock));
                        i--;
                    }
                    else if(SC[i].Args=="repeat")
                    {
                        i++;
                        if (SC[i].Command != "Brace" || SC[i].Args != "/")
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "// expected");
                        }
                        int Idx = i + 1;
                        int j = FindSizeOfCodeInBraces(SC, ref i);
                        BlockDo LoopBody = null;
                        try
                        {
                            Contex ctx = new Contex(c, db);
                            LoopBody = new BlockDo(ctx, ParseBlock(SC.SubCode(Idx, j), ctx, db));
                        }
                        catch
                        {
                            db.Error("Line " + SC[Idx].Line + ": Parsing Error: " + "cannot parse codeblock");
                        }
                        if (SC[i].Command != "Ctrl" || SC[i].Args != "while")
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "while expected");
                        }
                        i++;
                        if (SC[i].Command != "Brace" || SC[i].Args != "(")
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "'(' expected");
                        }
                        Idx = i + 1;
                        j = FindSizeOfCodeInBraces(SC, ref i);
                        IExp Cond = null;
                        try
                        {
                            Cond = ParseExp(SC.SubCode(Idx, j), c, db);
                        }
                        catch
                        {
                            db.Error("Line " + SC[Idx].Line + ": Parsing Error: " + "cannot parse condition");
                        }
                        if (SC[i].Command != ";")
                        {
                            db.Error("Line " + SC[i].Line + ": Parsing Error: " + "';' expected");
                        }
                        Code.Add(new DoCommand(LoopBody));
                        Code.Add(new WhileCommand(Cond, LoopBody));
                    }
                    else
                    {
                        db.Error("Line " + SC[i].Line + ": Parsing Error: " +"unknown operator \"" + SC[i].Args+"\"");
                    }
                }
                else
                {
                    db.Error("Line " + SC[i].Line + ": Parsing Error: " + "can't understand the command");
                }


            }
            return Code;

        }
        private static IExp ParseExp(SudoCode SC, Contex c, Debugger db)
        {
            int OperatorIndex = -1;
            int Rank = 0;
            int BraceCounter = 0;
            int BraketCounter = 0;
            for (int i = 0; i < SC.Length; i++)
            {
                if (SC[i].Command == "Brace")
                {
                    if (SC[i].Args == "/")
                    {
                        BraceCounter++;
                    }
                    else if (SC[i].Args == "\\")
                    {
                        BraceCounter--;
                    }
                    else if (SC[i].Args == "(")
                    {
                        BraketCounter++;
                    }
                    else if (SC[i].Args == ")")
                    {
                        BraketCounter--;
                    }
                }
                if (SC[i].Command == "Oper" && BraceCounter == 0 && BraketCounter == 0)
                {
                    if ((SC[i].Args == "~" || SC[i].Args == "~~" || SC[i].Args == "_" || SC[i].Args=="=") && Rank <= 1)
                    {
                        OperatorIndex = i;
                        Rank = 1;
                    }
                    else if (SC[i].Args == "**" && Rank <= 2)
                    {
                        OperatorIndex = i;
                        Rank = 2;
                    }
                    else if ((SC[i].Args == "*" || SC[i].Args == "/" || SC[i].Args == "%") && Rank <= 3)
                    {
                        OperatorIndex = i;
                        Rank = 3;
                    }
                    else if ((SC[i].Args == "+" || SC[i].Args == "-") && Rank <= 4)
                    {
                        OperatorIndex = i;
                        Rank = 4;
                    }
                    else if ((SC[i].Args == "<<" || SC[i].Args == ">>") && Rank <= 5)
                    {
                        OperatorIndex = i;
                        Rank = 5;
                    }
                    else if ((SC[i].Args == "<" || SC[i].Args == ">" || SC[i].Args == "<=" || SC[i].Args == ">=") && Rank <= 6)
                    {
                        OperatorIndex = i;
                        Rank = 6;
                    }
                    else if ((SC[i].Args == "==" || SC[i].Args == "!=") && Rank <= 7)
                    {
                        OperatorIndex = i;
                        Rank = 7;
                    }
                    else if ((SC[i].Args == "&") && Rank <= 8)
                    {
                        OperatorIndex = i;
                        Rank = 8;
                    }
                    else if ((SC[i].Args == "^") && Rank <= 9)
                    {
                        OperatorIndex = i;
                        Rank = 9;
                    }
                    else if ((SC[i].Args == "|") && Rank <= 10)
                    {
                        OperatorIndex = i;
                        Rank = 10;
                    }
                    else if ((SC[i].Args == "&&") && Rank <= 11)
                    {
                        OperatorIndex = i;
                        Rank = 11;
                    }
                    else if ((SC[i].Args == "^^") && Rank <= 12)
                    {
                        OperatorIndex = i;
                        Rank = 12;
                    }
                    else if ((SC[i].Args == "||") && Rank <= 13)
                    {
                        OperatorIndex = i;
                        Rank = 13;
                    }
                    else if ((SC[i].Args == "sub" || SC[i].Args == "has") && Rank <= 14)
                    {
                        OperatorIndex = i;
                        Rank = 14;
                    }

                }
            }
            if (OperatorIndex == -1)
            {
                if (SC[0].Command == "Brace" && SC[0].Args == "(")
                {
                    return ParseExp(SC.SubCode(1, SC.Length - 2), c,db);
                }
                else if (SC[0].Command == "Run")
                {
                    int ln = SC[0].Line;
                    List<string> Fn = new List<string>();
                    Fn.Add(SC[0].Args);
                    int i;
                    IExp CoolFunc = ParseVar(SC, c, db,out i,true);

                    if (SC[i].Args != "(") db.Error("Line " + SC[i].Line + ": Parsing Error: " + "'(' expected");
                    SC = SC.SubCode(i + 1, SC.Length - 2 - i);
                    int BracetCounter = 0;
                    List<IExp> Args = new List<IExp>();
                    for (i = 0; i < SC.Length; i++)
                    {
                        if (SC[i].Command == "Brace")
                        {

                            if (SC[i].Args == "(" || SC[i].Args == "/") BracetCounter++;
                            else if (SC[i].Args == ")" || SC[i].Args == "\\") BracetCounter--;

                        }
                        else if (SC[i].Command == "," && BracetCounter == 0)
                        {
                            Args.Add(ParseExp(SC.SubCode(0, i), c,db));
                            SC = SC.SubCode(i + 1);
                            i = -1;
                        }
                    }
                    if (SC.Length != 0)
                    {
                        Args.Add(ParseExp(SC, c,db));
                    }


                    return new FuncExp(CoolFunc, Args.ToArray(), ln);
                }
                else if (SC[0].Command == "Lamda")
                {
                    db.Error("Line " + SC[0].Line + ": Parsing Error: "+"анонимные функции это игрушка дьявола");
                    string[] Args = SC[0].Args.Length>0?SC[0].Args.Split(','):new string[0];
                    if (SC[1].Command != "Brace" || SC[1].Args != "/") db.Error("Line " + SC[1].Line + ": Parsing Error: " + "'/' expected");
                    if (SC[SC.Length - 1].Command != "Brace" || SC[SC.Length - 1].Args != "\\") db.Error("Line " + SC[SC.Length-1].Line + ": Parsing Error: " + "'\\' expected");
                    Contex BlockCtx = new Contex(c, db);
                    try
                    {
                        return new ObjExp(new Block(BlockCtx, ParseBlock(SC.SubCode(2, SC.Length - 3), BlockCtx, db), Args).ToMethod(), SC[0].Line);
                    }
                    catch
                    {
                        db.Error("Line " + SC[1].Line + ": Parsing Error: " + "can't parse the codeblock");
                        throw new Exception();
                    }
                }
                 
                else if (SC[0].Command == "String")
                {
                    return new ObjExp(SC[0].Args, SC[0].Line);
                }
                else if (SC[0].Command == "Int")
                {
                    return new ObjExp(Int32.Parse(SC[0].Args), SC[0].Line);
                }
                else if (SC[0].Command == "Float")
                {
                    return new ObjExp(Double.Parse(SC[0].Args, System.Globalization.NumberStyles.Currency, System.Globalization.CultureInfo.InvariantCulture.NumberFormat), SC[0].Line); 
                }
                else if (SC[0].Command == "Var")
                {
                    int j;
                    IExp ret=new EvalVariableExp(ParseVar(SC,c,db,out j,false), SC[0].Line);
                    if(SC.Length!=j)
                    {
                        db.Error("Line " + SC[0].Line + ": Parsing Error: " + "'|' expected");
                    }
                    return ret;
                }
                else if (SC[0].Command == "Ptr")
                {
                    int j;
                    IExp ret = ParseVar(SC, c, db, out j, false);
                    if (SC.Length != j)
                    {
                        db.Error("Line " + SC[0].Line + ": Parsing Error: " + "'|' expected");
                    }
                    return ret;
                }
                else if(SC[0].Command=="Val")
                {

                    int j;
                    IExp ret = new EvalVariableExp(new EvalVariableExp(ParseVar(SC, c, db, out j, false), SC[0].Line),SC[0].Line);
                    if (SC.Length != j)
                    {
                        db.Error("Line " + SC[0].Line + ": Parsing Error: " + "'|' expected");
                    }
                    return ret;
                }
                else
                {
                    db.Error("Line " + SC[0].Line + ": Parsing Error: " + "unexpected code");
                    throw new Exception();
                }
            }
            else
            {
                return ParseOperation(SC[OperatorIndex],SC.SubCode(0, OperatorIndex), SC.SubCode(OperatorIndex + 1), c, db);
            }
        }
        private static IExp ParseVar(SudoCode SC,Contex c,Debugger db,out int index,bool IsFunc)
        {
            IExp CoolVar;
            if (IsFunc)
            {
                CoolVar=new FuncNameFoundationExp(SC[0].Args, SC[0].Line);
            }
            else
            {
                CoolVar = new VarExp(SC[0].Args, SC[0].Line);
            }
            int i;
            for (i = 1; i < SC.Length; i++)
            {
                if (SC[i].Command != "Sub") break; //db.Error("Line " + SC[i].Line + ": Parsing Error: " + "'|' expected");
                if (SC[i].Args == "")
                {
                    i++;
                    if (SC[i].Command != "Brace" || SC[i].Args != "(")
                        db.Error("Line " + SC[i].Line + ": Parsing Error: " + "'(' expected");
                    i++;
                    int Idx = i;
                    int j = -1;
                    int BraceCounter = 1;
                    while (BraceCounter != 0)
                    {
                        if (SC[i].Command == "Brace")
                        {

                            if (SC[i].Args == "(" || SC[i].Args == "/") BraceCounter++;
                            else if (SC[i].Args == ")" || SC[i].Args == "\\") BraceCounter--;

                        }
                        i++;
                        j++;
                    }
                    i--;
                    CoolVar = IsFunc ? new FuncNameSubDynamicExp(CoolVar, ParseExp(SC.SubCode(Idx, j), c, db), SC[Idx].Line) : (IExp) new IndexSubExp(CoolVar, ParseExp(SC.SubCode(Idx, j), c, db), SC[Idx].Line);
                }
                else
                {
                    CoolVar = IsFunc? new FuncNameSubConstExp(CoolVar, SC[i].Args, SC[i].Line) : (IExp)new ConstSubExp(CoolVar, SC[i].Args, SC[i].Line);
                }
            }
            index = i;
            return CoolVar;
        }
        private static int FindSizeOfCodeInBraces(SudoCode SC,ref int i)
        {
            i++;
            int j = -1;
            int BraceCounter = 1;
            while (BraceCounter != 0)
            {
                if (SC[i].Command == "Brace")
                {
                    if (SC[i].Args == "/" || SC[i].Args == "(") BraceCounter++;
                    else if (SC[i].Args == "\\" || SC[i].Args == ")") BraceCounter--;
                }
                i++;
                j++;
            }
            return j;
        }
        private static IExp ParseOperation(Label oper, SudoCode pre, SudoCode post, Contex c, Debugger db)
        {
            bool Un;
            return ParseOperation(oper, pre, post, c, db, out Un);
        }
        private static IExp ParseOperation(Label oper, IExp pre, IExp post, Contex c, Debugger db)
        {
            bool Un;
            return ParseOperation(oper, pre, post, c, db, out Un);
        }
        private static IExp ParseOperation(Label oper, SudoCode pre, SudoCode post, Contex c, Debugger db, out bool IsUnari)
        {
            IExp PreExp=null, PostExp=null;
            if (pre.Length!=0)
            {
                PreExp = ParseExp(pre, c, db);
            }
            PostExp = ParseExp(post, c, db);
            return ParseOperation(oper, PreExp, PostExp, c, db, out IsUnari);
        }
        private static IExp ParseOperation(Label oper,IExp pre,IExp post,Contex c,Debugger db,out bool IsUnari)
        {
            IsUnari = false;
            if (post == null)
            {
                post = pre;
                pre = null;
            }
            switch (oper.Args)
            {
                case "+":
                    return new SumExp(pre, post, oper.Line);
                case "-":
                    if (pre == null)
                    {
                        IsUnari = true;
                        return new NegExp(post, oper.Line);
                    }
                    else
                    {
                        return new DiffExp(pre, post, oper.Line);
                    }
                case "*":
                    return new MultExp(pre, post, oper.Line);
                case "/":
                    return new DivExp(pre, post, oper.Line);
                case "%":
                    return new ModExp(pre, post, oper.Line);
                case "**":
                    return new PowExp(pre, post, oper.Line);
                case "==":
                    return new EqualExp(pre, post, oper.Line);
                case "!=":
                    return new NotEqualExp(pre, post, oper.Line);
                case ">":
                    return new GreaterExp(pre, post, oper.Line);
                case "<":
                    return new LessExp(pre, post, oper.Line);
                case ">=":
                    return new GreaterEqualExp(pre, post, oper.Line);
                case "<=":
                    return new LessEqualExp(pre, post, oper.Line);
                case ">>":
                    return new ShrExp(pre, post, oper.Line);
                case "<<":
                    return new ShlExp(pre, post, oper.Line);
                case "&":
                    return new AndExp(pre, post, oper.Line);
                case "^":
                    return new XorExp(pre, post, oper.Line);
                case "|":
                    return new OrExp(pre, post, oper.Line);
                case "~":
                    IsUnari = true;
                    return new NotExp( post, oper.Line);
                case "&&":
                    return new LAndExp(pre, post, oper.Line);
                case "^^":
                    return new LXorExp(pre, post, oper.Line);
                case "||":
                    return new LOrExp(pre, post, oper.Line);
                case "~~":
                    IsUnari = true;
                    return new LNotExp(post, oper.Line);
                case "_":
                    IsUnari = true;
                    return new NegExp(post, oper.Line);
                case "=":
                    IsUnari = true;
                    return new EvalVariableExp(post, oper.Line);
                case "has":
                    return new HasExp(pre, post, oper.Line);
                case "sub":
                    return new IndexSubExp(pre, post, oper.Line);
                default:
                    db.Error("Line " + oper.Line + ": Parsing Error: " + "unknown operator");
                    throw new Exception();
            }
        }

    }
}
