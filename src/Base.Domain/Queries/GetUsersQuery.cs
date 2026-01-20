using Base.Domain.Common.Pagination;

namespace Base.Domain.Queries
{
    public class GetUsersQuery : PagedQuery
    {
        public string? Search { get; init; }
        public bool? IsActive { get; init; }
    }

}
