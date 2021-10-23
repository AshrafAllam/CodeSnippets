//source
  // http://www.vcskicks.com/

//Hardware Info
        private string GetHardwareInfo(string wmiClass, string wmiProperty, string wmiPropertyCond="", string wmiPropertyValueCond="") =>
            (from ManagementObject managObj in new ManagementClass(wmiClass).GetInstances()
             where managObj.Properties[wmiProperty].Value != null && (string.IsNullOrEmpty(wmiPropertyCond) || managObj.Properties[wmiPropertyCond].Value != null || managObj.Properties[wmiPropertyCond].Value.ToString() == wmiPropertyValueCond)
             select managObj.Properties[wmiProperty].Value.ToString()).FirstOrDefault();

Call
string CPUID = GetHardwareInfo("win32_processor", "processorID")
string MacIdFirstEnabledNetworkCardID => GetHardwareInfo("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled", "True");
string VideoIdPrimaryVideoControllerID => GetHardwareInfo("Win32_VideoController", "DriverVersion") + Identifier("Win32_VideoController", "Name");
string BaseIdMotherboardID => GetHardwareInfo("Win32_BaseBoard", "Model") + Identifier("Win32_BaseBoard", "Manufacturer") + Identifier("Win32_BaseBoard", "Name") + Identifier("Win32_BaseBoard", "SerialNumber");
string DiskIdMainPhysicalHardDriveID => Identifier("Win32_DiskDrive", "Model") + Identifier("Win32_DiskDrive", "Manufacturer") + Identifier("Win32_DiskDrive", "Signature") + Identifier("Win32_DiskDrive", "TotalHeads");
string BiosId_BIOS_Identifier => Identifier("Win32_BIOS", "Manufacturer") + Identifier("Win32_BIOS", "SMBIOSBIOSVersion") + Identifier("Win32_BIOS", "IdentificationCode") + Identifier("Win32_BIOS", "SerialNumber") + Identifier("Win32_BIOS", "ReleaseDate") + Identifier("Win32_BIOS", "Version");
Identifier("Win32_Processor", "UniqueId");
Identifier("Win32_Processor", "Manufacturer");
Identifier("Win32_Processor", "Name");
Identifier("Win32_Processor", "ProcessorId");
Identifier("Win32_Processor", "MaxClockSpeed");


        public string getWMI(string[] parameters)
        {
            string ip = parameters[0];
            string username = parameters[1];
            string password = parameters[2];
            string query = parameters[3]; //SELECT * FROM Win32_OperatingSystem
            string result = "";
            ConnectionOptions options = new ConnectionOptions();
            ManagementScope scope;
            options.Username = username;
            options.Password = password;
            try
            {
                scope = new ManagementScope("\\\\" + ip + "\\root\\cimv2", options);
                scope.Connect();
                if (scope.IsConnected)
                {
                    ObjectQuery q = new ObjectQuery(query);
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, q);
                    ManagementObjectCollection objCol = searcher.Get();
                    foreach (ManagementObject mgtObject in objCol)
                    {
                        result = result + mgtObject.GetText(TextFormat.CimDtd20);
                    }
                }
                else
                {
                }
            }
            catch (Exception e)
            {
                //writeLogFile("WMI Error: " + e.Message);
                //writeLog("WMI Error: " + e.Message);
            }
            return result;
        }

        


//
//  wmiClass="Win32_Processor" wmiProperty="UniqueId"
//  wmiClass="Win32_Processor" wmiProperty="ProcessorId"
//  wmiClass="Win32_Processor" wmiProperty="Name"
//  wmiClass="Win32_Processor" wmiProperty="Manufacturer"
//  wmiClass="Win32_Processor" wmiProperty="MaxClockSpeed"
//  wmiClass="Win32_BIOS" wmiProperty="Manufacturer"
//  wmiClass="Win32_BIOS" wmiProperty="SMBIOSBIOSVersion"
//  wmiClass="Win32_BIOS" wmiProperty="IdentificationCode"
//  wmiClass="Win32_BIOS" wmiProperty=SerialNumber""
//  wmiClass="Win32_BIOS" wmiProperty="ReleaseDate"
//  wmiClass="Win32_BIOS" wmiProperty="Version"
//  wmiClass="Win32_DiskDrive" wmiProperty="Model"
//  wmiClass="Win32_DiskDrive" wmiProperty="Manufacturer"
//  wmiClass="Win32_DiskDrive" wmiProperty="Signature"
//  wmiClass="Win32_DiskDrive" wmiProperty="TotalHeads"
//  wmiClass="Win32_BaseBoard" wmiProperty="Model"
//  wmiClass="Win32_BaseBoard" wmiProperty="Manufacturer"
//  wmiClass="Win32_BaseBoard" wmiProperty="Name"
//  wmiClass="Win32_BaseBoard" wmiProperty="SerialNumber"
//  wmiClass="Win32_VideoController" wmiProperty="DriverVersion"
//  wmiClass="Win32_VideoController" wmiProperty="Name"
private void Get(string wmiClass, string wmiProperty){
    string result = "";
foreach (ManagementBaseObject mo in new ManagementClass(wmiClass).GetInstances())
{
    //Only get the first one
    if (result != "") continue;
    try
    {
        result = mo[wmiProperty].ToString();
        break;
    }
    catch
    {
    }
}
return result;

}

// wmiClass="Win32_NetworkAdapterConfiguration" wmiProperty="MACAddress" wmiMustBeTrue="IPEnabled"
private void Get(string wmiClass, string wmiProperty, string wmiMustBeTrue){
    string result = "";
System.Management.ManagementClass mc = new System.Management.ManagementClass(wmiClass);
System.Management.ManagementObjectCollection moc = mc.GetInstances();
foreach (System.Management.ManagementBaseObject mo in moc)
{
    if (mo[wmiMustBeTrue].ToString() != "True") continue;
    //Only get the first one
    if (result != "") continue;
    try
    {
        result = mo[wmiProperty].ToString();
        break;
    }
    catch
    {
    }
}
return result;


}