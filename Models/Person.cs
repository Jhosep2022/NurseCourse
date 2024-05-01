using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NurseCourse.Models;

[Table("Person")]
public partial class Person
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("firstName")]
    [StringLength(50)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [Column("lastName")]
    [StringLength(60)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [Column("secondLastName")]
    [StringLength(60)]
    [Unicode(false)]
    public string? SecondLastName { get; set; }

    [Column("email")]
    [StringLength(150)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("phoneNumber")]
    [StringLength(12)]
    [Unicode(false)]
    public string PhoneNumber { get; set; } = null!;

    [Column("ministryCode")]
    [StringLength(12)]
    [Unicode(false)]
    public string MinistryCode { get; set; } = null!;

    //Atributos de auditoria
    [Column("status")]
    public byte Status { get; set; }

    [Column("registerDate", TypeName = "datetime")]
    public DateTime RegisterDate { get; set; }

    [Column("lastUpdate", TypeName = "datetime")]
    public DateTime? LastUpdate { get; set; }

    [Column("registerUser")]
    public int RegisterUser { get; set; }


    [InverseProperty("IdNavigation")]
    public virtual User? User { get; set; }

}

