namespace PlatformService.DTOs
{
    public record PlatformReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Publisher { get; set; } = "";
        public double Cost { get; set; }
    }    
}