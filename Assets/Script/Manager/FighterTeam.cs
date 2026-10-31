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

        foreach (Fighter fighter in _party)
        {
            fighter.SetFighterTeam(this);
        }
    }

    public void InitiateFighterAction()
    {
        _party[_actingPartyMember].TakeAction();
    }

    public void EndFighterAction(AffinityTypes currentAffinity)
    {
        int pressTurnsUsed = 1;
        bool useHalfTurns = false;

        switch (currentAffinity)
        {
            case AffinityTypes.ABSORB or AffinityTypes.REPEL or AffinityTypes.NULL:
                pressTurnsUsed = 99;
                break;
            case AffinityTypes.RESIST:
                pressTurnsUsed = 2;
                break;
            case AffinityTypes.WEAK:
                useHalfTurns = true;
                break;
        }
        
        BattleManager.Instance.UpdatePressTurns(pressTurnsUsed, useHalfTurns);

        do
        {
            _actingPartyMember += 1;
            if (_actingPartyMember >= _party.Count) _actingPartyMember = 0;
        } while (_party[_actingPartyMember].CurrentStatusEffect == StatusEffectType.DEAD);
    }
}
