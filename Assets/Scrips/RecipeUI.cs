using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUI : MonoBehaviour
{
    [Header("Recipe Manager")]
    public RecipeManager recipeManager;

    [Header("Recipe Header")]
    public Image recipeIcon;
    public TMP_Text recipeNameText;

    [Header("Ingredient List")]
    public Transform ingredientContainer;
    public IngredientSlotUI ingredientSlotPrefab;

    private List<IngredientSlotUI> ingredientSlots =
        new List<IngredientSlotUI>();

    private RecipeData lastRecipe;

    void Start()
    {
        UpdateRecipeUI();
    }

    void Update()
    {
        RecipeData currentRecipe = recipeManager.GetCurrentRecipe();

        if (currentRecipe != lastRecipe)
        {
            UpdateRecipeUI();
        }
        else
        {
            UpdateIngredientAmounts();
        }
    }

    public void UpdateRecipeUI()
    {
        if (recipeManager == null)
        {
            Debug.LogWarning("RecipeUI: ยังไม่ได้ใส่ RecipeManager");
            return;
        }

        RecipeData recipe = recipeManager.GetCurrentRecipe();

        if (recipe == null)
        {
            Debug.LogWarning("RecipeUI: ไม่มี Recipe ปัจจุบัน");
            return;
        }

        lastRecipe = recipe;

        // =========================
        // Recipe Name
        // =========================

        if (recipeNameText != null)
        {
            recipeNameText.text = recipe.recipeName;
        }

        // =========================
        // Recipe Icon
        // =========================

        if (recipeIcon != null)
        {
            recipeIcon.sprite = recipe.recipeIcon;
            recipeIcon.enabled = recipe.recipeIcon != null;
        }

        // =========================
        // สร้าง Ingredient Slots
        // =========================

        ClearIngredientSlots();

        if (recipe.requiredItems == null)
            return;

        foreach (RecipeIngredient ingredient in recipe.requiredItems)
        {
            if (ingredient == null || ingredient.item == null)
                continue;

            IngredientSlotUI slot =
                Instantiate(
                    ingredientSlotPrefab,
                    ingredientContainer
                );

            int collectedAmount =
                recipeManager.GetCollectedAmount(ingredient.item);

            slot.Setup(
                ingredient.item,
                collectedAmount,
                ingredient.amount
            );

            ingredientSlots.Add(slot);
        }
    }

    void UpdateIngredientAmounts()
    {
        if (lastRecipe == null)
            return;

        for (int i = 0; i < ingredientSlots.Count; i++)
        {
            if (i >= lastRecipe.requiredItems.Length)
                break;

            RecipeIngredient ingredient =
                lastRecipe.requiredItems[i];

            if (ingredient == null || ingredient.item == null)
                continue;

            int collectedAmount =
                recipeManager.GetCollectedAmount(
                    ingredient.item
                );

            ingredientSlots[i].Setup(
                ingredient.item,
                collectedAmount,
                ingredient.amount
            );
        }
    }

    void ClearIngredientSlots()
    {
        foreach (IngredientSlotUI slot in ingredientSlots)
        {
            if (slot != null)
            {
                Destroy(slot.gameObject);
            }
        }

        ingredientSlots.Clear();
    }
}
