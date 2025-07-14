using System;
using System.Linq;
using System.Collections.Generic;

public class BirthdayService
{
    public void ShowAll(BirthdayContext db, bool pauseAfter = true)
    {
        var list = db.Birthdays
            .OrderBy(b => b.DateOfBirth.Month)
            .ThenBy(b => b.DateOfBirth.Day)
            .ToList();

        int tableWidth = 66;
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔" + new string('═', tableWidth - 1) + "╗");
        Console.WriteLine("║" + Helpers.CenterText("Список дней рождения", tableWidth - 1) + "║");
        Console.WriteLine("╠" + new string('═', 4) + "╦" + new string('═', 20) + "╦" + new string('═', 14) + "╦" + new string('═', 24) + "╣");

        Console.ForegroundColor = ConsoleColor.Green;
        string header =
            "ID".PadRight(4) + "│" +
            "Имя".PadRight(20) + "│" +
            "Дата рождения".PadRight(14) + "│" +
            "Следующий день рож.".PadRight(24) + "║";
        Console.WriteLine("║" + header);
        Console.WriteLine("╠" + new string('═', 4) + "╬" + new string('═', 20) + "╬" + new string('═', 14) + "╬" + new string('═', 24) + "╣");

        if (!list.Any())
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            string emptyLine = "В базе данных нет записей".PadRight(tableWidth);
            Console.WriteLine("║" + emptyLine + "║");
        }
        else
        {
            foreach (var b in list)
            {
                var nextBirthday = new DateTime(DateTime.Today.Year, b.DateOfBirth.Month, b.DateOfBirth.Day);
                if (nextBirthday < DateTime.Today) nextBirthday = nextBirthday.AddYears(1);

                int age = nextBirthday.Year - b.DateOfBirth.Year;

                string line =
                    b.Id.ToString().PadRight(4) + "│" +
                    Helpers.TruncateString(b.Name, 20).PadRight(20) + "│" +
                    b.DateOfBirth.ToString("dd.MM.yyyy").PadRight(14) + "│" +
                    $"{nextBirthday:dd.MM} (будет {age} лет)".PadRight(24) + "║";

                Console.WriteLine("║" + line);
            }
        }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╚" + new string('═', tableWidth - 1) + "╝");
        Console.ResetColor();

        if (pauseAfter)
        {
            Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
            Console.ReadKey();
        }
    }

    public void ShowUpcoming(BirthdayContext db)
    {
        var today = DateTime.Today;
        var list = db.Birthdays.ToList()
            .Where(b =>
            {
                var next = new DateTime(today.Year, b.DateOfBirth.Month, b.DateOfBirth.Day);
                if (next < today) next = next.AddYears(1);
                return (next - today).TotalDays <= 7;
            })
            .OrderBy(b => new DateTime(today.Year, b.DateOfBirth.Month, b.DateOfBirth.Day))
            .ToList();

        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("╔════════════════════════════════════════════╗");
        Console.WriteLine("║          Ближайшие дни рождения            ║");
        Console.WriteLine("╚════════════════════════════════════════════╝");
        Console.ResetColor();

        if (!list.Any())
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("        Ближайших дней рождений нет           ");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var b in list)
            {
                var next = new DateTime(today.Year, b.DateOfBirth.Month, b.DateOfBirth.Day);
                if (next < today) next = next.AddYears(1);
                int daysLeft = (next - today).Days;
                Console.WriteLine($"{b.Name} — {b.DateOfBirth:dd.MM.yyyy} (через {daysLeft} {Helpers.GetDaysWord(daysLeft)})");
            }
            Console.ResetColor();
        }

        Console.WriteLine("\nНажмите любую клавишу для возврата в меню...");
        Console.ReadKey();
    }

    public void AddBirthday(BirthdayContext db)
    {
        Console.Write("\nВведите имя: ");
        string name = Console.ReadLine();

        Console.Write("Введите дату рождения (дд.мм.гггг): ");
        if (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out var dob))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Неверный формат даты.");
            Console.ResetColor();
            Console.WriteLine("Нажмите любую клавишу для возврата...");
            Console.ReadKey();
            return;
        }

        db.Birthdays.Add(new BirthdayEntry { Name = name, DateOfBirth = dob });
        db.SaveChanges();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Запись успешно добавлена.");
        Console.ResetColor();
        Console.WriteLine("Нажмите любую клавишу для возврата...");
        Console.ReadKey();
    }

    public void EditBirthday(BirthdayContext db)
    {
        ShowAll(db, pauseAfter: false);
        Console.Write("\nВведите ID для редактирования: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Некорректный ввод.");
            Console.ResetColor();
            return;
        }

        var entry = db.Birthdays.Find(id);
        if (entry == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Запись не найдена.");
            Console.ResetColor();
            return;
        }

        Console.Write($"Новое имя (Enter — оставить {entry.Name}): ");
        var name = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(name)) entry.Name = name;

        Console.Write($"Новая дата (дд.мм.гггг, Enter — оставить {entry.DateOfBirth:dd.MM.yyyy}): ");
        var dateStr = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(dateStr) &&
            DateTime.TryParseExact(dateStr, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out var dob))
        {
            entry.DateOfBirth = dob;
        }

        db.SaveChanges();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Запись успешно обновлена.");
        Console.ResetColor();
    }

    public void DeleteBirthday(BirthdayContext db)
    {
        ShowAll(db, pauseAfter: false);
        Console.Write("\nВведите ID для удаления: ");
        if (!int.TryParse(Console.ReadLine(), out var id))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Некорректный ввод.");
            Console.ResetColor();
            return;
        }

        var entry = db.Birthdays.Find(id);
        if (entry == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Запись не найдена.");
            Console.ResetColor();
            return;
        }

        db.Birthdays.Remove(entry);
        db.SaveChanges();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Запись успешно удалена.");
        Console.ResetColor();
    }
}
