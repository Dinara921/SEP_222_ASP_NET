namespace ASP_Ekz.Model
{
    public class Client
    {
        public int id { get; set; }
        public string fio { get; set; }
        public DateTime dateBirth { get; set; }
        public string number { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string category { get; set; }
        public string special { get; set; }
        public string time { get; set; }
    }

    public class Client2
    {
        public int id { get; set; }
        public string fio { get; set; }
        public DateTime dateBirth { get; set; }
        public string number { get; set; }
        public string email { get; set; }
        public string gender { get; set; }
        public string login { get; set; }
        public string pwd { get; set; }
        public int category_user_id { get; set; }
    }
}
