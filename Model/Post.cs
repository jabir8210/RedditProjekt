namespace Model
{
    public class Post
    {
        long PostId { get; set; }
        string Text { get; set; }

        List<Comment> Comments { get; set; }

        User User { get; set; }

        DateTime Date { get; set; }

        public int Upvotes { get; set; }  = 0;
        public int Downvotes { get; set; } = 0;

  
        public Post() { }
        public Post(string text, User user, DateTime date, int upvotes, int downvotes)
        {
            Text = text;
            User = user;
            Date = date;
            Upvotes = upvotes;
            Downvotes = downvotes;
        }
    }
}
