using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Game Settings")]
public class GameSettings : ScriptableObject
{
	public Enemy enemyPrefab;
	public Tower towerPrefab;

	[Space]
	
	public int towerUpgradeCost;
	
	[Space] 
	
	public EnemyStats enemyStats;
	public TowerUpgradeSettings towerUpgradeSettings;
}

[Serializable]
public struct EnemyStats
{
	public int damage;
	public int health;
	public int goldReward;

	[Space]
	
	public int damageUpgradeMin;
	public int damageUpgradeMax;

	[Space]

	public int healthUpgradeMin;
	public int healthUpgradeMax;

	[Space]
	
	public int goldRewardUpgradeMin;
	public int goldRewardUpgradeMax;
}

[Serializable]
public struct TowerUpgradeSettings
{
	public int damageUpgrade;
	public float rateOfFireUpgrade;
}