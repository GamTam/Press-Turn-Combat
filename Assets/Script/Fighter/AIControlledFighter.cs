using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AIControlledFighter : Fighter
{
    public override void TakeAction()
    {
        StartCoroutine(WaitForPlayerInput());
    }

    private IEnumerator WaitForPlayerInput()
    {
        UIManager.Instance.UpdateText(_stats.Name + "'s action!");
        
        float time = 0f;
        float duration = 1.5f;
        
        while (time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        
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
