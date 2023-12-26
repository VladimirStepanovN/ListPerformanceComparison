using System.Diagnostics;

namespace ListPerformanceComparison;

internal class Program
{
    static void Main(string[] args)
    {
        List<string> list = new();
        LinkedList<string> linkedList = new();
        if (args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
        {
            try
            {
                string[] text;

                #region считываем строку, сразу удаляем пунктуацию и разбиваем в массив text, чтобы не влиять на производительность в замерах
                using (StreamReader reader = File.OpenText(args[0]))
                {
                    text = (new string(reader.ReadToEnd().Where(c => !char.IsPunctuation(c)).ToArray())).Split();
                }
                #endregion

                #region запуск замера добавления в конец List
                var stopWatch = Stopwatch.StartNew();
                foreach (string line in text)
                {
                    list.Add(line);
                }
                stopWatch.Stop();
                var listResultLast = stopWatch.Elapsed.TotalMilliseconds;
                #endregion

                #region запуск замера добавления в конец LinkedList
                stopWatch = Stopwatch.StartNew();
                foreach (string line in text)
                {
                    linkedList.AddLast(line);
                }
                stopWatch.Stop();
                var linkedListResultLast = stopWatch.Elapsed.TotalMilliseconds;
                #endregion

                int index = list.FindIndex(e => e == "всплеснув") + 1; //определяем индекс для вставки в середину List
                var node = linkedList.Find("всплеснув"); //определяем ноду для вставки в середину LinkedList

                #region запуск замера добавления в середниу List
                stopWatch = Stopwatch.StartNew();
                list.Insert(index, "Тест");
                stopWatch.Stop();
                var listResultMiddle = stopWatch.Elapsed.TotalMilliseconds;
                #endregion

                #region запуск замера добавления в середину LinkedList
                stopWatch = Stopwatch.StartNew();
                linkedList.AddAfter(node, "Тест");
                stopWatch.Stop();
                var linkedListResultMiddle = stopWatch.Elapsed.TotalMilliseconds;
                #endregion

                Console.WriteLine($"Скорость записи файла в конец List: {listResultLast}");
                Console.WriteLine($"Скорость записи файла в конец LinkedList: {linkedListResultLast}");
                Console.WriteLine();
                Console.WriteLine($"Скорость записи файла в середину List: {listResultMiddle}");
                Console.WriteLine($"Скорость записи файла в середину LinkedList: {linkedListResultMiddle}");
                Console.Read();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"Файл {args[0]} не найден.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Путь к файлу или папке содержит недопустимые символы.");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Недопустимое действие или недостаточно прав на чтение файлов.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}