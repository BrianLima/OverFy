using System.Collections.Specialized;

namespace OverFy
{
    public class AppSettings
    {
        public AppSettings()
        {
            this._autostart = Properties.Settings.Default.AutoStart;
            this._timeFormat = Properties.Settings.Default.TimeFormat;
            this._showSystemTime = Properties.Settings.Default.ShowSystemTime;
            this._propertiesOrder = Properties.Settings.Default.PropertiesOrder;
        }

        public void Save()
        {
            Properties.Settings.Default.Save();
        }

        private StringCollection _propertiesOrder;

        public StringCollection PropertiesOrder
        {
            get { return _propertiesOrder; }
            set { _propertiesOrder = value; }
        }

        private bool _showSystemTime;

        public bool ShowSystemTime
        {
            get { return _showSystemTime; }
            set { _showSystemTime = value; }
        }

        private string _timeFormat;

        public string TimeFormat
        {
            get { return _timeFormat; }
            set { _timeFormat = value; }
        }

        private bool _autostart;

        public bool AutoStart
        {
            get { return _autostart; }
            set { _autostart = value; }
        }
    }
}
