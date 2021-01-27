using System;
using UnityEngine;
using Zenject;

public class Enemy : MonoBehaviour, IPoolable<EnemyCommonSettings, IMemoryPool>, IDisposable, IHaveHealth
{
	private IMemoryPool _pool;
	private Registry<Enemy> _registry;
	private SignalBus _signalBus;
	private Session _session;
	
	private int _maxHealth;
	private int _currentHealth;
	private int _damage;

	public bool IsDead { get; private set; }
	
	private WaypointFollowing _waypointFollowing;
	private int _goldReward;
	public int GoldReward => _goldReward;
	
	[Inject]
	private void Construct(Registry<Enemy> registry, SignalBus signalBus, PlayerStats playerStats, Session session)
	{
		_registry = registry;
		_signalBus = signalBus;
		_session = session;
	}
	
	private void Awake()
	{
		_waypointFollowing = GetComponent<WaypointFollowing>();
		_waypointFollowing.OnReachedLastWaypointEvent += OnReachedPlayerBase;
	}

	private void OnReachedPlayerBase()
	{
		_session.OnEnemyReachedPlayerBase(this);

		Die();
	}
	
	public virtual void TakeDamage(int value)
	{
		_currentHealth -= value;

		if (_currentHealth <= 0 && !IsDead)
		{
			Die();
		}
	}

	public void Die()
	{
		IsDead = true;

		_signalBus.Fire(new EnemyDeadSignal { enemy = this });
		Dispose();
	}
	
	public void SetWaypoints(Vector2[] waypoints)
	{
		_waypointFollowing.SetWaypoints(waypoints);
	}
	
	public void OnDespawned()
	{
		_registry.Remove(this);
		_pool = null;
	}

	public void OnSpawned(EnemyCommonSettings settings, IMemoryPool memoryPool)
	{
		IsDead = false;
		
		_pool = memoryPool;
		_registry.Add(this);
		
		SetStats(settings);
	}
	
	private void SetStats(EnemyCommonSettings settings)
	{
		_maxHealth = _currentHealth = settings.health;
		_damage = settings.damage;
		_goldReward = settings.goldReward;
	}
	
	public void Dispose()
	{
		_pool.Despawn(this);
	}

	public int GetCurrentHealth()
	{
		return _currentHealth;
	}

	public int GetMaxHealth()
	{
		return _maxHealth;
	}

	public int GetDamage()
	{
		return _damage;
	}
	
	public class Factory : PlaceholderFactory<EnemyCommonSettings, Enemy> {}
}