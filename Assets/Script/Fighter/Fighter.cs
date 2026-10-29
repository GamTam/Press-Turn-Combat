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
    [SerializeField] private FighterStats _stats = new FighterStats();
    private int _HP;
    private int _MP;

    private Buffs _currentBuffs;
    private Buffs _tempBuffs;
    
    private StatusEffectType _currentStatusEffect;

    private bool _physCharge;
    private bool _magCharge;

    public string Name => _stats.Name;
    public StatusEffectType CurrentStatusEffect => _currentStatusEffect;

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

    private void BasicAttack(Fighter target)
    {
        target.TakeDamage(_stats.STR, _stats.BaseAttackType);
    }
    
    private void UseSkill(Skill skill, Fighter target = null)
    {
        if (skill.CostsHP) _HP -= skill.CostAmount;
        else _MP -= skill.CostAmount;
        
        if (target != null) target.TakeDamage(skill.BaseDamage, skill.ElementType);
    }
    
    private void PassTurn() {}
    private void UseItem() {}
    private void Guard() {}
    
    protected void TakeAction() {}
}
