using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Projeto.Avaliacao.API
{
    public class Startup
    {
        private readonly string MyAllowSpecificOrigins = "AnyOriginCORS";
        private readonly IConfiguration _configuration;
        
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddOptions();

            //AutoMapper
            services.AddAutoMapper(typeof(Startup));
            
            services.AddDbContext<Repository.DefaultContext>(options =>
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
            );
            services.AddControllers().AddJsonOptions(options =>
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
                );
            services.AddSwaggerGen(c => AddSwaggerGenConfig(c));
            services.AddMvcCore().AddApiExplorer();
        }

        /// <summary>
        /// Set swagger service configurations.
        /// </summary>
        private static void AddSwaggerGenConfig(SwaggerGenOptions c)
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Projeto.Avaliacao.API", Version = "v1" });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Projeto.Avaliacao.API v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins); 

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
