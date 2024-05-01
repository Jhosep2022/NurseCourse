using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NurseCourse.Models;

[Table("User")]
public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("userName")]
    [StringLength(15)]
    [Unicode(false)]
    public string UserName { get; set; } = null!;

    [Column("password")]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [Column("role")]
    [StringLength(30)]
    [Unicode(false)]
    public string Role { get; set; } = null!;

    [Column("codeRecovery")]
    [Unicode(false)]
    public string? codeRecovery { get; set; }

    [ForeignKey("Id")]
    [InverseProperty("User")]
    public virtual Person? IdNavigation { get; set; } = null!;
}
