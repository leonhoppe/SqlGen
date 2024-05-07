namespace SqlGen.Cli.Subroutines;

public interface ISubroutine {
    public Task Execute(string cmdArg);
}