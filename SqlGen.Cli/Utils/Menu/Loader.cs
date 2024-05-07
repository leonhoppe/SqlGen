namespace SqlGen.Cli.Utils.Menu;

public sealed class Loader : IConsoleMenu<string, bool> {
    private bool _interrupt;
    private string _message;
    
    public async Task<bool> Present(string message) {
        _message = message;
        _interrupt = false;
        var spinnerChars = new[] { '-', '\\', '|', '/' };
        int currIndex = 0;
        
        while (!_interrupt) {
            Console.WriteLine($"{spinnerChars[currIndex % spinnerChars.Length]} {message}");
            await Task.Delay(100);
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            currIndex++;
        }

        return true;
    }

    public async Task Destroy() {
        _interrupt = true;

        await Task.Delay(105);
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("\u2713 ");
        Console.ResetColor();
        
        Console.WriteLine(_message);
    }
    
}