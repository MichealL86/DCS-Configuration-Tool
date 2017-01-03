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
using System.ComponentModel;
using System.Threading;
using System.Drawing;


namespace DCS_Configuration_Tool
{



    public partial class Form1 : Form
    {
        delegate void SetTextCallback(string text);


        string aecPath = @"C:\";
        static string path = @"C:\Program Files (x86)\General Atomics";

        // A PingReplay variable named reply
        PingReply reply;
        // An integer variable named ipCount
        int ipCount;
        DirectoryInfo rootDirectory;

        string logName = string.Format("\\DCSLog_{0:MMddyyyy}.txt", DateTime.Now);
        string bscConfigPath = @"C:\Program Files (x86)\General Atomics\BSCSimulator\BSCSimulator.exe.config";
        string fullBSCExeConfig = "		<add key=\"countAECs\" value=\"0\"/>";
        string jctsBSCExeConfig = "     <add key=\"CountAECs\" value=\"4\"/> ";
        string nonJctsBSCExeConfig = " 		<add key=\"countAECs\" value=\"1\"/>";
        string jctsBSCAddKey = "		<add key=\"0\" value=\"BSC0\"/>";
        string nonJctsBSCAddKey = "		<add key=\"0\" value=\"BSC1\"/>";

        string aecConfigPath = @"C:\Program Files (x86)\General Atomics\DCS_Sim\DCSSimulator.exe.config";
        string fullAECExeConfig = "    <add key=\"countAECs\" value=\"0\"/> <!-- Set the number to disable the AECs from 0 thru 3 -->";
        string jctsAECExeConfig = "    <add key=\"countAECs\" value=\"4\"/> <!-- Set the number to disable the AECs from 0 thru 3 -->";
        string nonJctsAECExeconfig = "    <add key=\"countAECs\" value=\"1\"/> <!-- Set the number to disable the AECs from 0 thru 3 -->";
        string jctsAECAddKey = "    <add key=\"0\" value=\"AEC0\"/>";
        string nonJctsAECAddKey = "    <add key=\"0\" value=\"AEC1\"/>";

        string[] appDirs = Directory.GetDirectories(path);
        string[] desktopLinks;
        string[] appProcess = { "BSCSimulator", "DCSSimulator", "WindowsFormsApplication1", "MovEmulator",
                                "ModbusTestClient", "ScsAdmacsSim",  "SwitchSimulator", "UPSSimulator", "ScsDisplay"};

        string[] processName = {"BSC", "DCS", "ISM", "Mov", "Pickle", "ScsD", "ScsA", "Switch", "UPS",
                                "AEC", "ROCS", "SNMP"};



        // Pattern for : name + eight numbers at the end
        string rmPattern = ("^([a-zA-Z]:)?(\\\\[^<>:\"/\\\\|?*]+)+(\\d{8})$");
        // Pattern for : name 
        string mvPattern = ("^([a-zA-Z]:)?(\\\\[^<>:\"/\\\\|?*]+)+\\\\?$");
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        string[] startApps = {"C:\\Program Files (x86)\\General Atomics\\DCS_Sim\\DCSSimulator.exe", "C:\\Program Files (x86)\\General Atomics\\BSCSimulator\\BSCSimulator.exe",
            "C:\\Program Files (x86)\\General Atomics\\ISM_Sim\\WindowsFormsApplication1.exe", "C:\\Program Files (x86)\\General Atomics\\PickleSwitchSim\\ModbusTestClient.exe",
            "C:\\Program Files (x86)\\General Atomics\\ScsAdmacsSim\\ScsAdmacsSim.exe","C:\\Program Files (x86)\\General Atomics\\Switch_Sim\\SwitchSimulator.exe",
            "C:\\Program Files (x86)\\General Atomics\\UPS_Sim\\UPSSimulator.exe", "C:\\Program Files (x86)\\General Atomics\\SCS_Display\\ScsDisplay.exe"};

        // String array of IP addresses for HMAP LAN
        string[] hmapIP = {"172.24.4.1", "172.24.4.2", "172.24.4.3", "172.24.4.4", "172.24.4.5", "172.24.4.6",
                            "172.24.4.7", "172.24.4.8", "172.24.128.10", "172.24.128.11", "172.24.128.16",
                            "172.24.128.50", "172.24.128.51", "172.24.128.52", "172.24.129.10", "172.24.129.11",
                            "172.24.129.16", "172.24.129.50", "172.24.129.51", "172.24.129.52", "172.24.130.35",
                            "172.24.132.1", "172.24.132.2", "172.24.135.1", "172.24.135.2"};

        // String array of IP addresses for AGS LAN
        string[] ags1IP = {"172.20.1.1", "172.20.1.2", "172.20.1.3", "172.20.1.4", "172.20.5.1", "172.20.5.2",
                            "172.20.5.3", "172.20.5.4"};

        // String array of IP address for AGS LAN
        string[] ags2IP = { "172.21.1.1", "172.21.1.2", "172.21.1.3", "172.21.1.4", "172.21.5.1", "172.21.5.2",
                            "172.21.5.3", "172.21.5.4" };

        // String array of IP addresses for ADMACS LAN
        string[] admacsIp = { "172.16.4.10", "172.16.4.11" };

        // String array of IP addresses for HMAP LAN
        string[] jctsHmapIP = { "172.24.4.1", "172.24.4.2", "172.24.4.7", "172.24.128.10", "172.24.128.11",
                                "172.24.128.16", "172.24.128.50", "172.24.128.51", "172.24.128.52",
                                "172.24.129.52", "172.24.132.1", "172.24.132.2"};

