CREATE TABLE IF NOT EXISTS results (
    ResultID INTEGER PRIMARY KEY AUTOINCREMENT,
    UserID INTEGER NOT NULL,
    RoverName TEXT NOT NULL,
    WaterBodies INTEGER NOT NULL,
    TimeElapsed INTEGER NOT NULL,
    TerrainDiscovered REAL NOT NULL -- real is used for floats
);
