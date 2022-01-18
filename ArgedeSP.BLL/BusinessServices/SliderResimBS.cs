using ArgedeSP.Contracts.Entities;
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
    public class SliderResimBS : ISliderResimBS
    {
        private ISliderResimRepository _sliderResimRepository;

        public SliderResimBS(ISliderResimRepository sliderResimRepository)
        {
            _sliderResimRepository = sliderResimRepository;
        }

        public async Task SliderResimEkle(SliderResim sliderResim)
        {
            try
            {
                await _sliderResimRepository.InsertAsync(new SliderResim()
                {
                    OlusturmaTarihi = DateTime.Now,
                    ResimSira = sliderResim.ResimSira,
                    ResimUrl = sliderResim.ResimUrl,
                    AltBaslik = sliderResim.AltBaslik,
                    Dil = sliderResim.Dil,
                    Link = sliderResim.Link,
                    UstBaslik = sliderResim.UstBaslik,
                    SliderYeri = sliderResim.SliderYeri

                }, true);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(SliderResimEkle)} fonksiyonunda hata");
            }
        }

        public async Task<OperationResult> SliderResimGuncelle(SliderResim sliderResim)
        {
            try
            {
                SliderResim sliderResim2 = await _sliderResimRepository.GetByIdAsync(sliderResim.Id);

                if (sliderResim2 == null)
                {
                    return OperationResult.Error(MesajKodu.SliderBulunamadi);
                }
                sliderResim2.ResimSira = sliderResim.ResimSira;
                sliderResim2.Link = sliderResim.Link;
                sliderResim2.AltBaslik = sliderResim.AltBaslik;
                sliderResim2.Dil = sliderResim.Dil;
                sliderResim2.ResimSira = sliderResim.ResimSira;
                sliderResim2.UstBaslik = sliderResim.UstBaslik;
                sliderResim2.SliderYeri = sliderResim.SliderYeri;


                await _sliderResimRepository.UpdateAsync(sliderResim2, true);
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(SliderResimGuncelle)} fonksiyonunda hata");
                return OperationResult.Error(MesajKodu.BeklenmedikHata);
            }
        }

        public async Task<OperationResult> SliderResimleriGetir()
        {
            try
            {
                IList<SliderResim> sliderResimleri = await _sliderResimRepository.GetAllAsync();
                return OperationResult.Success(sliderResimleri.OrderBy(x => x.ResimSira).ToList());
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(SliderResimleriGetir)} fonksiyonunda hata");
                return OperationResult.Error(Enums.MesajKodu.BeklenmedikHata);
            }
        }

        public async Task<OperationResult> SliderResimleriGetir(Dil dil, SliderYeri sliderYeri)
        {
            try
            {
                IList<SliderResim> sliderResimleri = await _sliderResimRepository.FindAllAsync(x => x.Dil == dil && x.SliderYeri == sliderYeri);
                return OperationResult.Success(sliderResimleri.OrderBy(x => x.ResimSira).ToList());
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(SliderResimleriGetir)} fonksiyonunda hata");
                return OperationResult.Error(Enums.MesajKodu.BeklenmedikHata);
            }
        }


        public async Task<OperationResult> SliderBannerResimleriGetir(Dil dil, string sayfaAdi)
        {
            try
            {
                IList<SliderResim> sliderResimleri = await _sliderResimRepository.FindAllAsync(x => x.Dil == dil && x.AltBaslik == sayfaAdi);
                return OperationResult.Success(sliderResimleri.OrderBy(x => x.ResimSira).ToList());
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(SliderResimleriGetir)} fonksiyonunda hata");
                return OperationResult.Error(Enums.MesajKodu.BeklenmedikHata);
            }
        }


        public async Task<OperationResult> SliderResimSil(int sliderId)
        {
            try
            {
                SliderResim sliderResim = await _sliderResimRepository.GetByIdAsync(sliderId);

                if (sliderResim == null)
                {
                    return OperationResult.Error(MesajKodu.SliderBulunamadi);
                }

                await _sliderResimRepository.DeleteAsync(sliderResim.Id, true);
                return OperationResult.Success();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"{nameof(SliderResimSil)} fonksiyonunda hata");
                return OperationResult.Error(MesajKodu.BeklenmedikHata);
            }
        }
    }
}
