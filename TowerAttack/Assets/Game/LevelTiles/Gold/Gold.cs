using UnityEngine;
using UnityEngine.UI;

public class Gold : MonoBehaviour
{
    [SerializeField]
    private int goldAmount = 100;

    [SerializeField]
    private Image goldAmountVisualizer;

    private GameManager gameManager;

    public void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    public int CollectGold(int collectAmount)
    {
        var givenAmount = Mathf.Min(collectAmount, goldAmount);

        goldAmount -= givenAmount;
        goldAmountVisualizer.fillAmount = (float)goldAmount / 100;

        if (goldAmount == 0)
        {
            DestroyImmediate(gameObject);
            gameManager.UpdateNavMesh();
        }

        return givenAmount;
    }
}
