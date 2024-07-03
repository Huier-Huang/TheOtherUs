using System.Collections.Generic;
using TheOtherUs.Modules.Compatibility;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TheOtherUs.Objects;

internal class BloodyTrail
{
    private static readonly List<BloodyTrail> bloodytrail = [];
    private static readonly ResourceSpriteArray BloodySprites = new
    (
        [
            ("Blood1.png", 700),
            ("Blood2.png", 500),
            ("Blood3.png", 300),
        ]
    );
    private readonly GameObject blood;
    private readonly Color color;
    private readonly SpriteRenderer spriteRenderer;

    public BloodyTrail(PlayerControl player, PlayerControl bloodyPlayer)
    {
        color = Palette.PlayerColors[bloodyPlayer.Data.DefaultOutfit.ColorId];
        var index = BloodySprites.RandomIndex();


        blood = new GameObject("Blood" + index);
        var position = new Vector3(player.transform.position.x, player.transform.position.y,
            (player.transform.position.y / 1000) + 0.001f);
        blood.AddSubmergedComponent(SubmergedCompatibility.Classes.ElevatorMover);
        blood.transform.position = position;
        blood.transform.localPosition = position;
        blood.transform.SetParent(player.transform.parent);

        blood.transform.Rotate(0.0f, 0.0f, Random.Range(0.0f, 360.0f));

        spriteRenderer = blood.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = BloodySprites.Set(index);
        spriteRenderer.material = FastDestroyableSingleton<HatManager>.Instance.PlayerMaterial;
        bloodyPlayer.SetPlayerMaterialColors(spriteRenderer);

        blood.SetActive(true);
        bloodytrail.Add(this);

        /*FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(10f, new Action<float>(p =>
        {
            var c = color;
            if (Get<Camouflager>().camouflageTimer > 0 || Helpers.MushroomSabotageActive()) c = Palette.PlayerColors[6];
            if (spriteRenderer) spriteRenderer.color = new Color(c.r, c.g, c.b, Mathf.Clamp01(1 - p));

            if ((int)p != 1 || blood == null) return;
            Object.Destroy(blood);
            bloodytrail.Remove(this);
        })));*/
    }
}