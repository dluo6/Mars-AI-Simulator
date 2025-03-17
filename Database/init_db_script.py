import sqlite3

# Connect to (or create) the SQLite database
conn = sqlite3.connect('mars_ai_simulator.db')
cursor = conn.cursor()

# Create the "results" table as specified in the document
cursor.execute('''
CREATE TABLE IF NOT EXISTS results (
    ResultID INTEGER PRIMARY KEY AUTOINCREMENT,
    UserID INTEGER NOT NULL,
    RoverName TEXT NOT NULL,
    WaterChance REAL NOT NULL,
    TimeLimit INTEGER NOT NULL,
    TerrainDiscovered TEXT NOT NULL
);
''')

# Commit the changes and close the connection
conn.commit()
conn.close()

print("Database initialized and 'results' table was created.")
