using DAL;
using DTO;
using GraphQL;
using GraphQL.Types;

namespace GraphQLService
{
    public class NewsSchema : Schema
    {
        public NewsSchema(IDependencyResolver resolver) : base(resolver)
        { 
            
            Query = resolver.Resolve<NewsQuery>();
            Mutation = resolver.Resolve<NewsMutation>();
        }
    }

    public class NewsMutation : ObjectGraphType
    {
        public NewsMutation(INewsRepository newsRepository)
        {
            Field<NewsType>(
                "addNews",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<NewsInputType>> { Name = "news" }
                ),
                resolve: context =>
                {
                    var n = context.GetArgument<NewsDTO>("news");
                    return newsRepository.AddNews(n);
                });
        }
    }
    public class NewsQuery : ObjectGraphType
    {
        public NewsQuery(INewsRepository newsRepository)
        {
            Field<ListGraphType<NewsType>>(
                "news",
                resolve: context => newsRepository.GetAllNewsAsync()
            );

            Field<NewsType>(
                "newsById",
                arguments: new QueryArguments(new QueryArgument<StringGraphType> { Name = "id" }),
                resolve: context => newsRepository.GetNewsByIdAsync(context.GetArgument<string>("id"))
            );
        }
    }
    public class NewsType : ObjectGraphType<NewsDTO>
    {
        public NewsType()
        {
            Field(x => x.ID, true);
            Field(x => x.Author, true);
            Field(x => x.Title);
            Field(x => x.Url, true);
            Field(x => x.Description, true);
            Field(x => x.DateOfPublication, true);
        }
    }

    public class NewsInputType : InputObjectGraphType
    {
        public NewsInputType()
        {
            Name = "NewsInput";
            Field<NonNullGraphType<StringGraphType>>("author");
            Field<StringGraphType>("title");
            Field<StringGraphType>("url");
            Field<StringGraphType>("description");
            Field<DateGraphType>("dateOfPublication");
        }
    }


}
