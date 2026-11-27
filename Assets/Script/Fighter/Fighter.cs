using UnityEngine;

public class Fighter : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected FighterStats _stats = new FighterStats();
    
    [Space]
    [SerializeField] protected int _HP;
    [SerializeField] protected int _MP;

    protected Buffs _currentBuffs;
    protected Buffs _tempBuffs;
    
    [SerializeField] protected StatusEffectType _currentStatusEffect;
    [SerializeField] private Animator _animator;
    
    [Space]
    [Header("Voicelines")]
    [SerializeField] private AK.Wwise.Event _battleOpenVoiceline;
    [SerializeField] private AK.Wwise.Event _attackVoiceline;
    [SerializeField] private AK.Wwise.Event _bigAttackVoiceline;
    [SerializeField] private AK.Wwise.Event _skillVoiceline;
    [SerializeField] private AK.Wwise.Event _signatureSkillVoiceline;
    [SerializeField] private AK.Wwise.Event _guardVoiceline;
    [SerializeField] private AK.Wwise.Event _hitVoiceline;
    [SerializeField] private AK.Wwise.Event _bigHitVoiceline;
    [SerializeField] private AK.Wwise.Event _itemVoiceline;
    [SerializeField] private AK.Wwise.Event _dieVoiceline;
    [SerializeField] private AK.Wwise.Event _reviveVoiceline;

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

        if (_animator == null) _animator = GetComponent<Animator>();
    }

    private AffinityTypes AttemptDamage(int attackPower, DamageTypes element, bool healing=false)
    {
        AffinityTypes affinityToAttack = AffinityTypes.NONE;

        try { affinityToAttack = _stats.BaseAffinities[element]; }
        catch { /* ignored */ }

        if (healing) affinityToAttack = AffinityTypes.ABSORB;

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
            _dieVoiceline.Post(gameObject);
        }
        else
        {
            switch (affinityToAttack)
            {
                case AffinityTypes.NONE:
                    _hitVoiceline.Post(gameObject);
                    break;
                case AffinityTypes.WEAK:
                    _bigHitVoiceline.Post(gameObject);
                    break;
            }
        }

        if (healing) affinityToAttack = AffinityTypes.NONE;
        
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
        
        if (targetAffinity == AffinityTypes.WEAK)
            _bigAttackVoiceline.Post(gameObject); 
        else 
            _attackVoiceline.Post(gameObject);

        return targetAffinity;
    }

    protected AffinityTypes UseSkill(Skill skill, Fighter target = null)
    {
        AffinityTypes targetAffinity = AffinityTypes.NONE;
        
        if (!CheckIfCanUseSkill(skill))
        {
            UIManager.Instance.UpdateText(_stats.Name + " tried to cast " + skill.Name + "... but couldn't afford it!\n");
            return targetAffinity;
        }
        
        if (skill.CostsHP) _HP -= skill.CostAmount;
        else _MP -= skill.CostAmount;

        if (target == null) return targetAffinity;
        
        targetAffinity = target.AttemptDamage(skill.BaseDamage, skill.ElementType, skill.Healing);
            
        string str = _stats.Name + " cast " + skill.Name + " on " + target.Name + "!\n";

        if (skill.Healing)
        {
            str = _stats.Name + " healed " + target.Name + "!\n";
        }
        else
        {
            if (targetAffinity == AffinityTypes.WEAK)
                str += target.Name + " was weak to " + _stats.Name + "'s attack!\n";
            else if (targetAffinity == AffinityTypes.RESIST)
                str += target.Name + " resisted " + _stats.Name + "'s attack!\n";
            else if (targetAffinity == AffinityTypes.NULL)
                str += target.Name + " blocked " + _stats.Name + "'s attack!\n";
            else if (targetAffinity == AffinityTypes.ABSORB)
                str += target.Name + " absorbed " + _stats.Name + "'s attack!\n";
        }
        
        if (targetAffinity == AffinityTypes.WEAK)
            _bigAttackVoiceline.Post(gameObject); 
        else 
            _skillVoiceline.Post(gameObject);

        if (target.CurrentStatusEffect == StatusEffectType.DEAD) str += "<color=red>" + target.Name + " has collapsed!";
        // else str += target.Name + "'s HP is now " + target.HP + "!";
        UIManager.Instance.UpdateText(str);

        return targetAffinity;
    }

    protected AffinityTypes PassTurn()
    {
        UIManager.Instance.UpdateText(_stats.Name + " passed things to the next person!\n");
        _guardVoiceline.Post(gameObject);
        return AffinityTypes.WEAK;
    }
    
    protected void UseItem() {}

    protected AffinityTypes Guard()
    {
        UIManager.Instance.UpdateText(_stats.Name + " put up their guard!\n");
        _guarding = true;
        _guardVoiceline.Post(gameObject);
        return AffinityTypes.NONE;
    }
    
    public virtual void TakeAction() {}
}
