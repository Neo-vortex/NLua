using System;

namespace NLua.Exceptions
{
    public class LuaScriptOutOfMemoryException : Exception
    {
        public LuaScriptOutOfMemoryException() : base("lua script ran out of memory")
        {
        }
    }
}
