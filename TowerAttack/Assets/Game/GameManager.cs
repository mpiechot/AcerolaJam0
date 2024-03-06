using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int goldToCollect = 100;

    [SerializeField]
    private NavMeshSurface navMeshSurface;

    [SerializeField]
    private UiController uiController;

    [SerializeField]
    private GameObject[] availableTrapPrefabs;

    [SerializeField]
    private int trapsToSpawn = 1;

    private int collectedGold = 0;

    private Gold[] availableGold;

    private List<PathTile> pathTiles = new List<PathTile>();

    public int MinionsOnField { get; private set; } = 0;

    public int GoldOnField => availableGold.Where(gold => gold != null).Count();

    public IEnumerable<Gold> AvailableGold => availableGold;

    public int CollectedGold => collectedGold;

    public void Start()
    {
        availableGold = FindObjectsOfType<Gold>();
        uiController.Initialize(this);
        uiController.UpdateAvailableGoldText(collectedGold, goldToCollect);

        var randomizedTiles = pathTiles.Where(tile => !tile.IsOccupiedByTrap).OrderBy(tile => Random.value).Take(trapsToSpawn).ToList();

        for (int i = 0; i < trapsToSpawn; i++)
        {
            var randomTile = randomizedTiles[i];
            var trap = Instantiate(availableTrapPrefabs[Random.Range(0, availableTrapPrefabs.Length)], randomTile.SpawnPoint.position, Quaternion.identity);
            randomTile.SetOccupiedByTrap(true);
        }
    }

    public void AddMinion()
    {
        MinionsOnField++;
    }

    public void RemoveMinion()
    {
        MinionsOnField--;
    }

    public void AddGold(int amount)
    {
        collectedGold += amount;
        uiController.UpdateAvailableGoldText(collectedGold, goldToCollect);
    }

    public void UpdateNavMesh()
    {
        navMeshSurface.BuildNavMesh();
    }

    public void AddPathTile(PathTile pathTile)
    {
        pathTiles.Add(pathTile);
    }

    public void SelectTile(PathTile tile)
    {
        uiController.Select(tile);
    }

    public void DeselectTile(PathTile tile)
    {
        uiController.Deselect(tile);
    }

    internal void TryBuildHealingTower(PathTile selectedTile)
    {
        if (selectedTile == null || selectedTile.IsOccupiedByDefence)
        {
            Debug.Log("Selected tile is null or already occupied by defence tower");
            return;
        }

        throw new System.NotImplementedException("Building is not implemented for now");
    }

    internal void TryDestroyTower(PathTile selectedTile)
    {
        throw new System.NotImplementedException();
    }
}
