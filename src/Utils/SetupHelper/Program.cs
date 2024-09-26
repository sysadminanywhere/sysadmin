internal class Program
{
    private static void Main(string[] args)
    {
        GenerateList("D:\\GitHub\\sysadmin\\src\\Sysadmin\\bin\\Release\\net6.0-windows", "");

        GenerateList("D:\\GitHub\\sysadmin\\src\\Sysadmin\\bin\\Release\\net6.0-windows\\runtimes\\any\\lib\\netcoreapp3.0", "\\runtimes\\any\\lib\\netcoreapp3.0\\");
        GenerateList("D:\\GitHub\\sysadmin\\src\\Sysadmin\\bin\\Release\\net6.0-windows\\runtimes\\win\\lib\\net6.0", "\\runtimes\\win\\lib\\net6.0\\");

        System.Console.WriteLine("Press any key...");
        System.Console.ReadKey();
    }

    private static string GenerateList(string path, string source)
    {
        List<string> list = new List<string>();

        string[] files = Directory.GetFiles(path);
        foreach (string file in files)
        {
            FileInfo fileInfo = new FileInfo(file);
            string fileName = fileInfo.Name;
            string line = "<File Id=\"" + fileName + "\" Name=\"" + fileName + "\" Source=\"$(var.Sysadmin_TargetDir)" + source + fileName + "\" />";
            System.Console.WriteLine(line);
            list.Add(line);
        }

        string result = String.Join("\n", list);
        return result;
    }

}