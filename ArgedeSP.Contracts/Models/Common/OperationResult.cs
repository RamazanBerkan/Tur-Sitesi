using System;
using System.Collections.Generic;
using System.Text;
using static ArgedeSP.Contracts.Models.Common.Enums;

namespace ArgedeSP.Contracts.Models.Common
{
    public enum OperationResultTypes
    {
        Success,
        Error
    }

    public class OperationResult
    {
        public OperationResultTypes Type { get; set; }
        public MesajKodu Message { get; set; }
        public object ReturnObject { get; set; }
        public Exception Ex { get; set; }

        public bool IsSuccess
        {
            get
            {
                return Type == OperationResultTypes.Success;
            }
        }



        public static OperationResult Build(OperationResultTypes type, object obj, Exception ex, MesajKodu message)
        {
            return new OperationResult()
            {
                Type = type,
                ReturnObject = obj,
                Message = message

            };
        }


        #region Success
        public static OperationResult Success()
        {
            return OperationResult.Build(OperationResultTypes.Success, null, null, MesajKodu.Bos);
        }
        public static OperationResult Success(object obj)
        {
            return OperationResult.Build(OperationResultTypes.Success, obj, null, MesajKodu.Bos);
        }

        public static OperationResult Success(object obj, MesajKodu message)
        {
            return OperationResult.Build(OperationResultTypes.Success, obj, null, message);
        }
        #endregion

        #region Error
        public static OperationResult Error()
        {
            return OperationResult.Build(OperationResultTypes.Error, null, null, MesajKodu.Bos);
        }
        public static OperationResult Error(object obj)
        {
            return OperationResult.Build(OperationResultTypes.Error, obj, null, MesajKodu.Bos);
        }
        public static OperationResult Error(object obj, MesajKodu message)
        {
            return OperationResult.Build(OperationResultTypes.Error, obj, null, message);
        }
        public static OperationResult Error(MesajKodu message)
        {
            return OperationResult.Build(OperationResultTypes.Error, null, null, message);
        }
        #endregion
    }
}
