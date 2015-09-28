using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace Doc.Search.App.Extensions
{
    /// <summary>
    /// Convertisseur d'une valeur numérique en taille de document
    /// </summary>
    public class DocSizeConverter : IValueConverter
    {
        /// <summary>
        /// Modifie la source avant de la passer à l'UI
        /// </summary>
        /// <param name="value">La valeur source.</param>
        /// <param name="targetType">Le <see cref="T:System.Type"/> de donnée attendu par l'UI.</param>
        /// <param name="parameter">Paramètre utilisé dans la conversion.</param>
        /// <param name="langage">Le langage utilisé</param>
        /// <returns>La valeur passée à l'UI</returns>
        /// 
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            string sizeValue = string.Empty;
            float size = 0;
            int order = 0;

            if (value != null)
                sizeValue = value.ToString();

            if (!string.IsNullOrEmpty(sizeValue))
            {
                size = float.Parse(sizeValue);
                while (size >= 1024 && order + 1 < sizes.Length)
                {
                    order++;
                    size = size / 1024;
                }
                return String.Format("{0:0.#} {1}", size, sizes[order]);
            }
            else
            {
                return (sizeValue);
            }
        }

        /// <summary>
        /// Modifie la valeur de l'UI avant de la passer à la source.  Appelée uniquement dans le cas d'un <see cref="F:System.Windows.Data.BindingMode.TwoWay"/> binding.
        /// </summary>
        /// <param name="value">La valeur de l'UI.</param>
        /// <param name="targetType">Le <see cref="T:System.Type"/> de donnée attendu par la source.</param>
        /// <param name="parameter">Paramètre utilisé dans la conversion.</param>
        /// <param name="langage">Le langage utilisé</param>
        /// <returns>La valeur passée à la source</returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (null);
        }
    }
}
