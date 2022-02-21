using MadMilkman.Ini;
using System.Threading.Tasks;

namespace BemmTikTokv3
{
    class Setting
    {
        string path;
       public string getSetting(string sub, string key)
        {
            var ini = new IniFile();
            ini.Load(path + "\\Setting.ini");
            IniSection iniSection = ini.Sections[sub];
            string result = iniSection.Keys[key].Value;
            return result;
        }

       public void setSetting(string sub, string key, string data)
        {
            var ini = new IniFile();
            ini.Load(path + "\\Setting.ini");
            IniSection iniSection = ini.Sections[sub];
            iniSection.Keys[key].Value = data;
            ini.Save(path + "\\Setting.ini");
        }
        public Setting(string path)
        {
            this.path = path;
        }
    }
    public class testpral{
      
    }
}
