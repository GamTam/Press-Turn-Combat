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
    [SerializeField] private GameObject _backButton;
    [Space] [SerializeField] private GameObject _skillGrid;
    [SerializeField] private Button _skillButtonPrefab;
    [Space] [SerializeField] private TMP_Text _playerPartyHealthPrefab;
    [SerializeField] private TMP_Text _enemyPartyHealthPrefab;
    [Space] [SerializeField] private AK.Wwise.Event _confirmSound;
    [SerializeField] private AK.Wwise.Event _backSound;

    private List<TMP_Text> _playerPartyHealth = new List<TMP_Text>();
    private List<TMP_Text> _enemyPartyHealth = new List<TMP_Text>();

    private List<Button> _skillButtonList = new List<Button>();
    private List<Skill> _skillList = new List<Skill>();
    private int _skillToUse = 0;

    private bool _selectingTargets;
    private bool _selectingSkill;
    private FightActions _nextAction;
    public event Action<Fighter, FightActions, Skill> OnPlayerFighterAction;

    public void UpdateText(string str)
    {
        _updateTextPrefab.text = str;
        _updateTextPrefab.gameObject.SetActive(true);
    }

    public void UpdateSkillList(List<Skill> skillList)
    {
        _skillList = skillList;
    }

    public void ToggleActionButtonVisibility(bool on = false)
    {
        _actionButtons.SetActive(on);
    }

    public void FightButtonPressed()
    {
        _skillToUse = 0;
        _nextAction = FightActions.Fight;
        _confirmSound.Post(gameObject);
        _backButton.gameObject.SetActive(true);
        TargetSelectionInit();
    }

    public void SkillButtonPressed()
    {
        _skillToUse = 0;
        _selectingSkill = true;
        _nextAction = FightActions.Skill;
        _confirmSound.Post(gameObject);
        _backButton.gameObject.SetActive(true);
        _actionButtons.SetActive(false);
        _skillGrid.SetActive(true);

        foreach (Skill skill in _skillList)
        {
            Button button = Instantiate(_skillButtonPrefab, _skillButtonPrefab.transform.parent);
            button.gameObject.SetActive(true);
            _skillButtonList.Add(button);
            string costType = skill.CostsHP ? "HP" : "SP";
            button.GetComponentInChildren<TMP_Text>().text =
                skill.Name + " (" + skill.CostAmount + " " + costType + ")";
        }
    }

    public void SkillSelect(Transform obj)
    {
        _confirmSound.Post(gameObject);
        _skillToUse = obj.GetSiblingIndex() - 1;
        _backButton.gameObject.SetActive(true);
        
        foreach (Button button in _skillButtonList)
        {
            button.gameObject.SetActive(false);
        }
        
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

    public void BackButtonPressed()
    {
        _backSound.Post(gameObject);

        if (_nextAction == FightActions.Fight && _selectingTargets) {
            TargetSelectionStop();
            _actionButtons.SetActive(true);
            _selectingTargets = false;
            _backButton.gameObject.SetActive(false);
        } 
        else if (_nextAction == FightActions.Skill && _selectingTargets)
        {
            TargetSelectionStop();
            _selectingTargets = false;
        
            foreach (Button button in _skillButtonList)
            {
                button.gameObject.SetActive(true);
            }
        }
        else if (_nextAction == FightActions.Skill && _selectingSkill)
        {
            _backButton.gameObject.SetActive(false);
            _selectingSkill = false;
            foreach (Button button in _skillButtonList)
            {
                Destroy(button.gameObject);
            }

            _skillButtonList = new List<Button>();
            _actionButtons.SetActive(true);
        }
    }

    private void TargetSelectionInit()
    {
        _selectingTargets = true;
        _actionButtons.SetActive(false);
        _backButton.gameObject.SetActive(true);
        foreach (TMP_Text text in _enemyPartyHealth)
        {
            text.transform.GetComponentInChildren<Button>().interactable = true;
        }
    }

    private void TargetSelectionStop()
    {
        _selectingTargets = false;
        foreach (TMP_Text text in _enemyPartyHealth)
        {
            text.transform.GetComponentInChildren<Button>().interactable = false;
        }
    }

    public void TargetSelectionEnd(Transform obj)
    {
        _selectingTargets = false;
        _selectingSkill = false;
        
        _backButton.gameObject.SetActive(false);
        
        foreach (Button button in _skillButtonList)
        {
            Destroy(button.gameObject);
        }

        _skillButtonList = new List<Button>();
        
        foreach (TMP_Text text in _enemyPartyHealth)
        {
            text.transform.GetComponentInChildren<Button>().interactable = false;
        }
        
        OnPlayerFighterAction?.Invoke(BattleManager.Instance.Teams[1].PartyMembers[obj.GetSiblingIndex() - 1], _nextAction, _skillList[_skillToUse]);
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
