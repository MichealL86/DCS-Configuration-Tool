using System;
using System.IO;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using IWshRuntimeLibrary;
using System.Security.AccessControl;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Management;
using System.ComponentModel;
using System.Threading;

namespace DCS_Configuration_Tool
{

    public partial class Form1 : Form
    {
        DirectoryInfo rootDirectory;

        // String array to hold all processes for DCS
        string[] appProcess = { "BSCSimulator", "DCSSimulator", "WindowsFormsApplication1", "MovEmulator",
                                "ModbusTestClient", "ScsAdmacsSim", "ScsDisplay", "SwitchSimulator",
                                "UPSSimulator"};

        string[] processName = {"BSC", "DCS", "ISM", "Mov", "Pickle", "SCS_D", "ScsA", "Switch", "UPS",
                                "AEC", "ROCS", "SNMP"};

        string[] hmapIP = {"172.24.4.1", "172.24.4.2", "172.24.4.3", "172.24.4.4", "172.24.4.5", "172.24.4.6",
                            "172.24.4.7", "172.24.4.8", "172.24.128.10", "172.24.128.11", "172.24.128.16",
                            "172.24.128.50", "172.24.128.51", "172.24.128.52", "172.24.129.10", "172.24.129.11",
                            "172.24.129.16", "172.24.129.50", "172.24.129.51", "172.24.129.52", "172.24.130.35",
                            "172.24.132.1", "172.24.132.2", "172.24.135.1", "172.24.135.2"};

        string[] ags1IP = {"172.20.1.1", "172.20.1.2", "172.20.1.3", "172.20.1.4", "172.20.5.1", "172.20.5.2",
                            "172.20.5.3", "172.20.5.4"};

        string[] ags2IP = { "172.21.1.1", "172.21.1.2", "172.21.1.3", "172.21.1.4", "172.21.5.1", "172.21.5.2",
                            "172.21.5.3", "172.21.5.4" };

        string[] admacsIp = { "172.16.4.10", "172.16.4.11", "172.16.4.222" };

        string[] jctsHmapIP = { "172.24.4.1", "172.24.4.2", "172.24.4.7", "172.24.128.10", "172.24.128.11",
                                "172.24.128.16", "172.24.128.50", "172.24.128.51", "172.24.128.52",
                                "172.24.129.52", "172.24.132.1", "172.24.132.2"};

        string[] jctsAgs1IP = { "172.20.1.1", "172.20.5.1" };

        string[] jctsAgs2IP = { "172.21.1.1", "172.21.5.1" };

        string[] njctsHmapIP = {"172.24.4.3", "172.24.4.4", "172.24.4.5", "172.24.4.6", "172.24.4.8", "172.24.129.10",
                                  "172.24.129.11", "172.24.129.16", "172.24.129.50", "172.24.129.51", "172.24.130.35",
                                   "172.24.132.2 ", "172.24.135.1"};

        string[] njctsAgs1IP = { "172.20.1.2", "172.20.1.3", "172.20.1.4", "172.20.5.2", "172.20.5.3", "172.20.5.4" };

        string[] njctsAgs2IP = { "172.21.1.2", "172.21.1.3", "172.21.1.4", "172.21.5.2", "172.21.5.3", "172.21.5.4" };

        string[] desktopLinks;

        PingReply reply;
        int ipCount;

        public static void AddDirectorySecurity(string FileName, string Account, FileSystemRights Rights,
                                                AccessControlType ControlType)
        {
            // Create a new DirectoryInfo object
            DirectoryInfo dInfo = new DirectoryInfo(FileName);

            // Get a DirectorySecurity object that represents the 
            // current security settings
            DirectorySecurity dSecurity = dInfo.GetAccessControl();

            // Add the FileSystemAccessRule to the security settings
            dSecurity.AddAccessRule(new FileSystemAccessRule(Account, Rights, ControlType));

            // Set the new access settings
            dInfo.SetAccessControl(dSecurity);
        }

