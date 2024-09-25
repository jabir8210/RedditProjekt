using Model;
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

var app = builder.Build();
app.UseCors(AllowCors);


//List of all posts
//List<Post> posts = new List<Post>();
var user1 = new User("Hudayfa");
//Post post = new Post(postText : "hh", user : user1, upvotes : 0, downvotes : 0);
List<Post> posts = new List<Post>
{
    new Post("hh", user1),
    new Post("hej", user1),
    new Post("yo", user1),

};

//Get all Task
app.MapGet("/api/posts", () => posts);


//app.MapPut("/api/tasks/{id:int}", (int id, Todo updatedTask) =>
//{
//    var task = ToDoList.FirstOrDefault(t => t.Id == id);
//    if (task is null)
//    {
//        return Results.NotFound();
//    }

//    task.Text = updatedTask.Text;
//    task.Done = updatedTask.Done;
//    return Results.NoContent();
//});



////Put functionen til at ændre bool
//app.MapPut("/api/tasks/{id}", (int id, Todo updatedTask) =>
//{
//    var task = ToDoList.FirstOrDefault(t => t.Id == id);
//    if (task is null)
//    {
//        return Results.NotFound();
//    }

//    task.Text = updatedTask.Text;
//    task.Done = updatedTask.Done;
//    return Results.NoContent();

//});



app.MapGet("/", () => "Hello World!");


app.MapGet("/api/hello/{name}", (string name) => new { Message = $"Hello {name}!" });



app.Run();
//record Task(int Id, string Text, bool Done);


