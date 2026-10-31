using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    Half
}

public class BattleManager : Singleton<BattleManager>
{
    private BattleState CurrentBattleState = BattleState.EnemyTurn;
    public int CurrentTeam = 0;
    public List<PressTurn> PressTurns = new List<PressTurn>();
    
    public FighterTeam[] Teams = new FighterTeam[2];

    //TEMP
    [SerializeField] private float _timer;
    private float _timerDuration = 5f;

    public bool IsPlayerTurn => CurrentTeam == 0;
    
    public IEnumerator Start()
    {
        _timer = _timerDuration;
        yield return null;
        UpdateState();
    }
    
    //TEMP
    private void Update()
    {
        _timer -= Time.deltaTime;
        
        if (_timer > 0f && !Keyboard.current.spaceKey.wasPressedThisFrame) return;
        _timer = _timerDuration;
        
        if (CurrentBattleState == BattleState.Win || CurrentBattleState == BattleState.Lose)
        {
            UnityEditor.EditorApplication.isPlaying = false;
            return;
        }
        
        Teams[CurrentTeam].InitiateFighterAction();
        UpdateState();
    }

    private void UpdateState()
    {
        CheckIfTeamsAreAlive();

        if (CurrentBattleState == BattleState.Win)
        {
            UIManager.Instance.UpdateText("You win! Congrats!");
            return;
        }
        
        if (CurrentBattleState == BattleState.Lose)
        {
            UIManager.Instance.UpdateText("You lose... That sucks, man...");
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
            
            // TEMP
            if (CurrentBattleState == BattleState.PlayerTurn) UIManager.Instance.UpdateText("<color=red>The enemy ran out of Press Turns,\n it's now your Turn!");
            else if (CurrentBattleState == BattleState.EnemyTurn) UIManager.Instance.UpdateText("<color=red>You ran out of Press Turns,\n it's now the Enemy's Turn!");
            
            Debug.Log("<color=red>Current state: " + CurrentBattleState + "</color>");

            for (int i = 0; i < Teams.Length; i++)
            {
                string str = "<color=red>Team " + i + "'s party members:\n";
                foreach (Fighter fighter in Teams[i].PartyMembers)
                {
                    str += fighter.Name + " HP: " + fighter.HP + " MP: " + fighter.MP + "\n";
                }

                str += "</color>";
                Debug.Log(str);
            }
        }
        
        UIManager.Instance.ShowBattleState();
    }
    
    public void UpdatePressTurns(int turnCost, bool useHalfTurns)
    {
        if (!useHalfTurns)
            for (int i = 0; i < turnCost; i++)
            {
                PressTurns.RemoveAt(0);
                if (PressTurns.Count == 0) break;
            }
        else
        {
            for (int i = 0; i < turnCost; i++)
            {
                bool usedTurn = false;
                
                for (int j = 0; j < PressTurns.Count; j++)
                {
                    if (PressTurns[j] == PressTurn.Full)
                    {
                        PressTurns[j] = PressTurn.Half;
                        usedTurn = true;
                        break;
                    }
                }

                if (!usedTurn)
                {
                    PressTurns.RemoveAt(0);
                    if (PressTurns.Count == 0) break;
                }
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
