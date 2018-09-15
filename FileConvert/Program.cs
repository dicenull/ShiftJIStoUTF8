using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Newtonsoft.Json;

namespace ShiftJIStoUTF8
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // 変換する拡張子を読み込み
            var path = File.ReadAllText("./extentions.json");
            var extentions = JsonConvert.DeserializeObject<List<string>>(path);
            
            // ルートフォルダの設定
            var dialog = new FolderBrowserDialog();
            dialog.Description = "エンコードを変更するファイルがあるフォルダを指定";
            dialog.RootFolder = Environment.SpecialFolder.Desktop;

            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            var folder = dialog.SelectedPath;

            var files =
                Directory.EnumerateFiles(folder, "*", SearchOption.AllDirectories);

            var noBomUtf8 = new UTF8Encoding(false);

            // 各ファイルをShiftJISからUTF-8に変換
            foreach (var file in files)
            {
                var extention = Path.GetExtension(file);

                if(! extentions.Any(ext => ext == extention))
                {
                    continue;
                }

                var text = File.ReadAllText(file, Encoding.GetEncoding("Shift_JIS"));
                Console.WriteLine($"Convert : {file}");
                File.WriteAllText(file, text, noBomUtf8);
            }
        }
    }
}