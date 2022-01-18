using ArgedeSP.Contracts.DTO.Hizmet.Req;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Helpers.Extantions;
using ArgedeSP.Contracts.Interfaces.BusinessLogicLayers;
using ArgedeSP.Contracts.Interfaces.Repositories;
using ArgedeSP.Contracts.Models.Common;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.BLL.BusinessServices
{
    public class AnahtarDegerBS : IAnahtarDegerBS
    {
        private IAnahtarDegerRepository _anahtarDegerRepository;

        public AnahtarDegerBS(IAnahtarDegerRepository anahtarDegerRepository)
        {
            _anahtarDegerRepository = anahtarDegerRepository;
        }

        public OperationResult AnahtarGetir(Dil dil, string anahtar)
        {
            try
            {
                AnahtarDeger anahtarDeger = null;
                if (dil == Dil.Yok)
                {
                    anahtarDeger = _anahtarDegerRepository.Find(x => x.Anahtar == anahtar);
                }
                else
                {
                    anahtarDeger = _anahtarDegerRepository.Find(x => x.Anahtar == anahtar && x.Dil == dil);
                }
                
                return OperationResult.Success(anahtarDeger);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(AnahtarGetir)} fonksiyonunda hata");
                return OperationResult.Error(Enums.MesajKodu.BeklenmedikHata);
            }
        }

        public OperationResult AnahtarGetir(string anahtar)
        {
            try
            {
                IList<AnahtarDeger> anahtarDegerler = _anahtarDegerRepository.GetAllMatched(x => x.Anahtar == anahtar).OrderBy(x => x.Dil).ToList();

                return OperationResult.Success(anahtarDegerler);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(AnahtarGetir)} fonksiyonunda hata");
                return OperationResult.Error(Enums.MesajKodu.BeklenmedikHata);
            }
        }

        public OperationResult AnahtarGetir(int id)
        {
            try
            {
                AnahtarDeger anahtarDeger = _anahtarDegerRepository.Find(x => x.Id == id);

                return OperationResult.Success(anahtarDeger);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(AnahtarGetir)} fonksiyonunda hata");
                return OperationResult.Error(Enums.MesajKodu.BeklenmedikHata);
            }
        }

        public OperationResult AnahtarGuncelle(AnahtarDeger anahtarDeger)
        {
            try
            {
                _anahtarDegerRepository.Update(anahtarDeger, true);
             

                return OperationResult.Success(anahtarDeger);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(AnahtarGetir)} fonksiyonunda hata");
                return OperationResult.Error(Enums.MesajKodu.BeklenmedikHata);
            }
        }
    }
}