using TMPro;
using UnityEngine;
using Zenject;

public class HUD : MonoBehaviour
{
    [SerializeField] private TMP_Text _goldText = default;
    [SerializeField] private string _goldTextTemplate = default;
    
    [Space]
    
    [SerializeField] private TMP_Text _lifesText = default;
    [SerializeField] private string _lifesTextTemplate = default;

    [Space] 
    
    [SerializeField] private TMP_Text _upgradeCostText = default;
    [SerializeField] private string _upgradeTextTemplate = default;
    
    private PlayerStats _playerStats;
    private GameSettings _gameSettings;
    
    [Inject]
    private void Construct(PlayerStats playerStats, GameSettings gameSettings)
    {
        _playerStats = playerStats;
        _gameSettings = gameSettings;

        PlayerStats.OnPlayerStatsUpdatedEvent += Refresh;
    }


    private void Refresh()
    {
        _goldText.text = string.Format(_goldTextTemplate, _playerStats.Gold);
        _lifesText.text = string.Format(_lifesTextTemplate, Mathf.Clamp(_playerStats.Health, 0, _playerStats.Health));
        _upgradeCostText.text = string.Format(_upgradeTextTemplate, _gameSettings.towerUpgradeCost);
    }
}
