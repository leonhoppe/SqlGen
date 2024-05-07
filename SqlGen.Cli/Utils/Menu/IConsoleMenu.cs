namespace SqlGen.Cli.Utils.Menu;

public interface IConsoleMenu<in TInput, TOutput> {
    public Task<TOutput> Present(TInput argument);
}