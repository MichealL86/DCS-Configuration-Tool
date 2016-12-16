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
using DCS_Configuration_Tool;

namespace DCS_Configuration_Tool
{



    public partial class Form1 : Form
    {

        private updateApps update;
        public static Progress_Form prgfrm = new Progress_Form();
        
        
        // String array to hold all application names for DCS
        string[] appProcess = { "BSCSimulator", "DCSSimulator", "WindowsFormsApplication1", "MovEmulator",
                                "ModbusTestClient", "ScsAdmacsSim", "ScsDisplay", "SwitchSimulator",
                                "UPSSimulator"};

        // String array to hold all process names for DCS
        string[] processName = {"BSC", "DCS", "ISM", "Mov", "Pickle", "SCS_D", "ScsA", "Switch", "UPS",
                                "AEC", "ROCS", "SNMP"};

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

        // A PingReplay variable named reply
        PingReply reply;
        // An integer variable named ipCount
        int ipCount;

        static string path = @"C:\Program Files (x86)\General Atomics\";
        string logName = string.Format("DCSLog_{0:MMddyyyy}.txt", DateTime.Now) ;
        string bscConfigPath = @"C:\Program Files (x86)\General Atomics\BSCSimulator\BSCSimulator.exe.config";
        string fullBSCExeConfig = "<add key=\"CountAECs\" value=\"0\"/> ";
        string jctsBSCExeConfig = "<add key=\"CountAECs\" value=\"4\"/> ";
        string nonJctsBSCExeConfig = "<add key=\"CountAECs\" value=\"1\"/> ";
        string jctsBSCAddKey = "<add key=\"0\" value=\"BSC0\"/>";
        string nonJctsBSCAddKey = "<add key=\"0\" value=\"BSC1\"/>";

