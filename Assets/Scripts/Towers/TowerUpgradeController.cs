using UnityEngine;
using Zenject;

public class TowerUpgradeController
{
	private Registry<Tower> _towers;
	private PlayerStats _playerStats;
	private GameSettings _gameSettings;
	private Session _session;
	
	[Inject]
	public TowerUpgradeController(Registry<Tower> towers, PlayerStats playerStats, GameSettings gameSettings, Session session)
	{
		_towers = towers;
		_playerStats = playerStats;
		_gameSettings = gameSettings;
		_session = session;
	}
	
	public void Upgrade(Tower tower)
	{
		if (_session.SessionState == Session.State.Stopped)
			return;
		
		if(_playerStats.Gold < _gameSettings.towerUpgradeCost)
			return;

		_playerStats.Gold -= _gameSettings.towerUpgradeCost;
		
		tower.AddDamage(_gameSettings.towerUpgradeSettings.damageUpgrade);
		tower.AddRateOfFire(_gameSettings.towerUpgradeSettings.rateOfFireUpgrade);
		
		Debug.Log($"Upgrading tower");
	}

	public void ResetTowersUpgrades()
	{
		foreach (Tower tower in _towers.Items)
		{
			tower.ResetStats();
		}
	}
}