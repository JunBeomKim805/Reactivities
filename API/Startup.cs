using API.Extensions;
using API.Middleware;
using API.SignalR;
using Application.Activities;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace API
{
  public class Startup
  {
    private readonly IConfiguration _config;
    public Startup(IConfiguration config)
    {
      _config = config;
      // Configuration = configuration;
    }

    // public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    [Obsolete]
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers(opt=>
      {
        var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
        opt.Filters.Add(new AuthorizeFilter(policy));
      })
      .AddFluentValidation(
        config => 
        {
          config.RegisterValidatorsFromAssemblyContaining<Create>();
        });
      services.AddApplicationServices(_config);
      services.AddIdentityService(_config);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseMiddleware<ExceptionMiddleware>();

      app.UseXContentTypeOptions();
      app.UseReferrerPolicy(opt=>opt.NoReferrer());
      app.UseXXssProtection(opt =>opt.EnabledWithBlockMode());
      app.UseXfo(opt=>opt.Deny());
      app.UseCsp(opt=>opt
        .BlockAllMixedContent()
        .StyleSources(s=>s.Self().CustomSources("https://fonts.googleapis.com","https://cdn.jsdelivr.net"))
        .FontSources(s=>s.Self().CustomSources("https://fonts.gstatic.com","data:","https://cdn.jsdelivr.net"))
        .FormActions(s=>s.Self())
        .FrameAncestors(s=>s.Self())
        .ImageSources(s=>s.Self().CustomSources("https://res.cloudinary.com","http://localhost:5000"))
        .ScriptSources(s=>s.Self())
      );

      if (env.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv5 v1"));
      }
      else{
        app.Use(async (context,next)=>{
          context.Response.Headers.Add("Script-Transport-Security","max-age=31536000");
          await next.Invoke();
        });
      }

      // app.UseHttpsRedirection();

      app.UseRouting();

      app.UseDefaultFiles();
      app.UseStaticFiles();

      app.UseCors("CorsPolicy");

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
        endpoints.MapHub<ChatHub>("/chat");
        endpoints.MapFallbackToController("Index","Fallback");
      });
    }
  }
}
