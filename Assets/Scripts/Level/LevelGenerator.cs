using System.Linq;
using UnityEngine;
using Zenject;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private Transform _levelLayoutParent = default;
    private Tower.Factory _towerFactory;
    
    private Vector2[,] _grid;
    
    [Inject]
    private void Construct(Tower.Factory towerFactory)
    {
        _towerFactory = towerFactory;
    }
    
    public LevelData CreateLevel(LevelSettings levelSettings)
    {
        Vector2 cellSize = GetCellSize(levelSettings.LayoutPrefabs.GroundPrefab);
        
        GenerateLayout(levelSettings.LevelLayout, cellSize, levelSettings.LayoutPrefabs);
        SpawnTowers(levelSettings.TowersSettings);
        
        return new LevelData
        {
            maxAdditionalEnemiesPerWave = levelSettings.MaxAdditionalEnemiesPerWave,
            secondsBetweenWaves = levelSettings.SecondsBetweenWaves,
            path = CreatePath(levelSettings.EnemyPath, levelSettings.LayoutPrefabs),
            startingGold = levelSettings.StartingGold,
            startingHealth = levelSettings.StartingHealth
        };
    }

    private Vector2 GetCellSize(GameObject groundPrefab)
    {
        Renderer renderer = groundPrefab.GetComponent<Renderer>();
        return new Vector2(renderer.bounds.size.x, renderer.bounds.size.y);
    }
    
    private void GenerateLayout(int[,] layout, Vector2 cellSize, LevelLayoutPrefabs layoutPrefabs)
    {
        int width = layout.GetLength(1);
        int height = layout.GetLength(0);
        
        _grid = new Vector2[height, width];
        
        Vector2 topLeftCorner = new Vector2()
        {
            x = -(width / 2f) + cellSize.x / 2f,
            y = height / 2f - cellSize.y / 2f
        };
        
        for (int yCoord = 0; yCoord < height; yCoord++) 
        {
            for (int xCoord = 0; xCoord < width; xCoord++)
            {
                Vector2 spawnPos = new Vector2
                {
                    x = topLeftCorner.x + xCoord * cellSize.x,
                    y = topLeftCorner.y - yCoord * cellSize.y
                };

                _grid[height - yCoord - 1, xCoord] = spawnPos;
                GameObject prefab = layout[yCoord, xCoord] == 0 ? layoutPrefabs.GroundPrefab : layoutPrefabs.RoadPrefab;
                Instantiate(prefab, spawnPos, Quaternion.identity, _levelLayoutParent);
            }
        }
    }

    private void SpawnTowers(LevelSettings.TowerSettings[] towersSettings)
    {
        foreach (var towerSettings in towersSettings)
        {
            var coordinates = towerSettings.coordinates;
            Vector2 spawnPos = _grid[coordinates.y, coordinates.x];

            Tower newTower = _towerFactory.Create();
            newTower.transform.position = spawnPos;
        }
    }

    private LevelData.Path CreatePath(LevelSettings.Path enemyPathSettings, LevelLayoutPrefabs layoutPrefabs)
    {
        Vector2 spawnPos = _grid[enemyPathSettings.spawnerPosition.y, enemyPathSettings.spawnerPosition.x];
        Vector2[] waypointsPositions = enemyPathSettings.waypoints.Select(waypointCoord => _grid[waypointCoord.y, waypointCoord.x]).ToArray();

        Instantiate(layoutPrefabs.SpawnPointPrefab, spawnPos, Quaternion.identity, _levelLayoutParent);
        Instantiate(layoutPrefabs.DestinationPrefab, waypointsPositions.Last(), Quaternion.identity, _levelLayoutParent);
        
        return new LevelData.Path
        {
            spawnPos = spawnPos,
            waypoints = waypointsPositions 
        };
    }
}