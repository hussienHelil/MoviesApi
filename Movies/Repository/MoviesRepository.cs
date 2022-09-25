using Movies.Model;
using Movies.Repository.IRepository;

namespace Movies.Repository
{
    public class MoviesRepository : Repository<Movie>, IMoviesRepository
    {
        public MoviesRepository(ApplicationDbContext db) : base(db)
        {
        }

    }
}
