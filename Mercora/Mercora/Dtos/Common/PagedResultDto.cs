namespace Mercora.Api.Dtos.Common
{
    public class PagedResultDto<T>
    {
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; init; }
        public IReadOnlyList<T> Items { get; init; } = [];
    }
}
