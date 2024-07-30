namespace Comanda.WebApi.Entities;

public sealed class ProductIngredient : Entity
{
    public int StandardQuantity { get; set; }
    public bool IsMandatory { get; set; }

    public Product Product { get; set; }
    public Ingredient Ingredient { get; set; }

    public ProductIngredient()
    {
        /*
            Default parameterless constructor included due to Entity Framework Core not setting navigation properties
            when using constructors. For more information, see: https://learn.microsoft.com/pt-br/ef/core/modeling/constructors
        */
    }

    public ProductIngredient(Product product, Ingredient ingredient, int standardQuantity)
    {
        Product = product;
        Ingredient = ingredient;
        StandardQuantity = standardQuantity;
    }
}