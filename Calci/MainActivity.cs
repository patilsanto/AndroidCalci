using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using Android.Widget;
using System;

namespace Calci
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private TextView calculatorText;

        private string[] numbers = new string[2];
        private string @operator;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            calculatorText = FindViewById<TextView>(Resource.Id.calciScreen);
        }

       
        [Java.Interop.Export("ButtonClick")]
        public void ButtonClick(View v)
        {
            Button button = (Button)v;
            if ("0123456789.".Contains(button.Text))
                AddDigitOrDecimalPoint(button.Text);
            else if ("/*+-".Contains(button.Text))
                AddOperator(button.Text);
            else if ("=" == button.Text)
                Calculate();
            else
                Erase();
        }

        private void Erase()
        {
            numbers[0] = numbers[1] = null;
            @operator = null;
            UpdateCalculatorText();
        }

        

        private void AddOperator(string value)
        {
            if (numbers[1] != null)
            {
                Calculate(value);
                return;
            }

            @operator = value;

            UpdateCalculatorText();
        }

        private void AddDigitOrDecimalPoint(string value)
        {
            int index = @operator == null ? 0 : 1;

            if (value == "." && numbers[index].Contains("."))
                return;

            numbers[index] += value;

            UpdateCalculatorText();
        }

        private void UpdateCalculatorText() => calculatorText.Text = $"{numbers[0]} {@operator} {numbers[1]}";



        private void Calculate(string newOperator = null)
        {
            double? result = null;
            double? first = numbers[0] == null ? null : (double?)double.Parse(numbers[0]);
            double? second = numbers[1] == null ? null : (double?)double.Parse(numbers[1]);

            switch (@operator)
            {
                case "÷":
                    result = first / second;
                    break;
                case "×":
                    result = first * second;
                    break;
                case "+":
                    result = first + second;
                    break;
                case "-":
                    result = first - second;
                    break;
            }

            if (result != null)
            {
                numbers[0] = result.ToString();
                @operator = newOperator;
                numbers[1] = null;
                UpdateCalculatorText();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
