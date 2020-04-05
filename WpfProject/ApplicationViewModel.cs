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
using System.Threading.Tasks;

/*VIEW-MODEL*/
namespace WpfProject
{
    public class ApplicationViewModel: INotifyPropertyChanged
    {
        //variables declaration
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
            //GetPokeDex();
            pokesearch = "Bulbasaur";
            SearchPokemon();
            RandomPokemons = new ObservableCollection<Pokemon> { }; 
            RandomizePokemons(RandomPokemons);
            
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
                //name
                string namebuff = (string) json["name"];
                //transform first character to upper case
                string name = char.ToUpper(namebuff[0]) + namebuff.Substring(1);
                //image url
                string imgUrl = (string) json["sprites"]["front_default"];
                string[] types = {"none", "none"};
                //pokemon sizes
                float weight = ((float) json["weight"]) / 10; 
                float height = ((float) json["height"]) / 10; 

                //in the json, the secondary type is listed first
                //pokemon types
                if ((int) json["types"][0]["slot"] == 2)
                {
                    types[0] = (string) json["types"][1]["type"]["name"];
                    types[1] = (string) json["types"][0]["type"]["name"];
                    types[0] = char.ToUpper(types[0][0]) + types[0].Substring(1);
                    types[1] = char.ToUpper(types[1][0]) + types[1].Substring(1);
                }
                else
                {
                    types[0] = (string) json["types"][0]["type"]["name"];
                    types[0] = char.ToUpper(types[0][0]) + types[0].Substring(1);
                }
                
                
                //getting aditional data
                string flavorUrl = "https://pokeapi.co/api/v2/pokemon-species/" + name.ToLower() + "/";
                string test = await  client.GetStringAsync(flavorUrl);
                JObject jsontest = JObject.Parse(test);
                IEnumerable<JToken>  flavor = jsontest.SelectTokens("$.flavor_text_entries[?(@.language.name == 'en')].flavor_text");
                
                //pokemons lore description
                string flavor_text = "none";
                foreach (JToken item in flavor)
                {
                    flavor_text = (string) item;
                    break;
                }
                
                if(flavor_text == "none" || flavor_text == "" || string.IsNullOrEmpty(flavor_text) ) flavor_text = "none";

                //adding new pokemon to the list
                buffer.Add(new Pokemon {Name = name, ID = id, Url = url, ImgUrl = imgUrl, Type1 = types[0], Type2 = types[1], Weight = weight, Height = height, FlavorText= flavor_text});
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


