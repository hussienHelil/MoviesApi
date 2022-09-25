using Movies.Dtos;
using Movies.Model;
using Movies.Repository.IRepository;

namespace Movies.Repository
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        public GenreRepository(ApplicationDbContext db) : base(db)
        {
        }

    }
}
