using System;
using Yoakke.Collections;
using Yoakke.Ir.Model;
using Yoakke.Ir.Passes;
using Yoakke.Ir.Writers;
using Type = Yoakke.Ir.Model.Type;

namespace Yoakke.Ir.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var i32 = new Type.Int(true, 32);

            Value MakeInt(int v) => new Value.Int(true, new BigInt(32, BitConverter.GetBytes(v)));

            var builder = new AssemblyBuilder()
                .DefineProcedure("foo", out var foo)
                .DefineLocal(i32)
                .DefineParameter(i32, out var arg0)
                .DefineParameter(i32, out var arg1)
                .IntAdd(arg0, arg1, out var added)
                .Ret(added);
            foo.Return = i32;

            builder
                .DefineProcedure("main", out var main)
                .Call(new Value.Proc(foo), new Value[] { MakeInt(1), MakeInt(2) }.AsValue(), out var callRes)
                .Ret(callRes);
            main.Return = i32;

            var pass = new RemoveUnreferencedLocals();
            foreach (var proc in builder.Assembly.Procedures.Values) pass.Pass(proc);
        
            var writer = new AssemblyTextWriter();
            writer.Write(builder.Assembly);
            Console.WriteLine(writer.Result);
        }
    }
}
