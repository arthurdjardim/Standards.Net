namespace Standards.Net.API.Options;

/// <summary>
/// Configuration options for file upload validation.
/// </summary>
public sealed class FileUploadOptions
{
    /// <summary>
    /// Gets or sets the maximum allowed file size in bytes.
    /// Default: 5MB (5 * 1024 * 1024 bytes).
    /// </summary>
    public long MaxFileSizeBytes { get; set; } = 5 * 1024 * 1024;

    /// <summary>
    /// Gets or sets the minimum required file size in bytes.
    /// Default: 0 (no minimum).
    /// </summary>
    public long MinFileSizeBytes { get; set; } = 0;

    /// <summary>
    /// Gets or sets the allowed image file extensions.
    /// Default: .jpg, .jpeg, .png, .webp
    /// </summary>
    public HashSet<string> AllowedImageExtensions { get; set; } = new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" };

    /// <summary>
    /// Gets or sets the allowed document file extensions.
    /// Default: .pdf, .doc, .docx, .xls, .xlsx, .txt
    /// </summary>
    public HashSet<string> AllowedDocumentExtensions { get; set; } =
        new(StringComparer.OrdinalIgnoreCase) { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt" };

    /// <summary>
    /// Gets or sets all allowed file extensions (images + documents + custom).
    /// </summary>
    public HashSet<string> AllowedExtensions { get; set; } = new(StringComparer.OrdinalIgnoreCase);
}
