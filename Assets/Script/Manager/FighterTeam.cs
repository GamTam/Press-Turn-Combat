using System.Collections.Generic;
using UnityEngine;

public class FighterTeam : MonoBehaviour
{
    [SerializeField] private List<Fighter> _party;
    [SerializeField] private bool _playerTeam;

    private int _actingPartyMember;
    
    public List<Fighter> PartyMembers => _party;
    
    private void Start()
    {
        if (_playerTeam) BattleManager.Instance.Teams[0] = this;
        else BattleManager.Instance.Teams[1] = this;
    }

    public void ForceAttack(Fighter target)
    {
        bool useSkill = Random.Range(0, 100) > 49;

        if (!useSkill)
        {
            _party[_actingPartyMember].BasicAttack(target);
            Debug.Log(_party[_actingPartyMember].Name + " attacked " + target.Name + "!\n" + target.Name +
                      "'s HP is now " + target.HP + "!");
        }
        else
        {
            bool canAffordSkill = _party[_actingPartyMember].UseSkill(_party[_actingPartyMember].Skills[0], target);
            
            if (canAffordSkill)
                Debug.Log(_party[_actingPartyMember].Name + " cast " + _party[_actingPartyMember].Skills[0].Name +
                          " on " + target.Name + "!\n" + target.Name + "'s HP is now " + target.HP + "!");
            else
                Debug.Log(_party[_actingPartyMember].Name + " tried to cast " + _party[_actingPartyMember].Skills[0].Name + "... but couldn't afford it!\n");
        }

        BattleManager.Instance.UpdatePressTurns(1);

        do
        {
            _actingPartyMember += 1;
            if (_actingPartyMember >= _party.Count) _actingPartyMember = 0;
        } while (_party[_actingPartyMember].CurrentStatusEffect == StatusEffectType.DEAD);
    }
}
