using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace nomeCompleto
{
    public partial class Context : DbContext
    {
        public Context()
            : base("name=Context")
        {
        }

        public virtual DbSet<tbl_nomeCompleto> tbl_nomeCompleto { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
