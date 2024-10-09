using Data;
using Microsoft.EntityFrameworkCore;
using Model;
namespace Service
{
    public class Dataservice
    {
        private PostContext db { get; }

        public Dataservice (PostContext db)
        {
            this.db = db;
        }

        public void SeedData()
        {
            db.Database.EnsureCreated();
            if (db.Post.Count() == 0)
            {
                var user1 = new User("Hudayfa");
                var user2 = new User("Mads");
                var user3 = new User("Mikkel");
                var user4 = new User("Morten");
                var user5 = new User("Mads");
                var user6 = new User("Mikkel" );
                var user7 = new User("Morten");
                var user8 = new User("Mads");
                var user9 = new User("Mikkel");
                var user10 = new User("Morten");

                db.Post.AddRange(
                    new Post("hh", user1),
                    new Post("hej", user2),
                    new Post("yo", user3),
                    new Post("hh", user4),
                    new Post("hej", user5),
                    new Post("yo", user6),
                    new Post("hh", user7),
                    new Post("hej", user8),
                    new Post("yo", user9),
                    new Post("hh", user10)
                );

              

                db.SaveChanges();
            }
        }

        public List<Post> GetPosts()
        {
            return db.Post.Include(p => p.User).ToList();
        }


        public Post GetPost(int id)
        {
            return db.Post
                     .Include(p => p.User). Include(Post => Post.Comments).ThenInclude(Comment => Comment.User)  // Include the user who created the post
                     .FirstOrDefault(t => t.PostId == id);
        }

        public Post UpvotePost(int id)
        {
            var post = db.Post.FirstOrDefault(t => t.PostId == id);
            if (post is null)
            {
                return null;
            }

            post.Upvotes++;
            db.SaveChanges();
            return post;
        }

        public Post DownvotePost(int id)
        {
            var post = db.Post.FirstOrDefault(t => t.PostId == id);
            if (post is null)
            {
                return null;
            }

            post.Downvotes++;
            db.SaveChanges();
            return post;
        }

        public Comment UpvoteComment(int commentId, int postId)
        {
            var post = db.Post.Include(p => p.Comments).FirstOrDefault(t => t.PostId == postId);

            if (post == null || post.Comments == null)
            {
                return null;
            }

            // Find the specific comment within the post's comments
            var comment = post.Comments.FirstOrDefault(t => t.CommentId == commentId);

            if (comment == null)
            {
                return null;
            }

            // Increment the upvotes
            comment.Upvotes++;

            db.SaveChanges();
            return comment;
        }

        public Comment DownvoteComment(int commentId, int postId)
        {
            var post = db.Post.Include(p => p.Comments).FirstOrDefault(t => t.PostId == postId);

            if (post == null || post.Comments == null)
            {
                return null;
            }

            // Find the specific comment within the post's comments
            var comment = post.Comments.FirstOrDefault(t => t.CommentId == commentId);

            if (comment == null)
            {
                return null;
            }

            // Increment the downvotes
            comment.Downvotes++;

            db.SaveChanges();
            return comment;
        }



        public Comment CreateComment(Comment comment, int postId)
        {
            var post = db.Post.Include(p => p.Comments).FirstOrDefault(t => t.PostId == postId);
            if (post == null)
            {
                throw new Exception("Post not found");
            }

            if (post.Comments == null)
            {
                post.Comments = new List<Comment>();
            }

            // Add the comment to the post's comments collection
            post.Comments.Add(comment);
            db.SaveChanges(); // Save changes to the database
            return comment;
        }


        public Post CreatePost(Post post)
        {
            if (post == null)
            {
                return null;
            }
            db.Post.Add(post);
            db.SaveChanges();
            return post;
        }

        public User GetUser(int id)
        {
            return db.User.FirstOrDefault(t => t.UserId == id);
        }



    }
}
