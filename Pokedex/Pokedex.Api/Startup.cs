using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Flurl.Http.Configuration;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Pokedex.Api.Filters;
using Pokedex.Api.Validators;
using Pokedex.Services.PokemonService;
using Pokedex.Services.PokemonService.Options;
using Pokedex.Services.TranslationService.Common.Options;
using Pokedex.Services.TranslationService.ShakespeareTranslationService;
using Pokedex.Services.TranslationService.YodaTranslationService;

namespace Pokedex.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ValidationActionFilter));
                options.Filters.Add(typeof(PokemonExceptionActionFilter));
            })
            .AddFluentValidation(c => c.RegisterValidatorsFromAssemblyContaining<Startup>());
            
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Pokedex.Api", Version = "v1"});
            });

            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddOptions();
            
            services.AddSingleton<IFlurlClientFactory, DefaultFlurlClientFactory>();
            
            services.Configure<PokemonServiceOptions>(Configuration.GetSection("PokemonService"));
            services.AddSingleton<IPokemonService, PokemonService>();

            services.Configure<TranslationServiceOptions>(Configuration.GetSection("TranslationService"));
            services.AddSingleton<IShakespeareTranslationService, ShakespeareTranslationService>();
            services.AddSingleton<IYodaTranslationService, YodaTranslationService>();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pokedex.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}