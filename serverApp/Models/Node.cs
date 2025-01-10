namespace serverApp.Models
{
    public class Node
    {
        public string Level { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public List<Node> Children { get; set; }
    }
}
