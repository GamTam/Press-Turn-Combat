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

    public void TakeDamage(int attackPower, DamageTypes element)
    {
        _HP -= attackPower;

        if (_HP <= 0)
        {
            _HP = 0;
            _currentStatusEffect = StatusEffectType.DEAD;
        }
    }

    private bool CheckIfCanUseSkill(Skill skill)
    {
        if (skill.CostsHP & _HP <= skill.CostAmount) return false;
        if (!skill.CostsHP & _MP <= skill.CostAmount) return false;

        return true;
    }

    public void BasicAttack(Fighter target)
    {
        target.TakeDamage((int) Random.Range(_stats.STR * 0.75f, _stats.STR * 1.25f), _stats.BaseAttackType);
    }
    
    public bool UseSkill(Skill skill, Fighter target = null)
    {
        if (skill.CostsHP & _HP < skill.CostAmount) return false;
        if (!skill.CostsHP & _MP < skill.CostAmount) return false;
        
        if (skill.CostsHP) _HP -= skill.CostAmount;
        else _MP -= skill.CostAmount;
        
        if (target != null) target.TakeDamage(skill.BaseDamage, skill.ElementType);
        return true;
    }
    
    private void PassTurn() {}
    private void UseItem() {}
    private void Guard() {}
    
    protected void TakeAction() {}
}
