internal class Program
{

    static List<String> listIds = new List<String>();

    private static void Main(string[] args)
    {

        String result = "";

        result += "<Fragment>\r\n\t\t<ComponentGroup Id=\"ProductComponents\" Directory=\"INSTALLFOLDER\">\r\n\t\t\t<Component Id=\"Product\" Guid=\"107a594b-40cb-4622-a6e9-8e36c42f4e4b\">";

        result += GenerateList("D:\\GitHub\\sysadmin\\src\\Sysadmin\\bin\\Release\\net8.0-windows7.0", "");

        result += "</Component>\r\n\t\t</ComponentGroup>\r\n\r\n\t\t<ComponentGroup Id=\"ProductRuntimeWin\" Directory=\"WINLIBNET\">\r\n\t\t\t<Component Id='WinRuntime' Guid='3C7F95A4-96CC-46A8-A633-F797D8807B0C'>";

        result += GenerateList("D:\\GitHub\\sysadmin\\src\\Sysadmin\\bin\\Release\\net8.0-windows7.0\\runtimes\\win\\lib\\net8.0", "\\runtimes\\win\\lib\\net8.0\\");

        result += "</Component>\r\n\t\t</ComponentGroup>\r\n\r\n\t\t<ComponentGroup Id=\"ProductRuntimeAny\" Directory=\"NETCORE3\">\r\n\t\t\t<Component Id='AnyRuntime' Guid='3C7F95A4-96CC-46A8-A633-F797D8807B0D'>";

        result += GenerateList("D:\\GitHub\\sysadmin\\src\\Sysadmin\\bin\\Release\\net8.0-windows7.0\\runtimes\\any\\lib\\netcoreapp3.0", "\\runtimes\\any\\lib\\netcoreapp3.0\\");

        result += "</Component>\r\n\t\t</ComponentGroup>\r\n\r\n\t</Fragment>";

        System.Console.WriteLine(result);

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
            string id = fileInfo.Name;
            string name = fileInfo.Name;
            string fileName = fileInfo.Name;

            if (listIds.Contains(id))
                id = id + "1";

            string line = "<File Id=\"" + id + "\" Name=\"" + name + "\" Source=\"$(var.Sysadmin_TargetDir)" + source + fileName + "\" />";
            System.Console.WriteLine(line);
            list.Add(line);
            listIds.Add(id);
        }

        string result = String.Join("\n", list);
        return result;
    }

}