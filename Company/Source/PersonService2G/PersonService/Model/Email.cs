namespace PersonService.Model
{
    public class Email
    {
        public int Id { get; set; }
        
        public string Description { get; set; }
        
        public string SmallDescription { get; set; }
        
        public int EmailTypeId { get; set; }
        
        public bool IsPrincipal { get; set; }
        
        public string UpdateDate { get; set; }
    }
}