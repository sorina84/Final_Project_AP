using System;
using System.Data.SQLite;
using System.IO;
namespace GameEntity
{
    public class DataManager
    {
        private string dbPath = "game.db";
        public DataManager()
        {
            CreateDatabase();
        }
        private void CreateDatabase()
        {
            if (!File.Exists(dbPath))
            {
                SQLiteConnection.CreateFile(dbPath);
            }


            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbPath))
            {
                con.Open();
                string query =
                @"CREATE TABLE IF NOT EXISTS PlayerData
                (
                    Id INTEGER PRIMARY KEY,
                    TotalCoins INTEGER,
                    HighScore INTEGER,

                    MusicEnabled INTEGER,
                    SfxEnabled INTEGER,

                    ShipSkin TEXT,
                    BulletStyle TEXT,
                    Background TEXT,

                    ExtraLife INTEGER

                )";


                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.ExecuteNonQuery();
                // اگر اولین اجرا است
                SQLiteCommand check = new SQLiteCommand("SELECT COUNT(*) FROM PlayerData", con);
                int count = Convert.ToInt32(check.ExecuteScalar());
                if (count == 0)
                {
                    SQLiteCommand insert =
                    new SQLiteCommand(
                    @"INSERT INTO PlayerData
                    VALUES
                    (
                    1,0,0,
                    1,1,
                    'DefaultShip',
                    'DefaultBullet',
                    'DefaultBackground',
                    0
                    
                    )", con);


                    insert.ExecuteNonQuery();
                }
            }
        }



        public PlayerData LoadPlayerData()
        {
            PlayerData data = new PlayerData();


            using (SQLiteConnection con = new SQLiteConnection("Data Source=" + dbPath))
            {
                con.Open();
                SQLiteCommand cmd =
                new SQLiteCommand(
                "SELECT * FROM PlayerData WHERE Id=1",
                con);


                SQLiteDataReader reader = cmd.ExecuteReader();


                if (reader.Read())
                {
                    data.TotalCoins = Convert.ToInt32(reader["TotalCoins"]);


                    data.HighScore = Convert.ToInt32(reader["HighScore"]);


                    data.MusicEnabled = Convert.ToInt32(reader["MusicEnabled"]) == 1;


                    data.SfxEnabled = Convert.ToInt32(reader["SfxEnabled"]) == 1;


                    data.EquippedShipSkin = reader["ShipSkin"].ToString();


                    data.EquippedBulletStyle = reader["BulletStyle"].ToString();


                    data.EquippedBackground = reader["Background"].ToString();


                    data.ExtraLifeBoosterEnabled = Convert.ToInt32(reader["ExtraLife"]) == 1;
                }

            }


            return data;
        }



        public void SavePlayerData(PlayerData data)
        {

            using (SQLiteConnection con =
                new SQLiteConnection(
                "Data Source=" + dbPath))
            {

                con.Open();


                string query =
                @"UPDATE PlayerData SET

                TotalCoins=@coins,
                HighScore=@score,

                MusicEnabled=@music,
                SfxEnabled=@sfx,
                ShipSkin=@ship,
                BulletStyle=@bullet,
                Background=@bg,

                ExtraLife=@life


                WHERE Id=1";


                SQLiteCommand cmd = new SQLiteCommand(query, con);
                cmd.Parameters.AddWithValue("@coins", data.TotalCoins);
                cmd.Parameters.AddWithValue("@score", data.HighScore);
                cmd.Parameters.AddWithValue("@music", data.MusicEnabled ? 1 : 0);
                cmd.Parameters.AddWithValue("@sfx", data.SfxEnabled ? 1 : 0);
                cmd.Parameters.AddWithValue("@ship", data.EquippedShipSkin);
                cmd.Parameters.AddWithValue("@bullet", data.EquippedBulletStyle);
                cmd.Parameters.AddWithValue("@bg", data.EquippedBackground);
                cmd.Parameters.AddWithValue("@life", data.ExtraLifeBoosterEnabled ? 1 : 0);

                cmd.ExecuteNonQuery();

            }

        }
    }
}