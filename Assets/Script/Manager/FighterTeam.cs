using System.Collections;
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
        _actingPartyMember = _party.Count;

        foreach (Fighter fighter in _party)
        {
            fighter.SetFighterTeam(this);
        }
    }

    public void InitiateFighterAction()
    {
        do
        {
            _actingPartyMember += 1;
            if (_actingPartyMember >= _party.Count) _actingPartyMember = 0;
        } while (_party[_actingPartyMember].CurrentStatusEffect == StatusEffectType.DEAD);
        
        _party[_actingPartyMember].TakeAction();
    }

    public void EndFighterAction(AffinityTypes currentAffinity)
    {
        StartCoroutine(EndFighterActionCoroutine(currentAffinity));
    }

    private IEnumerator EndFighterActionCoroutine(AffinityTypes currentAffinity)
    {
        float time = 0f;
        float duration = 3f;

        while (time < duration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        
        
        
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
        BattleManager.Instance.ActionStart();
    }
}
