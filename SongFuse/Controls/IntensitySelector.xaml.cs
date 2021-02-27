using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SongFuse.Controls
{
    /// <summary>
    /// Interaction logic for IntensitySelector.xaml
    /// </summary>
    public partial class IntensitySelector : UserControl
    {
        private readonly Image[] Images;
        private readonly ImageSource NotSelected;
        private readonly ImageSource Selected;

        public IntensitySelector()
        {
            InitializeComponent();

            NotSelected = new BitmapImage(new Uri("../Assets/bolt_black.png", UriKind.Relative));
            Selected = new BitmapImage(new Uri("../Assets/bolt_white.png", UriKind.Relative));

            Images = new[]
            {
                Image_0,
                Image_1,
                Image_2,
                Image_3,
                Image_4,
                Image_5
            };
            
            foreach(var img in Images)
            {
                img.MouseEnter += (sender, e) =>
                {
                    var idx = Array.IndexOf(Images, sender);
                    UpdateImageSources(idx);
                };

                img.MouseLeave += (sender, e) =>
                {
                    UpdateImageSources(Level);
                };

                img.MouseDown += (sender, e) =>
                {
                    Level = Array.IndexOf(Images, sender);
                };
                
                img.Source = NotSelected;
            }
        }
        
        private void UpdateImageSources(int idx)
        {
            int current = 0;

            foreach (var img in Images)
            {
                img.Source = (current > idx | current <= 0) ? NotSelected : Selected;
                current++;
            }
        }

        public int Level { get; set; }
    }
}
