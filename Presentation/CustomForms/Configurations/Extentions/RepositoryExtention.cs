using CustomForms.Application.Repositories.Implementations;
using CustomForms.Application.Repositories.Interfaces;

namespace CustomForms.Configurations.Extentions
{
    internal static class RepositoryExtention
    {
        internal static WebApplicationBuilder ConfigureRepository(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAnswerRepository, AnswerRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            builder.Services.AddScoped<IFormRepository, FormRepository>();
            builder.Services.AddScoped<ILikeRepository, LikeRepository>();
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<ITemplateRepository, TemplateRepository>();
            builder.Services.AddScoped<ITemplateTagRepository, TemplateTagRepository>();
            builder.Services.AddScoped<ITopicRepository, TopicRepository>();
            builder.Services.AddScoped<IOptionRepository, OptionRepository>();

            return builder;
        }
    }
}
