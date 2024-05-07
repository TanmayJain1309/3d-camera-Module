from flask import Flask, request, jsonify
import psycopg2
import json
from urllib.parse import unquote

app = Flask(__name__)

# PostgreSQL connection settings
DB_NAME = "ModelProp"
DB_USER = "postgres"
DB_PASSWORD = "Abcd@1309"
DB_HOST = "localhost"
DB_PORT = "5432"

def get_db_connection():
    return psycopg2.connect(
        dbname=DB_NAME,
        user=DB_USER,
        password=DB_PASSWORD,
        host=DB_HOST,
        port=DB_PORT
    )


@app.route('/get_model_properties', methods=['GET'])
def get_model_properties():
    model_name = request.args.get('model')
    

    # try:
    connection = get_db_connection()
    cursor = connection.cursor()
    
    print("SELECT data FROM cube_data WHERE name= '" + model_name.replace("\"","") +"'")

    cursor.execute("SELECT data FROM cube_data WHERE name= '" + model_name.replace("\"","") +"'")
    # cursor.execute("SELECT data FROM cube_data WHERE name='Cube'")

    properties = cursor.fetchone()[0]
    print(properties)

    
    cursor.close()
    connection.close()

    if properties:
        return jsonify(properties)
    else:
        return jsonify({}), 404
    # except Exception as e:
    #     return jsonify({"error": str(e)}), 500

    


@app.route('/update_model_property', methods=['POST'])
def update_model_property():
    data = request.get_data()
 
    data = unquote(data.decode('utf-8'))
    data = json.loads(data)

    print(data)

    connection = get_db_connection()
    cursor = connection.cursor()
    for key,value in data.items():
        print(key)
        print(value)
        strvalue = json.dumps(value)
        cursor.execute("UPDATE cube_data SET data = %s WHERE name = %s", (strvalue, key.replace("\"","")))
    connection.commit()

    cursor.close()
    connection.close()

    return jsonify({"message": "Model properties updated successfully."}), 200

if __name__ == '__main__':
    app.run(debug=True)
