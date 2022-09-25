using Microsoft.EntityFrameworkCore;
using Movies.Model;

namespace Movies.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;

        public DbInitializer(ApplicationDbContext db)
        {
            this._db = db;
        }
        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }


            }
            catch
            {

            }
            return;
        }
    }
}
