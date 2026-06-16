using MudBlazor;
using SoopWorkshop.Frontend.Web.Services;

namespace SoopWorkshop.Frontend.Web.Components.Layout;

public partial class MainLayout : IDisposable
{
    private MudTheme _activeTheme = AppThemes.Dark;

    protected override void OnInitialized()
    {
        UpdateTheme();
        ThemeService.OnThemeChanged += OnThemeChanged;
    }

    private void OnThemeChanged()
    {
        UpdateTheme();
        InvokeAsync(StateHasChanged);
    }

    private void UpdateTheme()
    {
        _activeTheme = ThemeService.CurrentTheme switch
        {
            AppTheme.Light => AppThemes.Light,
            AppTheme.Dark => AppThemes.Dark,
            AppTheme.Oled => AppThemes.Oled,
            _ => AppThemes.Dark
        };
    }

    public void Dispose()
    {
        ThemeService.OnThemeChanged -= OnThemeChanged;
    }
}