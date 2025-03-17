import sqlite3

conn = sqlite3.connect('mars_ai_simulator.db')
cursor = conn.cursor()

data = [
    (1, 'Rover1', 75, 120, 48.08),
    (2, 'Rover2', 60, 100, 45.50),
    (3, 'Rover3', 85, 130, 50.25),
    (4, 'Rover4', 55, 90, 42.75),
    (5, 'Rover5', 90, 140, 55.10)
]

cursor.executemany('''
INSERT INTO results (UserID, RoverName, WaterBodies, TimeElapsed, TerrainDiscovered)
VALUES (?, ?, ?, ?, ?);
''', data)

conn.commit()
conn.close()

print("Sample records inserted.")
