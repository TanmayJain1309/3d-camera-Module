import json
import psycopg2

# PostgreSQL database connection settings
DB_NAME = 'newmodelprop'
DB_USER = 'postgres'
DB_PASSWORD = 'Abcd@1309'
DB_HOST = 'localhost'
DB_PORT = '5432'

# Load JSON file
with open(r'C:\Users\Lenovo\Desktop\ModelProp.json') as json_file:
    data = json.load(json_file)

# Connect to PostgreSQL database
conn = psycopg2.connect(
    dbname=DB_NAME,
    user=DB_USER,
    password=DB_PASSWORD,
    host=DB_HOST,
    port=DB_PORT
)
cur = conn.cursor()

cur.execute("""
    CREATE TABLE IF NOT EXISTS model_properties (
        model_name VARCHAR(255) PRIMARY KEY,
        property_name VARCHAR(255),
        property_value VARCHAR(255)
    )
""")

# Insert data into PostgreSQL table
for model_name, properties in data.items():
    for property_name, property_value in properties.items():
        cur.execute("INSERT INTO model_properties (model_name, property_name, property_value) VALUES (%s, %s, %s)", (model_name, property_name, property_value))

# Commit changes and close connection
conn.commit()
cur.close()
conn.close()

print("Data loaded into PostgreSQL successfully!")
