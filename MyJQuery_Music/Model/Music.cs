namespace MyJQuery.Model
{
    public class Music
    {
        public int id { get; set; }
        public string name { get; set; }
        public string category { get; set; }
        public string duration { get; set; }
    }
    public class Music2
    {
        public int id { get; set; }
        public string name { get; set; }
        public int category_id { get; set; }
        public string duration { get; set; }
    }
}
