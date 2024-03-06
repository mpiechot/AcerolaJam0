using Assets.Game.Minion;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Minion : MonoBehaviour
{
    [SerializeField]
    private int maxHealth = 100;

    [SerializeField]
    private int maxGoldCarryAmount = 20;

    [SerializeField]
    private Image healthBar;

    [SerializeField]
    private Image goldBar;

    [SerializeField]
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    private float taskExecutionDistance = 1;

    private int health;

    private int goldCarryAmount;

    private MinionTaskType currentTask;

    private PlayerBase playerBase;
    private GameManager gameManager;

    public float TaskExecutionDistance => taskExecutionDistance;

    public void Start()
    {
        health = maxHealth;
        healthBar.fillAmount = 1;
    }

    public void Update()
    {
        if (navMeshAgent.remainingDistance <= taskExecutionDistance)
        {
            DestinationReached();
        }
    }

    public void SetBase(PlayerBase placerBase)
    {
        this.playerBase = placerBase;
    }

    public void SetDestination(Vector3 destination, MinionTaskType taskType)
    {
        navMeshAgent.SetDestination(destination);
        currentTask = taskType;
    }

    /// <summary>
    ///     Called when the minion takes damage.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(int damage)
    {
        health -= damage;
        healthBar.fillAmount = (float)health / maxHealth;

        if (health <= 0)
        {
            Die();
        }
    }

    private void DestinationReached()
    {
        var layerMask = currentTask == MinionTaskType.CollectGold ? LayerMask.GetMask("Gold") : LayerMask.GetMask("Base");

        var results = Physics.OverlapSphere(transform.position, 1, layerMask);

        if (currentTask == MinionTaskType.CollectGold)
        {
            var gold = results.Select(result => result.GetComponent<Gold>()).FirstOrDefault(Gold => Gold != null);

            if (gold == null)
            {
                Debug.Log("Huh?! Where did all the gold go?");
                playerBase.AssignNewTask(this);
                return;
            }

            goldCarryAmount = gold.CollectGold(maxGoldCarryAmount);
            goldBar.fillAmount = (float)goldCarryAmount / maxGoldCarryAmount;
            SetDestination(playerBase.transform.position, MinionTaskType.ReturnToBase);
        }
        else
        {
            playerBase.ReceiveGold(goldCarryAmount);
            goldCarryAmount = 0;
            goldBar.fillAmount = 0;
            playerBase.AssignNewTask(this);
        }
    }

    private void Die()
    {
        gameManager.RemoveMinion();
        Destroy(gameObject);
    }

    public void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }
}
