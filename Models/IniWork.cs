using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TIniFile;

namespace PlugY_Configurator_Avalonia.Models
{
    class IniWork
    {
        public INIFile _iniPlugy;
        public INIFile? _iniDefault = null;
        public INIFile? IniBaseMod = null;
        public bool NotSet = true;

        private List<string>? _avlblLngs;


        public void Initialization(string path)
        {
            _iniPlugy = new INIFile(path, false, true);

            string mainDir = Path.GetDirectoryName(path);
            string iniDefaultPath = Path.Combine(mainDir, "PlugY", "PlugYDefault.ini");
            if (File.Exists(iniDefaultPath))
                _iniDefault = new INIFile(iniDefaultPath);

            string iniBaseModPath = Path.Combine(mainDir, "BaseMod.ini");
            if (File.Exists(iniBaseModPath))
                IniBaseMod = new INIFile(iniBaseModPath, false, true);

            try
            {
                var tmp_avlblLngs = GetVal("LANGUAGE", "AvailableLanguages", "");
                if(!string.IsNullOrEmpty(tmp_avlblLngs))
                _avlblLngs = new List<string>(tmp_avlblLngs.Split('|'));
            }
            catch (System.Exception)
            { }
        }

        public bool GetAvailableLanguages(string lng)
        {
            try
            {
	            var r = _avlblLngs?.Find(s => s.Contains(lng));
	            if (!string.IsNullOrEmpty(r))
	                return true;
            }
            catch (Exception)
            { }

            return false;
        }

        public void SetAvailableLanguages(string lng)
        {
            if (NotSet) return;

            if (_avlblLngs != null)
                _avlblLngs.Add(lng);
            else _avlblLngs = new List<string>() { lng };

            SetVal("LANGUAGE", "AvailableLanguages", string.Join("|", _avlblLngs));
        }

        public void DelAvailableLanguages(string lng)
        {
            if (NotSet) return;

            if (_avlblLngs != null)
            {
                _avlblLngs.Remove(lng);
                SetVal("LANGUAGE", "AvailableLanguages", string.Join("|", _avlblLngs));
            }
        }

        public string GetVal(string section, string param, string def = "")
        {
            try
            {
	            string paramIniPlugy = _iniPlugy.GetValue(section, param, def);
	
	            if (string.IsNullOrEmpty(paramIniPlugy))
	                return _iniDefault?.GetValue(section, param, def) ?? "";
	
	            return paramIniPlugy;
            }
            catch (Exception)
            { }

            return def;
        }

        public bool GetVal(string section, string param, bool def = false)
        {
            try
            {
	            var paramIniPlugy = _iniPlugy.GetValue(section, param, (bool?)null);
	
	            if (paramIniPlugy == null)
	            {
	                var result = _iniDefault?.GetValue(section, param, (bool?)null);
	                return result ?? def;
	            }

                return paramIniPlugy ?? def;
            }
            catch (Exception)
            { }

            return def;
        }

        public int GetVal(string section, string param, int def = 0)
        {
            try
            {
	            int? paramIniPlugy = _iniPlugy.GetValue(section, param, (int?)null);
	
	            if (paramIniPlugy == null)
	            {
	                int? result = _iniDefault?.GetValue(section, param, (int?)null);
	                return result ?? def;
	            }
	
	            return paramIniPlugy ?? def;
            }
            catch (Exception)
            { }

            return def;
        }

        public void SetVal(string section, string param, string value)
        {
            if (NotSet) return;

            string exisVal = GetVal(section, param, string.Empty);

            if (exisVal != value)
                _iniPlugy.SetValue(section, param, value);
        }

        public void SetVal(string section, string param, bool value)
        {
            if (NotSet) return;

            string exisVal = GetVal(section, param, string.Empty);

            bool luck = bool.TryParse(exisVal, out bool exisValB);
            if (!luck || luck && exisValB != value)
                _iniPlugy.SetValue(section, param, value);
        }

        public void SetVal(string section, string param, int value)
        {
            if (NotSet) return;

            string exisVal = GetVal(section, param, string.Empty);

            bool luck = int.TryParse(exisVal, out int exisValI);
            if (!luck || luck && exisValI != value)
                _iniPlugy.SetValue(section, param, value);
        }




    }
}
