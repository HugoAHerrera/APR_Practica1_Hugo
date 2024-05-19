using System;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using TMPro;
using System.Xml;

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
    private string JSON_NOMBRE = "marcados.json";
    private string xmlFilePath = "marcados.xml";

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
        string query = "SELECT Jugadores.nombre_jugador, Intentos.victorias, Intentos.derrotas, Partidas.modo, Partidas.tiempo_sobrevivido, Partidas.hora_juego " +
                       "FROM Jugadores " +
                       "LEFT JOIN Intentos ON Jugadores.id = Intentos.id_jugador " +
                       "LEFT JOIN Partidas ON Jugadores.id = Partidas.id_jugador";
        DescargarJSON(query);
        DescargarXML(query);
    }

    private void DescargarJSON(string query)
    {
        using (IDbConnection dbConnection = new SqliteConnection(dbUri))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                dbCmd.CommandText = query;

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    string jsonData = "{ \"Jugadores\": [";
                    while (reader.Read())
                    {
                        string nombreJugador = reader.GetString(0);
                        int victorias = reader.GetInt32(1);
                        int derrotas = reader.GetInt32(2);
                        string modo = reader.GetString(3);
                        string tiempoSobrevivido = reader.GetString(4);
                        string horaJuego = reader.GetString(5);

                        string jugadorData = $"{{\"nombre_jugador\": \"{nombreJugador}\", \"partidas_jugadas\": {victorias + derrotas}, \"victorias\": {victorias}, \"derrotas\": {derrotas}, \"partida\": {{ \"modo\": \"{modo}\", \"tiempo_sobrevivido_segundos\": {tiempoSobrevivido}, \"hora_juego\": \"{horaJuego}\" }} }}";

                        jsonData += jugadorData + ",";
                    }
                    jsonData = jsonData.Remove(jsonData.Length - 1, 1) + "]}";
                    FileManager.WriteToFile("positions.json", jsonData);
                    Debug.Log("Datos JSON guardados en AppData-->LocalLow-->DefaultCompany-->Pathfinding_game: " + JSON_NOMBRE);
                }
            }
        }
    }

    private void DescargarXML(string query)
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlNode nodoRaiz = xmlDoc.CreateElement("Jugadores");
        xmlDoc.AppendChild(nodoRaiz);

        using (IDbConnection dbConnection = new SqliteConnection(dbUri))
        {
            dbConnection.Open();

            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                dbCmd.CommandText = query;

                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string nombreJugador = reader.GetString(0);
                        int victorias = reader.GetInt32(1);
                        int derrotas = reader.GetInt32(2);
                        string modo = reader.GetString(3);
                        string tiempoSobrevivido = reader.GetString(4);
                        string horaJuego = reader.GetString(5);

                        XmlNode jugadorN = xmlDoc.CreateElement("Jugador");
                        nodoRaiz.AppendChild(jugadorN);

                        XmlNode nombre = xmlDoc.CreateElement("nombre_jugador");
                        nombre.InnerText = nombreJugador;
                        jugadorN.AppendChild(nombre);

                        XmlNode partidasN = xmlDoc.CreateElement("partidas_jugadas");
                        partidasN.InnerText = (victorias + derrotas).ToString();
                        jugadorN.AppendChild(partidasN);

                        XmlNode victoriasN = xmlDoc.CreateElement("victorias");
                        victoriasN.InnerText = victorias.ToString();
                        jugadorN.AppendChild(victoriasN);

                        XmlNode derrotasN = xmlDoc.CreateElement("derrotas");
                        derrotasN.InnerText = derrotas.ToString();
                        jugadorN.AppendChild(derrotasN);

                        XmlNode partidaN = xmlDoc.CreateElement("partida");
                        jugadorN.AppendChild(partidaN);

                        XmlNode modoN = xmlDoc.CreateElement("modo");
                        modoN.InnerText = modo;
                        partidaN.AppendChild(modoN);

                        XmlNode tiempoN = xmlDoc.CreateElement("tiempo_sobrevivido_segundos");
                        tiempoN.InnerText = tiempoSobrevivido;
                        partidaN.AppendChild(tiempoN);

                        XmlNode horaN = xmlDoc.CreateElement("hora_juego");
                        horaN.InnerText = horaJuego;
                        partidaN.AppendChild(horaN);
                    }
                }
            }
        }

        string xmlString = xmlDoc.OuterXml;
        FileManager.WriteToFile(xmlFilePath, xmlString);
        Debug.Log("Datos XML guardados dentro de AppData-->LocalLow-->DefaultCompany-->Pathfinding_game: " + xmlFilePath);
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
            if(jugadorId == -1)
            {
                Debug.Log(nombre + " no tiene partidas registradas");
            }else
            {
                IDbCommand deleteCmd = dbConnection.CreateCommand();
                deleteCmd.CommandText = $"DELETE FROM Jugadores WHERE nombre_jugador = '{nombre}';" +
                                $"DELETE FROM Partidas WHERE id_jugador = {jugadorId};" +
                                $"DELETE FROM Intentos WHERE id_jugador = {jugadorId};";
                deleteCmd.ExecuteNonQuery();
                Debug.Log("Registros borrados del jugador " + nombre);
            }
        }
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
            IDbCommand dbCmd = dbConn.CreateCommand();
            string sqlQuery = $"SELECT id FROM Jugadores WHERE nombre_jugador = '{nombreJugador}'";
            dbCmd.CommandText = sqlQuery;
            
            using (IDataReader reader = dbCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    return reader.GetInt32(0);
                }
                else
                {
                    return -1;
                }
            }
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
            string sqlQuery = $"INSERT INTO Intentos (id_jugador, numero_intentos, victorias, derrotas) VALUES ({idJugador}, 1, 0, 0)";
            dbCmd.CommandText = sqlQuery;
            dbCmd.ExecuteNonQuery();
        }
    }


}