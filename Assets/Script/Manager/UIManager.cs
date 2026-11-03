using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TMP_Text _updateTextPrefab;
    [SerializeField] private TMP_Text _turnText;
    [SerializeField] private GameObject _actionButtons;
    [Space]
    [SerializeField] private TMP_Text _playerPartyHealthPrefab;
    [SerializeField] private TMP_Text _enemyPartyHealthPrefab;
    [Space]
    [SerializeField] private AK.Wwise.Event _confirmSound;

    private List<TMP_Text> _playerPartyHealth = new List<TMP_Text>();
    private List<TMP_Text> _enemyPartyHealth = new List<TMP_Text>();

    private FightActions _nextAction;
    public event Action<Fighter, FightActions, Skill> OnPlayerFighterAction;
    
    public void UpdateText(string str)
    {
        _updateTextPrefab.text = str;
        _updateTextPrefab.gameObject.SetActive(true);
    }

    public void ToggleActionButtonVisibility(bool on = false)
    {
        _actionButtons.SetActive(on);
    }

    public void FightButtonPressed()
    {
        _nextAction = FightActions.Fight;
        _confirmSound.Post(gameObject);
        TargetSelectionInit();
    }
    
    public void SkillButtonPressed()
    {
        _nextAction = FightActions.Skill;
        _confirmSound.Post(gameObject);
        TargetSelectionInit();
    }

    public void GuardButtonPressed()
    {
        OnPlayerFighterAction?.Invoke(null, FightActions.Guard, new Skill());
        _confirmSound.Post(gameObject);
    }
    
    public void PassButtonPressed()
    {
        OnPlayerFighterAction?.Invoke(null, FightActions.Pass, new Skill());
        _confirmSound.Post(gameObject);
    }
    
    private void TargetSelectionInit()
    {
        _actionButtons.SetActive(false);
        foreach (TMP_Text text in _enemyPartyHealth)
        {
            text.transform.GetComponentInChildren<Button>().interactable = true;
        }
    }

    public void TargetSelectionEnd(Transform obj)
    {
        foreach (TMP_Text text in _enemyPartyHealth)
        {
            text.transform.GetComponentInChildren<Button>().interactable = false;
        }
        
        OnPlayerFighterAction?.Invoke(BattleManager.Instance.Teams[1].PartyMembers[obj.GetSiblingIndex() - 1], _nextAction, new Skill());
        _confirmSound.Post(gameObject);
    }
    
    public void ShowBattleState()
    {
        string str;

        if (BattleManager.Instance.IsPlayerTurn) str = "Player's Turn\n";
        else str = "Enemy's Turn\n";

        foreach (PressTurn pressTurn in BattleManager.Instance.PressTurns)
        {
            if (pressTurn == PressTurn.Half) str += "o";
            else str += "O";
        }

        _turnText.text = str;

        if (_playerPartyHealth.Count == 0)
        {
            foreach (Fighter fighter in BattleManager.Instance.Teams[0].PartyMembers)
            {
                TMP_Text instance =
                    Instantiate(_playerPartyHealthPrefab, _playerPartyHealthPrefab.transform.parent);
                instance.text = fighter.Name + "\n" + fighter.HP;
                instance.gameObject.SetActive(true);
                _playerPartyHealth.Add(instance);
            }
        }
        else
        {
            for (int i = 0; i < BattleManager.Instance.Teams[0].PartyMembers.Count; i++)
            {
                _playerPartyHealth[i].text = BattleManager.Instance.Teams[0].PartyMembers[i].Name + "\n" + BattleManager.Instance.Teams[0].PartyMembers[i].HP;
            }
        }
        
        if (_enemyPartyHealth.Count == 0)
        {
            foreach (Fighter fighter in BattleManager.Instance.Teams[1].PartyMembers)
            {
                TMP_Text instance =
                    Instantiate(_enemyPartyHealthPrefab, _enemyPartyHealthPrefab.transform.parent);
                instance.text = fighter.Name + "\n" + fighter.HP;
                instance.gameObject.SetActive(true);
                _enemyPartyHealth.Add(instance);
            }
        }
        else
        {
            for (int i = 0; i < BattleManager.Instance.Teams[1].PartyMembers.Count; i++)
            {
                if (BattleManager.Instance.Teams[1].PartyMembers[i].CurrentStatusEffect == StatusEffectType.DEAD) _enemyPartyHealth[i].gameObject.SetActive(false);
                else _enemyPartyHealth[i].gameObject.SetActive(true);
                
                _enemyPartyHealth[i].text = BattleManager.Instance.Teams[1].PartyMembers[i].Name + "\n" + BattleManager.Instance.Teams[1].PartyMembers[i].HP;
            }
        }
        
    }
}
