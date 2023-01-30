using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Videos_Folder_Length
{
    public class CustomTextBox : System.Windows.Controls.TextBox
    {
        // Place Holder Text Paramter
        private string placeholder = "";
        public string PlaceHolder
        {
            set { placeholder = value; Text = placeholder; }
            get { return placeholder; }
        }

        // Border Radius Parameter
        private int br = 0;
        public int Br
        {
            set { br = value; ConfigureCornerRadius(br); }
            get { return br; }
        }

        public void ConfigureCornerRadius(int r)
        {
            // Create New Style For type Border
            Style style = new Style(typeof(Border));
            // Configure Border style
            Setter setter = new Setter()
            {
                Property = Border.CornerRadiusProperty,
                Value = new CornerRadius(r)
            };
            style.Setters.Add(setter);
            // Add styled border to textbox resources
            Resources.Add(typeof(Border), style);
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (Text == PlaceHolder)
                Text = "";
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(Text))
                Text = PlaceHolder;
        }
    }

    public class CustomButton : System.Windows.Controls.Button
    {
        // Border Radius Parameter
        private int br = 0;
        public int Br
        {
            set { br = value; ConfigureCornerRadius(br); }
            get { return br; }
        }

        public void ConfigureCornerRadius(int r)
        {
            // Create New Style For type Border
            Style style = new Style(typeof(Border));
            // Configure Border style
            Setter setter = new Setter()
            {
                Property = Border.CornerRadiusProperty,
                Value = new CornerRadius(r)
            };
            style.Setters.Add(setter);
            // Add styled border to textbox resources
            Resources.Add(typeof(Border), style);

            ConfigureHoverColor();
        }

        public void ConfigureHoverColor()
        {
            // Create New Style For type Button
            Style style = new Style(typeof(Button));
            // Trigger
            Trigger trigger = new Trigger()
            {
                Property = IsMouseOverProperty,
                Value = true
            };
            // Setter of trigger
            Setter setter = new Setter()
            {
                Property = BackgroundProperty,
                Value = new SolidColorBrush(Color.FromRgb(0, 0, 0))
            };
            // Add Setter to trigger
            trigger.Setters.Add(setter);
            // Add Trigger to Style
            style.Triggers.Add(trigger);
            // Add style
            this.Resources.Add(typeof(Button), style);
        }
        
    }



}
