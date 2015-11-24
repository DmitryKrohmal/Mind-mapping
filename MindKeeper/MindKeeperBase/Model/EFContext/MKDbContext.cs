using MindKeeperBase.Model.TopicFactory;

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
        public DbSet<TopicPointer> TopicPointers { get; set; }
        public DbSet<ImageAttachment> ImageAttachments { get; set; }
        public DbSet<FileAttachment> FileAttachments { get; set; }
        public DbSet<MediaAttachment> MediaAttachments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Topic>()
                .HasOptional<TopicPointer>(t => t.Pointer)
                .WithOptionalDependent(tp => tp.Topic)
                .Map(p => p.MapKey("PointerId"));

            modelBuilder.Entity<Topic>()
                .HasOptional(t => t.Parent)
                .WithMany(t => t.ChildTopics)
                .HasForeignKey(t => t.ParentId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
