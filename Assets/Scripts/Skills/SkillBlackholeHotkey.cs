using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillBlackholeHotkey : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _hotkeyText;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private KeyCode _hotkey = KeyCode.None;
    private Transform _enemyTransform;
    private SkillBlackholeController _skillBlackholeController;

    public void SetupHotKey(KeyCode hotkey, Transform enemy, SkillBlackholeController skillBlackholeController)
    {
        _hotkey = hotkey;
        _hotkeyText.text = _hotkey.ToString();
        _enemyTransform = enemy;
        _skillBlackholeController = skillBlackholeController;
    } 
    private void Update()
    {
        if (Input.GetKeyDown(_hotkey))
        {
            _skillBlackholeController.AddEnemyToList(_enemyTransform);
            _hotkeyText.color = Color.clear;
            _spriteRenderer.color = Color.clear;
        }
    }
}