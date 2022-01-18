using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.Contracts.Models.DTO.Haberler.Req;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Interfaces.BusinessLogicLayers
{
    public interface IHaberBS
    {
        Task<OperationResult> HaberleriGetir();
        OperationResult HaberleriGetir(Dil dil, int adet);
        Task<OperationResult> HaberGetirIdIle(int id);
        Task<OperationResult> HaberEkle(HaberEkle_REQ inputEt);
        Task<OperationResult> HaberGuncelle(HaberEkle_REQ inputEt);
        Task<OperationResult> HaberSil(int id);

        OperationResult HaberleriSayfala(int sayfa, int sayfaBoyutu, int id, string baslik,Dil dil);
    }
}
