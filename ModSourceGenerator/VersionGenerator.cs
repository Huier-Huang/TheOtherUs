using CSharpPoet;
using Microsoft.CodeAnalysis;

namespace ModSourceGenerator;

[Generator(LanguageNames.CSharp)]
public class VersionGenerator: IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var time = DateTime.Now.ToString("yyyy-M-d-hh-mm");
        context.RegisterSourceOutput(context.CompilationProvider, (spc, source) =>
        {
            var Plugin = new CSharpClass(Visibility.Public, "TheOtherRolesPlugin")
            {
                IsPartial = true
            };
            Plugin.Add(new CSharpField(Visibility.Public, "string", "Build_Time")
            {
                IsStatic = true,
                IsReadonly = true,
                DefaultValue = time
            });
            var file = new CSharpFile("TheOtherUs") { Plugin };

            spc.AddSource("Main.Build", $"\"{time}\"");
        });
    }
}