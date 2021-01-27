using UnityEngine;

[CreateAssetMenu(fileName = "LevelLayout", menuName = "Level Layout Prefabs")]
public class LevelLayoutPrefabs : ScriptableObject
{
	[SerializeField] private GameObject _roadPrefab = default;
	public GameObject RoadPrefab => _roadPrefab;
	
	[SerializeField] private GameObject _groundPrefab = default;
	public GameObject GroundPrefab => _groundPrefab;
	
	[SerializeField] private GameObject _spawnPointPrefab = default;
	public GameObject SpawnPointPrefab => _spawnPointPrefab;

	[SerializeField] private GameObject _destinationPrefab = default;
	public GameObject DestinationPrefab => _destinationPrefab;
}