        // String array of IP addresses for AGS LAN
        string[] jctsAgs1IP = { "172.20.1.1", "172.20.5.1" };

        // String array of IP addresses for AGS LAN
        string[] jctsAgs2IP = { "172.21.1.1", "172.21.5.1" };

        // String array of IP addresses for HMAP LAN
        string[] njctsHmapIP = {"172.24.4.3", "172.24.4.4", "172.24.4.5", "172.24.4.6", "172.24.4.8", "172.24.129.10",
                                  "172.24.129.11", "172.24.129.16", "172.24.129.50", "172.24.129.51", "172.24.130.35",
                                   "172.24.132.2 ", "172.24.135.1"};

        // String array of IP addresses for AGS LAN
        string[] njctsAgs1IP = { "172.20.1.2", "172.20.1.3", "172.20.1.4", "172.20.5.2", "172.20.5.3", "172.20.5.4" };

        // String array of IP addresses for AGS LAN
        string[] njctsAgs2IP = { "172.21.1.2", "172.21.1.3", "172.21.1.4", "172.21.5.2", "172.21.5.3", "172.21.5.4" };

        
        private void SetText(string text)
        {
            if (this.listBox1.InvokeRequired)
            {
                SetTextCallback callBack = new SetTextCallback(SetText);
                this.Invoke(callBack, new object[] { text });
                
            }
            else
            {
                string add = text;
                this.listBox1.Items.Add(add);
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
            }
           
        }
  
        // Using a process to enable specified LANs
        public void EnableLAN(object interfaceName)
        {
            listBox1.Items.Add("Enabling LAN " + interfaceName); 

            ProcessStartInfo psi = new ProcessStartInfo("netsh", "interface set interface \"" + interfaceName +
                "\" enable");
            Process p = new Process();
            p.StartInfo = psi;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();
        }

        // Using a process to disable specified LANs
        public void DisableLAN(object interfaceName)
        {
            listBox1.Items.Add("Disabling LAN " + interfaceName);
            ProcessStartInfo psi = new ProcessStartInfo("netsh", "interface set interface \"" + interfaceName +
                "\" disable");
            Process p = new Process();
            p.StartInfo = psi;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.Start();
        }

        // Check to see if there is an IP connection
        public void checkIP(string ipAddress)
        {
            try
            {
                Ping checkPing = new Ping();
                reply = checkPing.Send(ipAddress);
            }
            
            catch(PingException pingError)
            {
                listBox1.Items.Add(pingError.Message);
            }

        }

        // Let the user know the status of the IPs tested
        public void ipConnection(string[] arrLAN)
        {
            try
            {
                ipCount = 0;
                int badCount = 0;
                var noConnection = new string[26];

                listBox1.Items.Add(" Checking IP Connections..." + Environment.NewLine);

                foreach (string ip in arrLAN)
                {
                    checkIP(ip);
                    listBox1.Items.Add("IP Addresses: " + ip + "  " + "Status: " + reply.Status.ToString());
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
                    SetText(String.Empty);
                    listBox1.Items.Add("All IP addresses connected successfully");
                }
                else if (ipCount != arrLAN.Length)
                {
                    SetText(String.Empty);
                    listBox1.Items.Add("The following address connected unsuccessfullly");
                    for (int i = 0; i < noConnection.Length; i++)
                    {
                        SetText(String.Empty);
                        string message = noConnection[i].ToString();
                        listBox1.Items.Add("Could not establish connection to " + message);
                    }

                }
            }
            catch (NullReferenceException e)
            {
                listBox1.Items.Add("If address '172.16.4.222' make sure ScsAdmacsSim on and running with SCS");
                listBox1.Items.Add("or ignore this message");
                listBox1.Items.Add("For all other address check to make sure address exist within specified LAN");
            }

        }

        // Using a process delete the specified IP addresses
        public void deleteIP(object interfaceName, string[] arrLAN)
        {
            string delIP = string.Empty;


            foreach (string ip in arrLAN)
            {
                try
                {
                    delIP = ip;
                    SetText("Deleting IP address " + ip);
                    ProcessStartInfo psi = new ProcessStartInfo("netsh", string.Format("interface ipv4 delete address name=\"{0}\" addr={1}", interfaceName, ip));
                    Process p = new Process();
                    p.StartInfo = psi;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = false;
                    p.Start();

                    Thread.Sleep(500);
                }

                catch
                {
                    SetText("Problem with deleting " + delIP);
                }
            }

            
        }

