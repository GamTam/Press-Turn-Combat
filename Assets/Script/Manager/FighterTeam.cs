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
}
