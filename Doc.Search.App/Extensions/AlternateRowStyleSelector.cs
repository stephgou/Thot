using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Doc.Search.App.Extensions
{
    class AlternateRowStyleSelector : StyleSelector
    {
        static int _index = 0;
        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            SolidColorBrush highlight;
            if (_index % 2 == 0)
                highlight = new SolidColorBrush(Colors.White);
            else
                highlight = new SolidColorBrush(Colors.CornflowerBlue);

            _index++;
            Style style = new Style(container.GetType());
            style.Setters.Add(new Setter(Control.BackgroundProperty, highlight));
            return style;
        }
    }
}
