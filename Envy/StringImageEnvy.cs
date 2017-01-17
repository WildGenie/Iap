using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Iap.Envy
{
   internal static class StringImageEnvy
    {
        internal static BitmapImage ToBitmapImage(this string path)
        {
            var assembly = Assembly.GetExecutingAssembly();

            if (path[0] == '/')
                path = path.Substring(1);

            return
                new BitmapImage(
                    new Uri(@"pack://application:,,,/"
                        + assembly.GetName().Name + ";component/"
                        + path,
                        UriKind.Absolute));
        }

        internal static IEnumerable<string> EnumerateImageFiles(this string path)
        {
            return new[]
            {
                "*.jpg",
                "*.png"
            }
            .AsParallel()
            .SelectMany(type =>
                Directory
                    .EnumerateFiles(
                        path,
                        type,
                        SearchOption.TopDirectoryOnly)
                    .Where(x => new FileInfo(x).Length > 0));
            }
    
      }
}
