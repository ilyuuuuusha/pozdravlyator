using System;
using Microsoft.EntityFrameworkCore;

public class Menu
{
    public void Run()
    {
        using var db = new BirthdayContext();
        db.Database.Migrate();

        var service = new BirthdayService();
        bool exit = false;

        while (!exit)
        {
            Console.Clear();
            PrintMenu();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\nВаш выбор: ");
            Console.ResetColor();

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1": service.ShowAll(db); break;
                case "2": service.ShowUpcoming(db); break;
                case "3": service.AddBirthday(db); break;
                case "4": service.EditBirthday(db); break;
                case "5": service.DeleteBirthday(db); break;
                case "0": exit = true; break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неверный ввод, попробуйте ещё раз.");
                    Console.ResetColor();
                    Console.WriteLine("Нажмите любую клавишу...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void PrintMenu()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine("║           М Е Н Ю   П Р О Г Р А М М Ы      ║");
        Console.WriteLine("╠════════════════════════════════════════════╣");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("║ 1. Показать все дни рождения               ║");
        Console.WriteLine("║ 2. Показать ближайшие дни рождения         ║");
        Console.WriteLine("║ 3. Добавить день рождения                  ║");
        Console.WriteLine("║ 4. Редактировать день рождения             ║");
        Console.WriteLine("║ 5. Удалить день рождения                   ║");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("║ 0. Выход                                   ║");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╚════════════════════════════════════════════╝");
    }
}
