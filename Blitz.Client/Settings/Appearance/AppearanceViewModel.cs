using System.ComponentModel;
using System.Linq;
using System.Windows.Media;

using Common.Logging;

using Naru.WPF.MVVM;
using Naru.WPF.ModernUI.Presentation;
using Naru.WPF.Scheduler;

namespace Blitz.Client.Settings.Appearance
{
    /// <summary>
    /// A simple view model for configuring theme, font and accent colors.
    /// </summary>
    public class AppearanceViewModel : Workspace
    {
        private const string FONT_SMALL = "small";
        private const string FONT_LARGE = "large";

        // 9 accent colors from metro design principles
        /*private Color[] accentColors = new Color[]{
            Color.FromRgb(0x33, 0x99, 0xff),   // blue
            Color.FromRgb(0x00, 0xab, 0xa9),   // teal
            Color.FromRgb(0x33, 0x99, 0x33),   // green
            Color.FromRgb(0x8c, 0xbf, 0x26),   // lime
            Color.FromRgb(0xf0, 0x96, 0x09),   // orange
            Color.FromRgb(0xff, 0x45, 0x00),   // orange red
            Color.FromRgb(0xe5, 0x14, 0x00),   // red
            Color.FromRgb(0xff, 0x00, 0x97),   // magenta
            Color.FromRgb(0xa2, 0x00, 0xff),   // purple            
        };*/

        // 20 accent colors from Windows Phone 8
        private readonly Color[] _accentColors =
        {
            Color.FromRgb(0xa4, 0xc4, 0x00),   // lime
            Color.FromRgb(0x60, 0xa9, 0x17),   // green
            Color.FromRgb(0x00, 0x8a, 0x00),   // emerald
            Color.FromRgb(0x00, 0xab, 0xa9),   // teal
            Color.FromRgb(0x1b, 0xa1, 0xe2),   // cyan
            Color.FromRgb(0x00, 0x50, 0xef),   // cobalt
            Color.FromRgb(0x6a, 0x00, 0xff),   // indigo
            Color.FromRgb(0xaa, 0x00, 0xff),   // violet
            Color.FromRgb(0xf4, 0x72, 0xd0),   // pink
            Color.FromRgb(0xd8, 0x00, 0x73),   // magenta
            Color.FromRgb(0xa2, 0x00, 0x25),   // crimson
            Color.FromRgb(0xe5, 0x14, 0x00),   // red
            Color.FromRgb(0xfa, 0x68, 0x00),   // orange
            Color.FromRgb(0xf0, 0xa3, 0x0a),   // amber
            Color.FromRgb(0xe3, 0xc8, 0x00),   // yellow
            Color.FromRgb(0x82, 0x5a, 0x2c),   // brown
            Color.FromRgb(0x6d, 0x87, 0x64),   // olive
            Color.FromRgb(0x64, 0x76, 0x87),   // steel
            Color.FromRgb(0x76, 0x60, 0x8a),   // mauve
            Color.FromRgb(0x87, 0x79, 0x4e) // taupe
        };

        public BindableCollection<string> FontSizes { get; private set; }

        #region SelectedFontSize

        private string _selectedFontSize;

        public string SelectedFontSize
        {
            get { return _selectedFontSize; }
            set
            {
                if (value == _selectedFontSize) return;
                _selectedFontSize = value;
                RaisePropertyChanged(() => SelectedFontSize);

                AppearanceManager.Current.FontSize = value == FONT_LARGE ? FontSize.Large : FontSize.Small;
            }
        }

        #endregion

        public BindableCollection<Color> AccentColors { get; private set; }

        #region SelectedAccentColor

        private Color _selectedAccentColor;

        public Color SelectedAccentColor
        {
            get { return _selectedAccentColor; }
            set
            {
                if (value.Equals(_selectedAccentColor)) return;
                _selectedAccentColor = value;
                RaisePropertyChanged(() => SelectedAccentColor);

                AppearanceManager.Current.AccentColor = value;
            }
        }

        #endregion

        public BindableCollection<ThemeItemViewModel> Themes { get; private set; }

        #region SelectedTheme

        private ThemeItemViewModel _selectedTheme;

        public ThemeItemViewModel SelectedTheme
        {
            get { return _selectedTheme; }
            set
            {
                if (Equals(value, _selectedTheme)) return;
                _selectedTheme = value;
                RaisePropertyChanged(() => SelectedTheme);

                // and update the actual theme
                AppearanceManager.Current.ThemeSource = value.Source;
            }
        }

        #endregion

        public AppearanceViewModel(ILog log, IScheduler scheduler, IViewService viewService, BindableCollectionFactory bindableCollectionFactory)
            : base(log, scheduler, viewService)
        {
            this.SetupHeader("Appearance");

            FontSizes = bindableCollectionFactory.Get<string>();
            FontSizes.AddRange(new[] {FONT_SMALL, FONT_LARGE});

            SelectedFontSize = AppearanceManager.Current.FontSize == FontSize.Large ? FONT_LARGE : FONT_SMALL;

            AccentColors = bindableCollectionFactory.Get<Color>();
            foreach (var accentColor in _accentColors)
            {
                AccentColors.Add(accentColor);
            }

            // add the default themes
            Themes = bindableCollectionFactory.Get<ThemeItemViewModel>();
            Themes.AddRange(new[]
            {
                new ThemeItemViewModel {Name = "Dark", Source = AppearanceManager.DarkThemeSource},
                new ThemeItemViewModel {Name = "Light", Source = AppearanceManager.LightThemeSource}
            });

            SyncThemeAndColor();

            AppearanceManager.Current.PropertyChanged += OnAppearanceManagerPropertyChanged;
        }

        private void SyncThemeAndColor()
        {
            // synchronizes the selected viewmodel theme with the actual theme used by the appearance manager.
            SelectedTheme = Themes.FirstOrDefault(l => l.Source.Equals(AppearanceManager.Current.ThemeSource));

            // and make sure accent color is up-to-date
            SelectedAccentColor = AppearanceManager.Current.AccentColor;
        }

        private void OnAppearanceManagerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ThemeSource" || e.PropertyName == "AccentColor")
            {
                SyncThemeAndColor();
            }
        }
    }
}