namespace ASP_Ekz.Model
{
    public class Trainer
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

    public class Trainer2
    {
        public int trainer_id { get; set; }
        public string fio { get; set; }
        public DateTime dateBirth { get; set; }
        public string number { get; set; }
        public string gender { get; set; }
        public int category_id { get; set; }
        public int special_id { get; set; }
        public string email { get; set; }
        public string pwd { get; set; }
        public int role_id { get; set; }
    }
}
