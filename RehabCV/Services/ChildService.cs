using AutoMapper;
using RehabCV.DTO;
using RehabCV.Models;
using RehabCV.Repositories;
using RehabCV.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RehabCV.Services
{
    public class ChildService : IService<ChildDTO>
    {
        private readonly IRepository<Child> _repository;
        private readonly IMapper _mapper;

        public ChildService(IRepository<Child> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public List<ChildDTO> FindById(string userId)
        {
            return _repository.FindAll().Where(c => c.UserId == userId).ToList().Select(_mapper.Map<ChildDTO>).ToList(); ;
        }

        public void Create(ChildDTO childDTO, string userId)
        {
            var child = new Child
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                FirstName = childDTO.FirstNameOfChild,
                MiddleName = childDTO.MiddleNameOfChild,
                LastName = childDTO.LastNameOfChild,
                Birthday = childDTO.BirthdayOfChild,
                Diagnosis = childDTO.Diagnosis,
                Priority = childDTO.Priority,
                HomeAddress = childDTO.HomeAddress
            };

            _repository.CreateAsync(child);
        }
    }
}
