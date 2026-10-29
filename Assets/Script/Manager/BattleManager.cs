using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum BattleState
{
        PlayerTurn,
        EnemyTurn,
        Win,
        Lose
}

[System.Serializable]
public enum PressTurn
{
    Full,
    Half,
    Empty
}

public class BattleManager : Singleton<BattleManager>
{
    private BattleState CurrentBattleState = BattleState.EnemyTurn;
    private int CurrentTeam = 0;
    public List<PressTurn> PressTurns = new List<PressTurn>();
    
    public FighterTeam[] Teams = new FighterTeam[2];

    public void Start()
    {
        UpdateState();
        Debug.Log("Current state: " + CurrentBattleState);

        for (int i = 0; i < Teams.Length; i++)
        {
            string str = "Team " + i + "'s party members:\n";
            foreach (Fighter fighter in Teams[i].PartyMembers)
            {
                str += fighter.Name + "\n";
            }
            Debug.Log(str);
        }
    }
    
    public void UpdatePressTurns(int turnCost)
    {
        PressTurns.Remove(0);

        UpdateState();
    }

    private void UpdateState()
    {
        CheckIfTeamsAreAlive();

        if (CurrentBattleState == BattleState.Win)
        {
            Debug.Log("You win! Congrats!");
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }
        
        if (CurrentBattleState == BattleState.Lose)
        {
            Debug.Log("You lose... That sucks, man...");
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }

        if (PressTurns.Count == 0)
        {
            if (CurrentBattleState == BattleState.PlayerTurn)
            {
                CurrentBattleState = BattleState.EnemyTurn;
                CurrentTeam = 1;
            }
            else if (CurrentBattleState == BattleState.EnemyTurn)
            {
                CurrentBattleState = BattleState.PlayerTurn;
                CurrentTeam = 0;
            }
            
            foreach (Fighter fighter in Teams[CurrentTeam].PartyMembers)
            {
                if (fighter.CurrentStatusEffect != StatusEffectType.DEAD)
                    PressTurns.Add(PressTurn.Full);
            }
        }
    }

    private void CheckIfTeamsAreAlive()
    {
        bool partyHasDied = true;
        
        foreach ( Fighter fighter in Teams[0].PartyMembers )
        {
            if (fighter.CurrentStatusEffect != StatusEffectType.DEAD)
            {
                partyHasDied = false;
                break;
            }
        }

        if (partyHasDied)
        {
            CurrentBattleState = BattleState.Lose;
            return;
        }
        
        foreach ( Fighter fighter in Teams[1].PartyMembers )
        {
            if (fighter.CurrentStatusEffect != StatusEffectType.DEAD) return;
        }

        CurrentBattleState = BattleState.Win;
    }
}
