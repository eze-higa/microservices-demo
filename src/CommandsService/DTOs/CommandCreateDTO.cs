using System.ComponentModel.DataAnnotations;

namespace CommandsService.DTOs
{
    public record CommandCreateDTO {
        [Required]
        public string HowTo {get;set;} = "";
        [Required]
        public string CommandLine { get; set; } = "";
        
    }
}