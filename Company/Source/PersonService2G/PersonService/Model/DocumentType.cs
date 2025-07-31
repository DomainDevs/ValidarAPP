namespace PersonService.Model
{
    public class DocumentType
    {
        public int Id { get; set; }
        
        public string Description { get; set; }
        
        public string SmallDescription { get; set; }
        
        public bool IsAlphanumeric { get; set; }
    }
}
