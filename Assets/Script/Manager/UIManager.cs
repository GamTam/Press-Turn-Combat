using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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

    private void Awake()
    {
        InputManager.Controls.UI.Cancel.performed += BackButtonPressed;  
    }
    
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
        if (on)
        {
            Debug.Log("Shown action buttons");
            InputManager.SetControlMode(InputManager.ControlType.Battle);
            EventSystem.current.SetSelectedGameObject(null);
            InputManager.Controls.Battle.Attack.performed += FightButtonPressed;
            InputManager.Controls.Battle.Skill.performed += SkillButtonPressed;
            InputManager.Controls.Battle.Guard.performed += GuardButtonPressed;
            InputManager.Controls.Battle.Pass.performed += PassButtonPressed;
        }
        else
        {
            Debug.Log("Hid action buttons");
            InputManager.SetControlMode(InputManager.ControlType.UI);
            InputManager.Controls.Battle.Attack.performed -= FightButtonPressed;
            InputManager.Controls.Battle.Skill.performed -= SkillButtonPressed;
            InputManager.Controls.Battle.Guard.performed -= GuardButtonPressed;
            InputManager.Controls.Battle.Pass.performed -= PassButtonPressed; 
        }
        
        _actionButtons.SetActive(on);
    }

    private void FightButtonPressed(InputAction.CallbackContext ctx)
    {
        FightButtonPressed();
    }
    
    public void FightButtonPressed()
    {
        _skillToUse = 0;
        _nextAction = FightActions.Fight;
        _confirmSound.Post(gameObject);
        _backButton.gameObject.SetActive(true);
        TargetSelectionInit();
    }
    
    private void SkillButtonPressed(InputAction.CallbackContext ctx)
    {
        SkillButtonPressed();
    }

    public void SkillButtonPressed()
    {
        _skillToUse = 0;
        _selectingSkill = true;
        _nextAction = FightActions.Skill;
        _confirmSound.Post(gameObject);
        _backButton.gameObject.SetActive(true);
        ToggleActionButtonVisibility(false);
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
        
        EventSystem.current.SetSelectedGameObject(_skillButtonList[0].GetComponentInChildren<Button>().gameObject);
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

    private void GuardButtonPressed(InputAction.CallbackContext ctx)
    {
        GuardButtonPressed();
    }
    
    public void GuardButtonPressed()
    {
        OnPlayerFighterAction?.Invoke(null, FightActions.Guard, new Skill());
        _confirmSound.Post(gameObject);
    }
    
    private void PassButtonPressed(InputAction.CallbackContext ctx)
    {
        PassButtonPressed();
    }

    public void PassButtonPressed()
    {
        OnPlayerFighterAction?.Invoke(null, FightActions.Pass, new Skill());
        _confirmSound.Post(gameObject);
    }
    
    private void BackButtonPressed(InputAction.CallbackContext ctx)
    {
        Debug.Log("aaaaaaaaaaaaaaaa");
        if (_backButton.gameObject.activeSelf) BackButtonPressed();
    }

    public void BackButtonPressed()
    {
        _backSound.Post(gameObject);

        if (_nextAction == FightActions.Fight && _selectingTargets) {
            TargetSelectionStop();
            ToggleActionButtonVisibility(true);
            _backButton.gameObject.SetActive(false);
        } 
        else if (_nextAction == FightActions.Skill && _selectingTargets)
        {
            TargetSelectionStop();
        
            foreach (Button button in _skillButtonList)
            {
                button.gameObject.SetActive(true);
            }
            EventSystem.current.SetSelectedGameObject(_skillButtonList[0].GetComponentInChildren<Button>().gameObject);
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
            ToggleActionButtonVisibility(true);
        }
    }

    private void TargetSelectionInit()
    {
        _selectingTargets = true;
        ToggleActionButtonVisibility(false);
        _backButton.gameObject.SetActive(true);
        StartCoroutine(EnableTargetSelectionButtons());
    }

    private IEnumerator EnableTargetSelectionButtons()
    {
        yield return null;
        
        foreach (TMP_Text text in _enemyPartyHealth)
        {
            text.transform.GetComponentInChildren<Button>().interactable = true;
        }

        foreach (TMP_Text text in _enemyPartyHealth)
        {
            if (text.gameObject.activeSelf)
            {
                EventSystem.current.SetSelectedGameObject(text.GetComponentInChildren<Button>().gameObject);
                yield break;
            }
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
