from flask import Flask, jsonify
import json
import psycopg2

app = Flask(__name__)

conn = psycopg2.connect(
    dbname="ModelProp",
    user="postgres",
    password="Abcd@1309",
    host="localhost",
    port="5432"
)

@app.route('/cube_data')
def upload_json_to_postgres():
    with open(r'C:\Users\Lenovo\Desktop\ModelProp.json') as f:
        json_data = json.load(f)

    formatted_data = []
    for key, value in json_data.items():
        formatted_data.append({'Name': key, 'Data': json.dumps(value)})  

    cursor = conn.cursor()
    cursor.execute('''CREATE TABLE IF NOT EXISTS cube_data
                      (Name TEXT, Data JSONB)''')  

    for data in formatted_data:
        cursor.execute('INSERT INTO cube_data VALUES (%s, %s)', (data['Name'], data['Data']))

    conn.commit()
    cursor.close()

    cursor = conn.cursor()
    cursor.execute('SELECT * FROM cube_data')
    data = cursor.fetchall()

    cursor.close()

    response = []
    for row in data:
        response.append({'Name': row[0], 'Data': row[1]})

    return jsonify(response)

    

if __name__ == '__main__':
    app.run(debug=True)
