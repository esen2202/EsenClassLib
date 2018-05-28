using Microsoft.Win32;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace EsenClassLib
{
    /// <summary>
    /// Microsoft Office Ürünlerinin Versiyonu ve Numarasını Verir. 
    /// Sorgulanan Office Ürünü Kurulu Değil ise 
    /// Version Numarası = 0 , İsmi = string.Empty
    /// </summary>
    public static class GetOfficeVersion
    {
        //Component'lerin Registry Path'i
        private const string RegKey = @"Software\Microsoft\Windows\CurrentVersion\App Paths";
      
        /// <summary>
        /// Istenen Office Componentinin Version Numarasını Verir.
        /// Eger Component Bulunamamış ise : Return = 0
        /// </summary>
        /// <param name="_component"></param>
        /// <returns>Version Number</returns>
        public static int GetVersionNumber(OfficeComponent _component)
        {
    
            return GetMajorVersion(GetComponentPath(_component));
        }

        /// <summary>
        /// Istenen Office Componentinin Version İsmini Verir.
        /// Eger Component Bulunamamış ise : Return = string.Empty
        /// </summary>
        /// <param name="_component"></param>
        /// <returns>Version Number</returns>
        public static string GetVersionName(OfficeComponent _component, VersionNumberConvertName _officeVersionStock)
        {
            string versionName = string.Empty;

            if (_officeVersionStock.GetName(GetVersionNumber(_component)) != string.Empty)
                versionName = _officeVersionStock.GetName(GetVersionNumber(_component));

            return versionName;
        }

        /// <summary>
        /// Registry'den Component'in Path'ini Al.
        /// Eğer Component'i Bulamaz ise : Return = string.Empty
        /// </summary>
        static string GetComponentPath(OfficeComponent _component)
        {
            string toReturn = string.Empty;
            string _key = string.Empty;

            switch (_component)
            {
                case OfficeComponent.Word:
                    _key = "winword.exe";
                    break;
                case OfficeComponent.Excel:
                    _key = "excel.exe";
                    break;
                case OfficeComponent.Access:
                    _key = "msaccess.exe";
                    break;
                case OfficeComponent.PowerPoint:
                    _key = "powerpnt.exe";
                    break;
                case OfficeComponent.Outlook:
                    _key = "outlook.exe";
                    break;
            }

            //CURRENT_USER içerisine bak:
            RegistryKey _mainKey = Registry.CurrentUser;
            try
            {
                _mainKey = _mainKey.OpenSubKey(RegKey + "\\" + _key, false);
                if (_mainKey != null)
                {
                    toReturn = _mainKey.GetValue(string.Empty).ToString();
                }
            }
            catch
            {
            }

            //Bulamaz ise, LOCAL_MACHINE içerisine bak:
            _mainKey = Registry.LocalMachine;
            if (string.IsNullOrEmpty(toReturn))
            {
                try
                {
                    _mainKey = _mainKey.OpenSubKey(RegKey + "\\" + _key, false);
                    if (_mainKey != null)
                    {
                        toReturn = _mainKey.GetValue(string.Empty).ToString();
                    }
                }
                catch
                {
                }
            }

            //Elde tuttuğumuz registerykey 'i kapat:
            if (_mainKey != null)
                _mainKey.Close();

            return toReturn;
        }

        /// <summary>
        /// Path'in Major Versiyonunu Al. 
        /// Eğer Bulamaz ise veya Başka Bir Durum Meydana Gelmiş ise : Return = 0
        /// </summary>
        static int GetMajorVersion(string _path)
        {
            int toReturn = 0;
            if (File.Exists(_path))
            {
                try
                {
                    FileVersionInfo _fileVersion = FileVersionInfo.GetVersionInfo(_path);
                    toReturn = _fileVersion.FileMajorPart;
                }
                catch
                {
                }
            }

            return toReturn;
        }

    }

    /// <summary>
    /// Office Version Numaralarına Karşılık İsimlerini Verir
    /// </summary>
    public class VersionNumberConvertName
    {
        // Office Versiyon Nolarının İsim Karşılığı Bu Havuzda Tutuluyor.
        public Dictionary<int, string> OfficeVersionNamePool = new Dictionary<int, string>(); 

        /// <summary>
        /// Constructure Method
        /// </summary>
        public VersionNumberConvertName()
        {
            OfficeVersionNamePool.Add(9, "Microsoft Office 2000");
            OfficeVersionNamePool.Add(10, "Microsoft Office 2002");
            OfficeVersionNamePool.Add(11,"Microsoft Office 2003");
            OfficeVersionNamePool.Add(12, "Microsoft Office 2007");
            OfficeVersionNamePool.Add(14, "Microsoft Office 2010");
            OfficeVersionNamePool.Add(16, "Microsoft Office 2016"); 
        }

        /// <summary>
        /// Yeni Office Versiyonu Ekler
        /// </summary>
        /// <param name="_number">Version No</param>
        /// <param name="_name">Version Name</param>
        public void  WriteNewVersion(int _number, string _name)
        {
            OfficeVersionNamePool.Add(_number, _name);
        }

        /// <summary>
        /// Istenen Version Numarasının İsmini Verir
        /// </summary>
        /// <param name="_number">Version No</param>
        /// <returns></returns>
        public string GetName(int _number)
        {
            string name = string.Empty;
            if (OfficeVersionNamePool.ContainsKey(_number)) {
                name = OfficeVersionNamePool[_number];

            }
            return name;
        }

    }

    /// <summary>
    /// Office Components Enum
    /// </summary>
    public enum OfficeComponent
    {
        Word,
        Excel,
        Access,
        PowerPoint,
        Outlook
    }

}
