namespace studentmanagementsystem.Models
{
    public class State
    {
        public int Id { get; set; }
        public string StateName { get; set; }

        public int CountryId { get; set; }
        public virtual Country Country { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}
