using TruckingTmsMaui.Core.Enums;

namespace TruckingTmsMaui.Core.Entities
{
    // 1.E - Upload Fields
    public class DocumentAttachment
    {
        public int Id { get; set; }
        public int JobId { get; set; }
        public virtual Job? Job { get; set; } // FK to Job

        public DocumentType DocumentType { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string FilePathOrBlobId { get; set; } = string.Empty; // Stub for storage identifier
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
        public int UploadedByUserId { get; set; }
        public virtual User? UploadedByUser { get; set; }
    }
}