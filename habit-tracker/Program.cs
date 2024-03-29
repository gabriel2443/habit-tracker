
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Data.Sqlite;

class Program
{

  static  string connectionString = @"Data Source=habit-tracker.db";
    static void Main(string[] args)
    {

        

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText =
                            @"CREATE TABLE IF NOT EXISTS drinking_water (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT,
                        Quantity INTEGER
                        )";

            tableCmd.ExecuteNonQuery();

            connection.Close();





        }

            Menu();

    }


    static void Menu()
    {
        bool closeApp = false;
       
        do
        {
            Console.WriteLine("\n\nMAIN MENU");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("\nType 0 to Close Application.");
            Console.WriteLine("Type 1 to View All Records.");
            Console.WriteLine("Type 2 to Insert Record.");
            Console.WriteLine("Type 3 to Delete Record.");
            Console.WriteLine("Type 4 to Update Record.");
            Console.WriteLine("------------------------------------------\n");

            string command = Console.ReadLine();





            switch (command)
            {
                case "0":
                    Console.WriteLine("\nGoodbye!\n");
                    closeApp = true;
                    Environment.Exit(0);
                    break;
                case "1":
                    GetAllRecords();
                    break;
                case "2":
                    Insert();
                    break;
            /*    case "3":
                    Delete();
                    break;
                case "4":
                    Update();
                    break;*/
                default:
                    Console.WriteLine("\nInvalid Command. Please type a number from 0 to 4.\n");
                    break;
            }



        } while (closeApp);


    }


    private static void GetAllRecords()
    {
        Console.Clear();

        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            var tableCmd= connection.CreateCommand();

            tableCmd.CommandText = $"SELECT * FROM drinking_water";

            List<DrinkingWater> tableData = new();

            SqliteDataReader reader = tableCmd.ExecuteReader();

            if(reader.HasRows)
            {
                while (reader.Read()){

                    tableData.Add(
                    new DrinkingWater
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                        Quantity = reader.GetInt32(2)
                    }); ;



                }

            }
            else
            {
                Console.WriteLine("No rows found");
            }
            connection.Close();
            Console.WriteLine("------------------------------------------\n");

            foreach (var dw in tableData)
            {
                Console.WriteLine($"{dw.Id} - {dw.Date.ToString("dd-MMM-yyyy")} - Quantity: {dw.Quantity}");


            }
            Console.WriteLine("------------------------------------------\n");
        }


    }






   private static void Insert()
    {
        string date = GetDateInput();

        int quantity = GetQuantity();

        using(var connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();

            tableCmd.CommandText = $"INSERT INTO drinking_water(date, quantity) VALUES('{date}', {quantity})";

            tableCmd.ExecuteNonQuery();

            connection.Close();


        }




    }

    internal static string GetDateInput()
    {
        Console.WriteLine("Please insert the date in this format: dd-mm-yy).Type 0 to return to main menu");
      
        var dateInput = Console.ReadLine();
        

        if (dateInput == "0") Menu();

        return dateInput;
    }



    internal static int GetQuantity()
    {
        Console.WriteLine("Please type the amount of glasses drinked or type 0 to return to main menu");

        var quantityInput = Console.ReadLine();

        if(quantityInput == "0") Menu();

        int finalInput = Convert.ToInt32(quantityInput);

        return finalInput;

        



    }



}

public class DrinkingWater
{
   public int Id { get; set; }

   public DateTime Date { get; set; }

   public int Quantity { get; set; }

}