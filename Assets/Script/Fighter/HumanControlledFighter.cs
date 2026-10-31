using UnityEngine;

public class HumanControlledFighter : Fighter
{
    public override void TakeAction()
    {
        AffinityTypes enemyAffinityToSkill = AffinityTypes.NONE;
        
        FighterTeam targetTeam = BattleManager.Instance.Teams[(BattleManager.Instance.CurrentTeam + 1) % 2];
        
        Fighter target = targetTeam.PartyMembers[Random.Range(0, targetTeam.PartyMembers.Count)];

        while (target.CurrentStatusEffect == StatusEffectType.DEAD)
        {
            target = targetTeam.PartyMembers[Random.Range(0, targetTeam.PartyMembers.Count)];
        }
        
        bool useSkill = Random.Range(0, 100) > 49;

        if (!useSkill)
            enemyAffinityToSkill = BasicAttack(target);
        else
            enemyAffinityToSkill = UseSkill(Skills[0], target);
        
        _currentTeam.EndFighterAction(enemyAffinityToSkill);
    }
}
