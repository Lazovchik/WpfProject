using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
/*VIEW-MODEL*/
namespace WpfProject
{
    public class ApplicationViewModel: INotifyPropertyChanged
    {
        private Pokemon chosenPokemon;
        private const string baseUrl = "https://pokeapi.co/api/v2/pokemon/";
        
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

        public ApplicationViewModel()
        { 
            RandomPokemons = new ObservableCollection<Pokemon> { };
            RandomizePokemons(RandomPokemons);
        }

        public void RandomizePokemons(ObservableCollection<Pokemon> buffer)
        {
            Random rnd = new Random();
            int id;
            
            for (int i = 0; i < 10; i++)
            {
                id = rnd.Next(0, 965);
                string url = baseUrl + id.ToString() + "/";

                buffer.Add(new Pokemon{Name="Bulbazaur", ID =id, Url = url, ImgUrl = url});
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