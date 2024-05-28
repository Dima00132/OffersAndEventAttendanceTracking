using ScannerAndDistributionOfQRCodes.Model;
using ScannerAndDistributionOfQRCodes.Service.Interface;
using Microsoft.VisualBasic;
using System;
using SQLite;
using SQLiteNetExtensions.Extensions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScannerAndDistributionOfQRCodes.Service
{

    public sealed class LocalDbService: ILocalDbService
    {
        private const string DB_NAME = "data_whole_event_save_9.db3";
        private SQLiteConnection _connection;
        private const SQLiteOpenFlags Flags =
            SQLiteOpenFlags.ReadWrite |
            SQLiteOpenFlags.Create  |
            SQLiteOpenFlags.SharedCache;


        public void Init()
        {
            if (_connection is not null)
                return;
            try
            {
                _connection = new SQLiteConnection(Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, DB_NAME), Flags);
            }
            catch (Exception)
            {

                throw;
            }
            

            try
            {
                _ = _connection.CreateTable<WholeEvent>();
                _ = _connection.CreateTable<ScheduledEvent>();
                _ = _connection.CreateTable<Guest>();
                _ = _connection.CreateTable<User>();
                _ = _connection.CreateTable<Mail>();
                _ = _connection.CreateTable<VerificationQRCode>();


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
            
        }

        public WholeEvent GetWholeEvent()
        {
            Init();
            WholeEvent wholeEvent = null;
            try
            {
                wholeEvent = _connection.GetAllWithChildren<WholeEvent>(recursive: true).FirstOrDefault();
            }
            catch (Exception)
            {

               
            }
            
            if (wholeEvent is null)
            {
                wholeEvent = new WholeEvent();
                Create(wholeEvent);
            }
            return wholeEvent;
        }

        public void CreateAndUpdate<TCreate, TUpdate>(TCreate valueCreate, TUpdate valueUpdate)
        {
            Create(valueCreate);
            Update(valueUpdate);
        }
        public void DeleteAndUpdate<TDelete, TUpdate>(TDelete valueDelete, TUpdate valueUpdate)
        {
            Delete(valueDelete);
            Update(valueUpdate);
        }

  

        public void Create<T>(T value)
        {
            try
            {
                Init(); 
                _connection.InsertWithChildren(value, recursive: true);
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        public void Update<T>(T value)
        {
            Init();
            _connection.UpdateWithChildren(value);
        }
        
        public void DeleteFileData()
        {
            Init();
            try
            {
                _connection.Dispose();
                File.Delete(Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, DB_NAME));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }    
        }

        public void Delete<T>(T value)
        {
            Init();
            try
            {
                _connection.Delete(value);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
    }
}
