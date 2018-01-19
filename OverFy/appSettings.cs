using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace OverFy
{
    public class AppSettings : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public AppSettings()
        {
            this._autostart = Properties.Settings.Default.AutoStart;
            this._time_format = Properties.Settings.Default.TimeFormat;
            this._show_system_time = Properties.Settings.Default.ShowSystemTime;
            this._properties_order = Properties.Settings.Default.PropertiesOrder;
            this._use_new_line = Properties.Settings.Default.UseNewLine;

            if (this._properties_order == null)
            {
                _properties_order = new StringCollection();
            }
        }

        public void Save()
        {
            Properties.Settings.Default.Save();
        }

        private void OnPropertyChanged(string propertyName)
        {
            switch (propertyName)
            {
                case "PropertiesOrder":
                    Properties.Settings.Default.PropertiesOrder = _properties_order;
                    break;
                case "ShowSystemTime":
                     break;
                case "TimeFormat":
                    Properties.Settings.Default.TimeFormat = _time_format;
                    break;
                case "AutoStart":
                    Properties.Settings.Default.AutoStart = _autostart;
                    break;
                case "UseNewLine":
                    Properties.Settings.Default.UseNewLine = _use_new_line;
                    break;
                default:
                    break;
            }
        }

        private StringCollection _properties_order;

        public StringCollection PropertiesOrder
        {
            get { return _properties_order; }
            set { _properties_order = value; OnPropertyChanged("PropertiesOrder"); }
        }

        private bool _show_system_time;

        public bool ShowSystemTime
        {
            get { return _show_system_time; }
            set { _show_system_time = value; OnPropertyChanged("ShowSystemTime"); }
        }

        private string _time_format;

        public string TimeFormat
        {
            get { return _time_format; }
            set { _time_format = value; OnPropertyChanged("TimeFormat"); }
        }

        private bool _autostart;

        public bool AutoStart
        {
            get { return _autostart; }
            set { _autostart = value; OnPropertyChanged("AutoStart"); }
        }

        private bool _use_new_line;

        public bool UseNewLine
        {
            get { return _use_new_line; }
            set { _use_new_line = value; OnPropertyChanged("UseNewLine"); }
        }

    }
}
