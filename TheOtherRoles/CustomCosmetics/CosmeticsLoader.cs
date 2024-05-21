#nullable enable
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx.Unity.IL2CPP.Utils.Collections;
using UnityEngine;

namespace TheOtherRoles.CustomCosmetics;

[MonoRegisterAndDontDestroy]
public class CosmeticsLoader : MonoBehaviour
{
    public bool createRunning;
    private static CosmeticsLoader? instance;
    public BlockingCollection<Sprite> sprites => CosmeticsManager.Instance.Sprites;

    public static CosmeticsLoader Instance => instance ??= Main.Instance.AddComponent<CosmeticsLoader>();

    public CosmeticsLoader()
    {
        instance = this;
    }
    

    public void LateUpdate()
    {
        if (!createRunning && CosmeticsManager.Instance.NoLoad.Any())
            StartCoroutine_Auto(Create().WrapToIl2Cpp());
    }

    private static int Max => CosmeticsManager.Instance.CustomCosmetics.Count;
    public int count = 1;
    
    
    public IEnumerator Create()
    {
        createRunning = true;

        Dequeue:
        if (CosmeticsManager.Instance.NoLoad.TryDequeue(out var cosmetic) && count <= Max)
        {
            var sprite = new List<Sprite>();
            foreach (var r in cosmetic.Resources)
            {
                if (!sprites.Any(n => n.name.EndsWith(r)))
                {
                    var p = CosmeticsManager.GetLocalPath(cosmetic.Flags, r);
                    if (!File.Exists(p))
                        continue;
                    var info = new FileInfo(p);
                    using var stream = info.OpenRead();
                    sprites.Add(stream.LoadHatSpriteFormDisk($"{info.DirectoryName}/{info.Name}"));
                }
                var sp = sprites.FirstOrDefault(n => n.name.EndsWith(r));
                if (sp) sprite.Add(sp!);
            }
            cosmetic.Create(sprite);
            cosmetic.HasLoad = true;
            Info($"Create {count}/{Max} {cosmetic.Id} {cosmetic.config.Name}");
            count++;
            yield return null;
            goto Dequeue;
        }

        CosmeticsManager.CheckAddAll();
        yield return null;
        createRunning = false;
    }
}
