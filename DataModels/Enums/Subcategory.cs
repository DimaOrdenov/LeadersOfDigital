using System.ComponentModel;

namespace DataModels.Responses.Enums
{
    public enum Subcategory
    {
        [Description("Аптека")]
        Pharmacy,

        [Description("Продуктовый магазин")]
        GroceryStore,
    }
}