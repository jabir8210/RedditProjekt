namespace Model
{
    public class Comment
    {
        public long CommentId { get; set; }
        public string Text { get; set; }

        // Fremmednøgle til Post
        public long PostId { get; set; }

        public User User { get; set; }

        public DateTime Date { get; set; }

        public int Upvotes { get; set; } = 0;
        public int Downvotes { get; set; } = 0;

        public Comment() { }

        public Comment(string text,User user)
        {
            Text = text;
            User = user;
            Date = DateTime.Now;
        }
    }
}
