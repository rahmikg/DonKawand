using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DonKawand.Models.Data
{
    [Table("tblRoles")]
    public class RoleDTO
    {
        //[Key, Column(Order = 0)]
        [Key]
        public int Id { get; set; }
        //[Key, Column(Order = 1)]
        public string Name { get; set; }

        //[ForeignKey("UserId")]
        //public virtual UserDTO User { get; set; }
        //[ForeignKey("RoleId")]
        //public virtual RoleDTO Role { get; set; }
    }
}