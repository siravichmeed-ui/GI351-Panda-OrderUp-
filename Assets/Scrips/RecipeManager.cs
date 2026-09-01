using UnityEngine;
using System.Collections.Generic;

public class RecipeManager : MonoBehaviour
{
    [Header("เมนูทั้งหมด")]
    public RecipeData[] recipes;

    [Header("เมนูปัจจุบัน")]
    public RecipeData currentRecipe;

    [Header("Game Manager")]
    public GameManager gameManager;

    [Header("ตั้งค่า")]
    public float nextRecipeDelay = 1f;

    private Dictionary<ItemData, int> collectedItems =
        new Dictionary<ItemData, int>();

    private bool recipeCompleted = false;

    // =========================
    // START
    // =========================

    void Awake()
    {
        SelectRandomRecipe();
    }

    // =========================
    // SELECT RANDOM RECIPE
    // =========================

    void SelectRandomRecipe()
    {
        if (recipes == null || recipes.Length == 0)
        {
            Debug.LogWarning("RecipeManager: ยังไม่มี Recipe");
            return;
        }

        int randomIndex;

        // =========================
        // ป้องกันสุ่มเมนูเดิม
        // =========================

        if (recipes.Length > 1 && currentRecipe != null)
        {
            do
            {
                randomIndex =
                    Random.Range(0, recipes.Length);

            }
            while (recipes[randomIndex] == currentRecipe);
        }
        else
        {
            randomIndex =
                Random.Range(0, recipes.Length);
        }

        // =========================
        // ตั้งค่าเมนูใหม่
        // =========================

        currentRecipe =
            recipes[randomIndex];

        collectedItems.Clear();

        recipeCompleted = false;

        Debug.Log(
            "เมนูใหม่: " +
            currentRecipe.recipeName
        );

        Debug.Log(
            "คะแนนเมนูนี้: " +
            currentRecipe.score
        );

        // =========================
        // แสดงวัตถุดิบที่ต้องใช้
        // =========================

        if (currentRecipe.requiredItems != null)
        {
            foreach (
                RecipeIngredient ingredient
                in currentRecipe.requiredItems
            )
            {
                if (
                    ingredient == null ||
                    ingredient.item == null
                )
                {
                    continue;
                }

                Debug.Log(
                    "ต้องใช้: " +
                    ingredient.item.itemName +
                    " x" +
                    ingredient.amount
                );
            }
        }
    }

    // =========================
    // COLLECT ITEM
    // =========================

    public bool CollectItem(ItemData item)
    {
        if (item == null)
            return false;

        // ถ้ากำลังรอเมนูใหม่
        if (recipeCompleted)
            return false;

        // =========================
        // HAZARD
        // =========================

        if (item.itemType == ItemType.Hazard)
        {
            Debug.Log(
                "เป็นของอันตราย: " +
                item.itemName
            );

            return false;
        }

        // =========================
        // INGREDIENT
        // =========================

        if (item.itemType != ItemType.Ingredient)
        {
            return false;
        }

        // =========================
        // เช็กว่าอยู่ในสูตรหรือไม่
        // =========================

        RecipeIngredient requiredIngredient =
            GetRequiredIngredient(item);

        if (requiredIngredient == null)
        {
            Debug.Log(
                "ไม่ต้องใช้: " +
                item.itemName
            );

            return false;
        }

        // =========================
        // จำนวนที่เก็บแล้ว
        // =========================

        int currentAmount =
            GetCollectedAmount(item);

        // =========================
        // เก็บครบแล้ว
        // =========================

        if (
            currentAmount >=
            requiredIngredient.amount
        )
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

        // =========================
        // เพิ่มจำนวน
        // =========================

        collectedItems[item] =
            currentAmount + 1;

        Debug.Log(
            "เก็บ " +
            item.itemName +
            " (" +
            collectedItems[item] +
            "/" +
            requiredIngredient.amount +
            ")"
        );

        // =========================
        // เช็กสูตร
        // =========================

        CheckComplete();

        return true;
    }

    // =========================
    // FIND REQUIRED INGREDIENT
    // =========================

    RecipeIngredient GetRequiredIngredient(
        ItemData item
    )
    {
        if (currentRecipe == null)
            return null;

        if (currentRecipe.requiredItems == null)
            return null;

        foreach (
            RecipeIngredient ingredient
            in currentRecipe.requiredItems
        )
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

    // =========================
    // CHECK REQUIRED ITEM
    // =========================

    public bool IsRequiredItem(ItemData item)
    {
        return GetRequiredIngredient(item) != null;
    }

    // =========================
    // GET COLLECTED AMOUNT
    // =========================

    public int GetCollectedAmount(ItemData item)
    {
        if (item == null)
            return 0;

        if (
            collectedItems.TryGetValue(
                item,
                out int amount
            )
        )
        {
            return amount;
        }

        return 0;
    }

    // =========================
    // GET REQUIRED AMOUNT
    // =========================

    public int GetRequiredAmount(ItemData item)
    {
        RecipeIngredient ingredient =
            GetRequiredIngredient(item);

        if (ingredient == null)
            return 0;

        return ingredient.amount;
    }

    // =========================
    // CHECK COMPLETE
    // =========================

    void CheckComplete()
    {
        if (currentRecipe == null)
            return;

        if (currentRecipe.requiredItems == null)
            return;

        // =========================
        // เช็กวัตถุดิบทุกตัว
        // =========================

        foreach (
            RecipeIngredient ingredient
            in currentRecipe.requiredItems
        )
        {
            if (
                ingredient == null ||
                ingredient.item == null
            )
            {
                continue;
            }

            int collectedAmount =
                GetCollectedAmount(
                    ingredient.item
                );

            if (
                collectedAmount <
                ingredient.amount
            )
            {
                return;
            }
        }

        // =========================
        // ทำอาหารสำเร็จ
        // =========================

        recipeCompleted = true;

        Debug.Log(
            "ทำ " +
            currentRecipe.recipeName +
            " สำเร็จ!"
        );

        // =========================
        // เพิ่มคะแนน
        // =========================

        if (gameManager != null)
        {
            gameManager.AddScore(
                currentRecipe.score
            );
        }
        else
        {
            Debug.LogWarning(
                "RecipeManager: " +
                "ยังไม่ได้ใส่ GameManager"
            );
        }

        // =========================
        // แสดง +คะแนน
        // =========================

        if (FloatingTextManager.Instance != null)
        {
            FloatingTextManager.Instance.ShowScore(
                currentRecipe.score
            );
        }

        // =========================
        // สุ่มเมนูใหม่
        // =========================

        Invoke(
            nameof(SelectRandomRecipe),
            nextRecipeDelay
        );
    }

    // =========================
    // GET CURRENT RECIPE
    // =========================

    public RecipeData GetCurrentRecipe()
    {
        return currentRecipe;
    }
}