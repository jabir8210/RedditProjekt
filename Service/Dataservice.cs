using Data;
using Model;
namespace Service
{
    public class Dataservice
    {
        private readonly PostContext _context;

        public Dataservice (PostContext context)
        {
            _context = context;
        }

        public void SeedData()
        {
            _context.Database.EnsureCreated();
            if (_context.Post.Count() == 0)
            {
                var user1 = new User("Hudayfa");
                var user2 = new User("Mads");
                var user3 = new User("Mikkel");
                var user4 = new User("Morten");
                var user5 = new User("Mads");
                var user6 = new User("Mikkel");
                var user7 = new User("Morten");
                var user8 = new User("Mads");
                var user9 = new User("Mikkel");
                var user10 = new User("Morten");

                _context.Post.AddRange(
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

                _context.SaveChanges();
            }
        }


    }
}
