namespace SqlGen.Cli.Utils.Menu;

public sealed class Multiselect : IConsoleMenu<string[], int> {
    public ConsoleColor SelectedColor { get; set; } = ConsoleColor.DarkCyan;

    private int _currentIndex = 0;
    
    public async Task<int> Present(string[] options) {
        _currentIndex = 0;
        PrintMenu(options);

        while (true) {
            var input = Console.ReadKey();

            switch (input.Key) {
                case ConsoleKey.UpArrow:
                    if (_currentIndex > 0) _currentIndex--;
                    break;
            
                case ConsoleKey.DownArrow:
                    if (_currentIndex < options.Length - 1) _currentIndex++;
                    break;
            
                case ConsoleKey.Enter:
                    PrintMenu(options, true, true);
                    return _currentIndex;
            }
            
            PrintMenu(options, true);
        }
    }

    private void PrintMenu(string[] options, bool reprint = false, bool selected = false) {
        if (reprint) {
            Console.SetCursorPosition(0, Console.CursorTop - options.Length);
        }
        
        for (int i = 0; i < options.Length; i++) {
            var option = options[i];

            if (i == _currentIndex && !selected) {
                Console.ForegroundColor = SelectedColor;
            } else {
                Console.ResetColor();
            }
            
            Console.Write((selected && i == _currentIndex ? "\u2713" : ">") + " " + option + "\n");
        }
        
        Console.ResetColor();
    }
}