        // Using a process add the specified IP addresses
        public void addIP(object interfaceName, string[] arrLAN)
        {
            string addIP = string.Empty;

            try
            {
                foreach (string ip in arrLAN)
                {
                    SetText("Adding IP address " + ip);
                    addIP = ip;

                    if (ip == "172.20.1.1")
                    {
                        ProcessStartInfo psi = new ProcessStartInfo("netsh", string.Format("interface ipv4 set address name=\"{0}\" addr={1} mask=255.255.0.0 gateway=172.20.0.1", interfaceName, ip));
                        Process p = new Process();
                        p.StartInfo = psi;
                        p.StartInfo.CreateNoWindow = true;
                        p.StartInfo.UseShellExecute = false;
                        p.Start();

                        Thread.Sleep(500);
                    }
                    else if (ip == "172.21.1.1")
                    {
                        ProcessStartInfo psi = new ProcessStartInfo("netsh", string.Format("interface ipv4 set address name=\"{0}\" addr={1} mask=255.255.0.0", interfaceName, ip));
                        Process p = new Process();
                        p.StartInfo = psi;
                        p.StartInfo.CreateNoWindow = true;
                        p.StartInfo.UseShellExecute = false;
                        p.Start();

                        Thread.Sleep(500);
                    }
                    else if (ip == "172.16.4.10")
                    {
                        ProcessStartInfo psi = new ProcessStartInfo("netsh", string.Format("interface ipv4 set address name=\"{0}\" addr={1} mask=255.255.0.0 gateway=172.16.4.222", interfaceName, ip));
                        Process p = new Process();
                        p.StartInfo = psi;
                        p.StartInfo.CreateNoWindow = true;
                        p.StartInfo.UseShellExecute = false;
                        p.Start();

                        Thread.Sleep(500);
                    }
                    else
                    {
                        ProcessStartInfo psi = new ProcessStartInfo("netsh", string.Format("interface ipv4 add address name=\"{0}\" addr={1} mask=255.255.0.0", interfaceName, ip));
                        Process p = new Process();
                        p.StartInfo = psi;
                        p.StartInfo.CreateNoWindow = true;
                        p.StartInfo.UseShellExecute = false;
                        p.Start();

                        Thread.Sleep(500);
                    }
                }
            }
            catch
            {
                SetText("Problem adding " + addIP);
            }
            
        }

        public void ConfigureIpNetwork()
        {
            if (radioButton1.Checked) // Full Setup
            {
                SetText("Healthmap IP Configuration:");
                SetText("-------------------------------------------------------");
                deleteIP(checkedListBox1.Items[0], hmapIP);
                SetText(String.Empty);
                addIP(checkedListBox1.Items[0], hmapIP);

                SetText(String.Empty);

                SetText("AGS 1 IP Configuration:");
                SetText("-------------------------------------------------------");
                deleteIP(checkedListBox1.Items[1], ags1IP);
                SetText(String.Empty);
                addIP(checkedListBox1.Items[1], ags1IP);

                SetText(String.Empty);

                SetText("AGS 2 IP Configuration:");
                SetText("-------------------------------------------------------");
                deleteIP(checkedListBox1.Items[2], ags2IP);
                SetText(String.Empty);
                addIP(checkedListBox1.Items[2], ags2IP);

                SetText(String.Empty);

                SetText("ADMACS IP Configuration:");
                SetText("-------------------------------------------------------");
                deleteIP(checkedListBox1.Items[3], admacsIp);
                SetText(String.Empty);
                addIP(checkedListBox1.Items[3], admacsIp);

                SetText(String.Empty);

                // Modify the .exe.config file for the BSC so that it works correctly
                SetText("Configuring the BSCSimulator.exe.config file");
                String fullBSCtext = System.IO.File.ReadAllText(bscConfigPath);
                fullBSCtext = fullBSCtext.Replace(jctsBSCExeConfig, fullBSCExeConfig);
                fullBSCtext = fullBSCtext.Replace(nonJctsBSCExeConfig, fullBSCExeConfig);
                fullBSCtext = fullBSCtext.Replace(jctsBSCAddKey, nonJctsBSCAddKey);
                System.IO.File.WriteAllText(bscConfigPath, fullBSCtext);

                // Modify the .exe.config file for the DCS so that it works correctly
                SetText("Configuring the AECSimulator.exe.config file");
                String fullAECtext = System.IO.File.ReadAllText(aecConfigPath);
                fullAECtext = fullAECtext.Replace(jctsAECExeConfig, fullAECExeConfig);
                fullAECtext = fullAECtext.Replace(nonJctsAECExeconfig, fullAECExeConfig);
                fullAECtext = fullAECtext.Replace(jctsAECAddKey, nonJctsAECAddKey);
                System.IO.File.WriteAllText(aecConfigPath, fullAECtext);

                SetText(String.Empty);

            }

            else if (radioButton2.Checked) // JCTS
            {
                SetText("JCTS Healthmap IP Configuration:");
                SetText("-------------------------------------------------------");
                deleteIP(checkedListBox1.Items[0], hmapIP);
                SetText(String.Empty);
                addIP(checkedListBox1.Items[0], jctsHmapIP);

                SetText(String.Empty);

                SetText("JCTS AGS 1 IP Configuration:");
                SetText("-------------------------------------------------------");
                deleteIP(checkedListBox1.Items[1], ags1IP);
                SetText(String.Empty);
                addIP(checkedListBox1.Items[1], jctsAgs1IP);

                SetText(String.Empty);

                SetText("JCTS AGS 2 IP Configuration:");
                SetText("-------------------------------------------------------");
                deleteIP(checkedListBox1.Items[2], ags2IP);
                SetText(String.Empty);
                addIP(checkedListBox1.Items[2], jctsAgs2IP);

                SetText(String.Empty);

                SetText("ADMACS IP Configuration:");
                SetText("-------------------------------------------------------");
                deleteIP(checkedListBox1.Items[3], admacsIp);
                SetText(String.Empty);
                addIP(checkedListBox1.Items[3], admacsIp);

                SetText(String.Empty);

                // Modify the .exe.config file for the BSC so that it works correctly
                SetText("Configuring the BSCSimulator.exe.config file");
                String jctsBSCtext = System.IO.File.ReadAllText(bscConfigPath);
                jctsBSCtext = jctsBSCtext.Replace(nonJctsBSCExeConfig, jctsBSCExeConfig);
                jctsBSCtext = jctsBSCtext.Replace(fullBSCExeConfig, jctsBSCExeConfig);
                jctsBSCtext = jctsBSCtext.Replace(nonJctsBSCAddKey, jctsBSCAddKey);
                System.IO.File.WriteAllText(bscConfigPath, jctsBSCtext);


                // Modify the .exe.config file for the DCS so that it works correctly
                SetText("Configuring the AECSimulator.exe.config file");
                String jctsAECtext = System.IO.File.ReadAllText(aecConfigPath);
                jctsAECtext = jctsAECtext.Replace(nonJctsAECExeconfig, jctsAECExeConfig);
                jctsAECtext = jctsAECtext.Replace(fullAECExeConfig, jctsAECExeConfig);
                jctsAECtext = jctsAECtext.Replace(nonJctsAECAddKey, jctsAECAddKey);
                System.IO.File.WriteAllText(aecConfigPath, jctsAECtext);

                SetText(String.Empty);
            }
            else if (radioButton3.Checked) // NON-JCTS
            {
                SetText("NON-JCTS Healthmap IP Configuration:");
                SetText("-------------------------------------------------------");
                deleteIP(checkedListBox1.Items[0], hmapIP);
                SetText(String.Empty);
                addIP(checkedListBox1.Items[0], njctsHmapIP);

                SetText(String.Empty);

                SetText("NON-JCTS AGS 1 IP Configuration:");
                SetText("-------------------------------------------------------");
                deleteIP(checkedListBox1.Items[1], ags1IP);
                SetText(String.Empty);
                addIP(checkedListBox1.Items[1], njctsAgs1IP);

                SetText(String.Empty);

                SetText("NON-JCTS AGS 2 IP Configuration:");
                SetText("-------------------------------------------------------");
                deleteIP(checkedListBox1.Items[2], ags2IP);
                SetText(String.Empty);
                addIP(checkedListBox1.Items[2], njctsAgs2IP);

                SetText(String.Empty);

                SetText("ADMACS IP Configuration:");
                SetText("-------------------------------------------------------");
                deleteIP(checkedListBox1.Items[3], admacsIp);
                SetText(String.Empty);
                addIP(checkedListBox1.Items[3], admacsIp);

                SetText(String.Empty);

                // Modify the .exe.config file for the BSC so that it works correctly
                SetText("Configuring the BSCSimulator.exe.config file");
                String nonJctsBSCtext = System.IO.File.ReadAllText(bscConfigPath);
                nonJctsBSCtext = nonJctsBSCtext.Replace(jctsBSCExeConfig, nonJctsBSCExeConfig);
                nonJctsBSCtext = nonJctsBSCtext.Replace(fullBSCExeConfig, nonJctsBSCExeConfig);
                nonJctsBSCtext = nonJctsBSCtext.Replace(jctsBSCAddKey, nonJctsBSCAddKey);
                System.IO.File.WriteAllText(bscConfigPath, nonJctsBSCtext);

                // Modify the .exe.config file for the DCS so that it works correctly
                SetText("Configuring the AECSimulator.exe.config file");
                String nonJctsAECtext = System.IO.File.ReadAllText(aecConfigPath);
                nonJctsAECtext = nonJctsAECtext.Replace(jctsAECExeConfig, nonJctsAECExeconfig);
                nonJctsAECtext = nonJctsAECtext.Replace(fullAECExeConfig, nonJctsAECExeconfig);
                nonJctsAECtext = nonJctsAECtext.Replace(jctsAECAddKey, nonJctsAECAddKey);
                System.IO.File.WriteAllText(aecConfigPath, nonJctsAECtext);

                SetText(String.Empty);
            }
        }

