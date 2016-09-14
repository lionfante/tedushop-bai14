using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeduShop.Common;
using TeduShop.Data.Infrastructure;
using TeduShop.Data.Repositories;
using TeduShop.Model.Models;

namespace TeduShop.Service
{
    public interface ICommonService
    {
        Footer GetFooter();
        IEnumerable<Slide> GetSlide();
    }
    public class CommonService : ICommonService
    {
        IFooterRepository _footerRepository;
        ISlideRepository _slideRepository;
        IUnitOfWork _unitOfWork;
        public CommonService(IFooterRepository footerRepository, 
                            ISlideRepository slideRepository, 
                            IUnitOfWork unitOfWork)
        {
            _footerRepository = footerRepository;
            _slideRepository = slideRepository;
            _unitOfWork = unitOfWork;
        }
        public Footer GetFooter()
        {
            return _footerRepository.GetSingleByCondition(x=>x.ID == CommonConstants.DefaultFooterID);
        }

        public IEnumerable<Slide> GetSlide()
        {
            return _slideRepository.GetMulti(x=>x.Status == true);
        }
    }
}
