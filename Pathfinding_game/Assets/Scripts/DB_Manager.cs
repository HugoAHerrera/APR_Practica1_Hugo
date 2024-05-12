using System;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using TMPro;

public class DB_Manager : MonoBehaviour
{
    private string dbUri = "URI=file:partidas.sqlite";
    private string SQL_CREATE_JUGADORES = "CREATE TABLE IF NOT EXISTS Jugadores (" +
                                                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                                                "nombre_jugador TEXT NOT NULL" +
                                            ");";
    
    private string SQL_CREATE_PARTIDAS = "CREATE TABLE IF NOT EXISTS Partidas (" +
                                                "id_partida INTEGER PRIMARY KEY AUTOINCREMENT," +
                                                "id_jugador INTEGER," +
                                                "modo TEXT NOT NULL," +
                                                "tiempo_sobrevivido TEXT NOT NULL," +
                                                "hora_juego TEXT NOT NULL," +
                                                "FOREIGN KEY (id_jugador) REFERENCES Jugadores(id)" +
                                            ");";

    private string SQL_CREATE_INTENTOS = "CREATE TABLE IF NOT EXISTS Intentos (" +
                                                "id_registro INTEGER PRIMARY KEY AUTOINCREMENT," +
                                                "id_jugador INTEGER," +
                                                "victorias INTEGER," +
                                                "derrotas INTEGER," +
                                                "numero_intentos INTEGER NOT NULL," +
                                                "FOREIGN KEY (id_jugador) REFERENCES Jugadores(id)" +
                                            ");";

    public TMP_Dropdown Dropdown;
    public TMP_InputField nombreInputField;

    void Start()
    {
        CrearTablas();
    }

    private void CrearTablas()
    {
        IDbConnection dbConnection = CrearYAbrirBaseDeDatos();
        InicializarDB(dbConnection);
        dbConnection.Close();
    }

    private IDbConnection CrearYAbrirBaseDeDatos()
    {
        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();
        return dbConnection;
    }

    private void InicializarDB(IDbConnection dbConnection)
    {
        IDbCommand dbCmd = dbConnection.CreateCommand();
        dbCmd.CommandText = SQL_CREATE_JUGADORES + SQL_CREATE_PARTIDAS + SQL_CREATE_INTENTOS;
        dbCmd.ExecuteReader();
    }

    public void DescargarMarcador()
    {
        Debug.Log("Descarga de datos");
    }

    public void BorrarHistorialJugador()
    {
        string nombre = nombreInputField.text;

        if (string.IsNullOrEmpty(nombre))
        {
            Debug.Log("Escribe el nombre del jugador que quieres borrar los datos");
            return; 
        }
        using (IDbConnection dbConnection = CrearYAbrirBaseDeDatos())
        {
            int jugadorId = ObtenerIdJugador(nombre, dbConnection);
            IDbCommand deleteCmd = dbConnection.CreateCommand();
            deleteCmd.CommandText = $"DELETE FROM Jugadores WHERE nombre_jugador = '{nombre}';" +
                             $"DELETE FROM Partidas WHERE id_jugador = {jugadorId};" +
                             $"DELETE FROM Intentos WHERE id_jugador = {jugadorId};";
            deleteCmd.ExecuteNonQuery();
        }
        Debug.Log("Registros borrados del jugador " + nombre);
    }

    public void InsertarJugador()
    {
        string nombreJugador = nombreInputField.text;
        if (VerificarJugadorExistente(nombreJugador))
        {
            ActualizarIntentos(nombreJugador);
        }
        else
        {
            RegistrarJugador(nombreJugador);
            RegistrarPrimerIntento(nombreJugador);
        }
    }

    private bool VerificarJugadorExistente(string nombreJugador)
    {
        using (IDbConnection dbConnection = CrearYAbrirBaseDeDatos())
        {
            IDbCommand dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"SELECT COUNT(*) FROM Jugadores WHERE nombre_jugador = '{nombreJugador}'";
            dbCmd.CommandText = sqlQuery;
            IDataReader reader = dbCmd.ExecuteReader();
            reader.Read();
            int count = reader.GetInt32(0);
            return count > 0; //Para que sirva como un booleano
        }
    }

    private void ActualizarIntentos(string nombreJugador)
    {
        using (IDbConnection dbConnection = CrearYAbrirBaseDeDatos())
        {
            int jugadorId = ObtenerIdJugador(nombreJugador, dbConnection);
            string sqlQuery = $"UPDATE Intentos SET numero_intentos = numero_intentos + 1 WHERE id_jugador = {jugadorId}";
            IDbCommand dbCmd = dbConnection.CreateCommand();
            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteNonQuery();
        }
    }

    private int ObtenerIdJugador(string nombreJugador, IDbConnection dbConnection)
    {
        using (IDbConnection dbConn = CrearYAbrirBaseDeDatos())
        {
            IDbCommand dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"SELECT id FROM Jugadores WHERE nombre_jugador = '{nombreJugador}'";
            dbCmd.CommandText = sqlQuery;
            IDataReader reader = dbCmd.ExecuteReader();
            reader.Read();
            return reader.GetInt32(0);
        }
    }

    private void RegistrarJugador(string nombreJugador)
    {
        using (IDbConnection dbConnection = CrearYAbrirBaseDeDatos()){
            IDbCommand dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"INSERT INTO Jugadores (nombre_jugador) VALUES ('{nombreJugador}')";
            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteNonQuery();
        }
    }

    private void RegistrarPrimerIntento(string nombreJugador)
    {
        using (IDbConnection dbConnection = CrearYAbrirBaseDeDatos()){
            int idJugador = ObtenerIdJugador(nombreJugador, dbConnection);
            IDbCommand dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"INSERT INTO Intentos (id_jugador, numero_intentos) VALUES ({idJugador}, 1)";
            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteNonQuery();
        }
    }


}


