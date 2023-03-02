using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using ImGuiNET;
using Silk.NET.Input;
using Silk.NET.Input.Glfw;
using Silk.NET.Input.Sdl;
using Silk.NET.Maths;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Glfw;
using Silk.NET.Windowing.Sdl;
using Monitor = Silk.NET.Windowing.Monitor;

namespace Plutonium.Framework;

public class Render
{
    public delegate void ImguiRenderDelegate();

    public ImGuiController Controller;

    public GL Gl;

    public int Height, Width;

    public IInputContext InputContext;

    public string? Title;

    public IWindow Window;

    /// <summary>
    ///     Create new Render instance
    /// </summary>
    /// <param name="title"></param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    public Render(string title, int width, int height)
    {
        var options = WindowOptions.Default;
        options.WindowBorder = WindowBorder.Hidden;
        options.Size = new Vector2D<int>(width, height);

        options.API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Compatability, ContextFlags.Default,
            new APIVersion(4, 0)); //Lower this if not working!

        Window = Silk.NET.Windowing.Window.Create(options);

        Width = width;
        Height = height;
        Title = title;

        Window.Load += WindowOnLoad;
        Window.FramebufferResize += WindowOnFramebufferResize;
        Window.Render += WindowOnRender;
    }

    [DllImport("user32.dll")]
    private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

    [DllImport("user32.dll")]
    private static extern bool ReleaseCapture();

    public event ImguiRenderDelegate ImguiRender;


    public static void Initialize()
    {
        SdlWindowing.RegisterPlatform();
        SdlInput.RegisterPlatform();
        GlfwWindowing.RegisterPlatform();
        GlfwInput.RegisterPlatform();
    }

    public void RunWindow()
    {
        Window.Run();
    }

    private void WindowOnRender(double obj)
    {
        Controller.Update((float)obj);

        Gl.ClearColor(Color.FromArgb(255, (int)(.45f * 255), (int)(.55f * 255), (int)(.60f * 255)));
        Gl.Clear((uint)ClearBufferMask.ColorBufferBit);

        ImguiRender();

        Controller.Render();

        Thread.Sleep(8);
    }

    private void WindowOnFramebufferResize(Vector2D<int> obj)
    {
        Gl.Viewport(obj);
    }

    private void WindowOnLoad()
    {
        Controller = new ImGuiController(Gl = Window.CreateOpenGL(),
            Window, InputContext = Window.CreateInput());
        Window.Center();
        InputContext.Mice[0].MouseDown += OnMouseDown;

        unsafe
        {
            Console.WriteLine("--------------------Render Information--------------------");
            Console.WriteLine(
                $"Screen Resolution [{Monitor.GetMainMonitor(Window).Bounds.Size.X},{Monitor.GetMainMonitor(Window).Bounds.Size.Y + 40}]");
            var api = Window.API.API.ToString();
            Console.WriteLine($"Render API              {api}");
            Console.WriteLine(
                $"Render API Version      {Window.API.Version.MajorVersion}.{Window.API.Version.MinorVersion}");
            Console.WriteLine($"Video Render Device     {new string((sbyte*)Gl.GetString(StringName.Renderer))}");
            Console.WriteLine($"Video Vendor            {new string((sbyte*)Gl.GetString(StringName.Vendor))}");
            Console.WriteLine($"Video Driver            {new string((sbyte*)Gl.GetString(StringName.Version))}");
            Console.WriteLine($"ImGui Version           {ImGui.GetVersion()}");
            Console.WriteLine("----------------------------------------------------------");
        }
    }

    private void OnMouseDown(IMouse arg1, MouseButton arg2)
    {
        if (arg2 == MouseButton.Left)
        {
            var pos = ImGui.GetMousePos();

            if (pos.Y <= 20)
            {
                var p = Process.GetCurrentProcess();
                DragMove(p.MainWindowHandle);
            }

            if (pos.X >= Width - 20 && pos.Y <= 20)
                Environment.Exit(0);
        }
    }

    private void DragMove(IntPtr hWnd)
    {
        ReleaseCapture();
        SendMessage(hWnd, 0xA1, 0x2, 0);
    }
}