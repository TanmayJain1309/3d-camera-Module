import psycopg2
import json

DB_NAME = "your_database_name"
DB_USER = "your_username"
DB_PASSWORD = "your_password"
DB_HOST = "your_host"  
DB_PORT = "your_port"  

# Load JSON data
json_file_path = r'C:\Users\Lenovo\Desktop\users.json'
with open(json_file_path) as f:
    data = json.load(f)

# Connect to PostgreSQL
conn = psycopg2.connect(
    database="LoginManager2",
    user="postgres",
    password="Abcd@1309",
    host="localhost",
    port="5432"
)

# Create a cursor object
cur = conn.cursor()

# Create table
cur.execute("""
    CREATE TABLE IF NOT EXISTS users (
        id SERIAL PRIMARY KEY,
        username VARCHAR(50),
        password VARCHAR(50)
    )
""")

# Insert data into the table
for user in data['users']:
    cur.execute("""
        INSERT INTO users (username, password)
        VALUES (%s, %s)
    """, (user['username'], user['password']))

# Commit changes and close cursor and connection
conn.commit()
cur.close()
conn.close()

print("Data loaded successfully!")
