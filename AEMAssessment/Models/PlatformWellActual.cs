
namespace AEMWebApplication.Models
{
    public class PlatformWellActual
    {
        public int Id { get; set; }
        public string UniqueName { get; set; } = string.Empty;
        public double Latitude { get; set; } = 0.0;
        public double Longitude { get; set; } = 0.0;
        public DateTime CreatedAt { get; set; } =  DateTime.MinValue;
        public DateTime UpdatedAt { get; set; } = DateTime.MinValue;
        public List<Well> Wells { get; set; }


        public class Well
        {
            public int Id { get; set; }
            public string PlatformId { get; set; } = string.Empty;
            public string UniqueName { get; set; } = string.Empty;
            public double Latitude { get; set; } = 0.0;
            public double Longitude { get; set; } = 0.0;
            public DateTime CreatedAt { get; set; } = DateTime.MinValue;
            public DateTime UpdatedAt { get; set; } = DateTime.MinValue;
        }
    }
}
