namespace ASP_Ekz.Model
{
    public class TrainingAttendance
    {
        public int trainingID { get; set; }
        public string trainingType { get; set; }
        public string trainingTime { get; set; }
        public string trainingFormat { get; set; }
        public int maxCapacity { get; set; }
        public int hall { get; set; }
        public string trainer { get; set; }
        public string clientName { get; set; }
    }
}
