namespace TenisRanking.Job
{
    public class MatchDaysLimitOptions
    {
        public bool Enabled { get; set; }

        public int Days { get; set; }

        public int LevelDrop { get; set; }

        public int RankDrop { get; set; }
    }
}
