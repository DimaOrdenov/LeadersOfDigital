using System.ComponentModel;

namespace DataModels.Responses.Enums
{
    public enum BarrierType
    {
        [Description("Лестница")]
        Ladder,

        [Description("Бордюр")]
        Border,

        [Description("Яма")]
        Hole,
    }
}