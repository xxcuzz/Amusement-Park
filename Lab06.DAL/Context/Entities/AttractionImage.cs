namespace Lab06.DAL.Entities
{
    public class AttractionImage
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public byte[] Payload { get; set; }

        public int AttractionId { get; set; }

        public virtual ParkAttraction ParkAttraction { get; set; }
    }
}
