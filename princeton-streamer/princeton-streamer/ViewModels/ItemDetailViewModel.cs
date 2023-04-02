using princeton_streamer.Models;
using princeton_streamer.Views;
using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace princeton_streamer.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        public class Root
        {
            public TransactionDetails transaction_details { get; set; }
        }

        public class TransactionDetails
        {
            public string transactionHash { get; set; }
            public string blockExplorer { get; set; }
            public string transactionID { get; set; }
            public string status { get; set; }
        }

        public bool IsShow { get; set; }
        private string itemId;
        private string text;
        private string description;
        private string tranxId;
        public string Id { get; set; }
        public Command BuyCommand { get; }

        public ItemDetailViewModel()
        {
            IsShow = false;
            BuyCommand = new Command(OnBuyClicked);
        }
        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }
        public string TranxId
        {
            get => tranxId;
            set => SetProperty(ref tranxId, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                LoadItemId(value);
            }
        }

        public async void LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                Id = item.Id;
                Text = item.Text;
                Description = item.Description;
                TranxId = ".";
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
        private async void OnBuyClicked(object obj)
        {
            string key = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 16);
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://api.verbwire.com/v1/nft/mint/quickMintFromMetadata"),
                Headers =
    {
        { "accept", "application/json" },
        { "X-API-Key", "sk_live_5a03b097-0797-49eb-ae7e-27d3c4b17780" },
    },
                Content = new MultipartFormDataContent
    {
        new StringContent("goerli")
        {
            Headers =
            {
                ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "chain",
                }
            }
        },
        new StringContent(key)
        {
            Headers =
            {
                ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    Name = "data",
                }
            }
        },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(body);
                IsShow = true;
                TranxId = "TranxId:" + myDeserializedClass.transaction_details.transactionID;                
            }
        }
    }
}
