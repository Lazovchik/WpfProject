using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
/*VIEW-MODEL*/
namespace WpfProject
{
    public class ApplicationViewModel: INotifyPropertyChanged
    {
        private Pokemon chosenPokemon;
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
            RandomPokemons = new ObservableCollection<Pokemon>
            {
                new Pokemon{Name="Bulbazaur", ID =555, Url = "https://asdjbasdjsd.com", ImgUrl = "kartinka"},
                new Pokemon{Name="FDPzaur", ID =556, Url = "https://asdjbasasasdsdjsd.com", ImgUrl = "kartinka2"}
            };
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