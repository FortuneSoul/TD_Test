using Zenject;

public class PlayerHealthObserver : ITickable
{
	private readonly PlayerStats _playerStats;
	private readonly GameManager _gameManager;
	
	[Inject]
	private PlayerHealthObserver(PlayerStats playerStats, GameManager gameManager)
	{
		_playerStats = playerStats;
		_gameManager = gameManager;
	}
	
	public void Tick()
	{
		if (_playerStats.Health <= 0 && _gameManager.Session.SessionState == Session.State.Running)
		{
			_gameManager.OnPlayerDead();
		}
	}
}