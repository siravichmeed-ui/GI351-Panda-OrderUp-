
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IngredientSlotUI : MonoBehaviour
{
    [Header("UI")]
    public Image itemIcon;
    public TMP_Text itemNameText;
    public TMP_Text amountText;

    public void Setup(
        ItemData item,
        int collectedAmount,
        int requiredAmount
    )
    {
        if (item == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);

        // รูป Item
        if (itemIcon != null)
        {
            itemIcon.sprite = item.icon;
            itemIcon.enabled = item.icon != null;
        }

        // ชื่อ Item
        if (itemNameText != null)
        {
            itemNameText.text = item.itemName;
        }

        // จำนวน
        if (amountText != null)
        {
            amountText.text =
                collectedAmount +
                "/" +
                requiredAmount;
        }
    }
}
