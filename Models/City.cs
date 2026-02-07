namespace studentmanagementsystem.Models
{
    public class City
    {
        public int Id { get; set; }
        public string CityName { get; set; }

        public int StateId { get; set; }
        public virtual State State { get; set; }
    }

}
