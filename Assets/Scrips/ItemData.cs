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

    [Header("ค่าความอิ่ม")]
    [Tooltip("ถ้าเป็นของที่ไม่ใช่ใน Menu จะเพิ่มความอิ่มเท่านี้")]
    public float fullnessAmount = 10f;

    [Header("ระบบเก่า - ไม่ใช้แล้ว")]
    [HideInInspector]
    public float timePenalty = 0f;
}