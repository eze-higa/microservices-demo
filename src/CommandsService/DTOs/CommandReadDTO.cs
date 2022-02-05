namespace CommandsService.DTOs
{
    public record CommandReadDTO(int Id, string HowTo, string CommandLine, int PlatformId);
}