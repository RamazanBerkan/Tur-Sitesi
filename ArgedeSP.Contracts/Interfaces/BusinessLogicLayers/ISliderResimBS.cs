using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Interfaces.BusinessLogicLayers
{
    public interface ISliderResimBS
    {
        Task<OperationResult> SliderResimleriGetir();
        Task<OperationResult> SliderResimleriGetir(Dil dil, SliderYeri sliderYeri);
        Task<OperationResult> SliderBannerResimleriGetir(Dil dil,string sayfaAdi);
        Task SliderResimEkle(SliderResim sliderResim);
        Task<OperationResult> SliderResimSil(int sliderId);
        Task<OperationResult> SliderResimGuncelle(SliderResim sliderResim);

    }
}
