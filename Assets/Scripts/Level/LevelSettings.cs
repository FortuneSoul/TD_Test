using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Level")]
public class LevelSettings : ScriptableObject
{
	[SerializeField] private Path _path = default;
	public Path EnemyPath => _path;
	
	[SerializeField]
	private TowerSettings[] _towersSettings = default;
	public TowerSettings[] TowersSettings => _towersSettings;

	[SerializeField] private int[,] _levelLayout =
	{
		{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
		{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
		{ 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1 },
		{ 0, 0, 1, 0, 1, 0, 0, 0, 1, 1, 1, 1, 0, 1, 0, 0, 0 },
		{ 1, 1, 1, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, 1, 0, 0, 0 },
		{ 0, 0, 0, 0, 1, 0, 1, 1, 0, 0, 0, 1, 0, 1, 0, 0, 0 },
		{ 0, 0, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 0 },
		{ 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0 },
		{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
	};

	public int[,] LevelLayout => _levelLayout;

	[SerializeField] private int _maxAdditionalEnemiesPerWave = default;
	public int MaxAdditionalEnemiesPerWave => _maxAdditionalEnemiesPerWave;

	[SerializeField] private float _secondsBetweenWaves = default;
	public float SecondsBetweenWaves => _secondsBetweenWaves;

	[SerializeField] private int _startingHealth = default;
	public int StartingHealth => _startingHealth;

	[SerializeField] private int _startingGold = default;
	public int StartingGold => _startingGold;

	[SerializeField] private LevelLayoutPrefabs _layoutPrefabs = default;
	public LevelLayoutPrefabs LayoutPrefabs => _layoutPrefabs;
	
	[System.Serializable]
	public struct Path
	{
		public Vector2Int spawnerPosition;
		public Vector2Int[] waypoints;
	}

	[System.Serializable]
	public struct TowerSettings
	{
		public Vector2Int coordinates;
	}
}