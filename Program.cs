using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

// Main Program Class
public class Program
{
    static void Main(string[] args)
    {
        // Call to write initial data to files
        InitialDataSetup.WriteInitialData();

        bool continueLoop = true;

        while (continueLoop) // Keep the system running until the user chooses to exit
        {
            // Initially, show the welcome message
            Console.WriteLine("                        ************************************************************");
            Console.WriteLine("                        *                                                          *");
            Console.WriteLine("                        *          Welcome to the Hotel Management System          *");
            Console.WriteLine("                        *                                                          *");
            Console.WriteLine("                        ************************************************************\n");

            // Login process
            Console.WriteLine("Enter username:");
            string username = Console.ReadLine();
            Console.WriteLine("Enter password:");
            string password = Console.ReadLine();

            string role;
            if (User.ValidateUser(username, password, out role))
            {
                Console.WriteLine($"Login successful! You are logged in as {role}.");
                Console.Clear();
                bool loggedOut = false;

                // Loop for Manager or Receptionist to keep using their functionalities
                while (!loggedOut)
                {
                    if (role == "Manager")
                    {
                        Manager manager = new Manager();
                        loggedOut = manager.ManagerOperations();  // This should return true if logged out
                    }
                    else if (role == "Receptionist")
                    {
                        Receptionist receptionist = new Receptionist();
                        loggedOut = receptionist.ReceptionistOperations();  // This should return true if logged out
                    }

                    if (!loggedOut)
                    {
                        Console.Clear(); // Clear the console to show the menu again
                    }
                }

                // After logout, prompt if they want to log in again
                Console.WriteLine("Do you want to log in again? (yes/no)");

                string response = Console.ReadLine();
                if (response?.ToLower() != "yes")
                {
                    continueLoop = false; // Exit the loop if the user doesn't want to log in again
                }
                else
                {
                    // Restart login loop if they want to log in again
                    Console.Clear(); // Clear the console after logging out
                }
            }
            else
            {
                Console.WriteLine("Invalid login credentials!");
            }
        }

        Console.WriteLine("Thank you for using the system. Goodbye!");
    }
}
public class InitialDataSetup
{
    public static void WriteInitialData()
    {
        // Write initial data for users.txt
        string[] users = {
            "admin,admin123,Manager",
            "receptionist1,pass123,Receptionist",
            "receptionist2,pass123,Receptionist",
            "receptionist3,pass123,Receptionist",
            // Add more receptionists as needed
        };
        File.WriteAllLines(@"D:\FCIS\DP_hotel\users.txt", users);

        // Write initial data for workers.txt
        string[] workers = {
            "John Doe,Manager,1000,5500",
            "Jane Smith,Receptionist,500,4500",
            "Jack Black,Housekeeper,400,3500"
            // Add more workers as needed
        };
        File.WriteAllLines(@"D:\FCIS\DP_hotel\workers.txt", workers);

        // Write initial data for rooms.txt
        string[] rooms = {
            "101,Single,Available",
            "102,Double,Available",
            "103,Triple,Occupied",
            "104,Single,Available",
            "105,Double,Occupied"
            // Add more rooms as needed
        };
        File.WriteAllLines(@"D:\FCIS\DP_hotel\rooms.txt", rooms);

        // Write initial data for residents.txt
        string[] residents = {
            "Alice Johnson,101,Single,2,FullBoard",
            "Bob Brown,103,Triple,3,BedAndBreakfast"
            // Add more residents as needed
        };
        File.WriteAllLines(@"D:\FCIS\DP_hotel\residents.txt", residents);

        // Write initial data for income.txt
        string[] income = {
            "2024-01-01,1000",
            "2024-01-02,1500"
            // Add more income records as needed
        };
        File.WriteAllLines(@"D:\FCIS\DP_hotel\income.txt", income);

    }
}

// User Class for Login Validation
public static class User
{
    // Validate user credentials
    public static bool ValidateUser(string username, string password, out string role)
    {
        role = string.Empty;
        string[] lines = File.ReadAllLines(@"D:\FCIS\DP_hotel\users.txt");

        foreach (var line in lines)
        {
            var parts = line.Split(',');
            if (parts.Length == 3 && parts[0] == username && parts[1] == password)
            {
                role = parts[2]; // role (Manager/Receptionist)
                return true;
            }
        }

        return false;
    }
}

