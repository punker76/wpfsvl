// Copyright (C) 2011 - 2012, Jacob Johnston 
//
// Permission is hereby granted, free of charge, to any person obtaining a 
// copy of this software and associated documentation files (the "Software"), 
// to deal in the Software without restriction, including without limitation 
// the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions: 
//
// The above copyright notice and this permission notice shall be included in 
// all copies or substantial portions of the Software. 
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL 
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING 
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
// DEALINGS IN THE SOFTWARE. 

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace UWPSoundVisualizationLib.Digital_Clock
{
    /// <summary>
    /// A digital LED clock display control with 
    /// hours, minutes, seconds, and hundredths of a second.
    /// </summary>
    //[DisplayName("Digital Clock")]
    //[Description("Displays timespans as a digital LED clock.")]
    //[ToolboxItem(true)]   
    //[TemplatePart(Name = "PART_ClockGrid", Type = typeof(Grid))]    
    public class DigitalClock : Control
    {
        #region Fields
        private readonly Rectangle hourRectangle1 = new Rectangle();
        private readonly Rectangle hourRectangle2 = new Rectangle();
        private readonly Rectangle separatorRectangle1 = new Rectangle();
        private readonly Rectangle minuteRectangle1 = new Rectangle();
        private readonly Rectangle minuteRectangle2 = new Rectangle();
        private readonly Rectangle separatorRectangle2 = new Rectangle();
        private readonly Rectangle secondRectangle1 = new Rectangle();
        private readonly Rectangle secondRectangle2 = new Rectangle();
        private readonly Rectangle decimalRectangle = new Rectangle();
        private readonly Rectangle subSecondRectangle1 = new Rectangle();
        private readonly Rectangle subSecondRectangle2 = new Rectangle();
        

        private TextBlock clockTextblock;
        #endregion

        #region Dependency Properties
        #region Time
        /// <summary>
        /// Identifies the <see cref="Time" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register("Time", typeof(TimeSpan), typeof(DigitalClock), new PropertyMetadata(TimeSpan.Zero, OnTimeChanged));

        private static void OnTimeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            DigitalClock bigClock = o as DigitalClock;
            if (bigClock != null)
                bigClock.OnTimeChanged((TimeSpan)e.OldValue, (TimeSpan)e.NewValue);
        }

        /// <summary>
        /// Called after the <see cref="Time"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="Time"/></param>
        /// <param name="newValue">The new value of <see cref="Time"/></param>
        private void OnTimeChanged(TimeSpan oldValue, TimeSpan newValue)
        {
            SplitDigits();
        }

        /// <summary>
        /// Gets or sets the time to be displayed in the Digital Clock.
        /// </summary>
        public TimeSpan Time
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (TimeSpan)GetValue(TimeProperty);
            }
            set
            {
                SetValue(TimeProperty, value);
            }
        }
        #endregion

        #region ShowHours
        /// <summary>
        /// Identifies the <see cref="ShowHours" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty ShowHoursProperty = DependencyProperty.Register("ShowHours", 
            typeof(bool), typeof(DigitalClock), new PropertyMetadata(true, OnShowHoursChanged));
        
        private static void OnShowHoursChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            DigitalClock digitalClock = o as DigitalClock;
            if (digitalClock != null)
                digitalClock.onShowHoursChanged((bool)e.NewValue);
        }

        /// <summary>
        /// Called after the <see cref="ShowHours"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="ShowHours"/></param>
        /// <param name="newValue">The new value of <see cref="ShowHours"/></param>
        private void onShowHoursChanged(bool newValue)
        {
            FormatClockLayout();
            SplitDigits();
        }

        /// <summary>
        /// Gets or sets whether the digital clock will show the hours portion
        /// in the digital clock display. This is useful if the times displayed
        /// are always less than an hour.
        /// </summary>
        public bool ShowHours
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (bool)GetValue(ShowHoursProperty);
            }
            set
            {
                SetValue(ShowHoursProperty, value);
            }
        }
        #endregion

        #region ShowSubSeconds
        /// <summary>
        /// Identifies the <see cref="ShowSubSeconds" /> dependency property. 
        /// </summary>
        public static readonly DependencyProperty ShowSubSecondsProperty = DependencyProperty.Register("ShowSubSeconds", 
            typeof(bool), 
            typeof(DigitalClock), 
            new PropertyMetadata(false, OnShowSubSecondsChanged));

        private static void OnShowSubSecondsChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            DigitalClock digitalClock = o as DigitalClock;
            if (digitalClock != null)
                digitalClock.onShowSubSecondsChanged((bool)e.NewValue);
        }

        /// <summary>
        /// Called after the <see cref="ShowSubSeconds"/> value has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of <see cref="ShowSubSeconds"/></param>
        /// <param name="newValue">The new value of <see cref="ShowSubSeconds"/></param>
        private void onShowSubSecondsChanged(bool newValue)
        {
            FormatClockLayout();
            SplitDigits();
        }

        /// <summary>
        /// Gets or sets whether fractions of a second are displayed in the digital
        /// clock display.
        /// </summary>
        public bool ShowSubSeconds
        {
            // IMPORTANT: To maintain parity between setting a property in XAML and procedural code, do not touch the getter and setter inside this dependency property!
            get
            {
                return (bool)GetValue(ShowSubSecondsProperty);
            }
            set
            {
                SetValue(ShowSubSecondsProperty, value);
            }
        }

        #endregion
        #endregion

        #region Constructors
        public DigitalClock()
        {
            this.DefaultStyleKey = typeof(DigitalClock);
        }
        #endregion

        #region Template Overrides
        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code
        /// or internal processes call System.Windows.FrameworkElement.ApplyTemplate().
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            clockTextblock = GetTemplateChild("clock") as TextBlock;
        }
        #endregion

        #region Private Utility Methods
        private void FormatClockLayout()
        {
            //if (clockGrid == null)
            //    return;

            //ColumnDefinitionCollection clockColumns = clockGrid.ColumnDefinitions;
            //clockColumns.Clear();
            //clockGrid.Children.Clear();

            //Thickness digitMargin = new Thickness(5.0d);
            //int columnIndex = 0;
            //double gridLength = 0;
            //double gridHeight = 115;

            //if (ShowHours)
            //{
            //    // First hours digit
            //    ColumnDefinition hours1Column = new ColumnDefinition();
            //    hours1Column.Width = new GridLength(50, GridUnitType.Star);
            //    clockColumns.Add(hours1Column);
            //    hourRectangle1.Margin = digitMargin;
            //    Grid.SetColumn(hourRectangle1, columnIndex);
            //    clockGrid.Children.Add(hourRectangle1);
            //    gridLength += 60;
            //    columnIndex++;

            //    // Second hours digit
            //    ColumnDefinition hours2Column = new ColumnDefinition();
            //    hours2Column.Width = new GridLength(50, GridUnitType.Star);
            //    clockColumns.Add(hours2Column);
            //    hourRectangle2.Margin = digitMargin;
            //    Grid.SetColumn(hourRectangle2, columnIndex);
            //    clockGrid.Children.Add(hourRectangle2);
            //    gridLength += 60;
            //    columnIndex++;

            //    // Hours separator
            //    ColumnDefinition seaparator1Column = new ColumnDefinition();
            //    seaparator1Column.Width = new GridLength(20, GridUnitType.Star);
            //    clockColumns.Add(seaparator1Column);
            //    separatorRectangle1.Margin = digitMargin;
            //    Grid.SetColumn(separatorRectangle1, columnIndex);
            //    clockGrid.Children.Add(separatorRectangle1);
            //    gridLength += 30;
            //    columnIndex++;
            //}

            //// First minutes digit            
            //ColumnDefinition minutes1Column = new ColumnDefinition();
            //minutes1Column.Width = new GridLength(50, GridUnitType.Star);
            //clockColumns.Add(minutes1Column);
            //minuteRectangle1.Margin = digitMargin;
            //Grid.SetColumn(minuteRectangle1, columnIndex);
            //clockGrid.Children.Add(minuteRectangle1);
            //gridLength += 60;
            //columnIndex++;

            //// Second minutes digit
            //ColumnDefinition minutes2Column = new ColumnDefinition();
            //minutes2Column.Width = new GridLength(50, GridUnitType.Star);
            //clockColumns.Add(minutes2Column);
            //minuteRectangle2.Margin = digitMargin;
            //Grid.SetColumn(minuteRectangle2, columnIndex);
            //clockGrid.Children.Add(minuteRectangle2);
            //gridLength += 60;
            //columnIndex++;

            //// Minutes separator
            //ColumnDefinition seaparator2Column = new ColumnDefinition();
            //seaparator2Column.Width = new GridLength(20, GridUnitType.Star);
            //clockColumns.Add(seaparator2Column);
            //separatorRectangle2.Margin = digitMargin;
            //Grid.SetColumn(separatorRectangle2, columnIndex);
            //clockGrid.Children.Add(separatorRectangle2);
            //gridLength += 30;
            //columnIndex++;

            //// First seconds digit
            //ColumnDefinition seconds1Column = new ColumnDefinition();
            //seconds1Column.Width = new GridLength(50, GridUnitType.Star);
            //clockColumns.Add(seconds1Column);
            //secondRectangle1.Margin = digitMargin;
            //Grid.SetColumn(secondRectangle1, columnIndex);
            //clockGrid.Children.Add(secondRectangle1);
            //gridLength += 60;
            //columnIndex++;

            //// Second seconds digit
            //ColumnDefinition seconds2Column = new ColumnDefinition();
            //seconds2Column.Width = new GridLength(50, GridUnitType.Star);
            //clockColumns.Add(seconds2Column);
            //secondRectangle2.Margin = digitMargin;
            //Grid.SetColumn(secondRectangle2, columnIndex);
            //clockGrid.Children.Add(secondRectangle2);
            //gridLength += 60;
            //columnIndex++;

            //if (ShowSubSeconds)
            //{
            //    // Subseconds decimal point
            //    ColumnDefinition subSecondsDecimalColumn = new ColumnDefinition();
            //    subSecondsDecimalColumn.Width = new GridLength(20, GridUnitType.Star);
            //    clockColumns.Add(subSecondsDecimalColumn);
            //    decimalRectangle.Margin = digitMargin;
            //    Grid.SetColumn(decimalRectangle, columnIndex);
            //    clockGrid.Children.Add(decimalRectangle);
            //    gridLength += 30;
            //    columnIndex++;

            //    // First subseconds digit
            //    ColumnDefinition subSeconds1Column = new ColumnDefinition();
            //    subSeconds1Column.Width = new GridLength(50, GridUnitType.Star);
            //    clockColumns.Add(subSeconds1Column);
            //    subSecondRectangle1.Margin = digitMargin;
            //    Grid.SetColumn(subSecondRectangle1, columnIndex);
            //    clockGrid.Children.Add(subSecondRectangle1);
            //    gridLength += 60;
            //    columnIndex++;

            //    // Second subseconds digit
            //    ColumnDefinition subSeconds2Column = new ColumnDefinition();
            //    subSeconds2Column.Width = new GridLength(50, GridUnitType.Star);
            //    clockColumns.Add(subSeconds2Column);
            //    subSecondRectangle2.Margin = digitMargin;
            //    Grid.SetColumn(subSecondRectangle2, columnIndex);
            //    clockGrid.Children.Add(subSecondRectangle2);
            //    gridLength += 60;
            //    columnIndex++;
            //}

            //clockGrid.Width = gridLength;
            //clockGrid.Height = gridHeight;
        }

        private void SplitDigits()
        {
        //    if (clockGrid == null)
        //        return;

        //    const string timeFormat = "00";

        //    string hoursString = Time.Hours.ToString(timeFormat);
        //    hourRectangle1.Fill = GetDigitElement(hoursString[0]);
        //    hourRectangle2.Fill = GetDigitElement(hoursString[1]);

        //    string minutesString = Time.Minutes.ToString(timeFormat);
        //    minuteRectangle1.Fill = GetDigitElement(minutesString[0]);
        //    minuteRectangle2.Fill = GetDigitElement(minutesString[1]);

        //    string secondsString = Time.Seconds.ToString(timeFormat);
        //    secondRectangle1.Fill = GetDigitElement(secondsString[0]);
        //    secondRectangle2.Fill = GetDigitElement(secondsString[1]);

        //    string subSecondsString = (Time.Milliseconds / 1000.0d).ToString("0.00");
        //    subSecondRectangle1.Fill = GetDigitElement(subSecondsString[2]);
        //    subSecondRectangle2.Fill = GetDigitElement(subSecondsString[3]);
        //}

        //private VisualBrush GetDigitElement(char digitChar)
        //{
        //    switch (digitChar)
        //    {
        //        case '1':
        //            return oneBrush;
        //        case '2':
        //            return twoBrush;
        //        case '3':
        //            return threeBrush;
        //        case '4':
        //            return fourBrush;
        //        case '5':
        //            return fiveBrush;
        //        case '6':
        //            return sixBrush;
        //        case '7':
        //            return sevenBrush;
        //        case '8':
        //            return eightBrush;
        //        case '9':
        //            return nineBrush;
        //        case '0':
        //            return zeroBrush;
        //        case '.':
        //            return decimalBrush;
        //        case ':':
        //            return colonBrush;
        //    }
        //    return null;
        }
        #endregion
    }
}
