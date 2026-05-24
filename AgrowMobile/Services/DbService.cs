using AgrowMobile.Models;
using MySql.Data.MySqlClient;

namespace AgrowMobile.Services;

public class DbService
{
    string connString = "server=yamanote.proxy.rlwy.net;" + "port=46245;" + "user=root;" + "password=bIcWYVezsEKKMJjTjNNGvvyPWsSKQmWI;" + "database=railway;";

    // REGISTER USER
    public async Task<bool> RegisterUser(
        string role,
        string mobile,
        string username,
        string email,
        string password)
    {
        try
        {
            using var conn = new MySqlConnection(connString);

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
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    ex.Message,
                    "OK");
            }

            Console.WriteLine(ex.Message);

            return false;
        }
    }

    // LOGIN USER
    public async Task<string?> LoginUser(
    string role,
    string username,
    string password)
    {
        try
        {
            using var conn = new MySqlConnection(connString);

            await conn.OpenAsync();

            string query =
                @"SELECT role FROM users
              WHERE TRIM(role)=TRIM(@role)
              AND TRIM(username)=TRIM(@username)
              AND TRIM(password)=TRIM(@password)";

            using var cmd = new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@role", role);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            var result = await cmd.ExecuteScalarAsync();

            if (result != null)
            {
                return result.ToString();
            }

            return null;
        }
        catch (Exception ex)
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "DB Error",
                    ex.Message,
                    "OK");
            }

            return null;
        }
    }

    public async Task<bool> ResetPassword(
    string username,
    string mobile,
    string newPassword)
    {
        try
        {
            using var conn =
                new MySqlConnection(connString);

            await conn.OpenAsync();

            string query =
                @"UPDATE users
              SET password=@password
              WHERE username=@username
              AND mobile=@mobile";

            using var cmd =
                new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue(
                "@password",
                newPassword);

            cmd.Parameters.AddWithValue(
                "@username",
                username);

            cmd.Parameters.AddWithValue(
                "@mobile",
                mobile);

            int rows =
                await cmd.ExecuteNonQueryAsync();

            return rows > 0;
        }
        catch (Exception ex)
        {
            if (Application.Current?.MainPage != null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "DB Error",
                    ex.Message,
                    "OK");
            }

            return false;
        }
    }

    public async Task<bool> AddProduct(
    string farmerName,
    string title,
    string description,
    int qty,
    double price,
    string image)
    {
        try
        {
            using var conn =
                new MySqlConnection(connString);

            await conn.OpenAsync();

            string query =
                @"INSERT INTO products
            (farmer_name,title,description,qty,price,image)
            VALUES
            (@farmer,@title,@description,@qty,@price,@image)";

            using var cmd =
                new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@farmer", farmerName);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@description", description);
            cmd.Parameters.AddWithValue("@qty", qty);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.Parameters.AddWithValue("@image", image);

            int rows =
                await cmd.ExecuteNonQueryAsync();

            return rows > 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<ProductModel>> GetProducts(
    string farmerName)
    {
        List<ProductModel> list = new();

        using var conn = new MySqlConnection(connString);

        await conn.OpenAsync();

        string query =
            @"SELECT * FROM products
          WHERE farmer_name=@farmer";

        using var cmd = new MySqlCommand(query, conn);

        cmd.Parameters.AddWithValue("@farmer", farmerName);

        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(new ProductModel
            {
                Id = Convert.ToInt32(reader["id"]),
                FarmerName = reader["farmer_name"].ToString() ?? "",
                Title = reader["title"].ToString() ?? "",
                Description = reader["description"].ToString() ?? "",
                Qty = Convert.ToInt32(reader["qty"]),
                Price = Convert.ToDouble(reader["price"]),
                Image = reader["image"].ToString() ?? ""
            });
        }

        return list;
    }

    public async Task<bool> DeleteProduct(
    int id)
    {
        try
        {
            using var conn =
                new MySqlConnection(connString);

            await conn.OpenAsync();

            string query =
                "DELETE FROM products WHERE id=@id";

            using var cmd =
                new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@id", id);

            int rows =
                await cmd.ExecuteNonQueryAsync();

            return rows > 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<ProductModel>> GetAllProducts()
    {
        List<ProductModel> list = new();

        using var conn =
            new MySqlConnection(connString);

        await conn.OpenAsync();

        string query = "SELECT * FROM products";

        using var cmd =
            new MySqlCommand(query, conn);

        using var reader =
            await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(new ProductModel
            {
                Id = Convert.ToInt32(reader["id"]),
                FarmerName =
                    reader["farmer_name"].ToString() ?? "",

                Title =
                    reader["title"].ToString() ?? "",

                Description =
                    reader["description"].ToString() ?? "",

                Qty =
                    Convert.ToInt32(reader["qty"]),

                Price =
                    Convert.ToDouble(reader["price"]),

                Image =
                    reader["image"].ToString() ?? ""
            });
        }

        return list;
    }

    public async Task<ProductModel?> GetProductById(
    int id)
    {
        using var conn =
            new MySqlConnection(connString);

        await conn.OpenAsync();

        string query =
            "SELECT * FROM products WHERE id=@id";

        using var cmd =
            new MySqlCommand(query, conn);

        cmd.Parameters.AddWithValue("@id", id);

        using var reader =
            await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new ProductModel
            {
                Id = Convert.ToInt32(reader["id"]),
                FarmerName =
                    reader["farmer_name"].ToString() ?? "",

                Title =
                    reader["title"].ToString() ?? "",

                Description =
                    reader["description"].ToString() ?? "",

                Qty =
                    Convert.ToInt32(reader["qty"]),

                Price =
                    Convert.ToDouble(reader["price"]),

                Image =
                    reader["image"].ToString() ?? ""
            };
        }

        return null;
    }

    public async Task<bool> AddOrder(
    string customerName,
    string farmerName,
    string productTitle,
    int qty,
    double total,
    string deliveryName,
    string deliveryMobile,
    string deliveryAddress)
    {
        try
        {
            using var conn =
                new MySqlConnection(connString);

            await conn.OpenAsync();

            string query =
                @"INSERT INTO orders
            (
                customer_name,
                farmer_name,
                product_title,
                qty,
                total,
                delivery_name,
                delivery_mobile,
                delivery_address,
                status
            )
            VALUES
            (
                @customer,
                @farmer,
                @title,
                @qty,
                @total,
                @dname,
                @dmobile,
                @daddress,
                'Pending'
            )";

            using var cmd =
                new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue(
                "@customer",
                customerName);

            cmd.Parameters.AddWithValue(
                "@farmer",
                farmerName);

            cmd.Parameters.AddWithValue(
                "@title",
                productTitle);

            cmd.Parameters.AddWithValue(
                "@qty",
                qty);

            cmd.Parameters.AddWithValue(
                "@total",
                total);

            cmd.Parameters.AddWithValue(
                "@dname",
                deliveryName);

            cmd.Parameters.AddWithValue(
                "@dmobile",
                deliveryMobile);

            cmd.Parameters.AddWithValue(
                "@daddress",
                deliveryAddress);

            int rows =
                await cmd.ExecuteNonQueryAsync();

            return rows > 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> AddToCart(
    string customerName,
    ProductModel product)
    {
        try
        {
            using var conn =
                new MySqlConnection(connString);

            await conn.OpenAsync();

            string query =
                @"INSERT INTO cart
            (
                customer_name,
                farmer_name,
                product_id,
                title,
                qty,
                price,
                image
            )
            VALUES
            (
                @customer,
                @farmer,
                @productid,
                @title,
                @qty,
                @price,
                @image
            )";

            using var cmd =
                new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue(
                "@customer",
                customerName);

            cmd.Parameters.AddWithValue(
                "@farmer",
                product.FarmerName);

            cmd.Parameters.AddWithValue(
                "@productid",
                product.Id);

            cmd.Parameters.AddWithValue(
                "@title",
                product.Title);

            cmd.Parameters.AddWithValue(
                "@qty",
                1);

            cmd.Parameters.AddWithValue(
                "@price",
                product.Price);

            cmd.Parameters.AddWithValue(
                "@image",
                product.Image);

            int rows =
                await cmd.ExecuteNonQueryAsync();

            return rows > 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<List<CartModel>> GetCartItems(
    string customerName)
    {
        List<CartModel> list = new();

        using var conn =
            new MySqlConnection(connString);

        await conn.OpenAsync();

        string query =
            @"SELECT * FROM cart
          WHERE customer_name=@customer";

        using var cmd =
            new MySqlCommand(query, conn);

        cmd.Parameters.AddWithValue(
            "@customer",
            customerName);

        using var reader =
            await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(new CartModel
            {
                Id = Convert.ToInt32(reader["id"]),
                CustomerName =
                    reader["customer_name"].ToString() ?? "",

                FarmerName =
                    reader["farmer_name"].ToString() ?? "",

                ProductId =
                    Convert.ToInt32(reader["product_id"]),

                Title =
                    reader["title"].ToString() ?? "",

                Qty =
                    Convert.ToInt32(reader["qty"]),

                Price =
                    Convert.ToDouble(reader["price"]),

                Image =
                    reader["image"].ToString() ?? ""
            });
        }

        return list;
    }

    public async Task<bool> ClearCart(
    string customerName)
    {
        try
        {
            using var conn =
                new MySqlConnection(connString);

            await conn.OpenAsync();

            string query =
                @"DELETE FROM cart
              WHERE customer_name=@customer";

            using var cmd =
                new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue(
                "@customer",
                customerName);

            int rows =
                await cmd.ExecuteNonQueryAsync();

            return rows >= 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateCartQty(
    int cartId,
    int qty)
    {
        try
        {
            using var conn =
                new MySqlConnection(connString);

            await conn.OpenAsync();

            string query =
                @"UPDATE cart
              SET qty=@qty
              WHERE id=@id";

            using var cmd =
                new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@qty", qty);
            cmd.Parameters.AddWithValue("@id", cartId);

            int rows =
                await cmd.ExecuteNonQueryAsync();

            return rows > 0;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteCartItem(
    int id)
    {
        try
        {
            using var conn =
                new MySqlConnection(connString);

            await conn.OpenAsync();

            string query =
                "DELETE FROM cart WHERE id=@id";

            using var cmd =
                new MySqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@id", id);

            int rows =
                await cmd.ExecuteNonQueryAsync();

            return rows > 0;
        }
        catch
        {
            return false;
        }
    }
}