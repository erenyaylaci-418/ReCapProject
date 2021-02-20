﻿using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class BrandManager : IBrandService
    {
        IBrandDal _brandDal;
        public BrandManager(IBrandDal brandDal)
        {
            _brandDal = brandDal;
        }
        [ValidationAspect(typeof(BrandValidator))]
        public IResult Add(Brand entity)
        {
            if (entity.BrandName.Length < 2)
            {
                return new ErrorResult(Messages.NameInvalid);
            }
            _brandDal.Add(entity);
            return new SuccessResult(Messages.Added);
        }
        [ValidationAspect(typeof(BrandValidator))]
        public IResult Delete(Brand entity)
        {
            _brandDal.Delete(entity);
            return new SuccessResult(Messages.Deleted);


        }

        public IDataResult<List<Brand>> GetAll()
        {
            return new SuccessDataResult<List<Brand>>(_brandDal.GetAll(), Messages.Listed);
        }

        public IDataResult<Brand> GetById(int Id)
        {
            return new SuccessDataResult<Brand>(_brandDal.Get(b => b.Id == Id));
        }
        [ValidationAspect(typeof(BrandValidator))]
        public IResult Upgrade(Brand entity)
        {
            _brandDal.Update(entity);
            return new SuccessResult(Messages.Upgraded);
        }
    }
}
