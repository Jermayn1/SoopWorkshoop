using Microsoft.Extensions.DependencyInjection;
using SoopWorkshop.Frontend.Services.HttpClients;
using SoopWorkshop.Frontend.Services.StateManagment;

namespace SoopWorkshop.Frontend.Services
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddFrontendServices(this IServiceCollection services)
        {
            services.AddHttpClient<TaskApiClient>();
            services.AddHttpClient<SubmissionApiClient>();
            services.AddHttpClient<AdminApiClient>();

            services.AddScoped<SubmissionPollingState>();

            return services;
        }
    }
}