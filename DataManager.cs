using System;
using System.Data.SQLite;

namespace GameEntity
{
    public static class DataManager
    {
        private const string ConnectionString =
            "Data Source=GameData.db;Version=3;";

        //------------------------------------------

        public static void InitializeDatabase()
        {
            using (SQLiteConnection connection =
                new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string sql =
                @"CREATE TABLE IF NOT EXISTS PlayerData
                (
                    Id INTEGER PRIMARY KEY,

                    Coins INTEGER,

                    HighScore INTEGER,

                    ShipSkin TEXT,

                    BulletSkin TEXT,

                    BackgroundSkin TEXT,

                    ExtraLives INTEGER
                );";

                SQLiteCommand command =
                    new SQLiteCommand(sql, connection);

                command.ExecuteNonQuery();

                sql =
                @"INSERT OR IGNORE INTO PlayerData
                (Id,Coins,HighScore,ShipSkin,BulletSkin,BackgroundSkin,ExtraLives)

                VALUES
                (1,0,0,'Default','Default','Default',0);";

                command =
                    new SQLiteCommand(sql, connection);

                command.ExecuteNonQuery();
            }
        }

        //------------------------------------------

        public static void Save(PlayerData data)
        {
            using (SQLiteConnection connection =
                new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string sql =
                @"UPDATE PlayerData

                SET

                Coins=@Coins,

                HighScore=@HighScore,

                ShipSkin=@ShipSkin,

                BulletSkin=@BulletSkin,

                BackgroundSkin=@BackgroundSkin,

                ExtraLives=@ExtraLives

                WHERE Id=1;";

                SQLiteCommand command =
                    new SQLiteCommand(sql, connection);

                command.Parameters.AddWithValue("@Coins",
                    CoinManager.Coins);

                command.Parameters.AddWithValue("@HighScore",
                    ScoreManager.Score);

                command.Parameters.AddWithValue("@ShipSkin",
                    data.ShipSkin);

                command.Parameters.AddWithValue("@BulletSkin",
                    data.BulletSkin);

                command.Parameters.AddWithValue("@BackgroundSkin",
                    data.BackgroundSkin);

                command.Parameters.AddWithValue("@ExtraLives",
                    data.ExtraLives);

                command.ExecuteNonQuery();
            }
        }

        //------------------------------------------

        public static PlayerData Load()
        {
            PlayerData data = new PlayerData();

            using (SQLiteConnection connection =
                new SQLiteConnection(ConnectionString))
            {
                connection.Open();

                string sql =
                    "SELECT * FROM PlayerData WHERE Id=1;";

                SQLiteCommand command =
                    new SQLiteCommand(sql, connection);

                SQLiteDataReader reader =
                    command.ExecuteReader();

                if (reader.Read())
                {
                    CoinManager.SetCoins(
                        Convert.ToInt32(reader["Coins"]));

                    ScoreManager.SetScore(
                        Convert.ToInt32(reader["HighScore"]));

                    data.ShipSkin =
                        reader["ShipSkin"].ToString();

                    data.BulletSkin =
                        reader["BulletSkin"].ToString();

                    data.BackgroundSkin =
                        reader["BackgroundSkin"].ToString();

                    data.ExtraLives =
                        Convert.ToInt32(reader["ExtraLives"]);
                }
            }

            return data;
        }

        //------------------------------------------

        public static void Reset()
        {
            CoinManager.Reset();

            ScoreManager.Reset();

            Save(new PlayerData());
        }
    }
}


}
