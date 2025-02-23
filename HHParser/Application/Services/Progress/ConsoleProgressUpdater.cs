using HHParser.Application.Interfaces.Progress;

namespace HHParser.Application.Services.Progress
{
    public class ConsoleProgressUpdater : IProgressUpdater
    {
        private readonly string _description;
        private readonly double _total;
        private double _current;

        public ConsoleProgressUpdater(string description, double total)
        {
            _description = description;
            _total = total;
            _current = 0;
            Render();
        }

        public void Increment(double amount)
        {
            _current += amount;
            Render();
        }

        private void Render()
        {
            double percentage = Math.Min(100, (_current / _total) * 100);
            // Можно добавить красивую визуализацию полоски, например, используя символы
            int barWidth = 30; // ширина полосы
            int progressBlocks = (int)((percentage / 100) * barWidth);
            string bar = new string('█', progressBlocks).PadRight(barWidth, '-');

            // Формируем строку прогресса
            string progressText = $"{_description} [{bar}] {percentage:0}%";
            // Обновляем строку (перевод каретки "\r" возвращает курсор в начало строки)
            Console.Write("\r" + progressText);
        }
    }

}
