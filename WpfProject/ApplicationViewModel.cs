using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Linq;
/*VIEW-MODEL*/
namespace WpfProject
{
    public class ApplicationViewModel: INotifyPropertyChanged
    {
        private Pokemon chosenPokemon;
        private const string baseUrl = "https://pokeapi.co/api/v2/pokemon/";
        private static HttpClient client = new HttpClient();
        private JObject pokedex;
        
        public ObservableCollection<Pokemon> RandomPokemons {get; set;}

        // On startup, get all the pokemon Names and routes
        public async void GetPokeDex()
        {
            string pokedexUrl = baseUrl + "/?limit=964";
            var result = await client.GetStringAsync(pokedexUrl);
            pokedex = JObject.Parse(result);
            //Console.WriteLine(pokedex);
        }
        
        public Pokemon ChosenPokemon
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
            GetPokeDex();
            
        }
        
        //To randomize pokemons list on the initial page
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
        
        //OnClick command for the button "Randomize", to randomize pokemons list again
        private RelayCommand randomize_btn_click;
        public RelayCommand Randomize_btn_click
        {
            get
            {
                return randomize_btn_click ??
                       (randomize_btn_click = new RelayCommand(obj =>
                       {
                           RandomPokemons.Clear();
                           RandomizePokemons(RandomPokemons);
                       }));
            }
        }

        // TODO: Fix the "" pokesearch, it makes the whole thing crash
        private async void GetPokemon()
        {
            try
            {
                string url;
                int id;
                // have to check if it is the id, or the name of the pokemon
                /// if it is a number included within thoses attributed to a pokemon, we good

                if ((pokesearch.All(char.IsDigit)))
                {
                    id = int.Parse((pokesearch));
                    url = baseUrl + id.ToString() + "/";
                }
                /// if it's not all numbers, we cann assume it's a pokemon name, worst case scenario, it raises an exception
                else
                {
                    url = baseUrl + pokesearch.ToLower() + "/";
                }

                var result = await client.GetStringAsync(url);
                Console.WriteLine(result);
                //Parsing received JSON string
                JObject json = JObject.Parse(result);
                //Console.WriteLine(json);
                //assigning the values
                id = (int) json["id"];
                string namebuff = (string) json["name"];
                string name = char.ToUpper(namebuff[0]) + namebuff.Substring(1);
                string imgUrl = (string) json["sprites"]["front_default"];
                //adding new pokemon to the list
                ChosenPokemon = new Pokemon {Name = name, ID = id, Url = url, ImgUrl = imgUrl};
            }
            //if the wrong typoe of request is made...
            catch (AggregateException e)
            {
                Console.Error.WriteLine(e.Message);
            }
            // invalid pokemon usually, most probably 404, so we generate a random pokemon that we are sure exists
            catch( HttpRequestException e)
            {
                Console.Error.WriteLine(e.StackTrace);
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.GetType());

                Random rnd = new Random();
                int id = rnd.Next(1, 807);
                //string to request pokemon info
                string url = baseUrl + id.ToString() + "/";
                var result = await client.GetStringAsync(url);
                //Parsing received JSON string
                JObject json = JObject.Parse(result);
                //assigning the values
                string namebuff = (string) json["name"];
                string name = char.ToUpper(namebuff[0]) + namebuff.Substring(1);
                string imgUrl = (string) json["sprites"]["front_default"];
                //adding new pokemon to the list
                ChosenPokemon = new Pokemon {Name = ("INCORRECT REQUEST SO HERE'S " +name.ToUpper()) , ID = id, Url = url, ImgUrl = imgUrl};
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Console.WriteLine("Couldn't properly find the Pokemon, so I decided to die");
                throw;
            }
        }
        
        
        private string pokesearch;

        public string Pokesearch
        {
            get => pokesearch;
            set
            {
                pokesearch = value.Trim();
                OnPropertyChanged();
            } 
        }

        private RelayCommand startSearch;

        public RelayCommand StartSearch
        {
            get {
                return startSearch ?? (startSearch = new RelayCommand( obj  =>
                {
                    GetPokemon();
                })); 
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