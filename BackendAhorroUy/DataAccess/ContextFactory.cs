using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ContextFactory
    {
        public static ContextObl GetMemoryContext(string nameBd)
        {
            var builder = new DbContextOptionsBuilder<ContextObl>();
            return new ContextObl(GetMemoryConfig(builder, nameBd));
        }

        private static DbContextOptions GetMemoryConfig(DbContextOptionsBuilder builder, string nameBd)
        {
            builder.UseInMemoryDatabase(nameBd);
            return builder.Options;
        }
    }
}
