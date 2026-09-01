using UnityEngine;

[System.Serializable]
public class RecipeIngredient
{
    public ItemData item;
    public int amount;
}

[CreateAssetMenu(
    fileName = "NewRecipe",
    menuName = "Recipe/Recipe Data"
)]
public class RecipeData : ScriptableObject
{
    [Header("ชื่อเมนู")]
    public string recipeName;

    [Header("รูปเมนู")]
    public Sprite recipeIcon;

    [Header("คะแนน")]
    public int score = 100;

    [Header("วัตถุดิบที่ต้องใช้")]
    public RecipeIngredient[] requiredItems;
}