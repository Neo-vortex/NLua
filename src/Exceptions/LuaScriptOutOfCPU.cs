namespace NLua.Exceptions;

public class LuaScriptOutOfCPU : Exception
{
    public LuaScriptOutOfCPU() : base("lua script ran out of memory")
    {
    }
}
