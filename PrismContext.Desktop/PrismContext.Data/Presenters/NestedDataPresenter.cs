using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Prism.Mvvm;
using PrismContext.Desktop.Data.Models;

namespace PrismContext.Desktop.Data.Presenters
{
    public class NestedDataPresenter : BindableBase
    {
        private NestedData Model { get; set; }

        public String Name
        {
            get { return Model.Name; }
            set
            {
                Model.Name = value;
                OnPropertyChanged();
            }
        }

        //public ObservableCollection<NestedDataPresenter> SubDatas
        //{
        //    get { return (ObservableCollection<NestedDataPresenter>)Model.SubDatas; }
        //    set
        //    {
        //        if (value != null) Model.SubDatas = new List<NestedData>(value);
        //        OnPropertyChanged();
        //    }
        //}

        //public NestedDataPresenter(String name)
        //{
        //    Name = name;
        //    SubDatas = null;
        //}

        //public NestedDataPresenter(String name, IEnumerable<NestedDataPresenter> subDatas)
        //    : this(name)
        //{
        //    SubDatas = (ObservableCollection<NestedDataPresenter>)subDatas;
        //}
    }
}
