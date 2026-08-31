using UnityEngine;
using System.Collections.Generic;

public class RecipeManager : MonoBehaviour
{
    [Header("เมนูทั้งหมด")]
    public RecipeData[] recipes;

    [Header("เมนูปัจจุบัน")]
    public RecipeData currentRecipe;

    private List<ItemData> collectedItems = new List<ItemData>();

    void Awake()
    {
        SelectRandomRecipe();
    }

    void SelectRandomRecipe()
    {
        if (recipes == null || recipes.Length == 0)
        {
            Debug.LogWarning("ยังไม่มี Recipe");
            return;
        }

        int randomIndex = Random.Range(0, recipes.Length);

        currentRecipe = recipes[randomIndex];

        collectedItems.Clear();

        Debug.Log("เมนูที่ต้องทำ: " + currentRecipe.recipeName);

        foreach (ItemData item in currentRecipe.requiredItems)
        {
            Debug.Log("ต้องเก็บ: " + item.itemName);
        }
    }

    public bool CollectItem(ItemData item)
    {
        if (item == null)
            return false;

        // ของอันตราย
        if (item.itemType == ItemType.Hazard)
        {
            return false;
        }

        // วัตถุดิบ
        if (item.itemType == ItemType.Ingredient)
        {
            // เช็กว่าอยู่ในสูตรปัจจุบันหรือไม่
            if (IsRequiredItem(item))
            {
                if (!collectedItems.Contains(item))
                {
                    collectedItems.Add(item);

                    Debug.Log("เก็บ: " + item.itemName);

                    CheckComplete();
                }
            }
            else
            {
                // เป็นวัตถุดิบ แต่ไม่ใช้ในเมนูนี้
                Debug.Log("ไม่ต้องใช้: " + item.itemName);
            }

            return true;
        }

        return true;
    }

    bool IsRequiredItem(ItemData item)
    {
        if (currentRecipe == null)
            return false;

        foreach (ItemData requiredItem in currentRecipe.requiredItems)
        {
            if (requiredItem == item)
            {
                return true;
            }
        }

        return false;
    }

    void CheckComplete()
    {
        if (collectedItems.Count >= currentRecipe.requiredItems.Length)
        {
            Debug.Log("เก็บวัตถุดิบครบ!");
            Debug.Log("ทำ " + currentRecipe.recipeName + " สำเร็จ!");
        }
    }

    public List<ItemData> GetCollectedItems()
    {
        return collectedItems;
    }

    public RecipeData GetCurrentRecipe()
    {
        return currentRecipe;
    }
}