using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Chat.Server.Messages;
using Chat.Server.People;
using Chat.Server.Users;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Execution.Configuration;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using HotChocolate.AspNetCore.Serialization;

namespace Chat.Server
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureAuthenticationServices(services); 
            services.AddCors();

            services.AddDbContext<ChatDbContext>();

            services.AddTransient<IHttpResultSerializer, DefaultHttpResultSerializer>();
            services.AddHttpResultSerializer(
    batchSerialization: HttpResultSerialization.JsonArray,
    deferSerialization: HttpResultSerialization.MultiPartChunked); 
            services.AddHttpResultSerializer<DefaultHttpResultSerializer>();

            services
                .AddGraphQL()
                .AddQueryType<PersonQueries>()
                .AddMutationType<MessageMutations>()
                .AddMutationType<PersonMutations>()
                .AddMutationType<UserMutations>()
                .AddSubscriptionType<MessageSubscriptions>()
                .AddSubscriptionType<PersonSubscriptions>()
                .AddTypeExtension<MessageExtension>()
                .AddTypeExtension<PersonExtension>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(o => o
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin());

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();
            app.UseEndpoints(x => x.MapGraphQL());

            
        }
    }
}
