import sqlite3

conn = sqlite3.connect('mars_ai_simulator.db')
cursor = conn.cursor()

cursor.execute('''
INSERT INTO results (UserID, RoverName, WaterChance, TimeLimit, TerrainDiscovered)
VALUES (1, 'Rover1', 75.5, 120, 'rocky');
''')

conn.commit()
conn.close()

print("Sample record was inserted.")
