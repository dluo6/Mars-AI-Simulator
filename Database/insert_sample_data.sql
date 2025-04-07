-- Insert sample records into results table
INSERT INTO results (UserID, RoverName, WaterBodies, TimeElapsed, TerrainDiscovered) VALUES
(1, 'Rover1', 75, 120, 48.08),
(2, 'Rover2', 60, 100, 45.50),
(3, 'Rover3', 85, 130, 50.25),
(4, 'Rover4', 55, 90, 42.75),
(5, 'Rover5', 90, 140, 55.10),
(2, 'Rover3', 0, 110, 98.10);

-- to insert sample data in the database, run the following command in the command line (after initializing db first):
-- sqlite3 mars_ai_simulator.db < insert_sample_data.sql

-- NOTE: only run command above after running: sqlite3 mars_ai_simulator.db < init_db.sql