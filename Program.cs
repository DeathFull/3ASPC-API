namespace _3ASPC_API;

public class Program
{
    static void Main(string[] args)
    {
        Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(builder => { builder.UseStartup<Startup>(); }).Build()
            .Run();
    }
}