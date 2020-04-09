namespace Geen.Core.Models.Mentions
{
    public class EntityInfo
    {
        public int Id { get; set; }

        public string Url { get; set; }

        public static EntityInfo Empty => new EntityInfo();
    }
}
