﻿using Caliburn.Micro;
using OngekiFumenEditor.UI.Controls.ObjectInspector.UIGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OngekiFumenEditor.UI.Controls.ObjectInspector.ViewModels
{
    public abstract class CommonUIViewModelBase : PropertyChangedBase
    {
        private IObjectPropertyAccessProxy propertyInfo;

        public CommonUIViewModelBase(IObjectPropertyAccessProxy wrapper)
        {
            PropertyInfo = wrapper;
        }

        public IObjectPropertyAccessProxy PropertyInfo
        {
            get
            {
                return propertyInfo;
            }
            set
            {
                propertyInfo = value;
                NotifyOfPropertyChange(() => PropertyInfo);
            }
        }
    }

    public abstract class CommonUIViewModelBase<T> : CommonUIViewModelBase where T : class
    {
        public T TypedProxyValue
        {
            get => ProxyValue as T;
            set => ProxyValue = value;
        }

        public object ProxyValue
        {
            get => PropertyInfo.ProxyValue;
            set => PropertyInfo.ProxyValue = value;
        }

        protected CommonUIViewModelBase(IObjectPropertyAccessProxy wrapper) : base(wrapper)
        {
            PropertyInfo.PropertyChanged += PropertyInfo_PropertyChanged;
        }

        private void PropertyInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ProxyValue):
                    Refresh();
                    break;
                default:
                    //NotifyOfPropertyChange(e.PropertyName);
                    break;
            }
        }
    }
}
