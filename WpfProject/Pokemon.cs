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