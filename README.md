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
            var memSize = Marshal.AllocHGlobal(8);
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

and this is the allocator :

```csharp
   private static unsafe IntPtr Allocator(IntPtr ud, IntPtr ptr, UIntPtr osize, UIntPtr nsize)
        {
            if (osize == nsize)
                return ptr;


            var inUse = (long*) ud;
            var newSize = (uint) nsize;
            var originalSize = (uint) osize;

            if (ptr == IntPtr.Zero)
            {
                if (*inUse - (int) newSize < 0)
                {
                    return IntPtr.Zero;
                }

                var intPtr = Marshal.AllocHGlobal((int) newSize);
                *inUse -= (int) newSize;
                return intPtr;
            }


            if (nsize == UIntPtr.Zero)
            {
                Marshal.FreeHGlobal(ptr);

                *inUse += (int) originalSize;
                return IntPtr.Zero;
            }

            if (*inUse - ((int) nsize - (int) osize) < 0)
            {
                return IntPtr.Zero;
            }

            ptr = Marshal.ReAllocHGlobal(ptr, (IntPtr) newSize);

            if (ptr != IntPtr.Zero)
            {
                *inUse -= ((int) nsize - (int) osize);
            }
}
```
For `KeraLua` backend use this one : https://github.com/Neo-vortex/KeraLua
            return ptr;
        }
```


I am also planning to make any of instruction and memory limit optional.
