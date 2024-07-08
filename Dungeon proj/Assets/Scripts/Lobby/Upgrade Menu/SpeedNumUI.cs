using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedNumUI : MonoBehaviour
{
    private GameObject _player;
    private PlayerMovement _playerMovement;
    private TMP_Text _speedText;

    private void Awake()
    {
        _speedText = GetComponent<TMP_Text>();
        _player = GameObject.Find("Player");
        if (_player != null)
        {
            _playerMovement = _player.GetComponent<PlayerMovement>();
        }
    }

    void Update()
    {
        UpdateUpgradeMenu(_playerMovement);
    }

    public void UpdateUpgradeMenu(PlayerMovement playerMovement)
    {
        _speedText.text = $"SPEED: {playerMovement.playerSpeedStat}"; //$ allows {} to be embedded within ""
    }
}
