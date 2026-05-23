using MySql.Data.MySqlClient;

namespace AgrowMobile.Services;

public class DbService
{
    string connectionString = "server=yamanote.proxy.rlwy.net;" + "port=46245;" + "user=root;" + "password=bIcWYVezsEKKMJjTjNNGvvyPWsSKQmWI;" + "database=railway;";


    public async Task<bool> RegisterUser(
        string role,
        string mobile,
        string username,
        string email,
        string password)
    {
        try
        {
            using var conn = new MySqlConnection(connectionString);

            await conn.OpenAsync();
            
            string query =
                @"INSERT INTO users
                (role,mobile,username,email,password)
                VALUES
                (@role,@mobile,@username,@email,@password)";

            using var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@role", role);
            cmd.Parameters.AddWithValue("@mobile", mobile);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", password);

            int rows = await cmd.ExecuteNonQueryAsync();

            return rows > 0;
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert(
                "Error",
                ex.Message,
                "OK");

            Console.WriteLine(ex.Message);

            return false;
        }
    }
}