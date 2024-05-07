from flask import Flask, request, jsonify
import psycopg2

app = Flask(__name__)

db_config = {
    'dbname': 'LoginManager2',
    'user': 'postgres',
    'password': 'Abcd@1309',
    'host': 'localhost'  
}

def validate_user(username, password):
    try:
        conn = psycopg2.connect(**db_config)
        cursor = conn.cursor()
        cursor.execute("SELECT * FROM users WHERE username = %s AND password = %s", (username, password))
        user = cursor.fetchone()
        cursor.close()
        conn.close()
        return user is not None
    except Exception as e:
        print(f"Error: {e}")
        return False

@app.route('/validate-login', methods=['GET'])
def validate_login():
    username = request.args.get('username')
    password = request.args.get('password')

    if username and password:
        is_valid = validate_user(username, password)
        return jsonify({'valid': is_valid})
    else:
        return jsonify({'valid': False})

if __name__ == '__main__':
    app.run(debug=True)
