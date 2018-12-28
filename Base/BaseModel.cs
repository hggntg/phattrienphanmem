using System;

namespace Base
{
    public class BaseModel
    {
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public BaseModel(DateTime? CreatedDate, DateTime? UpdatedDate)
        {
            this.CreatedDate = this.generateDate(CreatedDate);
            this.UpdatedDate = this.generateDate(UpdatedDate);
            this.Note = "";
        }

        public BaseModel(string Note, DateTime? CreatedDate, DateTime? UpdatedDate)
        {
            this.Note = Note;
            this.CreatedDate = this.generateDate(CreatedDate);
            this.UpdatedDate = this.generateDate(UpdatedDate);
        }

        public BaseModel()
        {
            this.CreatedDate = this.generateDate(null);
            this.UpdatedDate = this.generateDate(null);
            this.Note = "";
        }

        private DateTime generateDate(DateTime? DateTimeInput)
        {
            return DateTimeInput.HasValue ? DateTimeInput.Value : DateTime.Now;
        }

    }

}