        string aecConfigPath = @"C:\Program Files (x86)\General Atomics\DCS_Sim\DCSSimulator.exe.config";
        string fullAECExeConfig = "<add key=\"CountAECs\" value=\"0\"/> <!-- Set the number to disable the AECs from 0 thru 3 -->";
        string jctsAECExeConfig = "<add key=\"CountAECs\" value=\"4\"/> <!-- Set the number to disable the AECs from 0 thru 3 -->";
        string nonJctsAECExeconfig = "<add key=\"CountAECs\" value=\"1\"/> <!-- Set the number to disable the AECs from 0 thru 3 -->";
        string jctsAECAddKey = "<add key=\"0\" value=\"AEC0\"/>";
        string nonJctsAECAddKey = "<add key=\"0\" value=\"AEC1\"/>";

  
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
                    listBox1.Items.Add("All IP addresses connected successfully");
                }
                else if (ipCount != arrLAN.Length)
                {
                    listBox1.Items.Add("The following address connected unsuccessfully");
                    for (int i = 0; i < arrLAN.Length; i++)
                    {
                        string message = noConnection[i].ToString();
                        listBox1.Items.Add("Could not establish connection to " + message);
                    }

                }
            }
            catch (NullReferenceException e)
            {
                listBox1.Items.Add("If address '172.16.4.222' make sure ScsAdmacsSim is on and running with SCS");
                listBox1.Items.Add("or ignore the unsuccessful connection");
                listBox1.Items.Add("For all other addresses check to make sure the address exist within the specified LAN");
            }

        }

        // Using a process delete the specified IP addresses
        public void deleteIP(object interfaceName, string[] arrLAN)
        {
            foreach (string ip in arrLAN)
            {
                listBox1.Items.Add("Deleting " + arrLAN.ToString() + " IP address " + ip);
                ProcessStartInfo psi = new ProcessStartInfo("netsh", string.Format("interface ipv4 delete address name=\"{0}\" addr={1}", interfaceName, ip));
                Process p = new Process();
                p.StartInfo = psi;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.UseShellExecute = false;
                p.Start();
            }

            Thread.Sleep(500);
        }

        // Using a process add the specified IP addresses
        public void addIP(object interfaceName, string[] arrLAN)
        {
            foreach (string ip in arrLAN)
            {
                listBox1.Items.Add("Adding " + arrLAN.ToString() + " IP address " + ip);

                if (ip == "172.20.1.1")
                {
                    ProcessStartInfo psi = new ProcessStartInfo("netsh", string.Format("interface ipv4 set address name=\"{0}\" addr={1} mask=255.255.0.0 gateway=172.20.0.1", interfaceName, ip));
                    Process p = new Process();
                    p.StartInfo = psi;
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = false;
                    p.Start();
                }
                else if (ip == "172.21.1.1")
                {
                    ProcessStartInfo psi = new ProcessStartInfo("netsh", string.Format("interface ipv4 set address name=\"{0}\" addr={1} mask=255.255.0.0", interfaceName, ip));
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

            Thread.Sleep(500);
        }

        // Use paint graphics to create a progress bar that shows the status inside of it
        public void paintProgress(string text)
        {
            using (Graphics gr = prgfrm.progressBar1.CreateGraphics())
            {
                prgfrm.progressBar1.Refresh();
                gr.DrawString(text,
                    SystemFonts.DefaultFont,
                    Brushes.Black,
                    new PointF(prgfrm.progressBar1.Width / 2 - (gr.MeasureString(text,
                                SystemFonts.DefaultFont).Width / 2.0F),
                                prgfrm.progressBar1.Height / 2 - (gr.MeasureString(text,
                                SystemFonts.DefaultFont).Height / 2.0F)));

            }
            Application.DoEvents();
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
                // Report progress
                backgroundWorker1.ReportProgress(i);

                // Wait 100 milliseconds
                Thread.Sleep(60);
                
            }
        }

        // Used to update the progress bar status
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //This approach will cause some lag due to the progressive animation style
            // Change the value of the ProgressBar to the BackgroundWorker progress.
            // prgfrm.progressBar1.Value = e.ProgressPercentage;

            // This approach will side step the lag by removing the progressive animation
            prgfrm.progressBar1.SetProgressNoAnimation(e.ProgressPercentage);
            
            int percent = (int)((prgfrm.progressBar1.Value - prgfrm.progressBar1.Minimum) /
                (double)(prgfrm.progressBar1.Maximum - prgfrm.progressBar1.Minimum) * 100);       

            paintProgress(percent.ToString() + "%");
        }

        // Show the status of work when complete
        public void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //Progress_Form prgfrm = new Progress_Form();

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
                prgfrm.Close();
                backgroundWorker1.Dispose();
            }

        }

        // Calls updateApps to start background work (This is the update sim button)
        private void UpdateSimulators(object sender, EventArgs e)
        {


            this.UpdateSims.Enabled = false;
            backgroundWorker1.WorkerReportsProgress = true;
            prgfrm.Show();
            backgroundWorker1.RunWorkerAsync(update = new updateApps(this));
            
        }

        // Use to stop all known Simulators running
        private void StopSimulators(object sender, EventArgs e)
        {
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
            Process.Start("C:\\Program Files (x86)\\General Atomics\\DCS_Sim\\DCSSimulator.exe");
            Process.Start("C:\\Program Files (x86)\\General Atomics\\BSCSimulator\\BSCSimulator.exe");
            Process.Start("C:\\Program Files (x86)\\General Atomics\\ISM_Sim\\WindowsFormsApplication1.exe");
            Process.Start("C:\\Program Files (x86)\\General Atomics\\PickleSwitchSim\\ModbusTestClient.exe");
            Process.Start("C:\\Program Files (x86)\\General Atomics\\SCS_Display\\ScsDisplay.exe");
            Process.Start("C:\\Program Files (x86)\\General Atomics\\ScsAdmacsSim\\ScsAdmacsSim.exe");
            Process.Start("C:\\Program Files (x86)\\General Atomics\\Switch_Sim\\SwitchSimulator.exe");
            Process.Start("C:\\Program Files (x86)\\General Atomics\\UPS_Sim\\UPSSimulator.exe");
        }

        // Handles the LAN check boxes and either enables or disables through a called method
        private void SetLocalAreaNetwork(object sender, EventArgs e)
        {
            int count = 0;
            foreach (string indexChecked in checkedListBox1.Items)
            {
                
                listBox1.Items.Add(indexChecked.ToString() + ", is checked. Checked state is: " +
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
        }

        // Empty, not used
        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // Actual work to delete and add IP addresses
        private void SetIpNetwork(object sender, EventArgs e)
        {
            if (radioButton1.Checked) // Full Setup
            {
                listBox1.Items.Add("Healthmap IP configuration:");
                listBox1.Items.Add("---------------------------------------------------------------------");
                deleteIP(checkedListBox1.Items[0], hmapIP);
                listBox1.Items.Add(String.Empty);
                addIP(checkedListBox1.Items[0], hmapIP);

                listBox1.Items.Add(String.Empty);

                listBox1.Items.Add("AGS 1 IP configuration:");
                listBox1.Items.Add("---------------------------------------------------------------------");
                deleteIP(checkedListBox1.Items[1], ags1IP);
                listBox1.Items.Add(String.Empty);
                addIP(checkedListBox1.Items[1], ags1IP);

                listBox1.Items.Add(String.Empty);

                listBox1.Items.Add("AGS 2 IP configuration:");
                listBox1.Items.Add("---------------------------------------------------------------------");
                deleteIP(checkedListBox1.Items[2], ags2IP);
                listBox1.Items.Add(String.Empty);
                addIP(checkedListBox1.Items[2], ags2IP);

                listBox1.Items.Add(String.Empty);

                listBox1.Items.Add("ADMACS IP configuration:");
                listBox1.Items.Add("---------------------------------------------------------------------");
                deleteIP(checkedListBox1.Items[3], admacsIp);
                listBox1.Items.Add(String.Empty);
                addIP(checkedListBox1.Items[3], admacsIp);

                listBox1.Items.Add(String.Empty);

                // Modify the .exe.config file for the BSC so that it works correctly
                listBox1.Items.Add("Configuring the BSCSimulator.exe.config file");
                String fullBSCtext = System.IO.File.ReadAllText(bscConfigPath);
                fullBSCtext = fullBSCtext.Replace(jctsBSCExeConfig, fullBSCExeConfig);
                fullBSCtext = fullBSCtext.Replace(nonJctsBSCExeConfig, fullBSCExeConfig);
                fullBSCtext = fullBSCtext.Replace(jctsBSCAddKey, nonJctsBSCAddKey);
                System.IO.File.WriteAllText(bscConfigPath, fullBSCtext);

                // Modify the .exe.config file for the DCS so that it works correctly
                listBox1.Items.Add("Configuring the AECSimulator.exe.config file");
                String fullAECtext = System.IO.File.ReadAllText(aecConfigPath);
                fullAECtext = fullAECtext.Replace(jctsAECExeConfig, fullAECExeConfig);
                fullAECtext = fullAECtext.Replace(nonJctsAECExeconfig, fullAECExeConfig);
                fullAECtext = fullAECtext.Replace(jctsAECAddKey, nonJctsAECAddKey);
                System.IO.File.WriteAllText(aecConfigPath, fullAECtext);

                listBox1.Items.Add(String.Empty);

            }
            
            else if (radioButton2.Checked) // JCTS
            {
                listBox1.Items.Add("JCTS Healthmap IP configuration:");
                listBox1.Items.Add("---------------------------------------------------------------------");
                deleteIP(checkedListBox1.Items[0], hmapIP);
                listBox1.Items.Add(String.Empty);
                addIP(checkedListBox1.Items[0], jctsHmapIP);

                listBox1.Items.Add(String.Empty);

                listBox1.Items.Add("JCTS AGS 1 IP configuration:");
                listBox1.Items.Add("---------------------------------------------------------------------");
                deleteIP(checkedListBox1.Items[1], ags1IP);
                listBox1.Items.Add(String.Empty);
                addIP(checkedListBox1.Items[1], jctsAgs1IP);

                listBox1.Items.Add(String.Empty);

                listBox1.Items.Add("JCTS AGS 2 IP configuration:");
                listBox1.Items.Add("---------------------------------------------------------------------");
                deleteIP(checkedListBox1.Items[2], ags2IP);
                listBox1.Items.Add(String.Empty);
                addIP(checkedListBox1.Items[2], jctsAgs2IP);

                listBox1.Items.Add(String.Empty);

                listBox1.Items.Add("ADMACS IP configuration:");
                listBox1.Items.Add("---------------------------------------------------------------------");
                deleteIP(checkedListBox1.Items[3], admacsIp);
                listBox1.Items.Add(String.Empty);
                addIP(checkedListBox1.Items[3], admacsIp);

                listBox1.Items.Add(String.Empty);

                // Modify the .exe.config file for the BSC so that it works correctly
                listBox1.Items.Add("Configuring the BSCSimulator.exe.config file");
                String nonJctsBSCtext = System.IO.File.ReadAllText(bscConfigPath);
                nonJctsBSCtext = nonJctsBSCtext.Replace(jctsBSCExeConfig, nonJctsBSCExeConfig);
                nonJctsBSCtext = nonJctsBSCtext.Replace(fullBSCExeConfig, nonJctsBSCExeConfig);
                nonJctsBSCtext = nonJctsBSCtext.Replace(jctsBSCAddKey, nonJctsBSCAddKey);
                System.IO.File.WriteAllText(bscConfigPath, nonJctsBSCtext);
               

                // Modify the .exe.config file for the DCS so that it works correctly
                listBox1.Items.Add("Configuring the AECSimulator.exe.config file");
                String nonJctsAECtext = System.IO.File.ReadAllText(aecConfigPath);
                nonJctsAECtext = nonJctsAECtext.Replace(jctsAECExeConfig, nonJctsAECExeconfig);
                nonJctsAECtext = nonJctsAECtext.Replace(fullAECExeConfig, nonJctsAECExeconfig);
                nonJctsAECtext = nonJctsAECtext.Replace(jctsAECAddKey, nonJctsAECAddKey);
                System.IO.File.WriteAllText(aecConfigPath, nonJctsAECtext);

                listBox1.Items.Add(String.Empty);
            }
            else if (radioButton3.Checked) // NON-JCTS
            {
                listBox1.Items.Add("NON-JCTS Healthmap IP configuration:");
                listBox1.Items.Add("---------------------------------------------------------------------");
                deleteIP(checkedListBox1.Items[0], hmapIP);
                listBox1.Items.Add(String.Empty);
                addIP(checkedListBox1.Items[0], njctsHmapIP);

                listBox1.Items.Add(String.Empty);

                listBox1.Items.Add("NON-JCTS AGS 1 IP configuration:");
                listBox1.Items.Add("---------------------------------------------------------------------");
                deleteIP(checkedListBox1.Items[1], ags1IP);
                listBox1.Items.Add(String.Empty);
                addIP(checkedListBox1.Items[1], njctsAgs1IP);

                listBox1.Items.Add(String.Empty);

                listBox1.Items.Add("NON-JCTS AGS2 IP configuration:");
                listBox1.Items.Add("---------------------------------------------------------------------");
                deleteIP(checkedListBox1.Items[2], ags2IP);
                listBox1.Items.Add(String.Empty);
                addIP(checkedListBox1.Items[2], njctsAgs2IP);

                listBox1.Items.Add(String.Empty);

                listBox1.Items.Add("ADMACS IP configuration:");
                listBox1.Items.Add("---------------------------------------------------------------------");
                deleteIP(checkedListBox1.Items[3], admacsIp);
                listBox1.Items.Add(String.Empty);
                addIP(checkedListBox1.Items[3], admacsIp);

                listBox1.Items.Add(String.Empty);

                // Modify the .exe.config file for the BSC so that it works correctly
                listBox1.Items.Add("Configuring the BSCSimulator.exe.config file");
                String jctsBSCtext = System.IO.File.ReadAllText(bscConfigPath);
                jctsBSCtext = jctsBSCtext.Replace(nonJctsBSCExeConfig, jctsBSCExeConfig);
                jctsBSCtext = jctsBSCtext.Replace(fullBSCExeConfig, jctsBSCExeConfig);
                jctsBSCtext = jctsBSCtext.Replace(nonJctsBSCAddKey, jctsBSCAddKey);
                System.IO.File.WriteAllText(bscConfigPath, jctsBSCtext);

                // Modify the .exe.config file for the DCS so that it works correctly
                listBox1.Items.Add("Configuring the AECSimulator.exe.config file");
                String jctsAECtext = System.IO.File.ReadAllText(aecConfigPath);
                jctsAECtext = jctsAECtext.Replace(nonJctsAECExeconfig, jctsAECExeConfig);
                jctsAECtext = jctsAECtext.Replace(fullAECExeConfig, jctsAECExeConfig);
                jctsAECtext = jctsAECtext.Replace(nonJctsAECAddKey, jctsAECAddKey);
                System.IO.File.WriteAllText(aecConfigPath, jctsAECtext);

                listBox1.Items.Add(String.Empty);
            }
            
        }

        // Checks the specified network based on which radial button is specified
        private void CheckNetwork(object sender, EventArgs e)
        {
            try
            {

                if (radioButton1.Checked)
                {
                    listBox1.Items.Add("Healthmap IP check:");
                    listBox1.Items.Add("---------------------------------------------------------------------");
                    ipConnection(hmapIP);

                    listBox1.Items.Add(String.Empty);

                    listBox1.Items.Add("AGS 1 IP check:");
                    listBox1.Items.Add("---------------------------------------------------------------------");
                    ipConnection(ags1IP);

                    listBox1.Items.Add(String.Empty);

                    listBox1.Items.Add("AGS 2 IP check:");
                    listBox1.Items.Add("---------------------------------------------------------------------");
                    ipConnection(ags2IP);

                    listBox1.Items.Add(String.Empty);

                    listBox1.Items.Add("ADMACS IP check:");
                    listBox1.Items.Add("---------------------------------------------------------------------");
                    ipConnection(admacsIp);

                    listBox1.Items.Add(String.Empty);
                } 
                else if (radioButton2.Checked)
                {
                    listBox1.Items.Add("JCTS Healthmap IP check:");
                    listBox1.Items.Add("---------------------------------------------------------------------");
                    ipConnection(jctsHmapIP);

                    listBox1.Items.Add(String.Empty);

                    listBox1.Items.Add("JCTS AGS 1 IP check:");
                    listBox1.Items.Add("---------------------------------------------------------------------");
                    ipConnection(jctsAgs1IP);

                    listBox1.Items.Add(String.Empty);

                    listBox1.Items.Add("JCTS AGS 2 IP check:");
                    listBox1.Items.Add("---------------------------------------------------------------------");
                    ipConnection(jctsAgs2IP);

                    listBox1.Items.Add(String.Empty);

                    listBox1.Items.Add("JCTS ADMACS IP check:");
                    listBox1.Items.Add("---------------------------------------------------------------------");
                    ipConnection(admacsIp);

                    listBox1.Items.Add(String.Empty);
                }                
                else if (radioButton3.Checked)
                {
                    listBox1.Items.Add("NON-JCTS Healthmap IP check:");
                    listBox1.Items.Add("---------------------------------------------------------------------");
                    ipConnection(njctsHmapIP);

                    listBox1.Items.Add(String.Empty);

                    listBox1.Items.Add("NON-JCTS AGS 1 IP check:");
                    listBox1.Items.Add("---------------------------------------------------------------------");
                    ipConnection(njctsAgs1IP);

                    listBox1.Items.Add(String.Empty);

                    listBox1.Items.Add("NON-JCTS AGS 2 IP check:");
                    listBox1.Items.Add("---------------------------------------------------------------------");
                    ipConnection(njctsAgs2IP);

                    listBox1.Items.Add(String.Empty);

                    listBox1.Items.Add("ADMACS IP check:");
                    listBox1.Items.Add("---------------------------------------------------------------------");
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
        }

        private void deleteLog_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
    }

    // Class used to remove old apps, backup current apps, and place the new apps in the proper locations
    class updateApps
    {
        public Progress_Form prgfrm = new Progress_Form();
        private Form1 instance;
        DirectoryInfo rootDirectory;
        string aecPath = @"C:\";
        static string path = @"C:\Program Files (x86)\General Atomics";
        string[] appDirs = Directory.GetDirectories(path);
        string[] desktopLinks;
        string[] appProcess = { "BSCSimulator", "DCSSimulator", "WindowsFormsApplication1", "MovEmulator",
                                "ModbusTestClient", "ScsAdmacsSim",  "SwitchSimulator",
                                "UPSSimulator", "ScsDisplay", };

        string[] processName = {"BSC", "DCS", "ISM", "Mov", "Pickle", "SCS_D", "ScsA", "Switch", "UPS",
                                "AEC", "ROCS", "SNMP"};

        // Pattern for : name + eight numbers at the end
        string rmPattern = ("^([a-zA-Z]:)?(\\\\[^<>:\"/\\\\|?*]+)+(\\d{8})$");
        // Pattern for : name 
        string mvPattern = ("^([a-zA-Z]:)?(\\\\[^<>:\"/\\\\|?*]+)+\\\\?$");
        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        // AddDirectory method is used to give the named directory certain permissions
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
            string userName = @"hmuser";

            for (int i = 0; i < dirArr.Length; i++)
            {
                if (dirArr[i] == mainPath + oldDCS)
                {
                    try
                    {
                        for (int j = 1; j < 5; j++)
                        {
                            int count = j;
                            
                            instance.listBox1.Items.Add("Moving old AEC Folders to  " + dirArr[i] + @"\AEC" + count);
                            prgfrm.resultLabel.Text = ("Moving old AEC Folders to  " + dirArr[i] + @"\AEC" + count);
                            Directory.Move(aecHome + @"\AEC" + count, dirArr[i] + @"\AEC" + count);
                        }

                        instance.listBox1.Items.Add("Moving old ID folder to  " + dirArr[i] + @"\ID");
                        prgfrm.resultLabel.Text = ("Moving old ID folder to  " + dirArr[i] + @"\ID");
                        Directory.Move(aecHome + @"\ID", dirArr[i] + @"\ID");

                        if (Directory.Exists(aecHome + string.Format(@"\Users\{0}\DataFolder", userName)))
                        {
                            instance.listBox1.Items.Add("Moving old DataFolder to  " + dirArr[i] + @"\DataFolder");
                            prgfrm.resultLabel.Text = ("Moving old ID folder to  " + dirArr[i] + @"\DataFolder");
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
                        instance.listBox1.Items.Add("The AEC Folders already exists in " + dirArr[i] + 
                            Environment.NewLine + "Error Message: " + e.Message);

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
                            instance.listBox1.Items.Add("Moving new AEC Folders to  " + aecHome + @"\AEC" + count);
                            prgfrm.resultLabel.Text = ("Moving new AEC Folders to  " + aecHome + @"\AEC" + count);
                            Directory.Move(dirArr[i] + @"\AEC" + count, aecHome + @"\AEC" + count);
                            instance.listBox1.Items.Add("Adding permissions to   " + aecHome + @"\AEC" + count);
                            prgfrm.resultLabel.Text = ("Adding permissions to   " + aecHome + @"\AEC" + count);
                            AddDirectorySecurity(aecHome + @"\AEC" + count, Environment.UserName, FileSystemRights.FullControl,
                                                        AccessControlType.Allow);
                        }

                        instance.listBox1.Items.Add("Moving new ID Folder to  " + aecHome + @"\ID");
                        prgfrm.resultLabel.Text = ("Moving new ID Folder to  " + aecHome + @"\ID");
                        Directory.Move(dirArr[i] + @"\ID", aecHome + @"\ID");
                        instance.listBox1.Items.Add("Adding permissions to  " + aecHome + @"\ID");
                        prgfrm.resultLabel.Text = ("Adding permissions to  " + aecHome + @"\ID");
                        AddDirectorySecurity(aecHome + @"\ID", Environment.UserName, FileSystemRights.FullControl,
                                                        AccessControlType.Allow);

                        instance.listBox1.Items.Add("Moving new DataFolder Folder to  " + dataFldDir);
                        prgfrm.resultLabel.Text = ("Moving new DataFolder Folder to  " + dataFldDir);
                        Directory.Move(dirArr[i] + @"\DataFolder", dataFldDir);
                        instance.listBox1.Items.Add("Adding permissions to  " + dataFldDir);
                        prgfrm.resultLabel.Text = ("Adding permissions to  " + dataFldDir);
                        AddDirectorySecurity(dataFldDir, Environment.UserName, FileSystemRights.FullControl,
                                                        AccessControlType.Allow);

                        if (!Directory.Exists(TransFldDir))
                        {
                            instance.listBox1.Items.Add("Creating new TransFolder at  " + TransFldDir);
                            prgfrm.resultLabel.Text = ("Creating new TransFolder at  " + TransFldDir);
                            Directory.CreateDirectory(TransFldDir);
                        }
                        else if (Directory.Exists(TransFldDir))
                        {
                            instance.listBox1.Items.Add("The TransFolder Directory Exists");
                        }

                    }
                    catch (Exception e)
                    {
                        instance.listBox1.Items.Add(e.Message);
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
                    instance.listBox1.Items.Add("Copying new ASF files from thumb drive");
                    prgfrm.resultLabel.Text = ("Copying new ASF files from thumb drive");
                    System.IO.File.Copy(asfFile, asfLocation + i + @"\AirCraftSettingsFile.csv", true);
                }
            }
            else
            {
                instance.listBox1.Items.Add("AircraftSettingsFile.csv file missing on thumbdrive");
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
                        prgfrm.resultLabel.Text = ("Created shortcut " + shortcutAddress);
                        instance.listBox1.Items.Add("Created shortcut " + shortcutAddress);
                    }
                    else if (appName[count] == "ModbusTestClient")
                    {
                        shortcutAddress = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\PickleSwitchSim" + ".lnk";
                        prgfrm.resultLabel.Text = ("Created shortcut " + shortcutAddress);
                        instance.listBox1.Items.Add("Created shortcut " + shortcutAddress);
                    }
                    else
                    {
                        shortcutAddress = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + appName[count] + ".lnk";
                        prgfrm.resultLabel.Text = ("Created shortcut " + shortcutAddress);
                        instance.listBox1.Items.Add("Created shortcut " + shortcutAddress);
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

        public void DeleteShortcut(string[] appName, string srcDir, string[] delLink)
        {
            for (int i = 0; i < appName.Length; i++)
            {
                delLink = Directory.GetFiles(srcDir, string.Format("{0}*.lnk", appName[i]));

                foreach (string link in delLink)
                {
                    instance.listBox1.Items.Add("Deleting " + link);
                    prgfrm.resultLabel.Text = ("Deleting " + link);
                    System.IO.File.Delete(link);
                }
            }

            prgfrm.resultLabel.Text = String.Empty;
        }


        public updateApps(Form1 instance)
        {
            this.instance = instance;
            // This is used to find all Folders that end in a date and delete them
            foreach (string delDir in appDirs)
            {
                // Regular expression with the rmPattern to find numbers at the end of the title
                Match rmDir = Regex.Match(delDir, rmPattern);

                // While the regular expression rmDir is successful, 
                //delete the current directory and files listed     
                while (rmDir.Success)
                {
                    instance.listBox1.Items.Add("Deleting " + delDir.ToString());
                    prgfrm.resultLabel.Text = ("Deleting " + delDir.ToString());
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
                    instance.listBox1.Items.Add("Creating Backup " + repDir.ToString());
                    prgfrm.resultLabel.Text = ("Creating Backup " + repDir.ToString());
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
