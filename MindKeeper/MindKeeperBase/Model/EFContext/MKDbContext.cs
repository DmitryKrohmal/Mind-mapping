namespace MindKeeperBase.Model.EFContext
{
    using System.Data.Entity;
    public class MKDbContext : DbContext
    {
        public MKDbContext(): base("DbConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Map> Maps { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<ImageAttachment> ImageAttachments { get; set; }
        public DbSet<FileAttachment> FileAttachments { get; set; }
        public DbSet<MediaAttachment> MediaAttachments { get; set; }
    }
}
