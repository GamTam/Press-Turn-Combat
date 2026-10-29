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

    public void RandomAttack(Fighter target)
    {
        bool useSkill = Random.Range(0, 100) > 49;
        AffinityTypes currentAffinity = AffinityTypes.NONE;

        if (!useSkill)
        {
            currentAffinity = _party[_actingPartyMember].BasicAttack(target);
        }
        else
        {
            currentAffinity = _party[_actingPartyMember].UseSkill(_party[_actingPartyMember].Skills[0], target);
        }

        int pressTurnsUsed = 1;
        bool useHalfTurns = false;

        if (currentAffinity == AffinityTypes.ABSORB ||
            currentAffinity == AffinityTypes.REPEL ||
            currentAffinity == AffinityTypes.NULL)
            pressTurnsUsed = 99;
        else if (currentAffinity == AffinityTypes.RESIST)
            pressTurnsUsed = 2;
        else if (currentAffinity == AffinityTypes.WEAK)
            useHalfTurns = true;
        
        BattleManager.Instance.UpdatePressTurns(pressTurnsUsed, useHalfTurns);

        do
        {
            _actingPartyMember += 1;
            if (_actingPartyMember >= _party.Count) _actingPartyMember = 0;
        } while (_party[_actingPartyMember].CurrentStatusEffect == StatusEffectType.DEAD);
    }
}
