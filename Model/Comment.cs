using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class Comment
    {
        public long CommentId { get; set; }
        public string Text { get; set; }

        User User { get; set; }

        DateTime Date { get; set; }

        public int Upvotes { get; set; } = 0;
        public int Downvotes { get; set; } = 0;

        public Comment() { }
        public Comment(string text, User user, DateTime date)
        {
            Text = text;
            User = user;
            Date = DateTime.Now;
        }
    }
}