// Manager Class
public class Manager
{
    public bool ManagerOperations()
    {
        Console.WriteLine("Manager Menu:");
        Console.WriteLine("1. Add Worker");
        Console.WriteLine("2. Edit Worker");
        Console.WriteLine("3. Delete Worker");
        Console.WriteLine("4. View Worker Details");
        Console.WriteLine("5. View Resident Information");
        Console.WriteLine("6. Monitor Room Status");
        Console.WriteLine("7. Track Income");
        Console.WriteLine("8. Log out");
        int choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case 1: AddWorker(); break;
            case 2: EditWorker(); break;
            case 3: DeleteWorker(); break;
            case 4: ViewWorkerDetails(); break;
            case 5: ViewResidentInformation(); break;
            case 6: MonitorRoomStatus(); break;
            case 7: TrackIncome(); break;
            case 8: return true; // Log out
        }
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine(); // Wait for the user to press Enter
        return false; // Continue operations
    }


    // Add Worker
    private void AddWorker()
    {
        Console.WriteLine("Enter worker details: ");
        Console.Write("Name: ");
        string name = Console.ReadLine();
        Console.Write("Contact: ");
        string contact = Console.ReadLine();
        Console.Write("Job Title: ");
        string jobTitle = Console.ReadLine();
        Console.Write("Salary: ");
        decimal salary = decimal.Parse(Console.ReadLine());

        Worker worker = new Worker { Name = name, Contact = contact, JobTitle = jobTitle, Salary = salary };
        var workers = File.ReadAllLines(@"D:\FCIS\DP_hotel\workers.txt").ToList();
        workers.Add($"{worker.Name},{worker.Contact},{worker.JobTitle},{worker.Salary}");

        File.WriteAllLines(@"D:\FCIS\DP_hotel\workers.txt", workers);
        Console.WriteLine("Worker added successfully!");
    }

    // Edit Worker
    private void EditWorker()
    {
        Console.Write("Enter worker name to edit: ");
        string name = Console.ReadLine();

        var workers = File.ReadAllLines(@"D:\FCIS\DP_hotelD:\FCIS\DP_hotelworkers.txt").ToList();
        bool found = false;

        for (int i = 0; i < workers.Count; i++)
        {
            var parts = workers[i].Split(',');
            if (parts[0] == name)
            {
                Console.Write("New Contact: ");
                string newContact = Console.ReadLine();
                Console.Write("New Job Title: ");
                string newJobTitle = Console.ReadLine();
                Console.Write("New Salary: ");
                decimal newSalary = decimal.Parse(Console.ReadLine());

                workers[i] = $"{name},{newContact},{newJobTitle},{newSalary}";
                found = true;
                break;
            }
        }

        if (found)
        {
            File.WriteAllLines(@"D:\FCIS\DP_hotel\workers.txt", workers);
            Console.WriteLine("Worker updated successfully!");
        }
        else
        {
            Console.WriteLine("Worker not found.");
        }
    }

    // Delete Worker
    private void DeleteWorker()
    {
        Console.Write("Enter worker name to delete: ");
        string name = Console.ReadLine();

        var workers = File.ReadAllLines(@"D:\FCIS\DP_hotel\workers.txt").ToList();
        workers.RemoveAll(w => w.Split(',')[0] == name);

        File.WriteAllLines(@"D:\FCIS\DP_hotel\workers.txt", workers);
        Console.WriteLine("Worker deleted successfully!");
    }

    // View Worker Details
    private void ViewWorkerDetails()
    {
        var workers = File.ReadAllLines(@"D:\FCIS\DP_hotel\workers.txt");
        Console.WriteLine("Worker Details:");
        foreach (var worker in workers)
        {
            var parts = worker.Split(',');
            Console.WriteLine($"Name: {parts[0]}, Contact: {parts[1]}, Job Title: {parts[2]}, Salary: {parts[3]}");
        }
    }

    // View Resident Information
    private void ViewResidentInformation()
    {
        var residents = File.ReadAllLines(@"D:\FCIS\DP_hotel\residents.txt");
        Console.WriteLine("Resident Information:");
        foreach (var resident in residents)
        {
            var parts = resident.Split(',');
            Console.WriteLine($"Name: {parts[0]}, Room Type: {parts[1]}, Duration: {parts[2]} days, Boarding Type: {parts[3]}");
        }
    }

    // Monitor Room Status
    private void MonitorRoomStatus()
    {
        var rooms = File.ReadAllLines(@"D:\FCIS\DP_hotel\rooms.txt");
        Console.WriteLine("Room Status:");
        foreach (var room in rooms)
        {
            var parts = room.Split(',');
            Console.WriteLine($"Room Type: {parts[0]}, Status: {parts[1]}");
        }
    }

    // Track Income
    private void TrackIncome()
    {
        var income = File.ReadAllLines(@"D:\FCIS\DP_hotel\income.txt");
        Console.WriteLine("Hotel Income:");
        foreach (var entry in income)
        {
            Console.WriteLine(entry);
        }
    }
}

// Receptionist Class
public class Receptionist
{
    public bool ReceptionistOperations()
    {
        Console.WriteLine("Receptionist Menu:");
        Console.WriteLine("1. Add Resident");
        Console.WriteLine("2. Edit Resident");
        Console.WriteLine("3. Delete Resident");
        Console.WriteLine("4. Assign Room");
        Console.WriteLine("5. Calculate Resident Cost");
        Console.WriteLine("6. Log out");
        int choice = int.Parse(Console.ReadLine());

        switch (choice)
        {
            case 1: AddResident(); break;
            case 2: EditResident(); break;
            case 3: DeleteResident(); break;
            case 4: AssignRoom(); break;
            case 5: CalculateResidentCost(); break;
            case 6: return true; // Log out
        }
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine(); // Wait for the user to press Enter
        return false; // Continue operations
    }

