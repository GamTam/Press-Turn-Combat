using UnityEngine;

public class Fighter : MonoBehaviour
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
    protected FighterTeam _currentTeam;
    protected bool _guarding;

    public string Name => _stats.Name;
    public string HP => _HP + "/" + _stats.MaxHP;
    public string MP => _MP + "/" + _stats.MaxMP;
    public Skill[] Skills => _stats.Skills.ToArray();
    public StatusEffectType CurrentStatusEffect => _currentStatusEffect;

    public void SetFighterTeam(FighterTeam team)
    {
        _currentTeam = team;
    }

    private void Awake()
    {
        _HP = _stats.MaxHP;
        _MP = _stats.MaxMP;
    }

    private AffinityTypes AttemptDamage(int attackPower, DamageTypes element)
    {
        AffinityTypes affinityToAttack = AffinityTypes.NONE;

        try { affinityToAttack = _stats.BaseAffinities[element]; }
        catch { /* ignored */ }

        if (affinityToAttack == AffinityTypes.WEAK)
        {
            if (!_guarding) _HP -= attackPower * 2;
            else
            {
                _HP -= (int)(attackPower * 0.75);
                affinityToAttack = AffinityTypes.NONE;
            }
        }
        else if (affinityToAttack == AffinityTypes.RESIST)
            _HP -= attackPower / 2;
        else if (affinityToAttack == AffinityTypes.ABSORB)
            _HP += attackPower;
        else if (affinityToAttack == AffinityTypes.NONE)
        {
            if (!_guarding) _HP -= attackPower;
            else _HP -= (int)(attackPower * 0.75);
        }

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
        if (skill.CostsHP & _HP >= skill.CostAmount) return true;
        if (!skill.CostsHP & _MP >= skill.CostAmount) return true;

        return false;
    }

    protected AffinityTypes BasicAttack(Fighter target)
    {
        AffinityTypes targetAffinity = target.AttemptDamage((int)Random.Range(_stats.STR * 0.75f, _stats.STR * 1.25f),
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

        if (target.CurrentStatusEffect == StatusEffectType.DEAD) str += "<color=red>" + target.Name + " has collapsed!";
        // else str += target.Name + "'s HP is now " + target.HP + "!";
        
        UIManager.Instance.UpdateText(str);

        return targetAffinity;
    }

    protected AffinityTypes UseSkill(Skill skill, Fighter target = null)
    {
        AffinityTypes targetAffinity = AffinityTypes.NONE;
        
        if (!CheckIfCanUseSkill(skill))
        {
            UIManager.Instance.UpdateText(_stats.Name + " tried to cast " + Skills[0].Name + "... but couldn't afford it!\n");
            return targetAffinity;
        }
        
        if (skill.CostsHP) _HP -= skill.CostAmount;
        else _MP -= skill.CostAmount;

        if (target != null)
        {
            targetAffinity = target.AttemptDamage(skill.BaseDamage, skill.ElementType);
            
            string str = _stats.Name + " cast " + Skills[0].Name + " on " + target.Name + "!\n";
        
            if (targetAffinity == AffinityTypes.WEAK)
                str += target.Name + " was weak to " + _stats.Name + "'s attack!\n";
            else if (targetAffinity == AffinityTypes.RESIST)
                str += target.Name + " resisted " + _stats.Name + "'s attack!\n";
            else if (targetAffinity == AffinityTypes.NULL)
                str += target.Name + " blocked " + _stats.Name + "'s attack!\n";
            else if (targetAffinity == AffinityTypes.ABSORB)
                str += target.Name + " absorbed " + _stats.Name + "'s attack!\n";

            if (target.CurrentStatusEffect == StatusEffectType.DEAD) str += "<color=red>" + target.Name + " has collapsed!";
            // else str += target.Name + "'s HP is now " + target.HP + "!";
            UIManager.Instance.UpdateText(str);
        }
        
        return targetAffinity;
    }

    protected AffinityTypes PassTurn()
    {
        UIManager.Instance.UpdateText(_stats.Name + " passed things to the next person!\n");
        return AffinityTypes.WEAK;
    }
    protected void UseItem() {}

    protected AffinityTypes Guard()
    {
        UIManager.Instance.UpdateText(_stats.Name + " put up their guard!\n");
        _guarding = true;
        return AffinityTypes.NONE;
    }
    
    public virtual void TakeAction() {}
}
