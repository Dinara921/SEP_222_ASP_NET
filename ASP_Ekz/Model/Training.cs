namespace ASP_Ekz.Model
{
    public class Training
    {
        public int id { get; set; }
        public string trainer { get; set; }
        public string hall { get; set; }
        public string special { get; set; }
        public string time { get; set; }
        public string status { get; set; }
        public int quantity { get; set; }
    }

    public class Training2
    {
        public int id { get; set; }
        public int trainer_id { get; set; }
        public int timeT_id { get; set; }
        public int status_id { get; set; }
        public int hall_id { get; set; }
        public int max_capacity { get; set; }
    }

}
