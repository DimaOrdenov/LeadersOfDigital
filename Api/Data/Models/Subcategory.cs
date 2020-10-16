using System.ComponentModel;

namespace Api.Data.Models
{
    public enum Subcategory
    {
        [Description("Аптека")]
        Pharmacy,

        [Description("Продуктовый магазин")]
        GroceryStore,
    }
}