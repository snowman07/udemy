namespace HotelListing.API.Data
{
    public class Country
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? ShortName { get; set; }

        public virtual IList<Hotel>? Hotels { get; set; }   // ? means not nullable properties OR go to .csproj then replace Nullable=enable to Nullable=disable
    }
}