using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Common.Models.Others
{
    public class Node : INotifyPropertyChanged
    {
        public delegate void NodeHandler(Node message);
        public static event NodeHandler SendNode;

        private static object _selectedItem;
        public string Name { get; set; }
        public ObservableCollection<Node> Nodes { get; set; }

        public string DaddyName { get; set; }
        public string NodeType { get; set; }

        public string ViewName { get; set; }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged("IsSelected");
                    if (_isSelected)
                    {
                        SelectedItem = this;
                    }
                }
            }
        }

        public static object SelectedItem
        {
            get { return _selectedItem; }
            private set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;

                    OnSelectedItemChanged();
                }
            }
        }

        public static void OnSelectedItemChanged()
        {
            Node node = SelectedItem as Node;
            SendNode?.Invoke(node);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}