namespace ManageMossadAgentsApi.Models
{
    public class ViewAll
    {
        public Target? Target { get; set; }
        public Agent? agent { get; set; }    
        public Mission? mission { get; set; }
        public double? Distance { get; set; }
        public double? TimeOfExecution { get; set; }
    }
}
