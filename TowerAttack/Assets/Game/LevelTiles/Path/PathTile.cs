using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class PathTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private new Renderer renderer;

    [SerializeField]
    private float tweenDuration = 1;

    [SerializeField]
    private Color hoverColor;

    private Color defaultColor;

    private bool isOccupiedByTrap = false;

    private bool isOccupiedByDefence = false;

    private GameManager gameManager;

    public bool IsOccupiedByTrap => isOccupiedByTrap;

    public bool IsOccupiedByDefence => isOccupiedByDefence;

    public bool IsOccupied => isOccupiedByTrap || isOccupiedByDefence;

    public Transform SpawnPoint => spawnPoint;

    public void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.AddPathTile(this);

    }

    public void Start()
    {
        defaultColor = renderer.material.color;
    }

    public void SetOccupiedByTrap(bool isOccupied)
    {
        isOccupiedByTrap = isOccupied;
    }

    public void SetOccupiedByDefence(bool isOccupied)
    {
        isOccupiedByDefence = isOccupied;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Pointer exited");
        renderer.material.DOColor(defaultColor, tweenDuration);
        gameManager.DeselectTile(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Pointer entered");
        renderer.material.DOColor(hoverColor, tweenDuration);
        gameManager.SelectTile(this);
    }
}
