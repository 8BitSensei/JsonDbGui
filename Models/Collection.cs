namespace JsonDbGui.Models
{
    public class Collection
    {
        public string CollectionName { get; set; }
        public string CollectionDescription { get; set; }

        public CollectionType CollectionType { get; set; }
    }

    public enum CollectionType 
    {
        Local = 0,
        Github = 1,
        S3 = 2
    }
}
