using System;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    JumpBoost,
}

public enum ConsumableType
{
    Health,
    Hunger,
    JumpBoost,
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
    public float duration;
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]

public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefabs;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmout;

    [Header("Consumable")]
    public ItemDataConsumable[] cosumables;

    [Header("Equip")]
    public GameObject equipPrefab;

    [Header("Jump Boost")]
    public float jumpBoost = 0f;   // 점프 배수 (예: 1.5f)
    public float duration = 0f;    // 지속 시간 (초)
}
