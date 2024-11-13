namespace ConfigurationUDP
{
    using System.Xml.Linq;

    public class ConfigurationManager
    {
        private readonly string _configFilePath;

        public ConfigurationManager()
        {
            this._configFilePath = "config.xml"; ;
        }

        public (string Address, int Port) LoadMulticastConfig()
        {
            XDocument configXml = XDocument.Load(_configFilePath);

            string address = configXml.Descendants("Address").FirstOrDefault()?.Value;
            int port = int.Parse(configXml.Descendants("Port").FirstOrDefault()?.Value);

            return (address, port);
        }

        public (int MinValue, int MaxValue) LoadRandomConfig()
        {
            XDocument configXml = XDocument.Load(_configFilePath);

            int minValue = int.Parse(configXml.Descendants("MinValue").FirstOrDefault()?.Value);
            int maxValue = int.Parse(configXml.Descendants("MaxValue").FirstOrDefault()?.Value);

            return (minValue, maxValue);
        }
    }
}
