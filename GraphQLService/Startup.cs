using DAL;
using GraphQL;
using GraphQL.Types;
using GraphQL.Validation.Complexity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQLService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGraphQl(schema =>
            {
                schema.SetQueryType<NewsQuery>();
                schema.SetMutationType<NewsMutation>();
            });

            services.AddMvc();

            var dbConfig = new MongoConfig();
            Configuration.Bind("MongoConnection", dbConfig);
            services.AddSingleton(dbConfig);

            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<NewsQuery>();
            services.AddSingleton<NewsType>();
            services.AddSingleton<NewsInputType>();
            services.AddSingleton<NewsMutation>();
            services.AddSingleton<INewsRepository, NewsRepository>();

            var sp = services.BuildServiceProvider();
            services.AddSingleton<ISchema>(new NewsSchema(new FuncDependencyResolver(type => sp.GetService(type))));

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseGraphiql("/graphiql", options =>
            {
                options.GraphQlEndpoint = "/graphql";
            });
            app.UseGraphQl("/graphql", options =>
            {
                options.FormatOutput = false;
                options.ComplexityConfiguration = new ComplexityConfiguration { MaxDepth = 15 };
            });

            app.UseMvcWithDefaultRoute();
        }
    }

}
