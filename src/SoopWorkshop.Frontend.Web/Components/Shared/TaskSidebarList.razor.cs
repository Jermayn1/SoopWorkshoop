using Microsoft.AspNetCore.Components;
using SoopWorkshop.Frontend.Services.HttpClients;
using SoopWorkshop.Shared.DTOs.Tasks;

namespace SoopWorkshop.Frontend.Web.Components.Shared
{
    public partial class TaskSidebarList : ComponentBase
    {
        [Inject] private TaskApiClient TaskApiClient { get; set; } = default!;

        private List<TaskCategoryDto> _categories = [];
        private bool _isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            _categories = await TaskApiClient.GetVisibleCategoriesAsync();
            _isLoading = false;
        }
    }
}