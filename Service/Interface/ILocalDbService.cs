using ScannerAndDistributionOfQRCodes.Data.Message;
using ScannerAndDistributionOfQRCodes.Model;
using System.Collections.ObjectModel;

namespace ScannerAndDistributionOfQRCodes.Service.Interface
{
    public interface ILocalDbService
    {
        public void Init();
        public WholeEvent GetWholeEvent();
        public void Create<T>(T value);
        public void Update<T>(T value);
        //public void DeleteFileData();
        public MailAccount GetMailAccount();
        public void Delete<T>(T value);
        public void DeleteAndUpdate<TDelete, TUpdate>(TDelete valueDelete, TUpdate valueUpdate);
        public void CreateAndUpdate<TCreate, TUpdate>(TCreate valueCreate, TUpdate valueUpdate);
      

        //public ObservableCollection<CardQuestion> GetById(int id);
    }
}
