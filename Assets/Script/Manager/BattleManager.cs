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
    Half,
    Empty
}

public class BattleManager : Singleton<BattleManager>
{
    private BattleState CurrentBattleState = BattleState.EnemyTurn;
    private int CurrentTeam = 0;
    public List<PressTurn> PressTurns = new List<PressTurn>();
    
    public FighterTeam[] Teams = new FighterTeam[2];

    //TEMP
    [SerializeField] private float _timer;
    private float _timerDuration = 10f;
    
    public IEnumerator Start()
    {
        _timer = _timerDuration;
        yield return null;
        UpdateState();
    }
    
    public void UpdatePressTurns(int turnCost)
    {
        PressTurns.Remove(0);
    }

    //TEMP
    private void Update()
    {
        _timer -= Time.deltaTime;
        
        if (_timer > 0f && !Keyboard.current.spaceKey.wasPressedThisFrame) return;
        _timer = _timerDuration;

        FighterTeam targetTeam = Teams[(CurrentTeam + 1) % 2];
        Fighter targetFighter = targetTeam.PartyMembers[Random.Range(0, targetTeam.PartyMembers.Count)];

        while (targetFighter.CurrentStatusEffect == StatusEffectType.DEAD)
        {
            targetFighter = targetTeam.PartyMembers[Random.Range(0, targetTeam.PartyMembers.Count)];
        }
        
        Teams[CurrentTeam].RandomAttack(targetFighter);
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
            
            // TEMP
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
