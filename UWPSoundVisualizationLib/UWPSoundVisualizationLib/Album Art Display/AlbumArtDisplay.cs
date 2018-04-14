using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace UWPSoundVisualizationLib.Album_Art_Display
{
    public sealed class AlbumArtDisplay : Control
    {
        #region Fields
        private readonly BitmapImage noArtImage = new BitmapImage(new Uri("ms-appx:///NoArtwork.png"));
        private readonly BitmapImage overlayImage = new BitmapImage(new Uri("ms-appx:///Overlay.png"));
        private readonly BitmapImage underlayImage = new BitmapImage(new Uri("ms-appx:///Underlay.png"));
        private Image albumArtImage;
        #endregion

        #region Dependency Properties
        #region AlbumArtImage
        /// <summary>
        /// Identifies the <see cref="AlbumArtImage" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty AlbumArtImageProperty = DependencyProperty.Register("AlbumArtImage", typeof(ImageSource), typeof(AlbumArtDisplay), new PropertyMetadata(null, OnAlbumArtImageChanged));

        private static void OnAlbumArtImageChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            AlbumArtDisplay AlbumArtDisplay = o as AlbumArtDisplay;
            if (AlbumArtDisplay != null)
                AlbumArtDisplay.onAlbumArtImageChanged(e.NewValue as BitmapImage);
        }
        
        private void onAlbumArtImageChanged(BitmapImage newValue)
        {
            albumArtImage.Source = newValue;
        }
        
        public BitmapImage AlbumArtImage
        {
            get
            {
                return (BitmapImage)GetValue(AlbumArtImageProperty);
            }
            set
            {
                SetValue(AlbumArtImageProperty, value);
            }
        }
        #endregion
        #endregion

        #region Template Overrides
        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code
        /// or internal processes call System.Windows.FrameworkElement.ApplyTemplate().
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            albumArtImage = GetTemplateChild("PART_AlbumArt") as Image;
        }
        #endregion

        #region Constructors
        public AlbumArtDisplay()
        {
            this.DefaultStyleKey = typeof(AlbumArtDisplay);
        }
        #endregion
    }
}
