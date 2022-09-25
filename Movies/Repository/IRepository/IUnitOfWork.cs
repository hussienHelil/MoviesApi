namespace Movies.Repository.IRepository
{
    public interface IUnitOfWork
    {
        void Save();
        public IGenreRepository Genre { get; }
        public IMoviesRepository Movies{ get; }
    }
}
