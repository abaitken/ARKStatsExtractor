using ARKBreedingStats.Library;
using ARKBreedingStats.settings;
using ARKBreedingStats.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ARKBreedingStats
{
    partial class Program
    {
        static int RunCommandLine(string[] args)
        {
            return new Program_CommandLine().Run(args);
        }
    }

    internal class Program_CommandLine
    {
        internal int Run(string[] args)
        {
            if (args.Length == 0)
            {
                DisplayHelp();
                return 0;
            }

            var command = args[0];
            var options = FindOptions(args);
            if (command == "help" || options.ContainsKey("h") || options.ContainsKey("help"))
            {
                DisplayHelp();
                return 0;
            }

            var noload = options.ContainsKey("noload");

            if (!options.TryGetValue("o", out var outputLibraryOptions))
            {
                Console.WriteLine("Output library must be specified.");
                return 1;
            }

            var outputLibrary = outputLibraryOptions.FirstOrDefault();

            if (string.IsNullOrEmpty(outputLibrary))
            {
                Console.WriteLine("Output library must be specified.");
                return 1;
            }

            if (!options.TryGetValue("g", out var saveGamesOptions))
            {
                Console.WriteLine("Save games must be specified.");
                return 1;
            }

            var saveGames = (from item in saveGamesOptions
                             let parts = item.Split(new[] { ';' }, 2)
                             let serverName = parts.Length == 1 ? Path.GetFileNameWithoutExtension(item) : parts[0]
                             let savefile = parts[parts.Length - 1]
                             select new
                             {
                                 serverName,
                                 savefile
                             }).ToList();

            foreach (var item in saveGames)
            {
                if (!File.Exists(item.savefile))
                {
                    Console.WriteLine($"Save game ('{item.serverName}') '{item.savefile}' not found.");
                    return 1;
                }
            }

            MessageBoxes.Builder = new CommandLineMessages();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form = new Form_Accessor();
            form.Init();

            if (!noload && File.Exists(outputLibrary))
                form.LoadLibrary(outputLibrary);

            //var settings = form.CreateSettingsDialog();
            //settings.ImportSettings(modifiers);

            foreach (var item in saveGames)
                form.ImportCreatures(item.savefile, item.serverName);

            form.SaveLibrary(outputLibrary);

            return 0;
        }

        private Dictionary<string, List<string>> FindOptions(string[] args)
        {
            var result = new Dictionary<string, List<string>>();

            var switches = new[] { '-', '/' };

            // Look for switch-value pairs
            // Add to a dictionary where the key is the switch and the value is a list of values
            // Lone values are added under string.empty keys
            // Lone switches are added without empty values
            for (int i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                if (arg.Length == 0)
                    continue;
                var isSwitch = switches.Any(o => arg[0] == o);
                var key = isSwitch ? arg.Substring(1) : string.Empty;

                i++;
                var value = string.Empty;

                if (i < args.Length)
                {
                    value = args[i];
                    isSwitch = value.Length != 0 && switches.Any(o => value[0] == o);
                    value = isSwitch ? string.Empty : value;

                    if (isSwitch)
                        i--;
                }

                if (!result.TryGetValue(key, out var values))
                {
                    values = new List<string>();
                    result.Add(key, values);
                }

                if (!string.IsNullOrEmpty(value))
                    values.Add(value);
            }

            return result;
        }

        private void DisplayHelp()
        {
            Console.WriteLine(@"
ARKBreedingStats.exe command options...

Commands:
    import     Import from game data and save to library

Options:
    -g ""PATH_TO_ARK.ark""
    -g ""SERVER_NAME;PATH_TO_ARK.ark""
                         Game file to import. Multiple files can be specified

    -o ""PATH_TO_ASB.asb""
                         Destination to save library. If it already exists it will be loaded first, unless -noload is used

    -noload              
                         Skip loading of library, effectively overwrite the library with a new file

");
        }

        private class Settings_Accessor : Settings
        {
            public Settings_Accessor(CreatureCollection cc)
                : base(cc, SettingsTabPages.General)
            {
            }

            internal void ImportSettings(string files)
            {
                ExtractSettingsFromFile(files);
            }
        }

        private class Form_Accessor : Form1
        {
            public Form_Accessor()
            {
                Font = new System.Drawing.Font(Properties.Settings.Default.DefaultFontName, Properties.Settings.Default.DefaultFontSize);
            }

            public void Init()
            {
                // Prevent the prompt to update
                Properties.Settings.Default.lastUpdateCheck = DateTime.Now;
                // Prevent the prompt for unknown species
                Properties.Settings.Default.IgnoreUnknownBlueprintsOnSaveImport = true;
                Form1_Load(this, EventArgs.Empty);
                NewCollection(true);
            }

            internal Settings_Accessor CreateSettingsDialog()
            {
                return new Settings_Accessor(_creatureCollection);
            }

            internal bool ImportCreatures(string gameFile, string server)
            {
                ATImportFileLocation atImportFileLocation = new ATImportFileLocation("notused", server, gameFile);
                var importTask = Task.Run(() => RunSavegameImport(atImportFileLocation));
                importTask.Wait();
                var error = importTask.Result;
                return true;
            }

            internal void LoadLibrary(string library)
            {
                NewCollection(true);
                LoadCollectionFile(library);
            }

            internal void SaveLibrary(string library)
            {
                SaveCollectionToFileName(library);
            }
        }

        private class CommandLineMessages : MessageBoxes.MessageBoxBuilder
        {
            public override void ShowMessageBox(string message, string title = null, MessageBoxIcon icon = MessageBoxIcon.Hand, bool displayCopyMessageButton = false)
            {
                if (string.IsNullOrEmpty(title))
                {
                    switch (icon)
                    {
                        case MessageBoxIcon.Warning:
                            title = "Warning";
                            break;
                        case MessageBoxIcon.Information:
                            title = "Info";
                            break;
                        default:
                            title = Loc.S("error");
                            break;
                    }
                }

                Console.WriteLine(title + message);
            }

            public override void ExceptionMessageBox(Exception ex, string messageBeforeException = null, string title = null)
            {
                string message = ex.Message
                                 + "\n\n" + ex.GetType() + " in " + ex.Source
                                 + "\n\nMethod throwing the error: " + ex.TargetSite.DeclaringType?.FullName + "." +
                                 ex.TargetSite.Name
                                 + "\n\nStackTrace:\n" + ex.StackTrace
                                 + (ex.InnerException != null
                                     ? "\n\nInner Exception:\n" + ex.InnerException.Message
                                     : string.Empty);

                ShowMessageBox((string.IsNullOrEmpty(messageBeforeException) ? string.Empty : messageBeforeException + "\n\n") + message, title, displayCopyMessageButton: true);
            }
        }
    }
}