        // AddDirectorySecurity method is used to give the named directory certain permissions
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

        public void moveOldAec(string[] dirArr, string aecHome, string mainPath)
        {

            string oldDCS = string.Format(@"\DCS_Sim_{0:MMddyyyy}", DateTime.Now);
            int count = 0;

            string userName = @"hmuser";

            for (int i = 0; i < dirArr.Length; i++)
            {
                if (dirArr[i] == mainPath + oldDCS)
                {
                    try
                    {
                        for (int j = 1; j < 5; j++)
                        {
                            count = j;

                            SetText("Moving old AEC Folders to  " + dirArr[i] + @"\AEC" + count);

                            if (!Directory.Exists(aecHome + @"\AEC" + count))
                            {
                                SetText(aecHome + @"\AEC" + count + " does not exist");
                            }
                            else if (Directory.Exists(aecHome + @"\AEC" + count))
                            {
                                if (!Directory.Exists(dirArr[i] + @"\AEC" + count))
                                {
                                    Directory.Move(aecHome + @"\AEC" + count, dirArr[i] + @"\AEC" + count);
                                }
                                else if (Directory.Exists(dirArr[i] + @"\AEC" + count))
                                {
                                    Directory.Delete(dirArr[i] + @"\AEC" + count);
                                    Directory.Move(aecHome + @"\AEC" + count, dirArr[i] + @"\AEC" + count);
                                }
                            }
                        }

                        //listBox1.Items.Add("Moving old ID folder to  " + dirArr[i] + @"\ID");
                        SetText("Moving old ID folder to  " + dirArr[i] + @"\ID");


                        Directory.Move(aecHome + @"\ID", dirArr[i] + @"\ID");


                        // Need to add the delete method on the labtop for DataFolder, test here first
                        if (Directory.Exists(aecHome + string.Format(@"\Users\{0}\DataFolder", userName)))
                        {
                           // listBox1.Items.Add("Moving old DataFolder to  " + dirArr[i] + @"\DataFolder");
                            SetText("Moving old DataFolder to  " + dirArr[i] + @"\DataFolder");


                            Directory.Delete(string.Format(@"C:\Users\{0}\DataFolder", userName), true);
                        }
                        else
                        {
                            MessageBox.Show("There is not a current DataFolder located at:" + aecHome +
                                @"\Users\hmuser\");
                        }
                    }
                    catch (Exception e)
                    {
                        //listBox1.Items.Add("The AEC Folders already exists in " + dirArr[i] +
                           // Environment.NewLine + "Error Message: " + e.Message);

                        SetText(Environment.NewLine + "Error Message: " + e.Message);

                    }
                }
            }
        }

        // Move new AEC 1-4, ID, Datafolder folders and create a transfolder directory if not 
        // created
        public void mvNewAec(string[] dirArr, string aecHome, string mainPath)
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
                    try
                    {
                        for (int j = 1; j < 5; j++)
                        {
                            int count = j;
                            //listBox1.Items.Add("Moving new AEC Folders to  " + aecHome + @"\AEC" + count);
                            SetText("Moving new AEC Folders to  " + aecHome + @"\AEC" + count);

                            Directory.Move(dirArr[i] + @"\AEC" + count, aecHome + @"\AEC" + count);
                            SetText("Adding permissions to   " + aecHome + @"\AEC" + count);

                            AddDirectorySecurity(aecHome + @"\AEC" + count, Environment.UserName, FileSystemRights.FullControl,
                                                        AccessControlType.Allow);

                            Thread.Sleep(100);
                        }

                        //listBox1.Items.Add("Moving new ID Folder to  " + aecHome + @"\ID");
                        SetText("Moving new ID Folder to  " + aecHome + @"\ID");

                        Directory.Move(dirArr[i] + @"\ID", aecHome + @"\ID");
                        //listBox1.Items.Add("Adding permissions to  " + aecHome + @"\ID");
                        SetText("Adding permissions to  " + aecHome + @"\ID");

                        AddDirectorySecurity(aecHome + @"\ID", Environment.UserName, FileSystemRights.FullControl,
                                                        AccessControlType.Allow);

                        //listBox1.Items.Add("Moving new DataFolder Folder to  " + dataFldDir);
                        SetText("Moving new DataFolder Folder to  " + dataFldDir);

                        Directory.Move(dirArr[i] + @"\DataFolder", dataFldDir);
                        //listBox1.Items.Add("Adding permissions to  " + dataFldDir);
                        SetText("Adding permissions to  " + dataFldDir);

                        AddDirectorySecurity(dataFldDir, Environment.UserName, FileSystemRights.FullControl,
                                                        AccessControlType.Allow);

                        if (!Directory.Exists(TransFldDir))
                        {
                            //listBox1.Items.Add("Creating new TransFolder at  " + TransFldDir);
                            SetText("Creating new TransFolder at  " + TransFldDir);

                            Directory.CreateDirectory(TransFldDir);
                        }
                        else if (Directory.Exists(TransFldDir))
                        {
                            //listBox1.Items.Add("The TransFolder Directory Exists");
                            SetText("The TransFolder Directory Exists");
                        }

                    }
                    catch (Exception e)
                    {
                        //listBox1.Items.Add(e.Message);
                        SetText(e.Message);
                    }
                }
            }
        }

        public void cpASF(string srcName, string trgName)
        {
            string asfFile = Path.Combine(srcName, "AircraftSettingsFile.csv");
            string asfLocation = Path.Combine(trgName, "AEC");

            if (System.IO.File.Exists(asfFile))
            {
                for (int i = 1; i < 5; i++)
                {
                    //listBox1.Items.Add("Copying new ASF files from thumb drive");
                    SetText("Copying new ASF files from thumb drive");
                    if (!System.IO.File.Exists(asfLocation + i + @"\AircraftSettingsFile.csv"))
                    {
                        SetText(" Could not add asf to " + asfLocation + i);
                        SetText("The directory may not exist");
                    }
                    else if (System.IO.File.Exists(asfLocation + i + @"\AircraftSettingsFile.csv"))
                    {
                        System.IO.File.Copy(asfFile, asfLocation + i + @"\AircraftSettingsFile.csv", true);
                    }
                }
            }
            else
            {
                //listBox1.Items.Add("AircraftSettingsFile.csv file missing on thumbdrive");
                SetText("AircraftSettingsFile.csv file missing on thumbdrive");
            }
        }

        public void CreateShortcut(string[] appName, string mainPath, string[] gaDirs, string pattern)
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
                        //listBox1.Items.Add("Created shortcut " + shortcutAddress);
                        SetText("Created shortcut " + shortcutAddress);
                    }
                    else if (appName[count] == "ModbusTestClient")
                    {
                        shortcutAddress = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\PickleSwitchSim" + ".lnk";
                        //listBox1.Items.Add("Created shortcut " + shortcutAddress);
                        SetText("Created shortcut " + shortcutAddress);
                    }
                    else
                    {
                        shortcutAddress = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + appName[count] + ".lnk";
                        //listBox1.Items.Add("Created shortcut " + shortcutAddress);
                        SetText("Created shortcut " + shortcutAddress);
                    }

                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
                    shortcut.WorkingDirectory = checkDir + @"\";
                    shortcut.TargetPath = checkDir + @"\" + appName[count] + ".exe";
                    shortcut.Save();
                    Thread.Sleep(500);
                    checkDir = checkDir.NextMatch();
                }
                count++;
            }
        }

        public void DeleteShortcut(string[] appName, string srcDir, string[] delLink)
        {
            for (int i = 0; i < appName.Length; i++)
            {
                delLink = Directory.GetFiles(srcDir, string.Format("{0}*.lnk", appName[i]));

                foreach (string link in delLink)
                {
                    //listBox1.Items.Add("Deleting " + link);
                    SetText("Deleting " + link);

                    System.IO.File.Delete(link);
                }
            }
        }

        // Use paint graphics to create a progress bar that shows the status inside of it
        public void paintProgress(string text)
        {
            // Change this back to prgfrm.progressBar1.CreateGraphics is the one on Form1 does not work any different
            using (Graphics gr = progressBar1.CreateGraphics())
            {
                progressBar1.Refresh();
                //prgfrm.progressBar1.Refresh();
                gr.DrawString(text,
                    SystemFonts.DefaultFont,
                    Brushes.Black,
                    new PointF(progressBar1.Width / 2 - (gr.MeasureString(text,
                                SystemFonts.DefaultFont).Width / 2.0F),
                                progressBar1.Height / 2 - (gr.MeasureString(text,
                                SystemFonts.DefaultFont).Height / 2.0F)));

                    //new PointF(prgfrm.progressBar1.Width / 2 - (gr.MeasureString(text,
                    //            SystemFonts.DefaultFont).Width / 2.0F),
                    //            prgfrm.progressBar1.Height / 2 - (gr.MeasureString(text,
                    //            SystemFonts.DefaultFont).Height / 2.0F)));

            }
            Application.DoEvents();
        }

        public void updateApps()
        {
            
            // This is used to find all Folders that end in a date and delete them
            foreach (string delDir in appDirs)
            {
                // Regular expression with the rmPattern to find numbers at the end of the title
                Match rmDir = Regex.Match(delDir, rmPattern);

                // While the regular expression rmDir is successful, 
                //delete the current directory and files listed     
                while (rmDir.Success)
                {
                    //listBox1.Items.Add("Deleting " + delDir.ToString());
                    SetText("Deleting " + delDir.ToString());

                    try
                    {
                        //Delete the named folder and when set true delete everything within the folder
                        Directory.Delete(rmDir.Value, true);
                        // Look for the next matching folder
                        rmDir = rmDir.NextMatch();
                    }
                    catch
                    {
                        SetText("The location " + rmDir.Value + " does not exist to delete");
                        continue;
                    }
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
                    //listBox1.Items.Add("Creating Backup " + repDir.ToString());
                    SetText("Creating Backup " + repDir.ToString());

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

            foreach (string thumbDir in Directory.GetDirectories(Path.Combine(rootDirectory.Name, "4WS")))
            {
                SetText("Updating Application from " + thumbDir);
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

        public void logFile()
        {
            StreamWriter log;

            if (!System.IO.File.Exists(path +logName))
            {
                log = new StreamWriter(path + logName);
            }
            else
            {
                log = System.IO.File.AppendText(path + logName);
            }

            // Write to the file:
            log.WriteLine("Date Time:" + DateTime.Now);
            log.WriteLine(Environment.NewLine);
            log.WriteLine("----------------------------------------------");
            foreach (string item in listBox1.Items)
            {
                log.WriteLine(item);
            }
            log.WriteLine("----------------------------------------------");
            log.WriteLine(Environment.NewLine);
            //Close the stream

            log.Close();
        }

        public Form1()
        {
            InitializeComponent();
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
        }

        // While updateApps is do background work on the progress bar
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 1; i <= 100; i++)
            {
                // Wait 100 milliseconds
                Thread.Sleep(60);

                // Report progress
                backgroundWorker1.ReportProgress(i);

            }
        }

        // Used to update the progress bar status
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //This approach will cause some lag due to the progressive animation style
            // Change the value of the ProgressBar to the BackgroundWorker progress.
            // prgfrm.progressBar1.Value = e.ProgressPercentage;

            

            // This approach will side step the lag by removing the progressive animation
            progressBar1.SetProgressNoAnimation(e.ProgressPercentage);

            // This approach will side step the lag by removing the progressive animation
            //prgfrm.progressBar1.SetProgressNoAnimation(e.ProgressPercentage);

            int percent = (int)((progressBar1.Value - progressBar1.Minimum) /
                (double)(progressBar1.Maximum - progressBar1.Minimum) * 100); 

            //int percent = (int)((prgfrm.progressBar1.Value - prgfrm.progressBar1.Minimum) /
            //   (double)(prgfrm.progressBar1.Maximum - prgfrm.progressBar1.Minimum) * 100);       

            paintProgress(percent.ToString() + "%");
            
            

            // Wait 100 milliseconds
            Thread.Sleep(30);
        }

        // Show the status of work when complete
        public void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                paintProgress("Error: " + e.Error.Message);
            }
            else
            {
                paintProgress("Done!");
                // Wait 1000 milliseconds
                Thread.Sleep(1000);
                this.UpdateSims.Enabled = true;
                progressBar1.Value = 0;
                progressBar1.Visible = false;
                //prgfrm.Close();
                //backgroundWorker1.Dispose();
            }

        }

        // Calls updateApps to start background work (This is the update sim button)
        private void UpdateSimulators(object sender, EventArgs e)
        {
            SetText("Date Time:" + DateTime.Now);
            SetText(String.Empty);

            // Place GetProcesses on a variable
            var runningProcesses = Process.GetProcesses();

            // Use a for loop to cycle through the processKill array to stop the named applications
            for (int i = 0; i < runningProcesses.Length; i++)
            {
                if (appProcess.Contains(runningProcesses[i].ProcessName))
                {
                    listBox1.Items.Add("Stopping " + runningProcesses[i]);
                    runningProcesses[i].Kill();
                    runningProcesses[i].WaitForExit();
                }
            }

            this.progressBar1.Visible = true;
            this.UpdateSims.Enabled = false;
            backgroundWorker1.WorkerReportsProgress = true;

            backgroundWorker1.RunWorkerAsync();

            Thread updateThread = new Thread(updateApps);
            updateThread.Start();
        }

        // Use to stop all known Simulators running
        private void StopSimulators(object sender, EventArgs e)
        {
            SetText("Date Time:" + DateTime.Now);
            SetText(String.Empty);

            // Place GetProcesses on a variable
            var runningProcesses = Process.GetProcesses();

            // Use a for loop to cycle through the processKill array to stop the named applications
            for (int i = 0; i < runningProcesses.Length; i++)
            {
                if (appProcess.Contains(runningProcesses[i].ProcessName))
                {
                    listBox1.Items.Add("Stopping " + runningProcesses[i]);
                    runningProcesses[i].Kill();
                    runningProcesses[i].WaitForExit();
                }
            }
        }

        // Used to start all simulators
        private void StartSimulators(object sender, EventArgs e)
        {
            try
            {
                SetText("Date Time:" + DateTime.Now);
                SetText(String.Empty);

                if (aecChkBx.Checked)
                {
                    SetText("Starting " + startApps[0]);
                    Process.Start(startApps[0]);
                }

                if (bscChkBx.Checked)
                {
                    SetText("Starting " + startApps[1]);
                    Process.Start(startApps[1]);
                }

                if (ismChkBx.Checked)
                {
                    SetText("Starting " + startApps[2]);
                    Process.Start(startApps[2]);
                }

                if (pickleChkBx.Checked)
                {
                    SetText("Starting " + startApps[3]);
                    Process.Start(startApps[3]);
                }

                if (scsAChkBx.Checked)
                {
                    SetText("Starting " + startApps[4]);
                    Process.Start(startApps[4]);
                }

                if (switchChkBx.Checked)
                {
                    SetText("Starting " + startApps[5]);
                    Process.Start(startApps[5]);
                }

                if (upsChkBx.Checked)
                {
                    SetText("Starting " + startApps[6]);
                    Process.Start(startApps[6]);
                }

                if (scsDChkBx.Checked)
                {
                    SetText("Starting " + startApps[7]);
                    Process.Start(startApps[7]);
                }

                if (!aecChkBx.Checked && !bscChkBx.Checked && !ismChkBx.Checked && !pickleChkBx.Checked && !scsAChkBx.Checked
                    && !scsDChkBx.Checked && !switchChkBx.Checked && !upsChkBx.Checked)
                {
                    MessageBox.Show("Please select the simulators to start");
                }


            }
            catch
            {
                listBox1.Items.Add("No application has been selected to start");
                
            }

        }

        // Handles the LAN check boxes and either enables or disables through a called method
        private void SetLocalAreaNetwork(object sender, EventArgs e)
        {
            int count = 0;
            SetText("Date Time:" + DateTime.Now);
            SetText(String.Empty);

            // Place GetProcesses on a variable
            var runningProcesses = Process.GetProcesses();

            // Use a for loop to cycle through the processKill array to stop the named applications
            for (int i = 0; i < runningProcesses.Length; i++)
            {
                if (appProcess.Contains(runningProcesses[i].ProcessName))
                {
                    listBox1.Items.Add("Stopping " + runningProcesses[i]);
                    runningProcesses[i].Kill();
                    runningProcesses[i].WaitForExit();
                }
            }

            foreach (string indexChecked in checkedListBox1.Items)
            {
                SetText(String.Empty);
                                    
                listBox1.Items.Add(indexChecked.ToString() + " Checked state is: " +
                                    checkedListBox1.GetItemCheckState(count).ToString() + ".");

                if (checkedListBox1.GetItemChecked(count))
                {
                    EnableLAN(checkedListBox1.Items[count]);
                }

                if (!checkedListBox1.GetItemChecked(count))
                {
                    DisableLAN(checkedListBox1.Items[count]);
                }
                count = ++count;
            }

            SetText(String.Empty);
        }

        // Empty, not used
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // Actual work to delete and add IP addresses
        private void SetIpNetwork(object sender, EventArgs e)
        {
            /* This will be used for the Progress bar on the IP network setup when completed correctly
             * 
             * this.UpdateSims.Enabled = false;
             * backgroundWorker1.WorkerReportsProgress = true;
             * backgroundWorker1.RunWorkerAsync();
             * Thread updateThread = new Thread(ConfigureIpNetwork);
             * updateThread.Start();
            */
            SetText("Date Time:" + DateTime.Now);
            SetText(String.Empty);

            // Place GetProcesses on a variable
            var runningProcesses = Process.GetProcesses();

            // Use a for loop to cycle through the processKill array to stop the named applications
            for (int i = 0; i < runningProcesses.Length; i++)
            {
                if (appProcess.Contains(runningProcesses[i].ProcessName))
                {
                    listBox1.Items.Add("Stopping " + runningProcesses[i]);
                    runningProcesses[i].Kill();
                    runningProcesses[i].WaitForExit();
                }
            }

            ConfigureIpNetwork();

        }

        // Checks the specified network based on which radial button is specified
        private void CheckNetwork(object sender, EventArgs e)
        {
            try
            {
                SetText("Date Time:" + DateTime.Now);
                SetText(String.Empty);

                if (radioButton1.Checked)
                {
                    listBox1.Items.Add("Healthmap IP Check:");
                    listBox1.Items.Add("-------------------------------------------------------");
                    ipConnection(hmapIP);

                    listBox1.Items.Add(String.Empty);

                    listBox1.Items.Add("AGS 1 IP Check:");
                    listBox1.Items.Add("-------------------------------------------------------");
                    ipConnection(ags1IP);

                    listBox1.Items.Add(String.Empty);

                    listBox1.Items.Add("AGS 2 IP Check:");
                    listBox1.Items.Add("-------------------------------------------------------");
                    ipConnection(ags2IP);

                    listBox1.Items.Add(String.Empty);

                    listBox1.Items.Add("ADMACS IP Check:");
                    listBox1.Items.Add("-------------------------------------------------------");
                    ipConnection(admacsIp);

                    listBox1.Items.Add(String.Empty);
                } 
                else if (radioButton2.Checked)
                {
                    listBox1.Items.Add("JCTS Healthmap IP Check:");
                    listBox1.Items.Add("-------------------------------------------------------");
                    ipConnection(jctsHmapIP);

                    listBox1.Items.Add(String.Empty);

                    listBox1.Items.Add("JCTS AGS 1 IP Check:");
                    listBox1.Items.Add("-------------------------------------------------------");
                    ipConnection(jctsAgs1IP);

                    listBox1.Items.Add(String.Empty);

                    listBox1.Items.Add("JCTS AGS 2 IP Check:");
                    listBox1.Items.Add("-------------------------------------------------------");
                    ipConnection(jctsAgs2IP);

                    listBox1.Items.Add(String.Empty);

                    listBox1.Items.Add("ADMACS IP Check:");
                    listBox1.Items.Add("-------------------------------------------------------");
                    ipConnection(admacsIp);

                    listBox1.Items.Add(String.Empty);
                }                
                else if (radioButton3.Checked)
                {
                    listBox1.Items.Add("NON-JCTS Healthmap IP Check:");
                    listBox1.Items.Add("-------------------------------------------------------");
                    ipConnection(njctsHmapIP);

                    listBox1.Items.Add(String.Empty);

                    listBox1.Items.Add("NON-JCTS AGS 1 IP Check:");
                    listBox1.Items.Add("-------------------------------------------------------");
                    ipConnection(njctsAgs1IP);

                    listBox1.Items.Add(String.Empty);

                    listBox1.Items.Add("NON-JCTS AGS 2 IP Check:");
                    listBox1.Items.Add("-------------------------------------------------------");
                    ipConnection(njctsAgs2IP);

                    listBox1.Items.Add(String.Empty);

                    listBox1.Items.Add("ADMACS IP Check:");
                    listBox1.Items.Add("-------------------------------------------------------");
                    ipConnection(admacsIp);

                    listBox1.Items.Add(String.Empty);
                }
                
            }
            catch (PingException pError)
            {
                MessageBox.Show(pError.Message);
            }
        }

        private void logFile_Click(object sender, EventArgs e)
        {
            logFile();

            SetText("Log file has been saved at " + path);
        }

        private void deleteLog_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void allChkBx_CheckedChanged(object sender, EventArgs e)
        {
            if (allChkBx.Checked)
            {
                aecChkBx.Checked = true;
                bscChkBx.Checked = true;
                ismChkBx.Checked = true;
                pickleChkBx.Checked = true;
                switchChkBx.Checked = true;
                upsChkBx.Checked = true;
                scsAChkBx.Checked = true;
                scsDChkBx.Checked = true;
                movChkBx.Checked = true;
            }
            else if (allChkBx.Checked == false)
            {
                aecChkBx.Checked = false;
                bscChkBx.Checked = false;
                ismChkBx.Checked = false;
                pickleChkBx.Checked = false;
                switchChkBx.Checked = false;
                upsChkBx.Checked = false;
                scsAChkBx.Checked = false;
                scsDChkBx.Checked = false;
                movChkBx.Checked = false;
            }
            

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void formHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void aboutToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 box = new AboutBox1();

            box.Show();
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }


    public static class ExtensionMethods
    {
        /// <summary>
        /// Sets the progress bar value, without using 'Windows Aero' animation.
        /// This is to work around a known WinForms issue where the progress bar
        /// is slow to update.
        /// </summary>
        public static void SetProgressNoAnimation(this ProgressBar pb, int value)
        {
            // To get around the progressive animation, we need to move the 
            // progress bar backwards.
            if (value == pb.Maximum)
            {
                // Special case as value can't be set greater than Maximum.
                pb.Maximum = value + 1;     // Temporarily Increase Maximum
                pb.Value = value + 1;       // Move past
                pb.Maximum = value;         // Reset maximum
            }
            else
            {
                pb.Value = value + 1;       // Move past
            }
            pb.Value = value;               // Move to correct value
        }
    }
}
