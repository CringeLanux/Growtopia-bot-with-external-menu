using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using GTBot.Common;


namespace GTBot;

public static class addon
{
    private const int WmNclbuttondown = 0xA1;
    private const int HtCaption = 0x2;

    [DllImport("shlwapi.dll", SetLastError = true, EntryPoint = "#437")]
    private static extern bool IsOS(int os);

    [DllImport("user32.dll")]
    private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    [DllImport("user32.dll")]
    private static extern bool ReleaseCapture();

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    private static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    public static void HideConsole()
    {
        var handle = GetConsoleWindow();
        ShowWindow(handle, 0);
    }


    public static void DragMove(IntPtr hWnd)
    {
        ReleaseCapture();
        SendMessage(hWnd, WmNclbuttondown, HtCaption, 0);
    }

    public static Vector4 Vec_Color(int r, int g, int b, int a = 255)
    {
        return new Vector4(
            r / 255.0f,
            g / 255.0f,
            b / 255.0f,
            a / 255.0f);
    }

    public static string Between(string STR, string FirstString, string LastString)
    {
        string FinalString;
        var Pos1 = STR.IndexOf(FirstString) + FirstString.Length;
        var Pos2 = STR.IndexOf(LastString);
        FinalString = STR.Substring(Pos1, Pos2 - Pos1);
        return FinalString;
    }

    public static string ByteArrayToString(byte[] ba)
    {
        var hex = new StringBuilder(ba.Length * 2);
        foreach (var b in ba)
            hex.AppendFormat("{0:x2}", b);
        return hex.ToString();
    }

    public static byte[] StringToByteArray(string hex)
    {
        var NumberChars = hex.Length;
        var bytes = new byte[NumberChars / 2];
        for (var i = 0; i < NumberChars; i += 2)
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        return bytes;
    }

    public static string GenerateMAC()
    {
        var random = new Random();
        var buffer = new byte[6];
        random.NextBytes(buffer);
        var result = string.Concat(buffer.Select(x => string.Format("{0}:", x.ToString("X2"))).ToArray());
        return result.TrimEnd(':');
    }

    public static string GenerateRID()
    {
        var rand = new Random();
        var str = "0";
        const string chars = "ABCDEF0123456789";
        str += new string(Enumerable.Repeat(chars, 31)
            .Select(s => s[rand.Next(s.Length)]).ToArray());
        return str;
    }

    public static string GenerateUniqueWinKey()
    {
        var rand = new Random();
        var str = "7";
        const string chars = "ABCDEF0123456789";
        str += new string(Enumerable.Repeat(chars, 31)
            .Select(s => s[rand.Next(s.Length)]).ToArray());
        return str;
    }

    public static string FixName(string name)
    {
        return name.Substring(2, name.Length - 4);
    }

    public static uint HashBytes(byte[] b)
    {
        var n = b;
        uint acc = 0x55555555;

        for (var i = 0; i < b.Length; i++) acc = (acc >> 27) + (acc << 5) + n[i];
        return acc;
    }

    public static bool isInside(int circle_x, int circle_y, int rad, int x, int y)
    {
        if ((x - circle_x) * (x - circle_x) + (y - circle_y) * (y - circle_y) <= rad * rad)
            return true;
        return false;
    }

    

   
}