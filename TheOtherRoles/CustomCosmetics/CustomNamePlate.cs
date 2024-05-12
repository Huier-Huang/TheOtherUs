using UnityEngine;

namespace TheOtherRoles.CustomCosmetics;

public class CustomNamePlate : ICustomCosmetic
{
    public CustomCosmeticConfig config { get; set; }
    public Sprite Resource { get; set; }
    public CosmeticData data { get; set; }
    public CosmeticsManagerConfig ManagerConfig { get; set; }
}