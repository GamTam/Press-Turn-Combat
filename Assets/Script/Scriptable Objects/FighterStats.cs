using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[System.Flags]
public enum DamageTypes
{
    Almighty = 1,
    Strike = 2,
    Slash = 4,
    Pierce = 8,
    Fire = 16,
    Ice = 32,
    Elec = 64,
    Wind = 128,
    Light = 256,
    Dark = 512
}

[System.Serializable]
public enum BuffType
{
    NONE,
    KAJA,
    KUNDA
}

[System.Serializable]
[System.Flags]
public enum BuffStats
{
    NONE = 0,
    ATK = 1,
    DEF = 2,
    STR = 4
}

[System.Serializable]
public enum StatusEffectType
{
    NONE,
    DEAD
}

[System.Serializable]
public struct Skill
{
    public string Name;
    public int CostAmount;
    public bool CostsHP;
    public int BaseDamage;
    
    [Space]
    public bool UseMagForDamageCalc;
    public DamageTypes ElementType;
    public bool Multihit;

    [Space]
    public bool Healing;
    public BuffType BuffType;
    public BuffStats BuffStats;
    public int HowManyBuffTiers;
    
    [Space]
    public StatusEffectType StatusEffect;
    public int StatusEffectChance;
    public bool PhysCharge;
    public bool MagCharge;
    
    [Space]
    public List<PressTurn> PressTurnsToAdd;
}

[System.Serializable]
public struct FighterStats
{
    public string Name;
    
    [Space]
    public int MaxHP;
    public int MaxMP;
    public DamageTypes BaseAttackType;
    
    [Space]
    public int STR;
    public int MAG;
    public int END;
    public int AGI;
    public int LUK;

    [Space]
    public List<Skill> Skills;
}
