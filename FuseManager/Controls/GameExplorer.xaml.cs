using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace FuseManager.Controls
{
    public class GameExplorer : UserControl
    {
        public GameExplorer()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
