using System;

namespace Base
{
    public abstract class BaseModel
    {
        public string note { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime updatedDate { get; set; }

        public BaseModel(string? note, DateTime? createdDate, DateTime? updatedDate)
        {
            this.note = note.Value;
            this.createdDate = createdDate.HasValue ? createdDate.Value : new DateTime();
            this.updatedDate = updatedDate.HasValue ? updatedDate.Value : new DateTime();
        }
    }

}