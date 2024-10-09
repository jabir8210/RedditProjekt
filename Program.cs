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

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

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
app.MapPut("/api/posts/{id}/upvote", (Dataservice service, int id) =>
{
    // Call the UpvotePost method to increment the upvotes
    return service.UpvotePost(id);
});




//post downvote
app.MapPut("/api/posts/{id}/downvote", (Dataservice service, int id) =>
{
    // Call the UpvotePost method to increment the upvotes
    return service.DownvotePost(id);
});


//Create a new post with a user
app.MapPost("/api/posts", (Dataservice service, PostDTO postDTO)=>
{
    User user = service.GetUser(postDTO.UserId);
    Post post = new Post(postDTO.Text, user);

    return service.CreatePost(post);
   
});



// Upvote a comment
app.MapPut("/api/posts/{id}/comments/{commentId}/upvote", (Dataservice service, int id, int commentId) =>
{
    var updatedComment = service.UpvoteComment(commentId, id);
    if (updatedComment == null)
    {
        return Results.NotFound("Comment not found");
    }
    return Results.Ok(updatedComment);
});

// Downvote a comment
app.MapPut("/api/posts/{id}/comments/{commentId}/downvote", (Dataservice service, int id, int commentId) =>
{
    var updatedComment = service.DownvoteComment(commentId, id);
    if (updatedComment == null)
    {
        return Results.NotFound("Comment not found");
    }
    return Results.Ok(updatedComment);
});



//Create a new comment
app.MapPost("/api/posts/{id}/comments", (Dataservice service, int id, CommentDTO commentDTO) =>
{
    User user = service.GetUser(commentDTO.userID);

    Post post = service.GetPost(id);
   
    Comment comment = new Comment(commentDTO.Text, user);

    return service.CreateComment(comment, id);
});




//get user by id
app.MapGet("/api/users/{id}", (Dataservice service, int id) =>
{
    var user = service.GetUser(id);
    if (user is null)
    {
        return Results.NotFound();
    }

    return Results.Ok(user);
});


app.Run();

public record PostDTO(string Text, int UserId);
public record CommentDTO(string Text, int userID);

