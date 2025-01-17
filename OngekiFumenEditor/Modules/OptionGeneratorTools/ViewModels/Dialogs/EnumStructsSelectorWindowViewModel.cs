﻿using Caliburn.Micro;
using Gemini.Framework;
using OngekiFumenEditor.Modules.OptionGeneratorTools.Kernel;
using OngekiFumenEditor.Modules.OptionGeneratorTools.Models;
using OngekiFumenEditor.Modules.OptionGeneratorTools.Models.EnumStructs;
using OngekiFumenEditor.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Data;

namespace OngekiFumenEditor.Modules.OptionGeneratorTools.ViewModels.Dialogs
{
    public class EnumStructsSelectorWindowViewModel : WindowBase
    {
        public ObservableCollection<IEnumStruct> EnumStructs { get; set; }

        private ICollectionView dataView;
        private string filterString;
        public string FilterString
        {
            get => filterString;
            set => Set(ref filterString, value);
        }

        private IEnumStruct selected;
        public IEnumStruct Selected
        {
            get => selected;
            set => Set(ref selected, value);
        }

        private GridViewColumnHeader _lastHeaderClicked = null;
        private ListSortDirection _lastDirection = ListSortDirection.Ascending;

        public EnumStructsSelectorWindowViewModel(IEnumerable<IEnumStruct> enumStructs, IEnumStruct currentSelected)
        {
            EnumStructs = new(enumStructs);
            Selected = currentSelected;

            dataView = CollectionViewSource.GetDefaultView(EnumStructs);
            dataView.Filter = x =>
            {
                if (FilterString == null || FilterString.Length == 0)
                    return true;
                return x.ToString().Contains(FilterString, StringComparison.InvariantCultureIgnoreCase);
            };
            Sort("Id", ListSortDirection.Ascending);
        }

        private void Sort(string sortBy, ListSortDirection direction)
        {
            dataView.SortDescriptions.Clear();
            dataView.SortDescriptions.Add(new SortDescription(sortBy, direction));
            dataView.Refresh();
        }

        public void SortColumn(ActionExecutionContext ctx)
        {
            var e = ctx.EventArgs as RoutedEventArgs;

            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    Sort(sortBy, direction);

                    // Remove arrow from previously sorted header
                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        public void ApplyFilter()
        {
            dataView.Refresh();
        }

        public async void Comfirm()
        {
            await TryCloseAsync();
        }

        public async void Cancel()
        {
            Selected = default;
            await TryCloseAsync();
        }
    }
}
