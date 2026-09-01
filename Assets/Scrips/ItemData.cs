using UnityEngine;

public enum ItemType
{
    Ingredient,
    Hazard
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/Item Data")]
public class ItemData : ScriptableObject
{
    [Header("ข้อมูล Item")]
    public string itemName;

    public Sprite icon;

    [Header("ประเภท")]
    public ItemType itemType;

    [Header("เวลาที่หักเมื่อโดน")]
    public float timePenalty = 0f;
}