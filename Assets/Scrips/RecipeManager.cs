
using UnityEngine;
using System.Collections.Generic;

public class RecipeManager : MonoBehaviour
{
    [Header("เมนูทั้งหมด")]
    public RecipeData[] recipes;

    [Header("เมนูปัจจุบัน")]
    public RecipeData currentRecipe;

    [Header("ตั้งค่า")]
    public float nextRecipeDelay = 1f;

    private Dictionary<ItemData, int> collectedItems =
        new Dictionary<ItemData, int>();

    private bool recipeCompleted = false;

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

        recipeCompleted = false;

        Debug.Log("เมนูใหม่: " + currentRecipe.recipeName);

        foreach (RecipeIngredient ingredient in currentRecipe.requiredItems)
        {
            if (ingredient == null || ingredient.item == null)
                continue;

            Debug.Log(
                "ต้องใช้: " +
                ingredient.item.itemName +
                " x" +
                ingredient.amount
            );
        }
    }

    public bool CollectItem(ItemData item)
    {
        if (item == null)
            return false;

        // ถ้ากำลังรอเมนูใหม่
        if (recipeCompleted)
            return false;

        // Hazard
        if (item.itemType == ItemType.Hazard)
        {
            Debug.Log("เป็นของอันตราย: " + item.itemName);
            return false;
        }

        // ต้องเป็น Ingredient
        if (item.itemType != ItemType.Ingredient)
        {
            return false;
        }

        // เช็กว่า Item อยู่ในสูตรหรือไม่
        RecipeIngredient requiredIngredient =
            GetRequiredIngredient(item);

        if (requiredIngredient == null)
        {
            Debug.Log("ไม่ต้องใช้: " + item.itemName);
            return false;
        }

        // จำนวนที่เก็บแล้ว
        int currentAmount = GetCollectedAmount(item);

        // เก็บครบแล้ว
        if (currentAmount >= requiredIngredient.amount)
        {
            Debug.Log(
                item.itemName +
                " ครบแล้ว (" +
                currentAmount +
                "/" +
                requiredIngredient.amount +
                ")"
            );

            return false;
        }

        // เพิ่มจำนวน
        collectedItems[item] = currentAmount + 1;

        Debug.Log(
            "เก็บ " +
            item.itemName +
            " (" +
            collectedItems[item] +
            "/" +
            requiredIngredient.amount +
            ")"
        );

        // เช็กว่าครบสูตรหรือยัง
        CheckComplete();

        return true;
    }

    RecipeIngredient GetRequiredIngredient(ItemData item)
    {
        if (currentRecipe == null)
            return null;

        if (currentRecipe.requiredItems == null)
            return null;

        foreach (RecipeIngredient ingredient in currentRecipe.requiredItems)
        {
            if (ingredient == null)
                continue;

            if (ingredient.item == item)
            {
                return ingredient;
            }
        }

        return null;
    }

    public bool IsRequiredItem(ItemData item)
    {
        return GetRequiredIngredient(item) != null;
    }

    public int GetCollectedAmount(ItemData item)
    {
        if (item == null)
            return 0;

        if (collectedItems.TryGetValue(item, out int amount))
        {
            return amount;
        }

        return 0;
    }

    public int GetRequiredAmount(ItemData item)
    {
        RecipeIngredient ingredient =
            GetRequiredIngredient(item);

        if (ingredient == null)
            return 0;

        return ingredient.amount;
    }

    void CheckComplete()
    {
        if (currentRecipe == null)
            return;

        if (currentRecipe.requiredItems == null)
            return;

        // เช็กทุกวัตถุดิบ
        foreach (RecipeIngredient ingredient in currentRecipe.requiredItems)
        {
            if (ingredient == null || ingredient.item == null)
                continue;

            int collectedAmount =
                GetCollectedAmount(ingredient.item);

            if (collectedAmount < ingredient.amount)
            {
                return;
            }
        }

        // ทำสำเร็จแล้ว
        recipeCompleted = true;

        Debug.Log(
            "ทำ " +
            currentRecipe.recipeName +
            " สำเร็จ!"
        );

        // รอแล้วสุ่มเมนูใหม่
        Invoke(nameof(SelectRandomRecipe), nextRecipeDelay);
    }

    public RecipeData GetCurrentRecipe()
    {
        return currentRecipe;
    }
}

