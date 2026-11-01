using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanControlledFighter : Fighter
{
    public override void TakeAction()
    {
        _guarding = false;
        UIManager.Instance.UpdateText(_stats.Name + "'s action!");
        UIManager.Instance.ToggleActionButtonVisibility(true);
        UIManager.Instance.OnPlayerFighterAction += UseSkill;
    }

    private void UseSkill(Fighter target, FightActions actions, Skill skill)
    {
        AffinityTypes enemyAffinityToSkill = AffinityTypes.NONE;

        switch (actions)
        {
            case FightActions.Fight:
                enemyAffinityToSkill = BasicAttack(target);
                break;
            case FightActions.Skill:
                enemyAffinityToSkill = UseSkill(Skills[0], target);
                break;
            case FightActions.Guard:
                enemyAffinityToSkill = Guard();
                break;
            case FightActions.Pass:
                enemyAffinityToSkill = PassTurn();
                break;
        }

        UIManager.Instance.ToggleActionButtonVisibility(false);
        UIManager.Instance.OnPlayerFighterAction -= UseSkill;

        _currentTeam.EndFighterAction(enemyAffinityToSkill);
    }
}
