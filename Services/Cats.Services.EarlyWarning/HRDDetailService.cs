﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Cats.Data.UnitWork;
using Cats.Models;

namespace Cats.Services.EarlyWarning
{
    public class HRDDetailService : IHRDDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        public HRDDetailService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }
        public bool AddHRDDetail(HRDDetail hrdDetail)
        {
            _unitOfWork.HRDDetailRepository.Add(hrdDetail);
            _unitOfWork.Save();
            return true;
        }

        public bool DeleteHRDDetail(HRDDetail hrdDetail)
        {

            _unitOfWork.HRDDetailRepository.Edit(hrdDetail);
            _unitOfWork.Save();
            return true;
        }

        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.HRDDetailRepository.FindById(id);
            if (entity == null) return false;
            _unitOfWork.HRDDetailRepository.Delete(entity);
            _unitOfWork.Save();
            return true;
        }

        public bool EditHRDDetail(HRDDetail hrdDetail)
        {
            _unitOfWork.HRDDetailRepository.Edit(hrdDetail);
            _unitOfWork.Save();
            return true;
        }

        public HRDDetail FindById(int id)
        {
            return _unitOfWork.HRDDetailRepository.FindById(id);
        }

        public List<HRDDetail> GetAllHRDDetail()
        {
            return _unitOfWork.HRDDetailRepository.GetAll();
        }

        public List<HRDDetail> FindBy(Expression<Func<HRDDetail, bool>> predicate)
        {
            return _unitOfWork.HRDDetailRepository.FindBy(predicate);
        }

        public IEnumerable<HRDDetail> Get(Expression<Func<HRDDetail, bool>> filter = null,
                                    Func<IQueryable<HRDDetail>, IOrderedQueryable<HRDDetail>> orderBy = null,
                                    string includeProperties = "")
        {
            return _unitOfWork.HRDDetailRepository.Get(filter, orderBy, includeProperties);
        }
        public bool AddWoreda(HRDDetail hrdDetail)
        {
            var detail =_unitOfWork.HRDDetailRepository.FindBy(
                    m => m.HRDID == hrdDetail.HRDID && m.WoredaID == hrdDetail.WoredaID).FirstOrDefault();
            if (detail == null)
            {
                _unitOfWork.HRDDetailRepository.Add(hrdDetail);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }
        
        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}
