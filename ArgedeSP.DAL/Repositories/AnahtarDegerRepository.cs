using ArgedeSP.Contracts.Entities;
using ArgedeSP.Contracts.Interfaces.Repositories;
using ArgedeSP.Contracts.Models.Common;
using ArgedeSP.DAL.DataContext;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgedeSP.DAL.Repositories
{
    public class AnahtarDegerRepository : GenericRepository<Contracts.Entities.AnahtarDeger>, IAnahtarDegerRepository
    {
        private ArgedeSPContext argedeSPContext;
        public AnahtarDegerRepository(ArgedeSPContext context)
            : base(context)
        {
            argedeSPContext = context;
        }
    }
}
