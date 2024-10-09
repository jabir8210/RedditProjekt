namespace Model
{
    public class Post
    {
        public long PostId { get; set; }
        public string Text { get; set; }

        // Initialize Comments list to avoid null references
        public List<Comment> Comments { get; set; } = new List<Comment>();

        public User User { get; set; }
        public DateTime Date { get; set; }

        public int Upvotes { get; set; } = 0;
        public int Downvotes { get; set; } = 0;

        public Post() { }

        public Post(string postText, User user)
        {
            Text = postText;
            User = user;
            Date = DateTime.Now;
            Upvotes = 0;
            Downvotes = 0;
        }
    }
}
