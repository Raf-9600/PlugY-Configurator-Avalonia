using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace PlugY_Configurator_Avalonia.Models
{
    class MainModel
    {
        OperatingSystemType _detectedOS = DetectOS();
        /*
        public string UrlNewVer;
        public async Task<bool> UpdateFind()
        {
            string updatePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Raf-9600", "PlugY Configurator");
            string updateUrl = @"https://raw.githubusercontent.com/Raf-9600/PlugY-Configurator/master/PlugY%20Configurator/update.json";

            string updateFile = Path.Combine(updatePath, "Update.json");

#if RELEASE
            if (File.Exists(updateFile))
            {
                DateTime dateFile = File.GetLastWriteTime(updateFile).AddDays(7);
                DateTime dateThis = DateTime.Today;

                if (dateThis.Date > dateFile.Date)
                    try { await DownloadFileAsync(updateUrl, updateFile); }
                    catch (System.Exception) { }
            }
            else 
                try { await DownloadFileAsync(updateUrl, updateFile); }
                catch (System.Exception) { }
#endif

            UpdateStruct updateJson = JsonSerializer.Deserialize<UpdateStruct>(File.ReadAllText(updateFile));
            Version versionExe = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            Version versionJson = new Version(updateJson.Ver);

            if (versionJson > versionExe)
            {
                if (Environment.Is64BitProcess)
                    UrlNewVer = updateJson.Url;
                else UrlNewVer = updateJson.UrlX86;

                return true;
            }


            return false;
        }

        [Serializable]
        public struct UpdateStruct
        {
            public string Url { get; set; }
            public string UrlX86 { get; set; }
            public string Ver { get; set; }

            public UpdateStruct(string url, string urlX86, string ver)
            {
                Url = url;
                UrlX86 = urlX86;
                Ver = ver;
            }
        }


        HttpClient? httpClient = null;
        public async Task DownloadFileAsync(string url, string destPath, IProgress<double>? progress = null, CancellationToken cancellationToken = default, bool useUniqueName = true)
        {
            string destTemp;
            if (useUniqueName)
            {
                string dir = Path.GetDirectoryName(destPath);
                string fileName = Path.GetFileNameWithoutExtension(destPath);
                string ext = Path.GetExtension(destPath);

                destTemp = GetUniqueFileNameFromPath(dir, ext, fileName + "_");
            }
            else destTemp = destPath;


            httpClient ??= new HttpClient();
            using (HttpResponseMessage response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false))
            {
                using (FileStream fstream = new FileStream(destTemp, FileMode.CreateNew))
                    if (progress != null)
                    {
                        long length = response.Content.Headers.ContentLength ?? -1;
                        byte[] buffer = new byte[1048576];
                        int read;
                        int totalRead = 0;

                        using (Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                            while ((read = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) > 0)
                            {
                                await fstream.WriteAsync(buffer, 0, read, cancellationToken).ConfigureAwait(false);

                                totalRead += read;
                                progress?.Report((double)totalRead / length * 100);
                            }
                    }
                    else
                    {
                        if (cancellationToken == default)
                            await response.Content.CopyToAsync(fstream).ConfigureAwait(false);
                        else
                            using (Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                                await stream.CopyToAsync(fstream, 1048576, cancellationToken).ConfigureAwait(false);
                    }
            }
            if (useUniqueName)
                File.Move(destTemp, destPath, true);

        }

        public string GetUniqueFileNameFromPath(string pathDir, string ext = "", string prefix = "", string sufix = "")
        {
            string result;
            do
            {
                string tmpFull = Path.GetRandomFileName();
                string tmpName = Path.GetFileNameWithoutExtension(tmpFull);

                if (string.IsNullOrEmpty(ext))
                    ext = Path.GetExtension(tmpFull);

                result = Path.Combine(pathDir, prefix + tmpName + sufix + ext);
            }
            while (File.Exists(result));

            return result;
        }

        private bool DownloadNewFile(string WebPath, string DestPath)
        {
            string dp = Path.GetDirectoryName(DestPath);
            string destPathTemp = Path.Combine(dp, Path.GetRandomFileName());
            Directory.CreateDirectory(dp);
            File.Delete(destPathTemp);
            try
            {
                using System.Net.WebClient client = new System.Net.WebClient();
                client.DownloadFile(WebPath, destPathTemp);
            }
            catch (Exception)
            { }

            if (File.Exists(destPathTemp))
            {
                File.Delete(DestPath);
                File.Move(destPathTemp, DestPath);
                return true;
            }
            return false;
        }


        public void SelfUpdate(string newExeFullPath)
        {
            string thisExeFullPath = Process.GetCurrentProcess().MainModule.FileName;

            string batPath = GetUniqueFileNameFromPath(Path.GetTempPath(), ".bat", "Uninstall_");
            string batStr =
                $"set /a PID={Process.GetCurrentProcess().Id}" + Environment.NewLine +
                $"taskkill /fi \"/pid eq  %PID%\" | find \"{Process.GetCurrentProcess().ProcessName}\"" + Environment.NewLine +
                $":LOOP" + Environment.NewLine +
                $"tasklist | find /i %PID% >nul 2>&1" + Environment.NewLine +
                $"IF ERRORLEVEL 1 (" + Environment.NewLine +
                $"  GOTO CONTINUE" + Environment.NewLine +
                $") ELSE (" + Environment.NewLine +
                $"  ECHO Uninstall is still running" + Environment.NewLine +
                $"  Timeout /T 1 /Nobreak" + Environment.NewLine +
                $"  GOTO LOOP" + Environment.NewLine +
                $")" + Environment.NewLine +
                $":CONTINUE " + Environment.NewLine +
                $"del /Q /F \"{thisExeFullPath}\"" + Environment.NewLine +
                $"move /Y \"{newExeFullPath}\" \"{thisExeFullPath}\"" + Environment.NewLine +
                $"start \"Uninstall\" \"{thisExeFullPath}\" \"UpdateSuccessfully\"" + Environment.NewLine +
                $"del /Q /F \"{newExeFullPath}\"" + Environment.NewLine +
                $"del /Q /F \"{batPath}\"";

            File.WriteAllText(batPath, batStr);
            StartProcess(@"cmd.exe", $"/k \"{batPath}\"", string.Empty, true, false);
            Environment.Exit(0);
        }

        public static void StartProcess(string pathExe, string param, string workDir = "", bool hidden = true, bool wait = true)
        {
            Process prc = new Process();
            if (hidden)
            {
                prc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                prc.StartInfo.UseShellExecute = false;
                prc.StartInfo.CreateNoWindow = true;
            }

            if (!string.IsNullOrEmpty(workDir))
                prc.StartInfo.WorkingDirectory = workDir;

            prc.StartInfo.Arguments = param;
            prc.StartInfo.FileName = pathExe;
            prc.Start();
            //prc.CloseMainWindow();

            if (wait)
                prc.WaitForExit();
        }
        */




        public string FindPlugyIni(IEnumerable<string>? filesArray)
        {
            if (filesArray != null)
                foreach (var fle in filesArray)
                {
                    string fleString = Path.GetFileName(fle);

                    if (fleString.Equals("PlugY.ini", StringComparison.OrdinalIgnoreCase) && File.Exists(fle))
                        return fle;
                }

            return string.Empty;
        }


        public string FindWorkDir(string nameFile)
        {
            string fullPath = GetFileIgnoreCase(Directory.GetCurrentDirectory(), nameFile);

            if (File.Exists(fullPath))
                return Directory.GetCurrentDirectory();


            string? setpPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            if(!string.IsNullOrEmpty(setpPath))
            {
                string fullPath1 = GetFileIgnoreCase(setpPath, nameFile);

                if (File.Exists(fullPath1))
                    return setpPath;
            }

            return string.Empty;
        }


        public string FindInstalledDiablo2()
        {
#pragma warning disable CA1416 // Проверка совместимости платформы
            if(_detectedOS == OperatingSystemType.WinNT)
            {
                RegistryKey? registryHKCU = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);
                if (registryHKCU != null)
                {
                    RegistryKey? regD2cu = registryHKCU.OpenSubKey(@"Software\Blizzard Entertainment\Diablo II");
                    if (regD2cu != null)
                    {
                        var result = regD2cu.GetValue("InstallPath");
                        if (result != null)
                        {
                            string pathInstalledDiablo2 = (string)result;
                            string d2dataPath = GetFileIgnoreCase(pathInstalledDiablo2, "d2data.mpq", true);

                            if (!string.IsNullOrEmpty(d2dataPath))
                                return pathInstalledDiablo2;
                        }
                    }
                }
            }
#pragma warning restore CA1416 // Проверка совместимости платформы

            return string.Empty;
        }

        readonly EnumerationOptions _enumerationOptions = new EnumerationOptions { MatchCasing = MatchCasing.CaseInsensitive, ReturnSpecialDirectories = false };
        public string GetFileIgnoreCase(string path, string nameFile, bool canReturnEmpty = false)
        {
            string? result = null;

            if (!string.IsNullOrEmpty(path) && Directory.Exists(path))
                result = Directory.EnumerateFiles(path, nameFile, _enumerationOptions).FirstOrDefault();

            if (!canReturnEmpty && string.IsNullOrEmpty(result))
                result = Path.Combine(path, nameFile);

            return result ?? string.Empty;
        }

        /*
        public async string DlgFindFile(string fileName, string filter, string defaultExt = null, string initialDirectory = null)
        {


            /*
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            if (!string.IsNullOrEmpty(initialDirectory))
                dlg.InitialDirectory = initialDirectory;

            if (!string.IsNullOrEmpty(defaultExt))
                dlg.DefaultExt = defaultExt;

            dlg.FileName = fileName;
            dlg.Filter = filter;
            dlg.FilterIndex = 0;
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
                return dlg.FileName;

            return string.Empty; 
        } */

        public string ActAddRemove(string mainStr, string newStr)
        {
            mainStr = mainStr.Replace("-act 1", "");
            mainStr = mainStr.Replace("-act 2", "");
            mainStr = mainStr.Replace("-act 3", "");
            mainStr = mainStr.Replace("-act 4", "");
            mainStr = mainStr.Replace("-act 5", "");

            if (newStr == "-act 1")
                return mainStr;

            return mainStr + " " + newStr;
        }

        public string AddParam(string mainStr, string newStr)
        {
            if (mainStr.Contains(newStr))
                return mainStr;
            else return mainStr + " " + newStr;
        }

#pragma warning disable CA1416 // Проверка совместимости платформы
        private bool? _glideWindowed;
        public void GlideWindowed(bool? enab)
        {
            if (enab == _glideWindowed)
                return;
            _glideWindowed = enab;

            if (_detectedOS == OperatingSystemType.WinNT)
            {
                using RegistryKey? registryHKCU = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);
                if (registryHKCU != null)
                {
                    RegistryKey? regGlideCU = registryHKCU.CreateSubKey(@"SOFTWARE\GLIDE3toOpenGL");
                    if (regGlideCU != null)
                        regGlideCU.SetValue("windowed", Convert.ToInt32(enab));
                }
            }
        }
