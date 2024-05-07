namespace SqlGen.Cli.Utils.Menu;

public sealed class Prompt<TOutput> {
    
    public async Task<TOutput> Present(string prompt, TOutput defaultValue = default) {
        if (EqualityComparer<TOutput>.Default.Equals(defaultValue, default)) {
            Console.Write($"> {prompt}: ");
        } else {
            Console.Write($"> {prompt} ({defaultValue}): ");
        }

        var result = Console.ReadLine();

        if (string.IsNullOrEmpty(result)) return defaultValue;
        
        return (TOutput)Convert.ChangeType(result, typeof(TOutput));
    }
    
}