using System;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;

public class GameDB_Manager : MonoBehaviour
{
    private string dbUri = "URI=file:partidas.sqlite";
    private string nombreJugador;

    void Start()
    {
        nombreJugador = PlayerPrefs.GetString("NombreJugador", "");
    }

    private IDbConnection CrearYAbrirBaseDeDatos()
    {
        IDbConnection dbConnection = new SqliteConnection(dbUri);
        dbConnection.Open();
        return dbConnection;
    }

    public void InsertarDerrota()
    {
        using (IDbConnection dbConnection = CrearYAbrirBaseDeDatos())
        {
            int idJugador = ObtenerIdJugador(dbConnection);
            IDbCommand dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"UPDATE Intentos SET derrotas = derrotas + 1 WHERE id_jugador = {idJugador}";
            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteNonQuery();
            GuardarPartida();
        }
    }

    public void InsertarVictoria()
    {
        using (IDbConnection dbConnection = CrearYAbrirBaseDeDatos())
        {
            int idJugador = ObtenerIdJugador(dbConnection);
            IDbCommand dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"UPDATE Intentos SET victorias = victorias + 1 WHERE id_jugador = {idJugador}";
            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteNonQuery();
            GuardarPartida();
        }
    }
    
    private int ObtenerIdJugador(IDbConnection dbConnection)
    {
        IDbCommand dbCmd = dbConnection.CreateCommand();
        string sqlQuery = $"SELECT id FROM Jugadores WHERE nombre_jugador = '{nombreJugador}'";
        dbCmd.CommandText = sqlQuery;
        IDataReader reader = dbCmd.ExecuteReader();
        int id = -1;
        
        while (reader.Read())
        {
            id = reader.GetInt32(0);
            break;
        }
        reader.Close();
        return id;
    }

    private void GuardarPartida()
    {
        string modo = PlayerPrefs.GetString("ModoSeleccionado", "");
        string tiempoRestanteString = PlayerPrefs.GetString("TiempoRestante", "120");
        int tiempoSobrevivido = 120 - int.Parse(tiempoRestanteString);
        string horaJuego = PlayerPrefs.GetString("HoraInicio", DateTime.Now.ToString("HH:mm:ss"));

        using (IDbConnection dbConnection = CrearYAbrirBaseDeDatos())
        {
            int idJugador = ObtenerIdJugador(dbConnection);

            IDbCommand dbCmd = dbConnection.CreateCommand();
            string sqlQuery = $"INSERT INTO Partidas (id_jugador, modo, tiempo_sobrevivido, hora_juego) " +
                            $"VALUES ({idJugador}, '{modo}', '{tiempoSobrevivido} segundos', '{horaJuego}')";
            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteNonQuery();
        }
    }


}
