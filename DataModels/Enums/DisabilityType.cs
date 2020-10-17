
using System.ComponentModel;

namespace DataModels.Responses.Enums
{
    public enum DisabilityType
    {
        [Description("Колясочник")]
        K,
        [Description("Опорник")]
        O,
        [Description("Слепой")]
        S,
        [Description("Глухой")]
        G
    }
}