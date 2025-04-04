using System;
using System.Collections.Generic;

public class Mobile
{
    public int MobileId { get; set; }
    public string Brand { get; set; }
    public string Model { get; set; }
    public decimal Price { get; set; }
    public int LaunchedYear { get; set; }
}

public class MobileManager
{
    public List<Mobile> Mobiles { get; private set; }

    public MobileManager()
    {
        Mobiles = new List<Mobile>();
    }

    public void AddMobile(Mobile mobile)
    {
        if (Mobiles.Exists(m => m.MobileId == mobile.MobileId))
        {
            Console.WriteLine($"A mobile with ID {mobile.MobileId} already exists.");
        }
        else
        {
            Mobiles.Add(mobile);
            Console.WriteLine($"Mobile with ID {mobile.MobileId} added Successfully.");
        }
    }

    public void DisplayMobiles()
    {
        if (Mobiles.Count == 0)
        {
            Console.WriteLine("No mobiles available.");
        }
        else
        {
            foreach (var mobile in Mobiles)
            {
                Console.WriteLine($"Mobile ID: {mobile.MobileId}, Brand: {mobile.Brand}, Model: {mobile.Model}, Price: {mobile.Price:F2}, Launched Year: {mobile.LaunchedYear}");
            }
        }
    }

    public void SearchMobileByBrand(string brand)
    {
        var mobilesByBrand = Mobiles.FindAll(m => m.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase));
        if (mobilesByBrand.Count == 0)
        {
            Console.WriteLine($"No mobiles found for brand: {brand}");
        }
        else
        {
            Console.WriteLine($"Mobiles found for brand: {brand}");
            foreach (var mobile in mobilesByBrand)
            {
                Console.WriteLine($"Mobile ID: {mobile.MobileId}, Model: {mobile.Model}, Price: {mobile.Price:F2}, Launched Year: {mobile.LaunchedYear}");
            }
        }
    }

    public void DeleteMobile(int mobileId)
    {
        var mobile = Mobiles.Find(m => m.MobileId == mobileId);
        if (mobile != null)
        {
            Mobiles.Remove(mobile);
            Console.WriteLine($"Mobile with ID {mobileId} deleted.");
        }
        else
        {
            Console.WriteLine($"Mobile with ID {mobileId} not found.");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        MobileManager manager = new MobileManager();
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Add Mobile");
            Console.WriteLine("2. Display All Mobiles");
            Console.WriteLine("3. Search Mobile by Brand");
            Console.WriteLine("4. Delete Mobile");
            Console.WriteLine("5. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter Mobile ID: ");
                    int mobileId = int.Parse(Console.ReadLine());
                    Console.Write("Enter Brand: ");
                    string brand = Console.ReadLine();
                    Console.Write("Enter Model: ");
                    string model = Console.ReadLine();
                    Console.Write("Enter Price: ");
                    decimal price = decimal.Parse(Console.ReadLine());
                    Console.Write("Enter Launched Year: ");
                    int year = int.Parse(Console.ReadLine());

                    Mobile mobile = new Mobile
                    {
                        MobileId = mobileId,
                        Brand = brand,
                        Model = model,
                        Price = price,
                        LaunchedYear = year
                    };
                    manager.AddMobile(mobile);
                    break;

                case "2":
                    manager.DisplayMobiles();
                    break;

                case "3":
                    Console.Write("Enter Brand: ");
                    string searchBrand = Console.ReadLine();
                    manager.SearchMobileByBrand(searchBrand);
                    break;

                case "4":
                    Console.Write("Enter Mobile ID to delete: ");
                    int deleteId = int.Parse(Console.ReadLine());
                    manager.DeleteMobile(deleteId);
                    break;

                case "5":
                    Console.WriteLine("Exiting program...");
                    exit = true;
                    break;

                default:
                    Console.WriteLine("Invalid choice, please try again.");
                    break;
            }
        }
    }
}
