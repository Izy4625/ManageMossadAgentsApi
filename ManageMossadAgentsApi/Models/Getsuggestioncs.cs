namespace ManageMossadAgentsApi.Models
{
    public class Getsuggestion
    {
        public int MissionId { get; set; } 
        public string? TargetName { get; set; }
        public string? TargetNotes { get; set; }
        public string? Agentlocation { get; set; }
        public string? AgentNickname { get; set; }
        public string? Targetlocation { get; set; }
        public double? TimeLeft { get; set; }
        public double? DistanceOBetween {get; set; }
    }
}
