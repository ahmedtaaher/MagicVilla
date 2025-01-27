﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_WebAPI.Models
{
	public class VillaNumber
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int VillaNo { get; set; }
		public string SpecialDetails { get; set; }
		public DateTime CreatedDate { get; set; }
		public DateTime UpdatedDate { get; set; }
		[ForeignKey("Villa")]
		public int VillaId { get; set; }
		public Villa Villa { get; set; }
	}
}
