using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum DamageTypes
{
    ALMIGHTY = 1,
    STRIKE = 2,
    SLASH = 4,
    PIERCE = 8,
    FIRE = 16,
    ICE = 32,
    ELEC = 64,
    WIND = 128,
    LIGHT = 256,
    DARK = 512
}

public enum AffinityTypes
{
    NONE,
    WEAK,
    RESIST,
    NULL,
    REPEL,
    ABSORB
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
    AGI = 4
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
    [SerializeField] public Dictionary<DamageTypes, AffinityTypes> BaseAffinities;
    
    [Space]
    public int STR;
    public int MAG;
    public int END;
    public int AGI;
    public int LUK;

    [Space]
    public List<Skill> Skills;
}
