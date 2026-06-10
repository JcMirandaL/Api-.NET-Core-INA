using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using InaApp.Common.Enums;

namespace InaApp.Entities
{
    [Table(name:"tbClientes")]
    [Index(nameof(Cedula), nameof(TipoCedula), IsUnique = true)] 
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Cedula { get; set; } = string.Empty; 

        [Required]
        public TipoCedulaEnum TipoCedula { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Apellido1 { get; set; } = string.Empty;

        //el ? es para q permita null
        [StringLength(50, MinimumLength = 3)]
        public string? Apellido2 { get; set; } = string.Empty;

        [EmailAddress]
        [MaxLength(150)]
        public string? Correo { get; set; } = string.Empty;

        [Phone]
        [MaxLength(20)]
        public string? Telefono { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateOnly FechaNacimiento { get; set; }

        [Required]
        public bool Estado { get; set; } = true;

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Required]
        public DateTime FechaModificacion { get; set; } = DateTime.Now;

        //admin quemado x mientras se hace el login para sacr el usuario logueado
        [Required]
        public string UsuarioCreacion { get; set; } = "admin";

    }
}
