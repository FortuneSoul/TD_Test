using UnityEngine;

public struct LevelData
{
	public Path path;
	public float secondsBetweenWaves;
	public int maxAdditionalEnemiesPerWave;
	public int startingGold;
	public int startingHealth;
	
	public struct Path
	{
		public Vector2 spawnPos;
		public Vector2[] waypoints;
	}
}