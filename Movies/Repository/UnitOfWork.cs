using Movies.Model;
using Movies.Repository.IRepository;

namespace Movies.Repository
{
    public class UnitOfWork : IUnitOfWork
    {

        public IGenreRepository Genre { get; private set;}
        public IMoviesRepository Movies { get; private set; }
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Genre  = new GenreRepository(db);
            Movies = new MoviesRepository(db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
