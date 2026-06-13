namespace GlitchGame.Shared.Dto;

public record MapUpdate
{
    public Districts District { get; init; }
    public int NewPoints { get; init; }
}
