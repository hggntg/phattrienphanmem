namespace Base
{
    public interface HeaderRequirement
    {
        string XOrigin { get; set; }
        string XNote { get; set; }
        string XId { get; set; }
    }
    public class HeaderRequirementImp : HeaderRequirement
    {
        public string XOrigin { get; set; }
        public string XNote { get; set; }
        public string XId { get; set; }

        public HeaderRequirementImp(string XOrigin, string XNote, string XId){
            this.XOrigin = XOrigin;
            this.XNote = XNote;
            this.XId = XId;
        }

        private bool Compare(string type, string input)
        {
            if(type == "Origin")
            {
                return this.XOrigin == input;
            }
            else if(type == "Note")
            {
                return this.XNote == input;
            }
            else if(type == "Id")
            {
                return this.XId == input;
            }
            return false;
        }

        public bool IsValid(HeaderRequirement InputHeader)
        {
            return this.Compare("Origin", InputHeader.XOrigin)
                && this.Compare("Note", InputHeader.XNote)
                && this.Compare("Id", InputHeader.XId);
        }
    }
}
