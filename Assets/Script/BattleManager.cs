using System.Collections.Generic;
using UnityEngine;

public enum BattleState
{
        PlayerTurn,
        EnemyTurn,
        Win,
        Lose
}

public class BattleManager : Singleton<BattleManager>
{
    public enum PressTurn
    {
        Full,
        Half,
        Empty
    }

    private BattleState _currentBattleState = BattleState.PlayerTurn;
    private List<PressTurn> _pressTurns = new List<PressTurn>();

}
