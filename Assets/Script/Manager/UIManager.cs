using TMPro;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TMP_Text _updateTextPrefab;

    public void UpdateText(string str)
    {
        TMP_Text instance = Instantiate(_updateTextPrefab, _updateTextPrefab.transform.parent, true);
        instance.transform.SetSiblingIndex(0);
        instance.text = str;
        instance.gameObject.SetActive(true);
    }
}