        public static void moveOldAec(string[] dirArr, string aecHome, string mainPath)
        {
            string oldDCS = string.Format(@"\DCS_Sim_{0:MMddyyyy}", DateTime.Now);
            string userName = @"hmuser";

            for (int i = 0; i < dirArr.Length; i++)
            {
                if (dirArr[i] == mainPath + oldDCS)
                {
                    MessageBox.Show(string.Format(dirArr[i]));

                    try
                    {
                        for (int j = 1; j < 5; j++)
                        {
                            int count = j;
                            Directory.Move(aecHome + @"\AEC" + count, dirArr[i] + @"\AEC" + count);
                        }
                        Directory.Move(aecHome + @"\ID", dirArr[i] + @"\ID");
                        if (Directory.Exists(aecHome + string.Format(@"\Users\{0}\DataFolder", userName)))
                        {
                            Directory.Move(aecHome + string.Format(@"\Users\{0}\DataFolder", userName), dirArr[i] + @"\DataFolder");
                        }
                        else
                        {
                            MessageBox.Show("There is not a current DataFolder located at:" + aecHome +
                                @"\Users\hmuser\");
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
        }

        // Move new AEC 1-4, ID, Datafolder folders and create a transfolder directory if not 
        // created
        public static void mvNewAec(string[] dirArr, string aecHome, string mainPath)
        {
            string userName = @"hmuser";
            string newDCS = string.Format(@"{0}\DCS_Sim", mainPath);
            string dataFldDir = string.Format(@"{0}Users\{1}\DataFolder", aecHome, userName);
            string TransFldDir = string.Format(@"{0}Users\{1}\TransFolder", aecHome, userName);

            for (int i = 0; i < dirArr.Length; i++)
            {
                Regex rg = new Regex(@"_[0-9]{8}");

                dirArr[i] = rg.Replace(dirArr[i], string.Empty);

                if (dirArr[i] == newDCS)
                {
                    MessageBox.Show(string.Format(dirArr[i]));

                    try
                    {
                        for (int j = 1; j < 5; j++)
                        {
                            int count = j;
                            Directory.Move(dirArr[i] + @"\AEC" + count, aecHome + @"\AEC" + count);
                            AddDirectorySecurity(aecHome + @"\AEC" + count, Environment.UserName, FileSystemRights.FullControl,
                                                        AccessControlType.Allow);
                        }

                        Directory.Move(dirArr[i] + @"\ID", aecHome + @"\ID");
                        AddDirectorySecurity(aecHome + @"\ID", Environment.UserName, FileSystemRights.FullControl,
                                                        AccessControlType.Allow);

                        Directory.Move(dirArr[i] + @"\DataFolder", dataFldDir);
                        AddDirectorySecurity(aecHome + @"\DataFolder", Environment.UserName, FileSystemRights.FullControl,
                                                        AccessControlType.Allow);

                        MessageBox.Show("Moved new DataFolder and ID folders to proper location");
                        if (!Directory.Exists(TransFldDir))
                        {
                            Directory.CreateDirectory(TransFldDir);
                        }
                        else
                        {
                            MessageBox.Show("The TransFolder Directory Exists");
                        }

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                }
            }
        }

        public static void cpASF(string srcName, string trgName)
        {
            string asfFile = Path.Combine(srcName, "AircraftSettingsFile.csv");
            string asfLocation = Path.Combine(trgName, "AEC");

            if (System.IO.File.Exists(asfFile))
            {
                for (int i = 1; i < 5; i++)
                {
                    System.IO.File.Copy(asfFile, asfLocation + i + @"\AirCraftSettingsFile.csv", true);
                    MessageBox.Show("AircraftSettingsFile.csv is Up-To-Date");
                }
            }
            else
            {
                MessageBox.Show("AircraftSettingsFile.csv file missing on thumbdrive");
            }
        }

        public static void DeleteShortcut(string[] appName, string srcDir, string[] delLink)
        {
            for (int i = 0; i < appName.Length; i++)
            {
                delLink = Directory.GetFiles(srcDir, string.Format("{0}*.lnk", appName[i]));


                foreach (string link in delLink)
                {
                    System.IO.File.Delete(link);
                }
            }
        }

        public static void CreateShortcut(string[] appName, string mainPath, string[] gaDirs, string pattern)
        {

            string shortcutAddress;
            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            int count = 0;

            foreach (string dir in gaDirs)
            {
                // Regular expression with the rmPattern to find numbers at the end of the title
                Match checkDir = Regex.Match(dir, pattern);


                while (checkDir.Success)
                {

                    if (appName[count] == "WindowsFormsApplication1")
                    {
                        shortcutAddress = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\ISM_Sim" + ".lnk";
                    }
                    else if (appName[count] == "ModbusTestClient")
                    {
                        shortcutAddress = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\PickleSwitchSim" + ".lnk";
                    }
                    else
                    {
                        shortcutAddress = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + appName[count] + ".lnk";
                    }

                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
                    shortcut.WorkingDirectory = checkDir + @"\";
                    shortcut.TargetPath = checkDir + @"\" + appName[count] + ".exe";
                    shortcut.Save();
                    checkDir = checkDir.NextMatch();
                }
                count++;
            }
        }

        public void EnableLAN(object interfaceName)
        {
            ProcessStartInfo psi = new ProcessStartInfo("netsh", "interface set interface \"" + interfaceName +
                "\" enable");
            Process p = new Process();
            p.StartInfo = psi;
            p.Start();
        }

        public void DisableLAN(object interfaceName)
        {
            ProcessStartInfo psi = new ProcessStartInfo("netsh", "interface set interface \"" + interfaceName +
                "\" disable");
            Process p = new Process();
            p.StartInfo = psi;
            p.Start();
        }

        public void checkIP(string ipAddress)
        {
            try
            {
                Ping checkPing = new Ping();
                reply = checkPing.Send(ipAddress);
            }
            
            catch(PingException)
            {
                
            }

        }

        public void ipConnection(string[] arrLAN)
        {
            try
            {
                ipCount = 0;
                int badCount = 0;
                var noConnection = new string[26];
                foreach (string ip in arrLAN)
                {
                    
                    checkIP(ip);
                    if (reply.Status.ToString() == "Success")
                    {
                        ipCount = ++ipCount;
                    }
                    else
                    {
                        noConnection[badCount] = ip.ToString();
                        badCount = ++badCount;
                    }

                }

                if (ipCount == arrLAN.Length)
                {
                    MessageBox.Show("All IP addresses connected successfully");
                }
                else if (ipCount != arrLAN.Length)
                {
                    MessageBox.Show("The following address connected unsuccessfullly");
                    for (int i = 0; i < arrLAN.Length; i++)
                    {
                        string message = noConnection[i].ToString();
                        MessageBox.Show("Could not establish connection to " + message);
                    }

                }
            }
            catch (NullReferenceException)
            {
             
            }

        }

        public void deleteIP(object interfaceName, string[] arrLAN)
        {
            foreach (string ip in arrLAN)
            {
                
                ProcessStartInfo psi = new ProcessStartInfo("netsh", string.Format("interface ipv4 delete address name=\"{0}\" addr={1}", interfaceName, ip));
                Process p = new Process();
                p.StartInfo = psi;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.Start();
            }
        }

        public void addIP(object interfaceName, string[] arrLAN)
        {
            foreach (string ip in arrLAN)
            {
                if (ip == "172.20.1.1")
                {
                    ProcessStartInfo psi = new ProcessStartInfo("netsh", string.Format("interface ipv4 set address name=\"{0}\" addr={1} mask=255.255.0.0 gateway=172.20.0.1", interfaceName, ip));
                    Process p = new Process();
                    p.StartInfo = psi;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = false;
                    p.Start();
                }
                else if (ip == "172.16.4.10")
                {
                    ProcessStartInfo psi = new ProcessStartInfo("netsh", string.Format("interface ipv4 set address name=\"{0}\" addr={1} mask=255.255.0.0 gateway=172.16.4.222", interfaceName, ip));
                    Process p = new Process();
                    p.StartInfo = psi;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = false;
                    p.Start();
                }
                else
                {
                    ProcessStartInfo psi = new ProcessStartInfo("netsh", string.Format("interface ipv4 add address name=\"{0}\" addr={1} mask=255.255.0.0", interfaceName, ip));
                    Process p = new Process();
                    p.StartInfo = psi;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = false;
                    p.Start();
                }
            }
        }

        private void updateApps()
        {
            string aecPath = @"C:\";
            string path = @"C:\Program Files (x86)\General Atomics";
            string[] appDirs = Directory.GetDirectories(path);
            // Pattern for : name + eight numbers at the end
            string rmPattern = ("^([a-zA-Z]:)?(\\\\[^<>:\"/\\\\|?*]+)+(\\d{8})$");
            // Pattern for : name 
            string mvPattern = ("^([a-zA-Z]:)?(\\\\[^<>:\"/\\\\|?*]+)+\\\\?$");
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // This is used to find all Folders that end in a date and delete them
            foreach (string delDir in appDirs)
            {
                // Regular expression with the rmPattern to find numbers at the end of the title
                Match rmDir = Regex.Match(delDir, rmPattern);

                // While the regular expression rmDir is successful, 
                //delete the current directory and files listed     
                while (rmDir.Success)
                {
                    //Delete the named folder and when set true delete everything within the folder
                    Directory.Delete(rmDir.Value, true);
                    // Look for the next matching folder
                    rmDir = rmDir.NextMatch();
                }
            }

            // Used to refresh the Directory listing to current status
            string[] repDirs = Directory.GetDirectories(path);


            // While the RegexEx mvDir is successful,
            // rename the current folders used for the simulators
            foreach (string repDir in repDirs)
            {
                // Regular expression with the mvPattern for currently used folders
                Match mvDir = Regex.Match(repDir, mvPattern);


                while (mvDir.Success)
                {
                    //store a new string name of the folder with todays date appended to it
                    string appName = string.Format("{0}_{1:MMddyyyy}", repDir, DateTime.Now);
                    //Replace the old folder name with the new one
                    Directory.Move(repDir, appName);
                    // Find the next pattern matched folder
                    mvDir = mvDir.NextMatch();
                }
            }

            // Used to refresh the Directory listing to current status
            string[] cpDirs = Directory.GetDirectories(path);

            // Used to search any given removable drive (USB) that is mounted
            foreach (DriveInfo removableDrive in DriveInfo.GetDrives().Where(
                         drive => drive.DriveType == DriveType.Removable && drive.IsReady))
            {
                rootDirectory = removableDrive.RootDirectory;
                string monitoredDirectory = Path.Combine(rootDirectory.FullName, "4WS");

                new Microsoft.VisualBasic.Devices.Computer().
                    FileSystem.CopyDirectory(monitoredDirectory, path);
            }

            // Move old AEC's and ID folder to backup DCS folder
            moveOldAec(cpDirs, aecPath, path);

            // Move new AEC's, ID, and overwrite the old Datafolder to correct locations
            mvNewAec(cpDirs, aecPath, path);

            // Used to copy a newer ASF file to the C drive if the existing one is older
            cpASF(rootDirectory.FullName, aecPath);

            // Used to delete the current desktop shortcuts
            DeleteShortcut(processName, desktopPath, desktopLinks);

            // Used to create new shortcuts based on new executables
            CreateShortcut(appProcess, path, cpDirs, mvPattern);

        }

        public Form1()
        {
            InitializeComponent();

            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 100; i++)
            {            
                // Report progress
                backgroundWorker1.ReportProgress(i);

                // Wait 100 milliseconds
                Thread.Sleep(100);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Progress_Form prgfrm = new Progress_Form();
 
            // Change the value of the ProgressBar to the BackgroundWorker progress.
            prgfrm.progressBar1.Value = e.ProgressPercentage;
            // Set the text
            prgfrm.resultLabel.Text = (e.ProgressPercentage.ToString() + "%");
        }

        public void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Progress_Form prgfrm = new Progress_Form();

            if (e.Error != null)
            {
                prgfrm.resultLabel.Text = "Error: " + e.Error.Message;
            }
            else
            {
                prgfrm.resultLabel.Text = "Done!";
                prgfrm.Close();
                backgroundWorker1.Dispose();
            }

        }

        private void UpdateSimulators(object sender, EventArgs e)
        {
            
            Progress_Form prgfrm = new Progress_Form();
            prgfrm.Show();
            backgroundWorker1.RunWorkerAsync(10000);
            this.UpdateSims.Enabled = false;

           // updateApps();
        }

        private void StopSimulators(object sender, EventArgs e)
        {
            // Place GetProcesses on a variable
            var runningProcesses = Process.GetProcesses();

            // Use a for loop to cycle through the processKill array to stop the named applications
            for (int i = 0; i < runningProcesses.Length; i++)
            {
                if (appProcess.Contains(runningProcesses[i].ProcessName))
                {
                    runningProcesses[i].Kill();
                    runningProcesses[i].WaitForExit();
                }
            }
        }

        private void StartSimulators(object sender, EventArgs e)
        {
            Process.Start("C:\\Program Files (x86)\\General Atomics\\DCS_Sim\\DCSSimulator.exe");
            Process.Start("C:\\Program Files (x86)\\General Atomics\\BSCSimulator\\BSCSimulator.exe");
            Process.Start("C:\\Program Files (x86)\\General Atomics\\ISM_Sim\\WindowsFormsApplication1.exe");
            Process.Start("C:\\Program Files (x86)\\General Atomics\\PickleSwitchSim\\ModbusTestClient.exe");
            Process.Start("C:\\Program Files (x86)\\General Atomics\\SCS_Display\\ScsDisplay.exe");
            Process.Start("C:\\Program Files (x86)\\General Atomics\\ScsAdmacsSim\\ScsAdmacsSim.exe");
            Process.Start("C:\\Program Files (x86)\\General Atomics\\Switch_Sim\\SwitchSimulator.exe");
            Process.Start("C:\\Program Files (x86)\\General Atomics\\UPS_Sim\\UPSSimulator.exe");
        }

        private void SetLocalAreaNetwork(object sender, EventArgs e)
        {
            int count = 0;
            foreach (string indexChecked in checkedListBox1.Items)
            {

                MessageBox.Show("Index#: " + indexChecked.ToString() + ", is checked. Checked state is: " +
                                    checkedListBox1.GetItemCheckState(count).ToString() + ".");

                if (checkedListBox1.GetItemChecked(count))
                {
                    Console.WriteLine(checkedListBox1.Items[count]);
                    EnableLAN(checkedListBox1.Items[count]);
                }

                if (!checkedListBox1.GetItemChecked(count))
                {
                    DisableLAN(checkedListBox1.Items[count]);
                }
                count = ++count;
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SetIpNetwork(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                deleteIP(checkedListBox1.Items[0], hmapIP);               
                addIP(checkedListBox1.Items[0], hmapIP);

                deleteIP(checkedListBox1.Items[1], ags1IP);
                addIP(checkedListBox1.Items[1], ags1IP);

                deleteIP(checkedListBox1.Items[2], ags2IP);
                addIP(checkedListBox1.Items[2], ags2IP);

                deleteIP(checkedListBox1.Items[3], admacsIp);
                addIP(checkedListBox1.Items[3], admacsIp);
           
            }
            
            else if (radioButton2.Checked)
            {
                deleteIP(checkedListBox1.Items[0], hmapIP);
                addIP(checkedListBox1.Items[0], jctsHmapIP);

                deleteIP(checkedListBox1.Items[1], ags1IP);
                addIP(checkedListBox1.Items[1], jctsAgs1IP);

                deleteIP(checkedListBox1.Items[2], ags2IP);
                addIP(checkedListBox1.Items[2], jctsAgs2IP);

                deleteIP(checkedListBox1.Items[3], admacsIp);
                addIP(checkedListBox1.Items[3], admacsIp);
            }
            else if (radioButton3.Checked)
            {
                deleteIP(checkedListBox1.Items[0], hmapIP);
                addIP(checkedListBox1.Items[0], njctsHmapIP);

                deleteIP(checkedListBox1.Items[1], ags1IP);
                addIP(checkedListBox1.Items[1], njctsAgs1IP);

                deleteIP(checkedListBox1.Items[2], ags2IP);
                addIP(checkedListBox1.Items[2], njctsAgs2IP);
            }
            
        }

        private void CheckNetwork(object sender, EventArgs e)
        {
            try
            {

                if (radioButton1.Checked)
                {
                    ipConnection(hmapIP);
                    ipConnection(ags1IP);
                    ipConnection(ags2IP);
                    ipConnection(admacsIp);
                } 
                else if (radioButton2.Checked)
                {
                    ipConnection(jctsHmapIP);
                    ipConnection(jctsAgs1IP);
                    ipConnection(jctsAgs2IP);
                    ipConnection(admacsIp);
                }                
                else if (radioButton3.Checked)
                {
                    ipConnection(njctsHmapIP);
                    ipConnection(njctsAgs1IP);
                    ipConnection(njctsAgs2IP);
                }
                
            }
            catch (PingException pError)
            {
                MessageBox.Show(pError.Message);
            }
        }

        private void resultLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
