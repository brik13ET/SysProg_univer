.assembly extern System.Runtime
{
  .ver 6:0:0:0
}

.assembly extern System.Console
{
  .ver 6:0:0:0
}

.assembly HelloApp{}
.module HelloApp.dll
.class private auto MSIL
{
  .method private static void llcode() cil managed
  {
    .entrypoint
    ldstr      "Hello, METANIT.COM!"
    call       void [System.Console]System.Console::WriteLine(string)
    ret
  }
}