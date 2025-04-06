using SQLite4Unity3d;

[Table("results")]
public class RoverResult
{
    [PrimaryKey, AutoIncrement]
    public int ResultID { get; set; }
    
    public int UserID { get; set; }
    
    public string RoverName { get; set; }
    
    public int WaterBodies { get; set; }
    
    public int TimeElapsed { get; set; }
    
    public float TerrainDiscovered { get; set; }
}
