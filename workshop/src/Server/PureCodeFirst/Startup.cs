using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Chat.Server.Messages;
using Chat.Server.People;
using Chat.Server.Users;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Voyager;
using HotChocolate.Types;
using Microsoft.Extensions.Configuration;

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

            services
                .AddRepositories(Configuration)
                .AddInMemorySubscriptions()
                .AddGraphQL(
                    SchemaBuilder.New()
                        .AddQueryType(d => d.Name("Query"))
                        .AddType<PersonQueries>()
                        .AddMutationType(d => d.Name("Mutation"))
                        .AddType<PersonMutations>()
                        .AddType<UserMutations>()
                        .AddType<MessageMutations>()
                        .AddType<PersonExtension>()
                        .AddType<MessageExtension>()
                        .BindClrType<string, StringType>()
                        .BindClrType<Guid, IdType>());
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

            app.UseRouting();

            app.UseAuthentication();

            //app.UseWebSockets();

            app.UseEndpoints(x =>
            x.MapGraphQL());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
