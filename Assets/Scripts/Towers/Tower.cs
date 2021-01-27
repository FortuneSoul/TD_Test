using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Tower : MonoBehaviour
{
	[SerializeField] private float _range = default;
	
	[SerializeField] private int _baseDamage = default;
	[SerializeField] private int _baseRateOfFire = default;

	private TowerUpgradeController _upgradeController;
	
	private Registry<Enemy> _enemyRegistry;
	private Registry<Tower> _towerRegistry;
	
	private Enemy _targetEnemy;

	private float _currentDamage;
	private float _currentRateOfFire;

	private float _shootCooldown;
	private float _shootTimer;

	[Inject]
	private void Construct(
		Registry<Enemy> enemyRegistry, 
		Registry<Tower> towerRegistry,
		TowerUpgradeController upgradeController)
	{
		_enemyRegistry = enemyRegistry;
		_towerRegistry = towerRegistry;
		_upgradeController = upgradeController;
		
		ResetStats();
	}

	private void OnEnable()
	{
		_towerRegistry.Add(this);
	}

	private void OnDisable()
	{
		_towerRegistry.Remove(this);
	}

	public void ResetStats()
	{
		_currentDamage = _baseDamage;
		_currentRateOfFire = _baseRateOfFire;
		
		_shootCooldown = 1 / _currentRateOfFire;
	}
	
	private void Update()
	{
		if (!_targetEnemy || _targetEnemy.IsDead)
			_targetEnemy = FindNearestEnemy();

		Aim();
		Shoot();
	}

	private Enemy FindNearestEnemy()
	{
		IEnumerable<Enemy> enemies = _enemyRegistry.Items;

		float minDistance = float.MaxValue;
		Enemy closest = null;

		foreach (Enemy enemy in enemies)
		{
			float distance = Vector2.Distance(enemy.transform.position, transform.position);
			if (distance < minDistance)
			{
				minDistance = distance;
				closest = enemy;
			}
		}

		return minDistance <= _range ? closest : null;
	}

	private void Aim()
	{
		if (_targetEnemy == null)
			return;

		if (Vector2.Distance(transform.position, _targetEnemy.transform.position) > _range)
		{
			_targetEnemy = null;
			return;
		}

		Vector2 targetPos = _targetEnemy.transform.position;
		Vector2 direction = (targetPos - (Vector2) transform.position).normalized;

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.Euler(Vector3.forward * angle);
	}

	private void Shoot()
	{
		if (_shootTimer <= 0f && _targetEnemy)
		{
			_shootTimer = _shootCooldown;
			_targetEnemy.TakeDamage((int)_currentDamage);
		}
		
		_shootTimer -= Time.deltaTime;
	}

	private void OnMouseDown()
	{
		Upgrade();
	}

	private void Upgrade()
	{
		_upgradeController.Upgrade(this);
	}

	public void AddDamage(int amount)
	{
		_currentDamage += amount;
	}

	public void AddRateOfFire(float amount)
	{
		_currentRateOfFire += amount;
		_shootCooldown = 1f / _currentRateOfFire;
	}
	
	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, _range);
	}
	
	public class Factory : PlaceholderFactory<Tower> {}
}