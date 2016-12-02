using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System;

namespace hammer_backstock
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Ensure I'm the only hammer
                if (Process.GetProcessesByName("hammer").Count() > 0)
                {
                    MessageBox.Show("Backstock Hammer can only support one instance at a time");
                    return;
                }

                // Move configurations into place
                if (File.Exists("CmdSeq.wc")) File.Copy("CmdSeq.wc", "CmdSeq_original.wc", true);
                if (File.Exists("CmdSeq_backstock.wc")) File.Copy("CmdSeq_backstock.wc", "CmdSeq.wc", true);

                if (File.Exists("GameConfig.txt")) File.Copy("GameConfig.txt", "GameConfig_original.txt", true);
                if (File.Exists("GameConfig_backstock.txt")) File.Copy("GameConfig_backstock.txt", "GameConfig.txt", true);

                // Start hammer
                var hammer = new Process();
                hammer.StartInfo.FileName = "hammer.exe";
                hammer.StartInfo.Arguments = "-nop4";
                hammer.Start();
                hammer.WaitForExit();

                // Restore original configurations
                if (File.Exists("CmdSeq.wc")) File.Copy("CmdSeq.wc", "CmdSeq_backstock.wc", true);
                if (File.Exists("CmdSeq_original.wc"))
                {
                    File.Copy("CmdSeq_original.wc", "CmdSeq.wc", true);
                    File.Delete("CmdSeq_original.wc");
                }

                if (File.Exists("GameConfig.txt")) File.Copy("GameConfig.txt", "GameConfig_backstock.txt", true);
                if (File.Exists("GameConfig_original.txt"))
                {
                    File.Copy("GameConfig_original.txt", "GameConfig.txt", true);
                    File.Delete("GameConfig_original.txt");
                }
            }
            catch (Exception ex)
            {
                File.AppendAllText("hammer_backstock_exceptions.txt", ex.ToString() + Environment.NewLine + Environment.NewLine + new string('=', 128));
            }
        }
    }
}
