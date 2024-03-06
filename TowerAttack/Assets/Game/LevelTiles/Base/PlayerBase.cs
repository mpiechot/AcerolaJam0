using Assets.Game.Minion;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [SerializeField]
    private Minion minionPrefab;

    [SerializeField]
    private int minionsToSpawn = 3;

    [SerializeField]
    private float minionSpawnInterval = 2;

    private int minionsOffField = 0;

    private GameManager gameManager;

    private Coroutine spawnMinionsCoroutine;

    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        minionsOffField = minionsToSpawn;
    }

    private void Update()
    {
        if (minionsOffField > 0 && gameManager.GoldOnField > 0 && spawnMinionsCoroutine == null)
        {
            spawnMinionsCoroutine = StartCoroutine(SpawnMinions());
        }
    }

    private IEnumerator SpawnMinions()
    {
        for (int i = 0; i < minionsOffField; i++)
        {
            Minion minion = Instantiate(minionPrefab, transform.position, Quaternion.identity);
            minion.Initialize(gameManager);
            minion.SetBase(this);
            AssignNewTask(minion);
            gameManager.AddMinion();
            minionsOffField--;
            yield return new WaitForSeconds(minionSpawnInterval);
        }
        Debug.Log("All minions are on the field now");
        spawnMinionsCoroutine = null;
    }

    public void ReceiveGold(int amount)
    {
        gameManager.AddGold(amount);
    }

    public void AssignNewTask(Minion minion)
    {
        var goldToCollect = gameManager.AvailableGold.Where(availableGold => availableGold != null).FirstOrDefault();

        if (goldToCollect == null)
        {
            if (Vector3.Distance(minion.transform.position, transform.position) > minion.TaskExecutionDistance)
            {
                Debug.Log("This minion is too far from the base, let's send it back");
                minion.SetDestination(transform.position, MinionTaskType.ReturnToBase);
                return;
            }

            Debug.Log("There is no more known gold around here, just relax in the base then...");
            Destroy(minion.gameObject);
            gameManager.RemoveMinion();
            minionsOffField++;
            return;
        }


        minion.SetDestination(goldToCollect.transform.position, MinionTaskType.CollectGold);
    }
}
