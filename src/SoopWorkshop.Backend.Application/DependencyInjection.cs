using Microsoft.Extensions.DependencyInjection;
using SoopWorkshop.Backend.Application.Tasks.Interfaces;
using SoopWorkshop.Backend.Application.Tasks.Services;
using SoopWorkshop.Backend.Application.Evaluation.Interfaces;
using SoopWorkshop.Backend.Application.Evaluation.Services;

namespace SoopWorkshop.Backend.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ITaskCategoryService, TaskCategoryService>();
            services.AddScoped<ITaskItemService, TaskItemService>();
            services.AddScoped<IEvaluationService, EvaluationService>();

            return services;
        }
    }
}