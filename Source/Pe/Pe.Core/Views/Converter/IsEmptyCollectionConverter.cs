using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace ContentTypeTextNet.Pe.Core.Views.Converter
{
    public class IsEmptyCollectionConverter: IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(value is ICollectionView view) {
                return view.IsEmpty;
            }

            var enumerable = value as IEnumerable;
            if(enumerable != null) {
                var enumerator = enumerable.GetEnumerator();
                while(enumerator.MoveNext()) {
                    return false;
                }
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