#pragma warning restore CA1416 // Проверка совместимости платформы

        public bool IsFileWritable(string filePath, bool throwIfFails = false)
        {
            try
            {
                using (FileStream fs = File.OpenWrite(filePath))
                { }
                return true;
            }
            catch
            {
                if (throwIfFails)
                    throw;
                else
                    return false;
            }
        }

        public bool? DetectDarkTheme()
        {
#pragma warning disable CA1416 // Проверка совместимости платформы
            if (_detectedOS == OperatingSystemType.WinNT)
                using (RegistryKey? regHKCU = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", false))
                    if (regHKCU != null)
                        return !Convert.ToBoolean(regHKCU.GetValue("AppsUseLightTheme"));
#pragma warning restore CA1416 // Проверка совместимости платформы

            return null;
        }

        public string DllToLoad(string masetStr, string[] newStr, string currentDir)
        {
            HashSet<string>? dllList;
            if (masetStr.EndsWith(".dll"))
                dllList = masetStr.Trim().Split('|').ToHashSet(StringComparer.OrdinalIgnoreCase);


            foreach (string file in newStr)
            {
                string f = file.Remove(0, currentDir.Length);
                f = f.Trim(Path.DirectorySeparatorChar);
                f = f.Replace('/', '\\');

                if (masetStr.EndsWith(".dll"))
                {
                    dllList = masetStr.Trim().Split('|').ToHashSet(StringComparer.OrdinalIgnoreCase);
                    dllList.Add(f);
                    masetStr = string.Join<string>("|", dllList);
                }
                else
                    masetStr += f;
            }

            return masetStr;
        }

        public static async Task<string> OpenFolderDialog(Window targetWindow, string title = "", string startOpen = "")
        {
            var storageProvider = targetWindow.StorageProvider;

            var options = new FolderPickerOpenOptions
            {
                SuggestedStartLocation = await storageProvider.TryGetFolderFromPathAsync(startOpen),
                Title = title,
                AllowMultiple = false
            };

            IReadOnlyList<IStorageFolder> result = await storageProvider.OpenFolderPickerAsync(options);
            return result[0].Path.LocalPath;
        }

        

        public static async Task<string[]?> OpenFileDialog(Window? targetWindow, (string name, string pattern, string mime)[] fTypes, string title = "", string startOpen = "", bool multiFiles = false)
        {
            if (targetWindow == null) return null;

            var storageProvider = targetWindow.StorageProvider;

            var options = new FilePickerOpenOptions
            {
                SuggestedStartLocation = await storageProvider.TryGetFolderFromPathAsync(startOpen),
                Title = title,
                AllowMultiple = multiFiles
            };


            var fpft = new FilePickerFileType[fTypes.Length];
            for (int i = 0; i < fTypes.Length; i++)
            {
                var (name, pattern, mime) = fTypes[i];
                FilePickerFileType filePickerFileType = new(name) { Patterns = new[] { pattern } };

                if (!string.IsNullOrEmpty(mime))
                    filePickerFileType.MimeTypes = new[] { mime };

                fpft[i] = filePickerFileType;
            }


            options.FileTypeFilter = fpft;
            IReadOnlyList<IStorageFile> result = await storageProvider.OpenFilePickerAsync(options);


            if (result.Count == 0)
                return null;
            else
                return result.Select(v => v.Path.LocalPath).ToArray();
        }

        public static OperatingSystemType DetectOS()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return OperatingSystemType.WinNT;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return OperatingSystemType.OSX;
            else // if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return OperatingSystemType.Linux;
        }

    }
}
