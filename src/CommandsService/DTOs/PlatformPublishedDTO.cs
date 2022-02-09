namespace CommandsService.DTOs
{
    public record PlatformPublishedDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Event { get; set; } = default!;
    }
}