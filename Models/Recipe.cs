using System.ComponentModel.DataAnnotations;

namespace Spoonful.Models
{
	public class Recipe
	{
		public int Id { get; set; }

		[Required]
		public string? RecipeName { get; set; }

		[Required]
		public string? RecipeDesc { get; set; }

		[Required]
		public string? TimeTaken { get; set; }
		
	}
}
