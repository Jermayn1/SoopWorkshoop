using MudBlazor;

namespace SoopWorkshop.Frontend.Web.Services
{
    public static class AppThemes
    {
        public static MudTheme Light => new()
        {
            PaletteLight = new PaletteLight
            {
                Primary = "#1D9E75",
                PrimaryContrastText = "#FFFFFF",
                Secondary = "#378ADD",
                Background = "#F5F5F5",
                Surface = "#FFFFFF",
                AppbarBackground = "#FFFFFF",
                AppbarText = "#1A1A1A",
                DrawerBackground = "#FFFFFF",
                DrawerText = "#1A1A1A",
                TextPrimary = "#1A1A1A",
                TextSecondary = "#616161"
            }
        };

        public static MudTheme Dark => new()
        {
            PaletteDark = new PaletteDark
            {
                Primary = "#1D9E75",
                PrimaryContrastText = "#FFFFFF",
                Secondary = "#378ADD",
                Background = "#1A1A1A",
                Surface = "#242424",
                AppbarBackground = "#242424",
                AppbarText = "#FFFFFF",
                DrawerBackground = "#1E1E1E",
                DrawerText = "#FFFFFF",
                TextPrimary = "#FFFFFF",
                TextSecondary = "#AAAAAA"
            }
        };

        public static MudTheme Oled => new()
        {
            PaletteDark = new PaletteDark
            {
                Primary = "#1D9E75",
                PrimaryContrastText = "#FFFFFF",
                Secondary = "#378ADD",
                Background = "#000000",
                Surface = "#0A0A0A",
                AppbarBackground = "#000000",
                AppbarText = "#FFFFFF",
                DrawerBackground = "#000000",
                DrawerText = "#FFFFFF",
                TextPrimary = "#FFFFFF",
                TextSecondary = "#888888"
            }
        };
    }
}
