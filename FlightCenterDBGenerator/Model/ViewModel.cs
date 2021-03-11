using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Net.Http.Formatting;
using Newtonsoft.Json;

namespace FlightCenterDBGenerator
{
    public class ViewModel : INotifyPropertyChanged
    {
        public PropertyValue AirlineCompanies { get; set; }
        public PropertyValue Customers { get; set; }
        public PropertyValue Administrators { get; set; }
        public PropertyValue FlightsPerCompany { get; set; }
        public PropertyValue TicketsPerCustomer { get; set; }
        public PropertyValue Countries { get; set; }
        private int progressBarValue;
        public int ProgressBarValue
        {
            get
            {
                return progressBarValue;
            }
            set
            {
                progressBarValue = value;
                OnPropertyChanged("ProgressBarValue");
            }
        }
        public Command GenerateButtonClick { get; set; }
        public Command ClearButtonClick { get; set; }
        private string statusText;
        public string StatusText
        {
            get
            {
                return statusText;
            }
            set
            {
                statusText = value;
                OnPropertyChanged("StatusText");
            }
        }
        private Random r = new Random();
        private bool isIdle;
        public bool IsIdle
        {
            get
            {
                return isIdle;
            }
            set
            {
                isIdle = value;
                OnPropertyChanged("IsIdle");
            }
        }


        public ViewModel()
        {
            AirlineCompanies = new PropertyValue("Airline Companies");
            Customers = new PropertyValue("Customers");
            Administrators = new PropertyValue("Administrators");
            FlightsPerCompany = new PropertyValue("Flights Per Company");
            TicketsPerCustomer = new PropertyValue("Tickets Per Customer");
            Countries = new PropertyValue("Countries");
            ProgressBarValue = 0;
            GenerateButtonClick = new Command(new Action<object>(Generate), new Func<object, bool>((o) => { return true; }));
            ClearButtonClick = new Command(new Action<object>(Clear), new Func<object, bool>((o) => { return true; }));
            IsIdle = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private void Generate(object obj)
        {
            ProgressBarValue = 0;
            StatusText = "\nStarting generating records";
            int[] values = new int[6];
            if (!CheckInput(values))
            {
                StatusText = StatusText + "\nDetected Error, Stopping...";
                return;
            }
            RecordsGenerator recordsGenerator = new RecordsGenerator(
                values[0],
                values[1],
                values[2],
                values[3],
                values[4],
                values[5]);
            recordsGenerator.GenerateRecords(this);
        }
        private async void Clear(object obj)
        {
            ProgressBarValue = 0;
            StatusText = "\nStarting records delete";
            await Task.Delay(500);
            RecordsGenerator.ClearAll(this);
            ProgressBarValue = 6;
        }
        private bool CheckInput(int[] values)
        {
            PropertyValue[] properties = new PropertyValue[6];
            properties[0] = AirlineCompanies;
            properties[1] = Customers;
            properties[2] = Administrators;
            properties[3] = FlightsPerCompany;
            properties[4] = TicketsPerCustomer;
            properties[5] = Countries;
            int i = 0;
            foreach(PropertyValue property in properties)
            {
                if (property.IsRandom)
                {
                    try
                    {
                        Regex regex = new Regex(@"[0-9]+?-[0-9]+");
                        if (property.Range == null || property.Range == "" || regex.Match(property.Range) == Match.Empty)
                        {
                            throw new InvalidRangeException($"Invalid Range in {property.Name} Row");
                        }
                    }
                    catch (InvalidRangeException e)
                    {
                        MessageBox.Show(e.Message, "Invalid Range", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                    try
                    {
                        string[] range = property.Range.Split('-');
                        int value1 = int.Parse(range[0]);
                        int value2 = int.Parse(range[1]);
                        if (!((value1 >= 2 & value1 <= 20) & (value1 >= 2 & value1 <= 20)))
                        {
                            throw new InvalidRangeException($"The range of {property.Name} exceeds allowed values");
                        }
                        else
                        {
                            if (value1 >= value2)
                            {
                                values[i] = r.Next(value2, value1 + 1);
                            }
                            else
                            {
                                values[i] = r.Next(value1, value2 + 1);
                            }
                            
                        }
                    }
                    catch(InvalidRangeException e)
                    {
                        MessageBox.Show(e.Message, "Invalid Range", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }
                }
                else
                {
                    try
                    {
                        int value = property.Amount;
                        if (!(value >= 2 & value <= 20))
                        {
                            throw new InvalidValueException($"The value for {property.Name} exceeds allowed values");
                        }
                        values[i] = value;
                    }
                    catch (InvalidValueException e)
                    {
                        MessageBox.Show(e.Message, "Invalid Range", MessageBoxButton.OK, MessageBoxImage.Error);
                        return false;
                    }

                }
                i++;
            }
            //if FlightsPerCompany*AirlineCompanies>TicketsPerCustomer
            if (properties[3].Amount * properties[0].Amount < properties[4].Amount)
            {
                MessageBox.Show("Tickets per customer exceeds the amount of overall flights", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

    }
}
