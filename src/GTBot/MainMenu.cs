using GTBot.Common;

using ImGuiNET;
using GTBot;
using System.Numerics;
using Plutonium.Framework;

namespace GTProxy
{
    internal static class MainMenu
    {

        private static Render? _render;

        private static bool _open = true;

        private static string _tankIdNameStr = "";

        private static string _tankPassStr = "";

        private static string worldname = "";




        

        public static void Run()
        {
            new Thread(() =>
            {
                _render = new Render("Kultanen Mallu#4164", 750, 500);
                _render.ImguiRender += RenderOnRenderCall;
                _render.RunWindow();
                
            }).Start();
        }


        private static void RenderOnRenderCall()
        {

            ImGui.SetNextWindowSize(new Vector2(_render.Width, _render.Height));
            ImGui.SetNextWindowPos(new Vector2(0, 0));
            ImGui.Begin(_render.Title, ref _open,
                    ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoDocking | ImGuiWindowFlags.NoResize);




            var bot = new Bot(_tankIdNameStr, _tankPassStr);


            ImGui.Text("Hello world");

            if (ImGui.BeginChild("Settings", new Vector2(238, 430), true))
            {


                ImGui.SetNextItemWidth(200);
                /*ImGui.InputTextWithHint("##gid", "growid", ref _tankIdNameStr, 23);
                ImGui.SetNextItemWidth(200);
                ImGui.InputTextWithHint("##gpass", "password", ref _tankPassStr, 23);*/
                ImGui.InputText("##gpid", ref _tankIdNameStr, 23);
                ImGui.InputText("##gpass", ref _tankPassStr, 23);

                if (ImGui.Button("Connect"))
                {
                    bot.connect();
                }
                ImGui.SameLine();
                if (ImGui.Button("Disconnect"))
                {
                    
                    
                }



            }
            ImGui.EndChild();
            ImGui.SameLine();

            if (ImGui.BeginChild("MainMenu", new Vector2(492, 448), true))
            {
                ImGui.InputText("##world", ref worldname, 23);

                if (ImGui.Button("Warp"))
                {
                    bot.SendPacket(3, "action|join_request\nname|" + worldname + "\ninvitedWorld|0");
                }




            }
            ImGui.EndChild();













        }



    }



}
