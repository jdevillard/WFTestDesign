using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Activities.Presentation.Model;

namespace WFTestDesign.Activities.Designers.Helper.Converter
{
    [ValueConversion(typeof(string),typeof(string))]
    class FileNameReduceConverter : IValueConverter
    {

        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            int length = 20;

            if (value == null)
                return "No value";

            string PathToConvert = (string)value;

            string PathRoot = System.IO.Path.GetPathRoot(PathToConvert).ToString();
            string FileName = System.IO.Path.GetFileName(PathToConvert);
            string Path = System.IO.Path.GetDirectoryName(PathToConvert);

            string PathConvert;

            if (PathToConvert.Length > length)
            {
                if (PathRoot.Length + FileName.Length + "..".Length< length)
                    PathConvert = PathRoot + @".." + Path.Remove(0, Path.Length - (length - (PathRoot.Length + FileName.Length))) + @"\" + FileName;
                else
                    PathConvert = PathRoot + @".." + @"\" + FileName;
            }
            else
                PathConvert = PathToConvert;

            return PathConvert;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return  null;
        }
    }
}
