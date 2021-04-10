using System.ComponentModel;

namespace LandingControlSystem{
    public enum CellStatus{
        [Description("Out of Platform")]
        OutOfPlatform,
        [Description("Clash")]
        Clash,
        [Description("Ok For Landing")]
        OkForLanding
    }
}