        public async Task GetPokemon( string url)
        {
            try
            {
                string result =  await client.GetStringAsync(url);
                //Parsing received JSON string
                JObject json = JObject.Parse(result);
                //Console.WriteLine(json);
                //assigning the values
                int id = (int) json["id"];
                string namebuff = (string) json["name"];
                string name = char.ToUpper(namebuff[0]) + namebuff.Substring(1);
                string imgUrl = (string) json["sprites"]["front_default"];
                string[] types = {"none", "none"};
                float weight = ((float) json["weight"]) / 10; 
                float height = ((float) json["height"]) / 10; 

                ///in the json, the secondary type is listed first
                if ((int) json["types"][0]["slot"] == 2)
                {
                    types[0] = (string) json["types"][1]["type"]["name"];
                    types[1] = (string) json["types"][0]["type"]["name"];
                    types[0] = char.ToUpper(types[0][0]) + types[0].Substring(1);
                    types[1] = char.ToUpper(types[1][0]) + types[1].Substring(1);
                }
                else
                {
                    types[0] = (string) json["types"][0]["type"]["name"];
                    types[0] = char.ToUpper(types[0][0]) + types[0].Substring(1);
                }

                string flavorUrl = "https://pokeapi.co/api/v2/pokemon-species/" + name.ToLower() + "/";
                string test = await  client.GetStringAsync(flavorUrl);
                JObject jsontest = JObject.Parse(test);
                IEnumerable<JToken>  flavor = jsontest.SelectTokens("$.flavor_text_entries[?(@.language.name == 'en')].flavor_text");

                string flavor_text = "none";
                foreach (JToken item in flavor)
                {
                     flavor_text = (string) item;
                    break;
                }
               
                
                //chosing a pokemon to display
                ChosenPokemon = new Pokemon{Name = name, ID = id, Url = url, ImgUrl = imgUrl, Type1 = types[0], Type2 = types[1], Weight = weight, Height = height, FlavorText= flavor_text};
            }
            //if the wrong typoe of request is made...
            catch (AggregateException e)
            {
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.Source);
            }
            // invalid pokemon usually, most probably 404, so we generate a random pokemon that we are sure exists
            catch( HttpRequestException e)
            {
                Console.Error.WriteLine(e.Message);
                
                Random rnd = new Random();
                int id = rnd.Next(1, 807);
                //string to request pokemon info
                string newurl = baseUrl + id.ToString() + "/";
                var result = await  client.GetStringAsync(newurl);
                //Parsing received JSON string
                JObject json = JObject.Parse(result);
                //assigning the values
                string namebuff = (string) json["name"];
                string name = char.ToUpper(namebuff[0]) + namebuff.Substring(1);
                string imgUrl = (string) json["sprites"]["front_default"];
                float weight = ((float) json["weight"]) / 10; 
                float height = ((float) json["height"]) / 10;
                
                
                string[] types = {"none", "none"};
                ///in the json, the secondary type is listed first
                if ((int) json["types"][0]["slot"] == 2)
                {
                    types[0] = (string) json["types"][1]["type"]["name"];
                    types[1] = (string) json["types"][0]["type"]["name"];
                    types[0] = char.ToUpper(types[0][0]) + types[0].Substring(1);
                    types[1] = char.ToUpper(types[1][0]) + types[1].Substring(1);
                }
                else
                {
                    types[0] = (string) json["types"][0]["type"]["name"];
                    types[0] = char.ToUpper(types[0][0]) + types[0].Substring(1);
                }
                
                string flavorUrl = "https://pokeapi.co/api/v2/pokemon-species/" + name.ToLower() + "/";
                string test = await  client.GetStringAsync(flavorUrl);
                JObject jsontest = JObject.Parse(test);
                IEnumerable<JToken>  flavor = jsontest.SelectTokens("$.flavor_text_entries[?(@.language.name == 'en')].flavor_text");

                string flavor_text = "none";
                foreach (JToken item in flavor)
                {
                    flavor_text = (string) item;
                    break;
                }

                //pokemon to display becomes
                ChosenPokemon =  new Pokemon {Name = ("RANDOM : " + name), ID = id, Url = newurl, ImgUrl = imgUrl, Type1 = types[0], Type2 = types[1], Weight = weight, Height = height, FlavorText= flavor_text};
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Console.WriteLine("Test failed, not HttpRequestException nor AggregateException");
                throw;
            }
        }
        
        
        private async void SearchPokemon()
        {
            try
            {
                string url;
                int id;
                // have to check if it is the id, or the name of the pokemon
                // if it is a number included within thoses attributed to a pokemon, we good

                if ((pokesearch.All(char.IsDigit)))
                {
                    id = int.Parse((pokesearch));
                    url = baseUrl + id.ToString() + "/";
                }
                // if it's not all numbers, we cann assume it's a pokemon name, worst case scenario, it raises an exception
                else
                {
                    url = baseUrl + pokesearch.ToLower() + "/";
                }
                
                await GetPokemon(url);

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
                await GetPokemon(url);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Console.WriteLine("Couldn't properly find the Pokemon, so I decided to die");
                throw;
            }
        }
        
        //for the value of search textbox 
        private string pokesearch;
        public string Pokesearch
        {
            get => pokesearch;
            set
            {
                pokesearch = value.Trim();
                if (string.IsNullOrEmpty(pokesearch))
                {
                    if (string.IsNullOrEmpty(ChosenPokemon.Name)) pokesearch = "bulbasaur";
                    else pokesearch = ChosenPokemon.Name;
                }
                OnPropertyChanged();
            } 
        }

        //command for the search button
        private RelayCommand startSearch;
        public RelayCommand StartSearch
        {
            get {
                return startSearch ?? (startSearch = new RelayCommand( obj  =>
                {
                    SearchPokemon();
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