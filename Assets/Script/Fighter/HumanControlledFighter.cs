using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanControlledFighter : Fighter
{
    public override void TakeAction()
    {
        UIManager.Instance.UpdateText(_stats.Name + "'s action!");
        UIManager.Instance.ToggleActionButtonVisibility(true);
        UIManager.Instance.OnPlayerFighterAction += UseSkill;
    }

    private void UseSkill(Fighter target, FightActions actions, Skill skill)
    {
        AffinityTypes enemyAffinityToSkill = AffinityTypes.NONE;
        
        bool useSkill = actions == FightActions.Skill;

        if (!useSkill)
            enemyAffinityToSkill = BasicAttack(target);
        else
            enemyAffinityToSkill = UseSkill(Skills[0], target);
        
        UIManager.Instance.ToggleActionButtonVisibility(false);
        UIManager.Instance.OnPlayerFighterAction -= UseSkill;

        _currentTeam.EndFighterAction(enemyAffinityToSkill);
    }
}
