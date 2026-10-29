using UnityEngine;

[System.Serializable]
public struct Buffs
{
    public int TierATK;
    public int TierDEF;
    public int TierSPD;
    
    public int CooldownATK;
    public int CooldownDEF;
    public int CooldownSPD;
}

public abstract class Fighter : MonoBehaviour
{
    [SerializeField] protected FighterStats _stats = new FighterStats();
    
    [Space]
    [SerializeField] protected int _HP;
    [SerializeField] protected int _MP;

    protected Buffs _currentBuffs;
    protected Buffs _tempBuffs;
    
    [SerializeField] protected StatusEffectType _currentStatusEffect;

    protected bool _physCharge;
    protected bool _magCharge;
    protected bool _turnComplete;

    public string Name => _stats.Name;
    public string HP => _HP + "/" + _stats.MaxHP;
    public string MP => _MP + "/" + _stats.MaxMP;
    public Skill[] Skills => _stats.Skills.ToArray();
    public StatusEffectType CurrentStatusEffect => _currentStatusEffect;
    public bool TurnComplete { get; set; }

    private void Awake()
    {
        _HP = _stats.MaxHP;
        _MP = _stats.MaxMP;
    }

    public AffinityTypes TakeDamage(int attackPower, DamageTypes element)
    {
        AffinityTypes affinityToAttack = AffinityTypes.NONE;

        try { affinityToAttack = _stats.BaseAffinities[element]; }
        catch { /* ignored */ }

        if (affinityToAttack == AffinityTypes.WEAK)
            _HP -= attackPower * 2;
        else if (affinityToAttack == AffinityTypes.RESIST)
            _HP -= attackPower / 2;
        else if (affinityToAttack == AffinityTypes.ABSORB)
            _HP += attackPower;
        else if (affinityToAttack == AffinityTypes.NONE)
            _HP -= attackPower;

        if (_HP > _stats.MaxHP) _HP = _stats.MaxHP;
        
        if (_HP <= 0)
        {
            _HP = 0;
            _currentStatusEffect = StatusEffectType.DEAD;
        }

        return affinityToAttack;
    }

    private bool CheckIfCanUseSkill(Skill skill)
    {
        if (skill.CostsHP & _HP <= skill.CostAmount) return false;
        if (!skill.CostsHP & _MP <= skill.CostAmount) return false;

        return true;
    }

    public AffinityTypes BasicAttack(Fighter target)
    {
        AffinityTypes targetAffinity = target.TakeDamage((int)Random.Range(_stats.STR * 0.75f, _stats.STR * 1.25f),
            _stats.BaseAttackType);

        string str = _stats.Name + " attacked " + target.Name + "!\n";
        
        if (targetAffinity == AffinityTypes.WEAK)
            str += target.Name + " was weak to " + _stats.Name + "'s attack!\n";
        else if (targetAffinity == AffinityTypes.RESIST)
            str += target.Name + " resisted " + _stats.Name + "'s attack!\n";
        else if (targetAffinity == AffinityTypes.NULL)
            str += target.Name + " blocked " + _stats.Name + "'s attack!\n";
        else if (targetAffinity == AffinityTypes.ABSORB)
            str += target.Name + " absorbed " + _stats.Name + "'s attack!\n";

        str += target.Name + "'s HP is now " + target.HP + "!";
        Debug.Log(str);

        return targetAffinity;
    }
    
    public AffinityTypes UseSkill(Skill skill, Fighter target = null)
    {
        AffinityTypes targetAffinity = AffinityTypes.NONE;
        
        if (skill.CostsHP & _HP < skill.CostAmount ||
            !skill.CostsHP & _MP < skill.CostAmount)
        {
            Debug.Log(_stats.Name + " tried to cast " + Skills[0].Name + "... but couldn't afford it!\n");
            return targetAffinity;
        }
        
        if (skill.CostsHP) _HP -= skill.CostAmount;
        else _MP -= skill.CostAmount;

        if (target != null)
        {
            targetAffinity = target.TakeDamage(skill.BaseDamage, skill.ElementType);
            
            string str = _stats.Name + " cast " + Skills[0].Name + " on " + target.Name + "!\n";
        
            if (targetAffinity == AffinityTypes.WEAK)
                str += target.Name + " was weak to " + _stats.Name + "'s attack!\n";
            else if (targetAffinity == AffinityTypes.RESIST)
                str += target.Name + " resisted " + _stats.Name + "'s attack!\n";
            else if (targetAffinity == AffinityTypes.NULL)
                str += target.Name + " blocked " + _stats.Name + "'s attack!\n";
            else if (targetAffinity == AffinityTypes.ABSORB)
                str += target.Name + " absorbed " + _stats.Name + "'s attack!\n";

            str += target.Name + "'s HP is now " + target.HP + "!";
            Debug.Log(str);
        }
        
        return targetAffinity;
    }
    
    private void PassTurn() {}
    private void UseItem() {}
    private void Guard() {}
    
    protected void TakeAction() {}
}
