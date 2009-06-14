using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Squamster
{
    public partial class BrightnessContrastControl : Form
    {
        private int percentage = 0;
        public int Percentage
        {
            get
            {
                return percentage;
            }
        }
        public BrightnessContrastControl()
        {
            InitializeComponent();
        }

        public float getBrightness()
        {
            return (float)brightnessSlider.Value / 100;
        }

        public float getContrast()
        {
            return (float)contrastSlider.Value / 100;
        }

        private void value_TextChanged(object sender, EventArgs e)
        {
            string text = ((TextBox)sender).Text.Trim();
            float modifier = .5f;
            if (text.Contains('%'))
            {
                text = text.Replace("%", " ").Trim();
                double numValue = 0;
                bool isDouble = double.TryParse(text, out numValue);
                if (isDouble)
                {
                    if (numValue < 0)
                    {
                        modifier *= -1;
                    }
                    percentage = (int)(numValue + modifier);
                }
            }
            else if (text.Contains('.'))
            {
                double numValue = 0;
                bool isDouble = double.TryParse(text, out numValue);
                if (isDouble)
                {
                    if (numValue < 0)
                    {
                        modifier *= -1;
                    }
                    percentage = (int)(numValue * 100 + modifier);
                }
            }
            else
            {
                double numValue = 0;
                bool isDouble = double.TryParse(text, out numValue);
                if (isDouble)
                {
                    if (numValue < 0)
                    {
                        modifier *= -1;
                    }
                    percentage = (int)(numValue + modifier);
                }
            }
            if (percentage > 100)
            {
                percentage = 100;
            }
            else if (percentage < -100)
            {
                percentage = -100;
            }
            ((TextBox)sender).Text = percentage.ToString() + "%";
            updateSliders();
        }

        private void updateSliders()
        {
            int brightness = 0;
            int contrast = 0;
            int.TryParse(brightnessValue.Text.Replace("%", "").Trim(), out brightness);
            brightnessSlider.Value = brightness;
            int.TryParse(contrastValue.Text.Replace("%", "").Trim(), out contrast);
            contrastSlider.Value = contrast;
        }

        private void updateValues()
        {
            brightnessValue.Text = brightnessSlider.Value.ToString() + "%";
            contrastValue.Text = contrastSlider.Value.ToString() + "%";
        }

        private void slider_Scroll(object sender, EventArgs e)
        {
            updateValues();
        }

        private void value_Leave(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                value_TextChanged(sender, null);
            }
        }

        private void value_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender is TextBox && e.KeyCode == Keys.Enter)
            {
                value_TextChanged(sender, null);
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Hide();
        }

        private void btn_Okay_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Hide();
        }
    }
}
