using System;
using System.Linq;
using System.Reflection;

namespace TheOtherUs.Utilities;

[MeansImplicitUse]
[AttributeUsage(AttributeTargets.Class)]
public sealed class RegisterRole(bool isTemplate = false) : RegisterAttribute
{
    public readonly bool IsTemplate = isTemplate;

    [Register]
    public static void Register(Assembly assembly, CustomRoleManager _customRoleManager)
    {
        var types = assembly.GetTypes()
            .Where(n =>
            {
                if (!n.IsSubclassOf(typeof(RoleBase)))
                    return false;

                var attribute = n.GetCustomAttribute<RegisterRole>();

                if (attribute == null)
                    return false;

                return !attribute.IsTemplate;
            });

        foreach (var _type in types)
        {
            Info($"Register Role {_type.Name}");
            _customRoleManager.Register((RoleBase)AccessTools.CreateInstance(_type));
        }
    }
}