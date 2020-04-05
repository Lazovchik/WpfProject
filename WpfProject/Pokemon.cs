using System.ComponentModel;
using System.Runtime.CompilerServices;
/*MODEL*/
namespace WpfProject
{
    public class Pokemon: INotifyPropertyChanged
    {
        private string url;
        private int id;
        private string name;
        private string imgUrl;
        private string type1;
        private string type2;
        private float weight;
        private float height;
        private string flavorText;

        public string FlavorText
        {
            get => flavorText;
            set
            {
                flavorText = value;
                OnPropertyChanged("FlavorText");
            }
        }

        public float Height
        {
            get => height;
            set
            {
                height = value;
                OnPropertyChanged("Height");
            }
        }

        public float Weight
        {
            get => weight;
            set
            {
                weight = value;
                OnPropertyChanged("Weight");
            }
        }

        public string Type1
        {
            get => type1;
            set
            {
                type1 = value;
                OnPropertyChanged("Type1");
            }
        }

        public string Type2
        {
            get => type2;
            set
            {
                type2 = value;
                OnPropertyChanged("Type2");
            }
        }

        public string Url
        {
            get { return url; }
            set
            {
                url = value;
                OnPropertyChanged("Url");
            }
        }
        public int ID
        {
            get { return id; }
            set
            {
                id= value;
                OnPropertyChanged("ID");
            }
        }
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public string ImgUrl
        {
            get { return imgUrl; }
            set
            {
                imgUrl = value;
                OnPropertyChanged("ImgUrl");
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