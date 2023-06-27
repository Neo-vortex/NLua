For full doc visit https://github.com/NLua/NLua

this forks intents to add memory limit and instruction limit for NLua

Example :

```csharp
///5000000 bytes memory limit
///500 instruction limit
 var lua = new Lua(5000000, 500, false);
```

here is the full constructor that has been added :

```csharp
 public Lua(long memoryLimit , long instructionLimit, bool openLibs = true)
        {
            var memSize = Marshal.AllocHGlobal(4);
            unsafe
            {
                *(long*) memSize = memoryLimit;
            }
            _luaState = new LuaState(Allocator, memSize);
            if (openLibs)  _luaState.OpenLibs();
            Init();
            // We need to keep this in a managed reference so the delegate doesn't get garbage collected
            _luaState.AtPanic(PanicCallback);
            this.SetDebugHook(LuaHookMask.Count,1);
            this.DebugHook += (sender, args) =>
            {
                _currentInstructionCount++;
                if (_currentInstructionCount > instructionLimit)
                {
                    this._luaState.Error($"Instruction limit reached :{_currentInstructionCount}");
                }
            };
        }
```

I am also planning to make any of instruction and memory limit optional.
