namespace PersonService.Model
{
    public class Address
    {
        
        public int Id { get; set; }
        
        public int AddressTypeId { get; set; }
        
        public string Description { get; set; }
        
        public int CityId { get; set; }
        
        public string CityDescription { get; set; }
        
        public int StateId { get; set; }
        
        public string StateDescription { get; set; }
        
        public int CountryId { get; set; }

        public string CountryDescription { get; set; }
        
        public bool IsPrincipal { get; set; }
        
        public int StreetTypeId { get; set; }
        
        public string UpdateDate { get; set; }
        
        public string TipoDireccion { get; set; }
    }
}
