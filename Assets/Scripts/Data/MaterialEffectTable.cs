using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

public enum MaterialType
{
    Metal,
    Wood,
    Ground,
    Flesh,
    Bullet,
    Water,

}

[System.Serializable]
public class MaterialEffectData
{
    public MaterialType TypeA;
    public MaterialType TypeB;
    public string ParticleAddress;
    public float ParticleDuration=1.0f;
    public string DecalAddress;
    public float DecalDuration=1.0f;
    public string SFX;
    public string ExtraParamsRaw;

    public Dictionary<string, float> ExtraParams
    {
        get
        {
            var dict = new Dictionary<string, float>();
            if (string.IsNullOrEmpty(ExtraParamsRaw)) return dict;

            var pairs = ExtraParamsRaw.Split(';');
            foreach (var p in pairs)
            {
                var kv = p.Split('=');
                if (kv.Length == 2 && float.TryParse(kv[1], out var val))
                    dict[kv[0]] = val;
            }
            return dict;
        }
    }

}

[CreateAssetMenu(fileName = "MaterialEffectTable", menuName = "ScriptableObject/Material Effect Table")]
public class MaterialEffectTable : ScriptableObject
{
    public List<MaterialEffectData> rows = new List<MaterialEffectData>();

}