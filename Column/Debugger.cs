using System;
using System.Collections.Generic;

namespace Column
{
    public class Debugger
    {
        public Debugger()
        {
            this.ErrorMessage = new Stack<string>();
        }
        Stack<string> ErrorMessage;
        public bool IsError
        {
            get
            {
                return ErrorMessage.Count != 0;
            }
        }
        public string GetError()
        {
            return ErrorMessage.Count != 0 ? ErrorMessage.Pop() : null;
        }
        public void Error(string Error)
        {
            ErrorMessage.Push(Error);
            throw new Exception();
        }
    }
}
