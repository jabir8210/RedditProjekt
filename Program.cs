using Data;
using Microsoft.EntityFrameworkCore;
using Model;
using Service;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Åben op for "CORS" i din API.
// Læs om baggrunden her: https://docs.microsoft.com/en-us/aspnet/core/security/cors?view=aspnetcore-6.0

var AllowCors = "_AllowCors";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowCors, builder => {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});



builder.Services.AddDbContext<PostContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("ContextSQLite"));
});

builder.Services.AddScoped<Dataservice>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<PostContext>();
    context.Database.EnsureCreated();
    var dataService = services.GetRequiredService<Dataservice>();
    dataService.SeedData();
}

app.UseHttpsRedirection();
app.UseCors(AllowCors);

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Type", "application/json");
    await next(context);
});

//Get all posts
app.MapGet("/api/posts", (Dataservice service) => service.GetPosts());

//Get post by id
app.MapGet("/api/posts/{id}", (Dataservice service, int id) =>
{
    var post = service.GetPost(id);
    if (post is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(post);
});


//post upvote
app.MapPost("/api/posts/{id}/upvote", (Dataservice service, int id) =>
{
    // Call the UpvotePost method to increment the upvotes
    service.UpvotePost(id);

    // Return NoContent() since the upvote action is performed
    return Results.NoContent();
});




//post downvote
app.MapPost("/api/posts/{id}/downvote", (Dataservice service, int id) =>
{
    // Call the UpvotePost method to increment the upvotes
    service.DownvotePost(id);

    // Return NoContent() since the upvote action is performed
    return Results.NoContent();
});


//Create a new post
app.MapPost("/api/posts", (Dataservice service, Post post) =>
{
    service.AddPost(post);
    return Results.Created($"/api/posts/{post.PostId}", post);
});


//upvote a comment
app.MapPost("/api/posts/{id}/comments/{commentId}/upvote", (Dataservice service, int id, int commentId) =>
{
    service.UpvoteComment(commentId, id);
    return Results.NoContent();
});


//downvote a comment
app.MapPost("/api/posts/{id}/comments/{commentId}/downvote", (Dataservice service, int id, int commentId) =>
{
    service.DownvoteComment(commentId, id);
    return Results.NoContent();
});


//Create a new comment
app.MapPost("/api/posts/{id}/comments", (Dataservice service, int id, Comment comment) =>
{
    service.AddComment(comment,id);
    return Results.Created($"/api/posts/{id}/comments/{comment.CommentId}", comment);
});



app.Run();



