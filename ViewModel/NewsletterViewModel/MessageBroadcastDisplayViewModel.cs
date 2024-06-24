using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DocumentFormat.OpenXml.Vml;
using ScannerAndDistributionOfQRCodes.Data.Message;
using ScannerAndDistributionOfQRCodes.Data.Parser;
using ScannerAndDistributionOfQRCodes.Data.Parser.Interface;
using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerAndDistributionOfQRCodes.ViewModel.NewsletterViewModel
{
    public partial class MessageBroadcastDisplayViewModel : ViewModelBase
    {
        private MailAccount _mailAccount;
        private Stream _streamImage;
        private MessageText _textMessage;
        private List<Dictionary<string, string>> _listxlsxParser;
        [ObservableProperty]
        private List<string> _listMail = new List<string>();
        [ObservableProperty]
        private string _columnNumber;

       

        public RelayCommand SendCommand => new(async () =>
        {

        });

        public RelayCommand ParserCommand => new(async()=>
        {
            FilePickerFileType? customFileType =
            new(new Dictionary<DevicePlatform, IEnumerable<string>>
            {
                    { DevicePlatform.WinUI, new[] { ".xlsx" } }
            });

            var result = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = customFileType
            }).ConfigureAwait(true);
            ListOfParsed(result, new XlsxParser());
        });

        public async Task ListOfParsed(FileResult result,IParser xlsxParser)
        {
        
            try
            {
                var stream = await result.OpenReadAsync().ConfigureAwait(true);
                _listxlsxParser = xlsxParser.Pars(stream);
            }
            catch (Exception ex)
            {
                DisplayAlertError($"Произлшла ошибка при чтение файла!\n {ex.Message} \n Повторите попытку заново!");
                return;
            }
            if (CheckingListFilling(_listxlsxParser))
            {
                DisplayAlertError($"Файл {result.FileName} не содержит необходимых данных");
                return;
            }


        }
        private bool CheckingListFilling(List<Dictionary<string, string>> listxlsxParser)
        {
            foreach (var item in listxlsxParser)
            {
                if (item.ContainsKey(ColumnNumber))
                    ListMail.Add(item[ColumnNumber]);
            }
            //ListMail = listxlsxParser[]
            return listxlsxParser.Count == 0;
        }

        private void DisplayAlertError(string errorMessage)
        {
            //IsError = true;
            Application.Current.MainPage.DisplayAlert("Предупреждение", errorMessage, "Ок");
        }
        public void MessageBroadcast(MailAccount mailAccount, Stream streamImage, MessageText textMessage)
        {
            _mailAccount = mailAccount;
            _streamImage = streamImage;
            _textMessage = textMessage;
        }
    }
}
