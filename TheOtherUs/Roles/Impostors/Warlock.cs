using Hazel;
using TheOtherUs.Objects;
using UnityEngine;

namespace TheOtherUs.Roles.Impostors;

[RegisterRole]
public class Warlock : RoleBase
{
    public float cooldown = 30f;

    public PlayerControl currentTarget;

    private readonly ResourceSprite curseButtonSprite = new("CurseButton.png");
    private readonly ResourceSprite curseKillButtonSprite = new("CurseKillButton.png");
    public PlayerControl curseVictim;
    public PlayerControl curseVictimTarget;
    public float rootTime = 5f;
    public PlayerControl warlock;
    public CustomOption warlockCooldown;
    public CustomButton warlockCurseButton;
    public CustomOption warlockRootTime;

    public override RoleInfo RoleInfo { get; protected set; } = new()
    {
        Name = nameof(Warlock),
        RoleClassType = typeof(Warlock),
        Color = Palette.ImpostorRed,
        RoleId = RoleId.Warlock,
        RoleType = CustomRoleType.Main,
        RoleTeam = RoleTeam.Impostor,
        GetRole = Get<Warlock>,
        IntroInfo = "Curse other players and kill everyone",
        DescriptionText = "Curse and kill everyone",
        CreateRoleController = player => new WarlockController(player)
    };
    public class WarlockController(PlayerControl player) : RoleControllerBase(player)
    {
        public override RoleBase _RoleBase => Get<Warlock>();
    }
    
    public override CustomRoleOption roleOption { get; set; }

    public override void ClearAndReload()
    {
        warlock = null;
        currentTarget = null;
        curseVictim = null;
        curseVictimTarget = null;
    }

    public void resetCurse()
    {
        warlockCurseButton.Timer = warlockCurseButton.MaxTimer;
        warlockCurseButton.Sprite = curseButtonSprite;
        warlockCurseButton.actionButton.cooldownTimerText.color = Palette.EnabledColor;
        currentTarget = null;
        curseVictim = null;
        curseVictimTarget = null;
    }

    public override void OptionCreate()
    {
        roleOption = new CustomRoleOption(this);
        warlockCooldown = roleOption.AddChild("Warlock Cooldown", new FloatOptionSelection(30f, 10f, 60f, 2.5f));
        warlockRootTime = roleOption.AddChild("Warlock Root Time", new FloatOptionSelection(5f, 0f, 15f, 1f));
    }

    public override void ButtonCreate(HudManager _hudManager)
    {
        // Warlock curse
        warlockCurseButton = new CustomButton(
            () =>
            {
                if (curseVictim == null)
                {
                    /*if (Helpers.checkAndDoVetKill(currentTarget)) return;
                    Helpers.checkWatchFlash(currentTarget);*/
                    // Apply Curse
                    curseVictim = currentTarget;
                    warlockCurseButton.Sprite = curseKillButtonSprite;
                    warlockCurseButton.Timer = 1f;
                    SoundEffectsManager.play("warlockCurse");

                    // Ghost Info
                    var writer = AmongUsClient.Instance.StartRpcImmediately(
                        LocalPlayer.Control.NetId, (byte)CustomRPC.ShareGhostInfo,
                        SendOption.Reliable);
                    writer.Write(LocalPlayer.PlayerId);
                    /*writer.Write((byte)RPCProcedure.GhostInfoTypes.WarlockTarget);*/
                    writer.Write(curseVictim.PlayerId);
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }
                /*else if (curseVictim != null && curseVictimTarget != null)
                {
                    var murder = Helpers.checkMurderAttemptAndKill(warlock, curseVictimTarget,
                        showAnimation: false);
                    if (murder == MurderAttemptResult.SuppressKill) return;

                    // If blanked or killed
                    if (rootTime > 0)
                    {
                        Get<AntiTeleport>().position = LocalPlayer.transform.position;
                        LocalPlayer.Control.moveable = false;
                        LocalPlayer.NetTransform
                            .Halt(); // Stop current movement so the warlock is not just running straight into the next object
                        FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(rootTime,
                            new Action<float>(p =>
                            {
                                // Delayed action
                                if (p == 1f) LocalPlayer.Control.moveable = true;
                            })));
                    }

                    curseVictim = null;
                    curseVictimTarget = null;
                    warlockCurseButton.Sprite = curseButtonSprite;
                    warlock.killTimer = warlockCurseButton.Timer = warlockCurseButton.MaxTimer;

                    var writer = AmongUsClient.Instance.StartRpcImmediately(
                        LocalPlayer.Control.NetId, (byte)CustomRPC.ShareGhostInfo,
                        SendOption.Reliable);
                    writer.Write(LocalPlayer.PlayerId);
                    writer.Write((byte)RPCProcedure.GhostInfoTypes.WarlockTarget);
                    writer.Write(byte.MaxValue); // This will set it to null!
                    AmongUsClient.Instance.FinishRpcImmediately(writer);
                }*/
            },
            () => warlock != null && warlock == LocalPlayer.Control &&
                  !LocalPlayer.IsDead,
            () =>
            {
                ButtonHelper.showTargetNameOnButton(currentTarget, warlockCurseButton,
                    curseVictim != null ? "KILL" : "CURSE");
                return ((curseVictim == null && currentTarget != null) ||
                        (curseVictim != null && curseVictimTarget != null)) &&
                       LocalPlayer.Control.CanMove;
            },
            () =>
            {
                warlockCurseButton.Timer = warlockCurseButton.MaxTimer;
                warlockCurseButton.Sprite = curseButtonSprite;
                curseVictim = null;
                curseVictimTarget = null;
            },
            curseButtonSprite,
            DefButtonPositions.upperRowLeft,
            _hudManager,
            KeyCode.F
        );
    }

    public override void ResetCustomButton()
    {
        warlockCurseButton.MaxTimer = cooldown;
    }
}