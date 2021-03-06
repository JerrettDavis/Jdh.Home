﻿using Jdh.Home.Mobile.Events;
using Prism.AppModel;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Threading;

namespace Jdh.Home.Mobile.ViewModels.Dialogs
{
    public class ScanningWifiViewModel : BindableBase, IDialogAware, IAutoInitialize
    {
        private CancellationTokenSource _tokenSource;
        private Guid? _id;

        public ScanningWifiViewModel(IEventAggregator eventAggregator)
        {
            try
            {
                CloseCommand = new DelegateCommand(Close);
                eventAggregator.GetEvent<CloseDialogEvent>()
                    .Subscribe(CloseEventReceived);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        public DelegateCommand CloseCommand { get; }

        public event Action<IDialogParameters> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
            _tokenSource?.Cancel();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            var hasToken = parameters.TryGetValue<CancellationTokenSource>(
                ScanningWifiParams.CancellationTokenSource,
                out var cancellationToken);

            if (hasToken)
                _tokenSource = cancellationToken;

            _id = parameters.GetValue<Guid>(ScanningWifiParams.Identifier);
        }

        private void Close()
        {
            try
            {
                RequestClose?.Invoke(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }

        private void CloseEventReceived(Guid id)
        {
            if (id == _id)
                Close();
        }
    }

    public struct ScanningWifiParams
    {
        public static string Identifier = "Identifier";
        public static string CancellationTokenSource = "CancellationTokenSource";
    }
}
