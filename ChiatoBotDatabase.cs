using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ChiatoBot
{
    class ChiatoBotDatabase
    {
        public static string dbConnectionString = "" //Database connection string, should probably get this from somewhere that is not in the code;

        public static bool CheckIfUserExist(ulong id)
        {
            using var connection = new SqlConnection(dbConnectionString);
            connection.Open();

            SqlCommand dbCommand;
            SqlDataAdapter dbAdapter = new SqlDataAdapter();
            string sql = "";

            sql = ($"Select COUNT({id}) from dbo.DiscordCoins where DiscordID = {id}");


            dbCommand = new SqlCommand(sql, connection);

            int userExist = (int)dbCommand.ExecuteScalar();
            if (userExist > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task DBUpdateCoins(ulong id, int coinAmount, bool give)
        {
            using var connection = new SqlConnection(dbConnectionString);
            connection.Open();

            SqlCommand dbCommand;
            SqlDataAdapter dbAdapter = new SqlDataAdapter();
            string sql = "";
            int currentCoins = 0;
            int fixedCoins = 0;
            int resultingCoins = 0;

            if (give)
                fixedCoins = coinAmount;
            else
                fixedCoins = -coinAmount;

            currentCoins = await DBReadCoins(id);
            resultingCoins = currentCoins + fixedCoins;

            sql = ($"Select COUNT({id}) from dbo.DiscordCoins where DiscordID = {id}");



            dbCommand = new SqlCommand(sql, connection);

            int userExist = (int)dbCommand.ExecuteScalar();
            if (userExist > 0)
            {
                sql = ($"Update dbo.DiscordCoins set Coins={resultingCoins} where DiscordID = {id}");
                dbAdapter.UpdateCommand = new SqlCommand(sql, connection);
                await dbAdapter.UpdateCommand.ExecuteNonQueryAsync();
            }
            else
            {
                sql = ($"Insert into dbo.DiscordCoins (DiscordID, Coins) values ({id}, '" + "100" + "')");
                dbCommand = new SqlCommand(sql, connection);
                dbAdapter.InsertCommand = new SqlCommand(sql, connection);
                dbAdapter.InsertCommand.ExecuteNonQuery();
            }
            dbCommand.Dispose();
            dbAdapter.Dispose();

        }

        public static async Task<int> DBReadCoins(ulong id)
        {
            using var connection = new SqlConnection(dbConnectionString);
            connection.Open();

            SqlCommand dbCommand;
            SqlDataReader dbReader;
            string sql = "";

            sql = ($"Select Coins from dbo.DiscordCoins where DiscordID={id}");

            dbCommand = new SqlCommand(sql, connection);
            dbReader = await dbCommand.ExecuteReaderAsync();

            int CoinsRead = 0;

            while (dbReader.Read())
            {
                CoinsRead = dbReader.GetInt32(0);
            }
            dbReader.Close();
            dbCommand.Dispose();
            connection.Close();
            return CoinsRead;

        }

        public static async Task<int> DBReadRows()
        {
            using var connection = new SqlConnection(dbConnectionString);
            connection.Open();

            int amountOfRows = 0;

            string sql = ($"Select COUNT(*) from dbo.DiscordCoins");

            await using (SqlConnection thisConnection = new SqlConnection(dbConnectionString))
            {
                await using (SqlCommand cmdCount = new SqlCommand(sql, thisConnection))
                {
                    thisConnection.Open();
                    amountOfRows = (int)cmdCount.ExecuteScalar();
                }
            }
            connection.Close();
            return amountOfRows;
        }


        public static async Task<List<string>> DBLeaderBoard()
        {
            using var connection = new SqlConnection(dbConnectionString);
            connection.Open();

            SqlCommand dbCommand;
            SqlDataReader dbReader;
            string sql = "";

            sql = ($"Select * From dbo.DiscordCoins order by Coins DESC");

            dbCommand = new SqlCommand(sql, connection);
            dbReader = await dbCommand.ExecuteReaderAsync();

            List<string> leaderboard = new List<string>();
            while (dbReader.Read())
            {
                leaderboard.Add(dbReader.GetValue(0).ToString());
                leaderboard.Add(dbReader.GetValue(1).ToString());
            }
            dbReader.Close();
            dbCommand.Dispose();
            connection.Close();
            return leaderboard;
        }

        public static async Task<List<ulong>> GetAllUsersInDB()
        {
            using var connection = new SqlConnection(dbConnectionString);
            connection.Open();

            SqlCommand dbCommand;
            SqlDataReader dbReader;
            string sql = "";

            sql = ($"Select distinct DiscordID From dbo.DiscordCoins");

            dbCommand = new SqlCommand(sql, connection);
            dbReader = await dbCommand.ExecuteReaderAsync();

            List<ulong> dBUsers = new List<ulong>();
            while (dbReader.Read())
            {
                dBUsers.Add(Convert.ToUInt64(dbReader.GetValue(0)));
            }
            dbReader.Close();
            dbCommand.Dispose();
            connection.Close();
            return dBUsers;
        }
    } 
}
