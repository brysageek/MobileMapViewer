using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace MobileMapViewer.Services
{
    class NavigationService : INavigationService
    {
        private readonly Dictionary<string, Type> _appPages = new Dictionary<string, Type>();

        public string CurrentPageKey { get; set; }

        public void GoBack()
        {
            throw new System.NotImplementedException();
        }

        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            lock (_appPages)
            {
                var _page = _appPages[pageKey];

                if (_appPages.ContainsKey(pageKey))
                {
                    ConstructorInfo ctorInfo;
                    object[] parameters;
                    if (parameter == null)
                    {
                        ctorInfo = _page.GetTypeInfo()
                            .DeclaredConstructors
                            .FirstOrDefault(c => !c.GetParameters().Any());

                        parameters = new object[] { };
                    }
                    else
                    {
                        ctorInfo = _page.GetTypeInfo()
                            .DeclaredConstructors
                            .FirstOrDefault(c =>
                            {
                                var p = c.GetParameters();
                                return p.Length == 1 && p[0].ParameterType == parameter.GetType();
                            });

                        parameters = new object[] { };
                    }

                    if (ctorInfo == null)
                    {
                        throw new Exception("No Suitable constructor found for page " + pageKey);
                    }

                    var page = ctorInfo.Invoke(parameters) as ContentPage;
                    CurrentPageKey = pageKey;
                    if (Application.Current.MainPage is NavigationPage)
                    {
                        ((NavigationPage) Application.Current.MainPage).PushAsync(page);
                    }
                    else
                    {
                        Application.Current.MainPage = page;
                    }
                }
                else
                {
                    throw new ArgumentException(
                        $"no Such Page: {pageKey}. Check to make sure that the view has been register");
                }
            }
        }

        public void RegisterView(string pageKey, Type pageType)
        {
            lock (_appPages)
            {
                if (_appPages.ContainsKey(pageKey))
                {
                    _appPages[pageKey] = pageType;
                }
                else
                {
                    _appPages.Add(pageKey, pageType);
                }
            }
        }
    }
}