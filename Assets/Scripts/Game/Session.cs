using System.Linq;
using UnityEngine;
using Zenject;

public class Session : ITickable
{
	public State SessionState { get; private set; }
	
	private float _secondsBetweenWaves;
	private int _maxAdditionalEnemiesPerWave;
	private int _waveNumber;
	private int _score;

	private float _waveCountdown;
	private float _wavesTimer;
	

	private PlayerStats _playerStats;
	private Spawner _spawner;

	private LevelData _levelData;
	private Registry<Enemy> _enemyRegistry;

	private readonly SignalBus _signalBus;
	private readonly EnemyStats _enemyStatsSettings;
	private EnemyCommonSettings _currentEnemyStats;
	
	public enum State
	{
		Stopped,
		Running
	}
	
	public Session(SignalBus signalBus, PlayerStats playerStats, Spawner spawner, Registry<Enemy> enemyRegistry, GameSettings gameSettings)
	{
		_signalBus = signalBus;
		_signalBus.Subscribe<EnemyDeadSignal>(OnEnemyDead);
		
		_playerStats = playerStats;
		_spawner = spawner;
		_enemyRegistry = enemyRegistry;

		_enemyStatsSettings = gameSettings.enemyStats;
	}
	
	~Session()
	{
		_signalBus.Unsubscribe<EnemyDeadSignal>(OnEnemyDead);
	}
	
	public void OnEnemyDead(EnemyDeadSignal args)
	{
		if(SessionState != State.Running)
			return;

		_playerStats.Gold += args.enemy.GoldReward;
		_score++;
	}

	public void OnEnemyReachedPlayerBase(Enemy enemy)
	{
		int damage = enemy.GetDamage();
		_playerStats.Health -= damage;
	}
	
	public void Start(LevelData level)
	{
		_levelData = level;
		_waveCountdown = 0;
		_waveNumber = 0;
		_score = 0;

		RefreshParameters(level);
		RefreshPlayerStats(level);
		RefreshEnemyStats();
		
		SessionState = State.Running;
	}

	public void End()
	{
		SessionState = State.Stopped;
		_spawner.StopSpawning();
		RemoveLeftoverEnemies();
	}

	private void RemoveLeftoverEnemies()
	{
		Enemy[] leftoverEnemies = _enemyRegistry.Items.ToArray();
		foreach (Enemy enemy in leftoverEnemies)
		{
			if(enemy.IsDead)
				continue;
			
			enemy.Die();
		}
	}
	
	private void RefreshParameters(LevelData level)
	{
		_secondsBetweenWaves = level.secondsBetweenWaves;
		_maxAdditionalEnemiesPerWave = level.maxAdditionalEnemiesPerWave;
	}

	private void RefreshPlayerStats(LevelData levelSettings)
	{
		_playerStats.Gold = levelSettings.startingGold;
		_playerStats.Health = levelSettings.startingHealth;
	}

	private void RefreshEnemyStats()
	{
		_currentEnemyStats.damage = _enemyStatsSettings.damage;
		_currentEnemyStats.health = _enemyStatsSettings.health;
		_currentEnemyStats.goldReward = _enemyStatsSettings.goldReward;
	}

	public void Tick()
	{
		if (SessionState != State.Running)
			return;
		
		if (_waveCountdown <= 0)
		{
			SpawnNextWave();
			_waveCountdown = _secondsBetweenWaves;
		}

		_waveCountdown -= Time.deltaTime;
	}

	public int GetSessionScore()
	{
		return _score;
	}
	
	private void SpawnNextWave()
	{
		if(_waveNumber != 0)
			UpgradeEnemies();
		
		int wave = ++_waveNumber;
		
		Debug.Log($"Spawning enemies with {_currentEnemyStats.damage} damage, {_currentEnemyStats.health} health, {_currentEnemyStats.goldReward} reward");
		
		int maxPossibleEnemies = wave + _maxAdditionalEnemiesPerWave;
		int enemiesCount = Random.Range(wave, maxPossibleEnemies + 1);
		
		_spawner.Spawn(enemiesCount, _levelData.path, _currentEnemyStats);
	}

	private void UpgradeEnemies()
	{
		_currentEnemyStats.damage +=
			Random.Range(_enemyStatsSettings.damageUpgradeMin, _enemyStatsSettings.damageUpgradeMax + 1);
		
		_currentEnemyStats.health +=
			Random.Range(_enemyStatsSettings.healthUpgradeMin, _enemyStatsSettings.healthUpgradeMax + 1);
		
		_currentEnemyStats.goldReward +=
			Random.Range(_enemyStatsSettings.goldRewardUpgradeMin, _enemyStatsSettings.goldRewardUpgradeMax + 1);
	}
}