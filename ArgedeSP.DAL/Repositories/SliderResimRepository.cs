using ArgedeSP.Contracts.Interfaces.Repositories;
using ArgedeSP.DAL.DataContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace ArgedeSP.DAL.Repositories
{
    public class SliderResimRepository : GenericRepository<Contracts.Entities.SliderResim>, ISliderResimRepository
    {
        private ArgedeSPContext argedeSPContext;
        public SliderResimRepository(ArgedeSPContext context)
            : base(context)
        {
            argedeSPContext = context;
        }
    }
}
