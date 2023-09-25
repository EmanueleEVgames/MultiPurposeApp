using FoodPreferences.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FoodPreferences.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "ALLERGIE ed INTOLLERANZE";
            SubmitButtonPressed = new Command(async () => await SendDataAsync());
            DownloadButtonPressed = new Command(async () => await DownloadDataAsync());

            Feedbacks = new ObservableCollection<FeedbackModel>();
        }

        public ICommand SubmitButtonPressed { get; }
        public ICommand DownloadButtonPressed { get; }

        bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string name;
        public string Name { get => name; set => SetProperty(ref name, value); }

        private string food;
        public string Food { get => food; set => SetProperty(ref food, value); }

        private string preference;
        public string Preference { get => preference; set => SetProperty(ref preference, value); }

        public ObservableCollection<FeedbackModel> Feedbacks { get; set; }
        async Task SendDataAsync()
        {
            var client = new HttpClient();
            var model = new FeedbackModel()
            {
                //Name = NameEntry.Text,
                //Phone = PhoneEntry.Text,
                //Email = EmailEntry.Text,
                //Feedback = FeedbackEntry.Text
                Name = Name,
                Food = Food,
                Preference = Preference,

            };
            var uri = "https://script.google.com/macros/s/AKfycbw8R_pMGM30HN2J3JBfP5JQe2hfIQ0KPjhgRVASErm03tOeZUhIgwlMDUj3798myg-Z/exec";
            var jsonString = JsonConvert.SerializeObject(model);
            var requestContent = new StringContent(jsonString);
            var result = await client.PostAsync(uri, requestContent);
            var resultContent = await result.Content.ReadAsStringAsync();
            try
            {
                var response = JsonConvert.DeserializeObject<ResponseModel>(resultContent);
            }
            catch (Exception ex)
            {

            }
        }

        async Task DownloadDataAsync()
        {
            var client = new HttpClient();

            var uri = "https://script.google.com/macros/s/AKfycbw8R_pMGM30HN2J3JBfP5JQe2hfIQ0KPjhgRVASErm03tOeZUhIgwlMDUj3798myg-Z/exec";

            //var requestContent = new StringContent(jsonString);
            var response = await client.GetAsync(uri);
            var resultContent = await response.Content.ReadAsStringAsync();
            try
            {
                var cc = resultContent.Replace("[[", "[").Replace("]]", "]");
                var ccc = resultContent.Replace("[", "").Replace("]", "").Replace("\"", "");
                string[] elements = ccc.Split(',');

                int numOfRows = elements.Length / 3 - 1;

                Feedbacks.Clear();
                FeedbackModel[] temp = new FeedbackModel[numOfRows];
                int pnt = 0;
                for (int c = 0; c < elements.Count() - 3; c += 3)
                {
                    FeedbackModel model = new FeedbackModel();
                    model.Name = elements[c + 3];
                    model.Food = elements[c + 1 + 3];
                    model.Preference = elements[c + 2 + 3];
                    temp[pnt] = model;
                    pnt += 1;
                }

                IEnumerable<FeedbackModel> query = temp.OrderBy(s => s.Name);

                foreach (var item in query)
                {
                    Feedbacks.Add(item);
                }
                OnPropertyChanged("Feedbacks");

                //var test = JsonConvert.DeserializeObject<FeedbackModel>(cc);
            }
            catch (Exception ex)
            {
            }

        }

    }
}