using System;
using System.ComponentModel.DataAnnotations;

namespace EnciclopediaDarwin.Data.Entidades

{
    [MetadataType(typeof(EspecieModelMetaData))]
    public partial class Especie
    {
        public Especie()
        {
        }
    }

    public class EspecieModelMetaData
    {
        [Required]
        public string Nombre { get; set; } = null;

        [Range(1,1000, ErrorMessage = "Límite de peso")]
        public decimal? PesoPromedioKg { get; set; }

        public int? EdadPromedioAños { get; set; }
    }
}

