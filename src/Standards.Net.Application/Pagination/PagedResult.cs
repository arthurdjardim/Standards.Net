namespace Standards.Net.Application.Pagination;

/// <summary>
/// Represents a paginated collection of items with metadata.
/// Used for API responses that support pagination.
/// </summary>
/// <typeparam name="T">The type of items in the collection.</typeparam>
public sealed class PagedResult<T>
{
    /// <summary>
    /// Gets or sets the collection of items for the current page.
    /// </summary>
    public IReadOnlyCollection<T> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the current page number (1-indexed).
    /// </summary>
    public int PageNumber { get; set; }

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets or sets the total number of items across all pages.
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets the total number of pages.
    /// </summary>
    public int TotalPages => PageSize > 0 ? (int)Math.Ceiling(TotalCount / (double)PageSize) : 0;

    /// <summary>
    /// Gets whether there is a previous page.
    /// </summary>
    public bool HasPreviousPage => PageNumber > 1;

    /// <summary>
    /// Gets whether there is a next page.
    /// </summary>
    public bool HasNextPage => PageNumber < TotalPages;

    /// <summary>
    /// Creates a new paged result.
    /// </summary>
    /// <param name="items">The items for the current page.</param>
    /// <param name="pageNumber">The current page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <param name="totalCount">The total count of items.</param>
    /// <returns>A new paged result.</returns>
    public static PagedResult<T> Create(
        IReadOnlyCollection<T> items,
        int pageNumber,
        int pageSize,
        int totalCount
    ) =>
        new()
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
        };

    /// <summary>
    /// Creates an empty paged result.
    /// </summary>
    /// <param name="pageNumber">The current page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>An empty paged result.</returns>
    public static PagedResult<T> Empty(int pageNumber = 1, int pageSize = 10) =>
        new()
        {
            Items = [],
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = 0,
        };
}
