using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text availableGoldLabel;

    [SerializeField]
    private Image selectionFieldImage;

    [SerializeField]
    private Button buildHealingTowerButton;

    [SerializeField]
    private Button destroyTowerButton;

    [SerializeField]
    private List<(FieldType FieldType, Sprite Image)> fieldTypes;

    private PathTile selectedTile;

    private GameManager gameManager;

    public void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void UpdateAvailableGoldText(int collectedGold, int goldToCollect)
    {
        availableGoldLabel.text = $"{collectedGold} / {goldToCollect}";
    }

    public void BuildHealingTower()
    {
        gameManager.TryBuildHealingTower(selectedTile);
    }

    public void DestroyTower()
    {
        gameManager.TryDestroyTower(selectedTile);
    }

    public void Deselect(PathTile tile)
    {
        if (selectedTile == tile)
        {
            selectedTile = null;
            selectionFieldImage.sprite = fieldTypes.Find(type => type.FieldType == FieldType.Empty).Image;
            destroyTowerButton.enabled = false;
            buildHealingTowerButton.enabled = false;
        }
    }

    public void Select(PathTile tile)
    {
        selectedTile = tile;
        if (tile.IsOccupiedByDefence && tile.IsOccupiedByTrap)
        {
            selectionFieldImage.sprite = fieldTypes.Find(type => type.FieldType == FieldType.TrapWithTower).Image;
            destroyTowerButton.enabled = true;
            buildHealingTowerButton.enabled = false;
        }
        else if (tile.IsOccupiedByDefence)
        {
            selectionFieldImage.sprite = fieldTypes.Find(type => type.FieldType == FieldType.HealingTower).Image;
            destroyTowerButton.enabled = true;
            buildHealingTowerButton.enabled = false;
        }
        else if (tile.IsOccupiedByTrap)
        {
            selectionFieldImage.sprite = fieldTypes.Find(type => type.FieldType == FieldType.Trap).Image;
            destroyTowerButton.enabled = false;
            buildHealingTowerButton.enabled = true;
        }
        else
        {
            selectionFieldImage.sprite = fieldTypes.Find(type => type.FieldType == FieldType.Empty).Image;
            destroyTowerButton.enabled = false;
            buildHealingTowerButton.enabled = true;
        }
    }

    [Serializable]
    private enum FieldType
    {
        Empty,
        HealingTower,
        Trap,
        TrapWithTower,
    }

}
