﻿using GongSolutions.Wpf.DragDrop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KeyViewer
{
    public class SettingsViewModel : BaseNotifyPropertyChanged, IDropTarget
    {
        public KeyModelCollection KeyModels { get; set; }

        private int selectedIndex;
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                OnPropertyChanged();
                OnPropertyChanged("CurrModel");
                OnPropertyChanged("IsSelecting");
            }
        }

        public KeyModel CurrModel => SelectedIndex != -1 ? KeyModels[SelectedIndex] : null;
        public bool IsSelecting => selectedIndex != -1;
        
        public ICommand RemoveKeyCommand { get; set; }

        public SettingsViewModel()
        {
            KeyModels = KeyModelCollection.Instance;

            RemoveKeyCommand = new Command(RemoveKey);

            SelectedIndex = -1;
        }

        public void AddKey(Key key)
        {
            KeyModels.Add(key);
            OnPropertyChanged("KeyModels");
        }

        public void RemoveKey()
        {
            if (SelectedIndex != -1)
            {
                KeyModels.RemoveAt(SelectedIndex);
                SelectedIndex = -1;
                OnPropertyChanged("KeyModels");
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
            dropInfo.Effects = System.Windows.DragDropEffects.Move;
        }

        public void Drop(IDropInfo dropInfo)
        {
            KeyModels.Move(KeyModels.IndexOf((KeyModel)dropInfo.Data), Math.Max(dropInfo.InsertIndex, 0));
        }
    }
}