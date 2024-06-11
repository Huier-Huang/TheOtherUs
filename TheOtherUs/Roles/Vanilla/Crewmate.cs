using TheOtherUs.Options;

namespace TheOtherUs.Roles.Vanilla;

[RegisterRole]
public class Crewmate : VanillaRole
{
    public override RoleInfo RoleInfo { get; protected set; }
    public override CustomRoleOption roleOption { get; set; }
}