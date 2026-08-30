using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "Recipe/Recipe Data")]
public class RecipeData : ScriptableObject
{
    [Header("ชื่อเมนู")]
    public string recipeName;

    [Header("รูปเมนู")]
    public Sprite recipeIcon;

    [Header("วัตถุดิบที่ต้องใช้")]
    public ItemData[] requiredItems;
}