    // Add Resident
    private void AddResident()
    {
        Console.WriteLine("Enter resident details: ");
        Console.Write("Name: ");
        string name = Console.ReadLine();
        Console.Write("Room Type: ");
        string roomType = Console.ReadLine();
        Console.Write("Duration (in days): ");
        int duration = int.Parse(Console.ReadLine());
        Console.Write("Boarding Type (FullBoard/HalfBoard/BedAndBreakfast): ");
        string boardingType = Console.ReadLine();

        Resident resident = new Resident { Name = name, RoomType = roomType, Duration = duration, BoardingType = boardingType };
        var residents = File.ReadAllLines(@"D:\FCIS\DP_hotel\residents.txt").ToList();
        residents.Add($"{resident.Name},{resident.RoomType},{resident.Duration},{resident.BoardingType}");

        File.WriteAllLines(@"D:\FCIS\DP_hotel\residents.txt", residents);
        Console.WriteLine("Resident added successfully!");
    }

    // Edit Resident
    private void EditResident()
    {
        Console.Write("Enter resident name to edit: ");
        string name = Console.ReadLine();

        var residents = File.ReadAllLines(@"D:\FCIS\DP_hotel\residents.txt").ToList();
        bool found = false;

        for (int i = 0; i < residents.Count; i++)
        {
            var parts = residents[i].Split(',');
            if (parts[0] == name)
            {
                Console.Write("New Room Type: ");
                string newRoomType = Console.ReadLine();
                Console.Write("New Duration (in days): ");
                int newDuration = int.Parse(Console.ReadLine());
                Console.Write("New Boarding Type (FullBoard/HalfBoard/BedAndBreakfast): ");
                string newBoardingType = Console.ReadLine();

                residents[i] = $"{name},{newRoomType},{newDuration},{newBoardingType}";
                found = true;
                break;
            }
        }

        if (found)
        {
            File.WriteAllLines(@"D:\FCIS\DP_hotel\residents.txt", residents);
            Console.WriteLine("Resident updated successfully!");
        }
        else
        {
            Console.WriteLine("Resident not found.");
        }
    }

    // Delete Resident
    private void DeleteResident()
    {
        Console.Write("Enter resident name to delete: ");
        string name = Console.ReadLine();

        var residents = File.ReadAllLines(@"D:\FCIS\DP_hotel\residents.txt").ToList();
        residents.RemoveAll(r => r.Split(',')[0] == name);

        File.WriteAllLines(@"D:\FCIS\DP_hotel\residents.txt", residents);
        Console.WriteLine("Resident deleted successfully!");
    }

    // Assign Room
    private void AssignRoom()
    {
        Console.Write("Enter resident name: ");
        string name = Console.ReadLine();
        Console.Write("Enter room type: ");
        string roomType = Console.ReadLine();

        var rooms = File.ReadAllLines(@"D:\FCIS\DP_hotel\rooms.txt").ToList();
        var availableRoom = rooms.FirstOrDefault(r => r.StartsWith(roomType) && r.Split(',')[1] == "Available");

        if (availableRoom != null)
        {
            rooms[rooms.IndexOf(availableRoom)] = $"{roomType},Occupied";
            File.WriteAllLines(@"D:\FCIS\DP_hotel\rooms.txt", rooms);
            Console.WriteLine($"Room {roomType} assigned to {name}");
        }
        else
        {
            Console.WriteLine("No available room of the specified type.");
        }
    }

    // Calculate Resident Cost
    private void CalculateResidentCost()
    {
        Console.Write("Enter resident name to calculate cost: ");
        string name = Console.ReadLine();
        Console.Write("Enter room type: ");
        string roomType = Console.ReadLine();
        Console.Write("Enter duration (in days): ");
        int duration = int.Parse(Console.ReadLine());
        Console.Write("Enter boarding type (FullBoard/HalfBoard/BedAndBreakfast): ");
        string boardingType = Console.ReadLine();

        var resident = new Resident { Name = name, RoomType = roomType, Duration = duration, BoardingType = boardingType };
        decimal cost = resident.CalculateCost();
        Console.WriteLine($"Total cost for {name}: {cost:C}");
    }
}

// Worker Class
public class Worker
{
    public string Name { get; set; }
    public string Contact { get; set; }
    public string JobTitle { get; set; }
    public decimal Salary { get; set; }
}

// Resident Class
public class Resident
{
    public string Name { get; set; }
    public string RoomType { get; set; }
    public int Duration { get; set; }
    public string BoardingType { get; set; }

    public decimal CalculateCost()
    {
        decimal baseCost = 0;
        switch (RoomType)
        {
            case "Single":
                baseCost = 100;
                break;
            case "Double":
                baseCost = 150;
                break;
            case "Triple":
                baseCost = 200;
                break;
            default:
                baseCost = 0;
                break;
        }

        decimal boardingCost = 0;
        switch (BoardingType)
        {
            case "FullBoard":
                boardingCost = 50;
                break;
            case "HalfBoard":
                boardingCost = 30;
                break;
            case "BedAndBreakfast":
                boardingCost = 20;
                break;
            default:
                boardingCost = 0;
                break;
        }

        return (baseCost + boardingCost) * Duration;
    }

}