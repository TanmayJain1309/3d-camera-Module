import json
import psycopg2

with open(r'C:\Users\Lenovo\Desktop\ModelProp.json') as f:
    json_data = json.load(f)

formatted_data = []
for key, value in json_data.items():
    formatted_data.append({'Name': key, 'Data': json.dumps(value)})  

conn = psycopg2.connect(
    dbname="ModelProp",
    user="postgres",
    password="Abcd@1309",
    host="localhost",
    port="5432"
)

cursor = conn.cursor()


cursor.execute('''CREATE TABLE IF NOT EXISTS cube_data
                  (Name TEXT, Data JSONB)''')  

for data in formatted_data:
    cursor.execute('INSERT INTO cube_data VALUES (%s, %s)', (data['Name'], data['Data']))

conn.commit()
conn.close()