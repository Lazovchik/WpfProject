using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

/*VIEW-MODEL*/
namespace WpfProject
{
    public class ApplicationViewModel: INotifyPropertyChanged
    {
        private Pokemon chosenPokemon;
        private const string baseUrl = "https://pokeapi.co/api/v2/pokemon/";
        private static HttpClient client = new HttpClient();
        
        public ObservableCollection<Pokemon> RandomPokemons {get; set;}
        
        public Pokemon  ChosenPokemon
        {
            get { return chosenPokemon; }
            set
            {
                chosenPokemon = value;
                OnPropertyChanged("ChosenPokemon");
            }
        }

        public  ApplicationViewModel()
        { 
            RandomPokemons = new ObservableCollection<Pokemon> { }; 
            RandomizePokemons(RandomPokemons);
        }

        static async void RandomizePokemons(ObservableCollection<Pokemon> buffer)
        {   
            //will randomize the id of pokemon
            Random rnd = new Random();
            int id;
            
            //initializing title list of 10 random pokemons
            for (int i = 0; i < 10; i++)
            {
                //pokemon id randomized
                id = rnd.Next(1, 807);
                //string to request pokemon info
                string url = baseUrl + id.ToString() + "/";
                //API call
                var result = await client.GetStringAsync(url);
                //Parsing received JSON string
                JObject json = JObject.Parse(result);
                //assigning the values
                string namebuff = (string) json["name"];
                string name = char.ToUpper(namebuff[0]) + namebuff.Substring(1);
                string imgUrl = (string) json["sprites"]["front_default"];
                //adding new pokemon to the list
                buffer.Add(new Pokemon{Name=name, ID =id, Url = url, ImgUrl = imgUrl});
            }
        }

        //implementation of INotifyPropertyChanged interface
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}