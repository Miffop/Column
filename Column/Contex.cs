using System;
using System.Collections.Generic;

namespace Column
{
    class Contex
    {
        Stack<ColumnData> StackFrame;
        ColumnData RootData;   
        Dictionary<string,Method> FuncData;

        Contex Parent;
        public Debugger db;

        public Contex(Contex parent, Debugger db)
        {
            this.db = db;
            this.Parent = parent;

            this.RootData = new ColumnData(null);
            this.FuncData = new Dictionary<string, Method>();
            
            this.StackFrame = new Stack<ColumnData>();
        }

        private bool GetV(string name, out ColumnData Res)
        {
            if (RootData.Exist(name))
            {
                Res = RootData[name];
                return true;
            }
            else if (Parent != null)
            {
                return Parent.GetV(name, out Res);
            }
            else
            {
                Res = null;
                return false;
            }
        }
        public ColumnData GetVar(string name)
        {
            ColumnData Res;
            if (GetV(name, out Res))
            {
                return Res;
            }
            else
            {
                return RootData[name];
            }
        }
        public Method GetFunc(string name)
        {
            Method Res;
            if (FuncData.TryGetValue(name, out Res))
            {
                return Res;
            }
            else if (Parent != null)
            {
                return Parent.GetFunc(name);
            }
            else
            {
                return null;
            }
        }
        public void StateVar(string name, ColumnData val)
        {
            RootData[name] = val;
        }
        public void StateFunc(string name,Method func)
        {
            FuncData.Add(name, func);
        }
        //Ptr things
        public void SetVar(string name,ColumnData change)
        {
            if (!SetV(name, change))
            {
                RootData[name] = change;
            }
        }
        private bool SetV(string name,ColumnData change)
        {
            if (RootData.Exist(name))
            {
                RootData[name]=change;
                return true;
            }
            else if (Parent != null)
            {
                return Parent.SetV(name, change);
            }
            else
            {
                return false;
            }
        }

        //ContexFrames
        public void Push()
        {
            StackFrame.Push(RootData);
            RootData = new ColumnData(null);
        }
        public void Pop()
        {
            RootData = StackFrame.Pop();
        }
    }
}
