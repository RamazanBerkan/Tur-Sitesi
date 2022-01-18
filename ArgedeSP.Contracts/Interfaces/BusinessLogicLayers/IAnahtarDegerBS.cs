using ArgedeSP.Contracts.DTO.Hizmet.Req;
using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Models.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Interfaces.BusinessLogicLayers
{
    public interface IAnahtarDegerBS
    {
        OperationResult AnahtarGetir(Dil dil, string anahtar);
        OperationResult AnahtarGetir(string anahtar);
        OperationResult AnahtarGetir(int id);
        OperationResult AnahtarGuncelle(AnahtarDeger anahtarDeger);

    }
}