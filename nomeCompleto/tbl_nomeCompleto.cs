namespace nomeCompleto
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class tbl_nomeCompleto
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Nome { get; set; }

        [StringLength(50)]
        public string Nome_completo { get; set; }
    }
}
