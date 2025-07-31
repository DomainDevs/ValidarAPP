namespace PersonService.Model
{
    public class Phone
    {
        public int Id { get; set; }
        
        public string Description { get; set; }
        
        public string SmallDescription { get; set; }
        
        public int PhoneTypeId { get; set; }
        
        public bool IsPrincipal { get; set; }
        
        public int StreetTypeId { get; set; }
        
        public int CountryCode { get; set; }
        
        public int CityCode { get; set; }
        
        public int Extension { get; set; }
        
        public string ScheduleAvailability { get; set; }
        
        public string UpdateDate { get; set; }
    }
}
